import { specialLevelListWithColor, defaultPagingOption } from '../constants';
import PlayerListDataHelper from '../helpers/playerListDataHelper';

export default class PlayerItemQueryService {
    static getPlayerById(param, commonService, apiInfo) {
        const { lrfDataById, lapGetPlayerByLicenseeId } = apiInfo;
        const { ibcCustId, licenseeCustId } = param;

        const lrfParam = {
            idList: ibcCustId,
            specialLevel: Object.keys(specialLevelListWithColor).map(item => item).join(','),
            pageSize: defaultPagingOption.PageSize,
            pageIndex: 1
        }

        const lapParam = {
            custId: licenseeCustId
        }

        const lrfPromise = ibcCustId > 0 ? commonService.fetchData(lrfDataById.type, lrfDataById.action, lrfParam) : null;
        const lapPromise = commonService.fetchData(lapGetPlayerByLicenseeId.type, lapGetPlayerByLicenseeId.action, lapParam);

        return this._getResult(lrfPromise, lapPromise, commonService);
    }

    static _getResult(lrfPromise, lapPromise, commonService) {
        return commonService.fetchMultiple([lrfPromise, lapPromise], (lrfResponse, lapResponse) => {
                const { DataList: lrfPlayerList = [] } = (lrfResponse && lrfResponse.data) || {};
                const { Player } = lapResponse.data;

                const lapPlayerList = Player ? [Player] : [];
                const playerList = PlayerListDataHelper.mergePlayerList(lrfPlayerList, lapPlayerList);

                return playerList.length ? playerList[0] : null;
            })
            .catch((error) => {
                console.log(error.message);
                return null;
            })
    }
}