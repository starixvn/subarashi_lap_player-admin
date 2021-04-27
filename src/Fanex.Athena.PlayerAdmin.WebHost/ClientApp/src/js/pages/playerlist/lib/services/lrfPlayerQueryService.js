import PlayerListDataHelper from '../helpers/playerListDataHelper';

export default class LrfPlayerQueryService {
    static fetchData(param, commonService, apiInfo) {
        const { lrfSearch, lapDataById } = apiInfo;

        return commonService
            .fetchData(lrfSearch.type, lrfSearch.action, { ...param })
            .then(lrfResponse => {
                const { DataList: lrfPlayerList } = lrfResponse.data;

                return !lrfPlayerList || !lrfPlayerList.length
                    ? PlayerListDataHelper.createEmptyResult()
                    : this._fetchFromLap(param, lrfPlayerList, lapDataById, commonService);
            })
            .catch(() => PlayerListDataHelper.createEmptyResult());
    }

    static _fetchFromLap(param, lrfPlayerList, lapDataById, commonService) {
        const lapParam = {
            idList: lrfPlayerList.map(x => x.IbcCustId),
            bankAccountNumber: param.bankAccountNumber,
            email: param.email,
            ip: param.ipLogin
        };

        return commonService
            .fetchData(lapDataById.type, lapDataById.action, lapParam)
            .then(lapResponse => {
                const { PlayerList: lapPlayerList } = lapResponse.data;
                if (!lapPlayerList || !lapPlayerList.length) {
                    return PlayerListDataHelper.createEmptyResult();
                }

                const playerList = PlayerListDataHelper.mergePlayerList(lrfPlayerList, lapPlayerList);

                const startIndex = param.pageSize * (param.pageIndex - 1);
                const endIndex = startIndex + param.pageSize;
                const dataList = playerList.slice(startIndex, endIndex);

                return {
                    playerList: dataList,
                    totalItem: playerList.length
                };
            })
            .catch(() => PlayerListDataHelper.createEmptyResult());
    }
}
