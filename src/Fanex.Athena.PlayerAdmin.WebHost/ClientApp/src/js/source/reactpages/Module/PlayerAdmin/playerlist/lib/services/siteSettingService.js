import FactoryCommonService from '../../../../lib/factoryCommonService';
import { apiSiteSetting } from '../constants';

export default class SiteSettingService {
    constructor() {
        this.commonService = FactoryCommonService.createInstance();
    }

    getSetting() {
        return this.commonService.fetchData(apiSiteSetting.type, apiSiteSetting.action, {});
    }
}