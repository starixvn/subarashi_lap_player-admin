import QueryServiceFactory from '../../services/queryServiceFactory';
import LapPlayerQueryService from '../../services/lapPlayerQueryService';
import PlayerMultipleQueryService from '../../services/playerMultipleQueryService';
import LrfPlayerQueryService from '../../services/lrfPlayerQueryService';
import { specialLevelListWithColor } from '../../constants';

describe('QueryServiceFactory', () => {
    it('getPlayerListService - return correct service by param', () => {
        const runTest = (param, expectedServiceType) => {
            const result = QueryServiceFactory.getPlayerListService(param);

            expect(result).toBe(expectedServiceType);
        }

        const allSpecialLevel = Object.keys(specialLevelListWithColor).join(',')

        runTest({ bankAccountNumber: '', email: 'abc@gmail.com' }, LapPlayerQueryService);

        runTest({ baFilter: 0, currencyIdList: '', oddsGroup: '', specialLevel: '1,2,3' }, LrfPlayerQueryService);
        runTest({ baFilter: 2, currencyIdList: '', oddsGroup: '', specialLevel: allSpecialLevel }, LrfPlayerQueryService);
        runTest({ baFilter: 0, currencyIdList: '9,20', oddsGroup: '', specialLevel: allSpecialLevel }, LrfPlayerQueryService);
        runTest({ baFilter: 0, currencyIdList: '', oddsGroup: 'A', specialLevel: allSpecialLevel }, LrfPlayerQueryService);
        runTest({ baFilter: 1, currencyIdList: '20', oddsGroup: 'A', specialLevel: '1,2' }, LrfPlayerQueryService);

        runTest({ userName: 'aa', baFilter: 0, currencyIdList: '', oddsGroup: '', specialLevel: allSpecialLevel }, PlayerMultipleQueryService);
    })
})