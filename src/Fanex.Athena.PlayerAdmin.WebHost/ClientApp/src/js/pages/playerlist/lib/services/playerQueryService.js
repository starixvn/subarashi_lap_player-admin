import FactoryCommonService from '../../../../lib/factoryCommonService';
import { apiInfo, isDepositedList } from '../constants';
import ReportExcel from '../reportExcel';
import QueryServiceFactory from './queryServiceFactory';
import PlayerItemQueryService from './playerItemQueryService';
import ArchivedPlayerQueryService from "./archivedPlayerQueryService";

export default class PlayerQueryService {
    constructor() {
        this.commonService = FactoryCommonService.createInstance();
    }

    getTabList() {
        return this.commonService.getTabList();
    }

    getPlayerList(param) {
        const playerListService = QueryServiceFactory.getPlayerListService(param, this.commonService);

        param.ip = param.ipLogin;
        param.deposited = this.getDepositedName(param.isDeposited);

        return playerListService.fetchData(param, this.commonService, apiInfo);
    }

    getArchivedPlayerList(param) {
        return ArchivedPlayerQueryService.fetchData(param, this.commonService, apiInfo);
    }

    getSinglePlayer(param) {
        return PlayerItemQueryService.getPlayerById(param, this.commonService, apiInfo);
    }

    getLink(systemType, action) {
        return this.commonService.getLink(systemType, action);
    }

    getDepositedName(isDeposited) {
        switch (isDeposited) {
            case isDepositedList.All:
                return "All";
            case isDepositedList.No:
                return "No";
            default:
                return "Yes";
        }
    }

    exportExcel(playerList, displayedColumn, fileName = 'PlayerList') {
        const reportExcel = new ReportExcel(playerList, displayedColumn, fileName);
        reportExcel.export();
    }
}
