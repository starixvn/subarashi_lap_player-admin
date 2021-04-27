import FactoryCommonService from '../../../lib/factoryCommonService';
import PlayerAccountService from "../lib/playerAccountService";
import * as subtabAction from './subtabAction';

const getCustomerBettingStatus = (param) => () => {
    const service = new PlayerAccountService();

    return service.getCustomerBettingStatus(param);
}

const getLink = (sysType, action) => () => {
    const commonService = FactoryCommonService.createInstance();

    return commonService.getLink(sysType, action);
}

export const ActionCreators = {
    ...subtabAction,
    getCustomerBettingStatus,
    getLink
}