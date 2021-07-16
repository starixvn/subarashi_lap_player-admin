import PlayerListDataHelper from '../helpers/playerListDataHelper';

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
                if (!lapPlayerList || !lapPlayerList.length) {
                    return PlayerListDataHelper.createEmptyResult();
                }

                const playerList = PlayerListDataHelper.mergePlayerList(lrfPlayerList, lapPlayerList);

                return {
                    playerList,
                    totalItem: lrfTotalItem
                };
            })
            .catch(() => PlayerListDataHelper.createEmptyResult());
    }
}

