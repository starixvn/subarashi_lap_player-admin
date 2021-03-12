import FactoryCommonService from '../../../../lib/factoryCommonService';
import { systemType } from '../../../../constants/configInfo';

export default class LrfFiltersService {
    constructor() {
        this.commonService = FactoryCommonService.createInstance();
    }

    fetchData() {
        return this.commonService.fetchData(systemType.lrf, 'api/PlayerList/LRFFilters', {});
    }
}
