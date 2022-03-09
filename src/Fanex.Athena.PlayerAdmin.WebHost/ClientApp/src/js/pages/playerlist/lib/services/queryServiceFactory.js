import { specialLevelListWithColor } from "../constants";
import LapPlayerQueryService from "./lapPlayerQueryService";
import LrfPlayerQueryService from "./lrfPlayerQueryService";

export default class QueryServiceFactory {
    static getPlayerListService(param) {
        return !this._isChangeLAPSearch(param) && (this._isDeposited(param) || this._isChangeAdvancedSearch(param))
            ? LrfPlayerQueryService
            : LapPlayerQueryService;
    }

    static isSearchAllSides(param) {
        return this._isChangeLAPSearch(param) && this._isChangeAdvancedSearch(param);
    }

    static _isDeposited(param) {
        return param.isDeposited === 1;
    }
    
    static _isChangeLAPSearch(param) {
        const { bankAccountNumber, email, ipLogin } = param;
        return bankAccountNumber || email || ipLogin;
    }

    static _isChangeAdvancedSearch(param) {
        return Number(param.baFilter) !== 0 ||
            param.currencyIdList !== "" ||
            param.oddsGroup !== "" ||
            Object.keys(specialLevelListWithColor).length !== param.specialLevel.split(",").length ||
            param.customerClass !== "" ||
            param.changeOnCommGroups;
    }
}

