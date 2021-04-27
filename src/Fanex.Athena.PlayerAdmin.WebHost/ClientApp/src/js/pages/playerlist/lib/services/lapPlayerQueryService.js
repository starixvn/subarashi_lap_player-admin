import PlayerListDataHelper from '../helpers/playerListDataHelper';
import { getNoneDepositOrArchivedList, getCustIdListFromLapData, isNonDepositOrArchived } from '../helpers/utils';
import { buildLapPlayerList } from '../helpers/playerDataBuilder';
import { maxLapPageSize } from '../constants';

export default class LapPlayerQueryService {
    static fetchData(param, commonService, apiInfo) {
        const { lrfDataById, lapSearch } = apiInfo;
        let request = null;
        if (this._shouldBatchRequest(param)) {
            request = this._batchRequest(param, maxLapPageSize, commonService, lapSearch);
        } else {
            request = commonService.fetchData(lapSearch.type, lapSearch.action, { ...param });
        }
        return request.then(lapResponse => {
            const { PlayerList: lapPlayerList = [], TotalItem: lapTotalItem = 0 } = lapResponse.data;
            if (!lapPlayerList || !lapPlayerList.length) {
                return PlayerListDataHelper.createEmptyResult();
            }
            return this._fetchFromLrf(param, lrfDataById, lapPlayerList, commonService, lapTotalItem)
        })
        .catch(() => PlayerListDataHelper.createEmptyResult());;
    }

    static _shouldBatchRequest(param) {
        const { isMatchPrecise, bankAccountNumber, userName, email, pageSize } = param;
        return (!userName || !isMatchPrecise) && !bankAccountNumber && !email && pageSize > maxLapPageSize;
    }

    static _batchRequest(param, maxLapPageSize, commonService, lapSearch) {
        const promises = this._buildBatchPromise(param, maxLapPageSize, commonService, lapSearch);
        return commonService.fetchMultiple(promises, (...responses) => {
            const lapTotalItem = responses[0].data.TotalItem;
            const offset = (param.pageIndex - 1) * param.pageSize
            const lapPlayerList = responses.reduce((result, current) => {
                const currentPlayerList = current.data.PlayerList;
                if(offset + result.length + currentPlayerList.length > lapTotalItem) {
                    return result;
                }
                return [...result, ...currentPlayerList]
            }, []);

            return {
                data: {
                    PlayerList: lapPlayerList,
                    TotalItem: lapTotalItem
                }
            }
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
            }
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

        if (depositIdList.length === 0) {
            return {
                playerList: buildLapPlayerList(noneDepositOrArchivedList),
                totalItem: lapTotalItem
            }
        }

        const lrfParam = {
            currencyIdList: param.currencyIdList,
            idList: depositIdList.join(','),
            specialLevel: param.specialLevel,
            oddsGroup: param.oddsGroup,
            baFilter: param.baFilter,
            customerClass: param.customerClass,
            commGroups: param.commGroups,
            pageSize: param.pageSize,
            pageIndex: 1
        }

        return commonService.fetchData(lrfDataById.type, lrfDataById.action, lrfParam)
            .then(lrfResponse => {
                const { DataList: lrfPlayerList } = lrfResponse.data;
                if (!lrfPlayerList || !lrfPlayerList.length) {
                    return {
                        playerList: buildLapPlayerList(noneDepositOrArchivedList),
                        totalItem: lapTotalItem - lapPlayerList.length + noneDepositOrArchivedList.length
                    }
                }

                const lrfPlayerDic = PlayerListDataHelper.toLrfPlayerDic(lrfPlayerList);
                const filteredLapPlayerList = lapPlayerList.filter(player => isNonDepositOrArchived(player) || !!lrfPlayerDic[player.IBCCustId]);
                const playerList = PlayerListDataHelper.mergePlayerList(lrfPlayerList, filteredLapPlayerList);

                return {
                    playerList,
                    totalItem: lapTotalItem - lapPlayerList.length + filteredLapPlayerList.length
                };
            })
            .catch(() => {
                return {
                    playerList: buildLapPlayerList(nonDepositOrArchivedList),
                    totalItem: nonDepositOrArchivedList.length
                }
            });
    }
}
