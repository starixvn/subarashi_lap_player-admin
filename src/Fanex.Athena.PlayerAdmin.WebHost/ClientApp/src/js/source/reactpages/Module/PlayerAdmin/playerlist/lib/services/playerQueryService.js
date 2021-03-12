import FactoryCommonService from '../../../../lib/factoryCommonService';
import { apiInfo } from '../constants';

import ReportExcel from '../reportExcel';
import QueryServiceFactory from './queryServiceFactory';
import PlayerItemQueryService from './playerItemQueryService';

export default class PlayerQueryService {
    constructor() {
        this.commonService = FactoryCommonService.createInstance();
    }

    getTabList() {
        return this.commonService.getTabList();
    }

    getPlayerList(param) {
        const playerListService = QueryServiceFactory.getPlayerListService(param, this.commonService);
        return playerListService.fetchData(param, this.commonService, apiInfo);
    }

    getSinglePlayer(param) {
        return PlayerItemQueryService.getPlayerById(param, this.commonService, apiInfo);
    }

    getLink(systemType, action) {
        return this.commonService.getLink(systemType, action);
    }

    exportExcel(playerList, param, displayedColumn, hasCommPermissionRead, fileName = 'PlayerList') {
        const reportExcel = new ReportExcel(playerList, displayedColumn, hasCommPermissionRead, fileName);
        reportExcel.export();
    }
}
