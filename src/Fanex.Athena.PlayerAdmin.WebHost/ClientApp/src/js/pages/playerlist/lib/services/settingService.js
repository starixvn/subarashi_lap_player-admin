import FactoryCommonService from '@jsroot/lib/factoryCommonService';
import { apiSiteSetting, apiDepositSetting, apiSubsidiarySetting } from '../constants';

export default class SettingService {
    constructor() {
        this.commonService = FactoryCommonService.createInstance();
    }

    getSubsidiarySetting() {
        return this.commonService.fetchData(apiSubsidiarySetting.type, apiSubsidiarySetting.action, {});
    }

    getDepositSetting() {
        return this.commonService.fetchData(apiDepositSetting.type, apiDepositSetting.action, {});
    }

    getSiteSetting() {
        return this.commonService.fetchData(apiSiteSetting.type, apiSiteSetting.action, {});
    }
}
