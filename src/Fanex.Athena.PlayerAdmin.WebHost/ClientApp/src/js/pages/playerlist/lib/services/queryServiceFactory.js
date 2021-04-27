import { specialLevelListWithColor } from "../constants";
import LapPlayerQueryService from "./lapPlayerQueryService";
import LrfPlayerQueryService from "./lrfPlayerQueryService";

export default class QueryServiceFactory {
    static getPlayerListService(param) {
        return this._isChangeAdvancedSearch(param) ? LrfPlayerQueryService : LapPlayerQueryService;
    }

    static _isChangeAdvancedSearch(param) {
        return Number(param.baFilter) !== 0 ||
            param.currencyIdList !== "" ||
            param.oddsGroup !== "" ||
            Object.keys(specialLevelListWithColor).length !== param.specialLevel.split(",").length ||
            param.customerClass != "" ||
            param.changeOnCommGroups;
    }
}
