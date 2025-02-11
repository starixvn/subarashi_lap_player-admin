import FactoryCommonService from '@jsroot/lib/factoryCommonService';
import { apiUpdateInfo } from '../constants';

export default class PlayerCommandService {
    constructor() {
        this.commonService = FactoryCommonService.createInstance();
    }

    updateOddsGroup(param) {
        const { updateOddsGroup } = apiUpdateInfo;

        return this.commonService.fetchData(updateOddsGroup.type, updateOddsGroup.action, { ...param });
    }

    updateSpecialLevel(param) {
        const { updateSpecialLevel } = apiUpdateInfo;

        return this.commonService.fetchData(updateSpecialLevel.type, updateSpecialLevel.action, { ...param });
    }

    updatePlayerStatus(param) {
        const { updatePlayerStatus } = apiUpdateInfo;

        return this.commonService.fetchData(updatePlayerStatus.type, updatePlayerStatus.action, { ...param });
    }
}
