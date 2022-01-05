import PlayerListDataHelper from '../helpers/playerListDataHelper';
import { buildLrfPlayerList } from '../helpers/playerDataBuilder';

export default class ArchivedPlayerQueryService {
    static fetchData(param, commonService, apiInfo) {
        const { lrfArchivedSearch } = apiInfo;

        return commonService
            .fetchData(lrfArchivedSearch.type, lrfArchivedSearch.action, { ...param })
            .then(lrfResponse => {
                const { DataList, TotalItem: totalItem } = lrfResponse.data;
                if(!DataList || !DataList.length) {
                    return PlayerListDataHelper.createEmptyResult();
                }

                return {
                    playerList: buildLrfPlayerList(DataList),
                    totalItem
                }
            })
            .catch(() => PlayerListDataHelper.createEmptyResult());
    }
}

