import SiteSettingService from '../../services/siteSettingService';
import { apiSiteSetting } from '../../constants';
import FactoryCommonService from '../../../../../lib/factoryCommonService';

describe('SiteSettingService', () => {
    let originalCreateInstance;

    beforeAll(() => {
        originalCreateInstance = FactoryCommonService.createInstance;
    })

    afterAll(() => {
        FactoryCommonService.createInstance = originalCreateInstance;
    })

    it('getSetting - fetchData should be called with correct parameter', () => {
        const commonService = {
            fetchData: jest.fn()
        }
        FactoryCommonService.createInstance = jest.fn(() => commonService);

        const param = {};
        const instance = new SiteSettingService();

        instance.getSetting();

        expect(commonService.fetchData).toHaveBeenCalledWith(apiSiteSetting.type, apiSiteSetting.action, param);
    })
})