import PlayerSuggestionService from '../../services/playerSuggestionService';
import { apiSuggestion } from '../../constants';
import FactoryCommonService from '../../../../../lib/factoryCommonService';

describe('PlayerSuggestionService', () => {
    let originalCreateInstance;

    beforeAll(() => {
        originalCreateInstance = FactoryCommonService.createInstance;
    })

    afterAll(() => {
        FactoryCommonService.createInstance = originalCreateInstance;
    })

    it('getSuggestionList - fetchData should be called with correct parameter', () => {
        const commonService = {
            fetchData: jest.fn()
        }
        FactoryCommonService.createInstance = jest.fn(() => commonService);

        const configOption = {
            method: 'get'
        }
        const param = { userName: 'aa' };
        const instance = new PlayerSuggestionService();

        instance.getSuggestionList(param);

        expect(commonService.fetchData).toHaveBeenCalledWith(apiSuggestion.type, apiSuggestion.action, param, configOption);
    })
})