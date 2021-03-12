import PlayerCommandService from '../../services/playerCommandService';
import FactoryCommonService from '../../../../../lib/factoryCommonService';
import { apiUpdateInfo } from '../../constants';

describe('PlayerCommandService', () => {
    let originalCreateInstance;
    const defaultCommonService = {
        fetchData: jest.fn()
    }

    beforeEach(() => {
        originalCreateInstance = FactoryCommonService.createInstance;
    })

    afterEach(() => {
        FactoryCommonService.createInstance = originalCreateInstance;
    })

    it('updateBaStatus - call fetchData with correct parameter', () => {
        const commonService = {...defaultCommonService };
        FactoryCommonService.createInstance = jest.fn(() => commonService);
        const { updateBaStatus } = apiUpdateInfo;

        const commandService = new PlayerCommandService();
        const param = {
            custId: 1,
            isBa: true
        }

        commandService.updateBaStatus(param);

        expect(commonService.fetchData).toHaveBeenCalledWith(updateBaStatus.type, updateBaStatus.action, param);;
    })

    it('updateOddsGroup - call fetchData with correct parameter', () => {
        const commonService = {...defaultCommonService };
        FactoryCommonService.createInstance = jest.fn(() => commonService);
        const { updateOddsGroup } = apiUpdateInfo;

        const commandService = new PlayerCommandService();
        const param = {
            custId: 1,
            oddsGroup: 'A'
        }

        commandService.updateOddsGroup(param);

        expect(commonService.fetchData).toHaveBeenCalledWith(updateOddsGroup.type, updateOddsGroup.action, param);;
    })

    it('updateSpecialLevel - call fetchData with correct parameter', () => {
        const commonService = {...defaultCommonService };
        FactoryCommonService.createInstance = jest.fn(() => commonService);
        const { updateSpecialLevel } = apiUpdateInfo;

        const commandService = new PlayerCommandService();
        const param = {
            custId: 1,
            specialLevel: 2
        }

        commandService.updateSpecialLevel(param);

        expect(commonService.fetchData).toHaveBeenCalledWith(updateSpecialLevel.type, updateSpecialLevel.action, param);;
    })

    it('updatePlayerStatus - call fetchData with correct parameter', () => {
        const commonService = {...defaultCommonService };
        FactoryCommonService.createInstance = jest.fn(() => commonService);
        const { updatePlayerStatus } = apiUpdateInfo;

        const commandService = new PlayerCommandService();
        const param = {
            custId: 1,
            status: 2
        }

        commandService.updatePlayerStatus(param);

        expect(commonService.fetchData).toHaveBeenCalledWith(updatePlayerStatus.type, updatePlayerStatus.action, param);;
    })
})