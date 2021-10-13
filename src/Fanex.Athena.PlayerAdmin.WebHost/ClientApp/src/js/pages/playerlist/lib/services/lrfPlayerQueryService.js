import PlayerListDataHelper from '../helpers/playerListDataHelper';
import { buildLrfPlayerList } from '../helpers/playerDataBuilder';

export default class LrfPlayerQueryService {
    static fetchData(param, commonService, apiInfo) {
        const { lrfSearch, lapDataById } = apiInfo;

        return commonService
            .fetchData(lrfSearch.type, lrfSearch.action, { ...param })
            .then(lrfResponse => {
                const { DataList: lrfPlayerList, TotalItem } = lrfResponse.data;

                return !lrfPlayerList || !lrfPlayerList.length
                    ? PlayerListDataHelper.createEmptyResult()
                    : this._fetchFromLap(lrfPlayerList, lapDataById, commonService, TotalItem);
            })
            .catch(() => PlayerListDataHelper.createEmptyResult());
    }

    static _fetchFromLap(lrfPlayerList, lapDataById, commonService, lrfTotalItem) {
        const lapParam = {
            idList: lrfPlayerList.map(x => x.IbcCustId)
        };

        return commonService
            .fetchData(lapDataById.type, lapDataById.action, lapParam)
            .then(lapResponse => {
                const { PlayerList: lapPlayerList } = lapResponse.data;
                let playerList = [];
                if (!lapPlayerList || !lapPlayerList.length) {
                    playerList = buildLrfPlayerList(lrfPlayerList);
                } else {
                    playerList = PlayerListDataHelper.mergePlayerListBasedLrf(lrfPlayerList, lapPlayerList);
                }
                return {
                    playerList,
                    totalItem: lrfTotalItem
                };
            })
            .catch(() => PlayerListDataHelper.createEmptyResult());
    }
}

