import PlayerQueryService from '../../services/playerQueryService';
import FactoryCommonService from '../../../../../lib/factoryCommonService';
import QueryServiceFactory from '../../services/queryServiceFactory';
import PlayerItemQueryService from '../../services/playerItemQueryService';
import ReportExcel from '../../reportExcel';
import { systemType } from '../../../../../constants/configInfo';

jest.mock('../../reportExcel');

describe('PlayerQueryService', () => {
    let originalCreateInstance;
    let originalGetPlayerListService;
    let originalGetPlayerById;

    beforeAll(() => {
        originalCreateInstance = FactoryCommonService.createInstance;
        originalGetPlayerListService = QueryServiceFactory.getPlayerListService;
        originalGetPlayerById = PlayerItemQueryService.getPlayerById;
    })

    afterAll(() => {
        FactoryCommonService.createInstance = originalCreateInstance;
        QueryServiceFactory.getPlayerListService = originalGetPlayerListService;
        PlayerItemQueryService.getPlayerById = originalGetPlayerById;
    })

    it('constructor - commonService should be assigned correctly', () => {
        const expectedCommonService = {};
        FactoryCommonService.createInstance = jest.fn(() => expectedCommonService);

        const instance = new PlayerQueryService();

        expect(instance.commonService).toBe(expectedCommonService);
    })

    it('getTabList - return correct result', () => {
        const expectedTabList = [{
            Name: "lblPlayerAdminPlayerInfo",
            PermissionKey: "acc_info_key",
            Action: "PlayerAccountInfo",
            SystemType: 1
        }];
        const commonService = {
            getTabList: jest.fn(() => expectedTabList)
        };
        FactoryCommonService.createInstance = jest.fn(() => commonService);

        const instance = new PlayerQueryService();

        const result = instance.getTabList();

        expect(result).toBe(expectedTabList);
    })

    it('getPlayerList - return correct data', () => {
        const commonService = {};
        FactoryCommonService.createInstance = jest.fn(() => commonService);

        const expectedDataList = [{ custId: 1, userName: 'abc', systemId: 'demo123abc' }];
        const playerListService = {
            fetchData: jest.fn(() => expectedDataList)
        }
        QueryServiceFactory.getPlayerListService = jest.fn(() => playerListService);

        const instance = new PlayerQueryService();

        const param = {
            userName: 'ab'
        }
        const result = instance.getPlayerList(param);

        expect(result).toBe(expectedDataList);
    })

    it('getSinglePlayer - return correct result', () => {
        const commonService = {};
        FactoryCommonService.createInstance = jest.fn(() => commonService);

        const expectedResult = {
            custId: 1,
            ibcCustId: 10,
            userName: 'test'
        }

        PlayerItemQueryService.getPlayerById = jest.fn(() => expectedResult);

        const instance = new PlayerQueryService();
        const param = { userName: 'test' };

        const result = instance.getSinglePlayer(param);

        expect(result).toBe(expectedResult);
    })

    it('getLink - return correct result', () => {
        const expectedResult = '/Action/ControllerName';
        const commonService = {
            getLink: jest.fn(() => expectedResult)
        };
        FactoryCommonService.createInstance = jest.fn(() => commonService);

        const instance = new PlayerQueryService();
        const sysType = systemType.lrf;
        const action = 'Action/ControllerName';

        const result = instance.getLink(sysType, action);

        expect(result).toBe(expectedResult);
    })

    it('exportExcel - should call export function', () => {
        const commonService = {};
        FactoryCommonService.createInstance = jest.fn(() => commonService);

        const exportFnMock = jest.fn();
        const reportExcelMock = jest.fn(() => {
            return {
                export: exportFnMock
            }
        });
        ReportExcel.mockImplementation(reportExcelMock);

        const instance = new PlayerQueryService();
        const playerList = [{ custId: 1, userName: 'test' }];
        const param = { userName: 'test' };
        const displayedColumn = { UserName: true };

        instance.exportExcel(playerList, param, displayedColumn);

        expect(exportFnMock).toHaveBeenCalled();
    })
})