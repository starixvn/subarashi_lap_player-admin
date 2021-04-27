import FactoryCommonService from '@jsroot/lib/factoryCommonService';
import { systemType } from '@jsroot/constants/config';

export default class LrfFiltersService {
    constructor() {
        this.commonService = FactoryCommonService.createInstance();
    }

    fetchData() {
        return this.commonService.fetchData(systemType.playerAdmin, 'api/PlayerList/LRFFilters', {});
    }
}
