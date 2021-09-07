import PlayerListDataHelper from '../helpers/playerListDataHelper';
import { getNoneDepositOrArchivedList, getCustIdListFromLapData, isNonDepositOrArchived } from '../helpers/utils';
import { buildLapPlayerList } from '../helpers/playerDataBuilder';
import { maxLapPageSize, maxLapSearchItems} from '../constants';
import QueryServiceFactory from './queryServiceFactory';

export default class LapPlayerQueryService {
    static fetchData(param, commonService, apiInfo) {
      const { lrfDataById, lapSearch } = apiInfo;
      let request = null;
      const isSearchAllSides = QueryServiceFactory.isSearchAllSides(param);
      const lapParam = {
        ...param,
        pageSize: isSearchAllSides ? maxLapSearchItems : param.pageSize,
        pageIndex: isSearchAllSides ? 1 : param.pageIndex
      };
  
      request = this._shouldBatchRequest(lapParam)
        ? this._batchRequest(lapParam, maxLapPageSize, commonService, lapSearch)
        : commonService.fetchData(lapSearch.type, lapSearch.action, { ...param });
  
      return request.then(lapResponse => {
        const { PlayerList: lapPlayerList = [], TotalItem: lapTotalItem = 0 } = lapResponse.data;
        if (!lapPlayerList || !lapPlayerList.length) {
          return PlayerListDataHelper.createEmptyResult();
        }
  
        return this._fetchFromLrf(param, lrfDataById, lapPlayerList, commonService, lapTotalItem);
      }).catch(() => PlayerListDataHelper.createEmptyResult());
    }
  
    static _shouldBatchRequest(param) {
      const { isMatchPrecise, userName, pageSize } = param;
      return (!userName || !isMatchPrecise) && !QueryServiceFactory._isChangeLAPSearch(param) && pageSize > maxLapPageSize;
    }
  
    static _batchRequest(param, maxLapPageSize, commonService, lapSearch) {
      const promises = this._buildBatchPromise(param, maxLapPageSize, commonService, lapSearch);
      return commonService.fetchMultiple(promises, (...responses) => {
        const lapTotalItem = responses[0].data.TotalItem;
        const offset = (param.pageIndex - 1) * param.pageSize
        const lapPlayerList = responses.reduce((result, current) => {
          const currentPlayerList = current.data.PlayerList;
          if (offset + result.length + currentPlayerList.length > lapTotalItem) {
            return result;
          }
  
          return [...result, ...currentPlayerList]
        }, []);
  
        return {
          data: {
            PlayerList: lapPlayerList,
            TotalItem: lapTotalItem
          }
        };
      })
    }
  
    static _buildBatchPromise(param, maxLapPageSize, commonService, lapSearch) {
      const { pageIndex, pageSize } = param;
      const batchSize = pageSize / maxLapPageSize;
      const pageIndexStart = batchSize * (pageIndex - 1);
      const promises = [];
      for (let batchIndex = 1; batchIndex <= batchSize; batchIndex++) {
        const batchParam = {
          ...param,
          pageIndex: pageIndexStart + batchIndex,
          pageSize: maxLapPageSize
        };
  
        promises.push(commonService
          .fetchData(lapSearch.type, lapSearch.action, { ...batchParam })
          .catch(error => {
            console.log(error.message);
            window.alert(LangHelper.getSingleResource('HttpError_Default'));
            return { data: { PlayerList: [], TotalItem: 0 } };
          })
        );
      }
  
      return promises;
    }
  
    static _fetchFromLrf(param, lrfDataById, lapPlayerList, commonService, lapTotalItem) {
      const noneDepositOrArchivedList = getNoneDepositOrArchivedList(lapPlayerList);
      const depositIdList = getCustIdListFromLapData(lapPlayerList);
  
      const isChangeAdvanceSearch = QueryServiceFactory._isChangeAdvancedSearch(param);
      if (isChangeAdvanceSearch && param.deposited == "No") {
        return PlayerListDataHelper.createEmptyResult();
      }
  
      if (depositIdList.length === 0) {
        const resultList = buildLapPlayerList(noneDepositOrArchivedList);
        return this._pagingResult(param, resultList, lapTotalItem);
      }
  
      const lrfParam = {
        currencyIdList: param.currencyIdList,
        idList: depositIdList.join(','),
        specialLevel: param.specialLevel,
        oddsGroup: param.oddsGroup,
        baFilter: param.baFilter,
        customerClass: param.customerClass,
        commGroups: param.commGroups,
        pageSize: depositIdList.length,
        pageIndex: 1
      };
  
      return commonService.fetchData(lrfDataById.type, lrfDataById.action, lrfParam)
        .then(lrfResponse => {
          const { DataList: lrfPlayerList } = lrfResponse.data;
          if (!lrfPlayerList || !lrfPlayerList.length) {
            const resultList = buildLapPlayerList(noneDepositOrArchivedList);
            return this._pagingResult(param, resultList, lapTotalItem - depositIdList.length);
          }
  
          const lrfPlayerDic = PlayerListDataHelper.toLrfPlayerDic(lrfPlayerList);
          const filteredLapPlayerList = lapPlayerList.filter(player => isNonDepositOrArchived(player) || !!lrfPlayerDic[player.IBCCustId]);
          const playerList = PlayerListDataHelper.mergePlayerList(lrfPlayerList, filteredLapPlayerList);
  
          return this._pagingResult(param, playerList, lapTotalItem - lapPlayerList.length + filteredLapPlayerList.length);
        })
        .catch(() => {
          const resultList = buildLapPlayerList(noneDepositOrArchivedList);
          return this._pagingResult(param, resultList, lapTotalItem);
        });
    }
  
    static _pagingResult(param, playerList, totalItem) {
      const isSearchAllSides = QueryServiceFactory.isSearchAllSides(param);
  
      if (isSearchAllSides) {
        const startIndex = param.pageSize * (param.pageIndex - 1);
        const endIndex = startIndex + param.pageSize;
        const dataList = playerList.slice(startIndex, endIndex);
  
        return {
          playerList: dataList,
          totalItem: playerList.length
        };
      }
  
      return { playerList, totalItem };
    }
  }