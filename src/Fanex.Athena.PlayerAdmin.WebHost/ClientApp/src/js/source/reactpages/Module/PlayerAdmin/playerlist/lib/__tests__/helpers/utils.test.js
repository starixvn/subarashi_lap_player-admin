import {
    getSpecialLevelText,
    getBaStatusText,
    getPlayerStatus,
    buildDisplayedColumnObject,
    getCustIdListFromLapData,
    getErrorMessageForUpdating
} from '../../helpers/utils';

import LangHelper from '@jsroot/lib/langHelper';
import { playerStatusByName } from '../../constants';

describe('utils', () => {
    let originalGetSingleResource;
    beforeAll(() => {
        originalGetSingleResource = LangHelper.getSingleResource;
        LangHelper.getSingleResource = jest.fn(v => v);
    })
    afterAll(() => {
        LangHelper.getSingleResource = originalGetSingleResource;
    })

    it('getSpecialLevelText - return correct text', () => {
        const runTest = (value, expectedResult) => {
            const result = getSpecialLevelText(value);

            expect(result).toBe(expectedResult);
        }

        runTest(null, '');
        runTest(undefined, '');
        runTest(0, 'None');
        runTest(1, 1);
    })

    it('getBaStatusText - return correct text', () => {
        const runTest = (isBA, expectedResult) => {
            const result = getBaStatusText(isBA);

            expect(result).toBe(expectedResult);
        }

        runTest(false, 'Off');
        runTest(true, 'On');
    })

    it('getPlayerStatus - return correct text', () => {
        const runTest = (value, expectedResult) => {
            const result = getPlayerStatus(value);

            expect(result).toBe(expectedResult);
        }

        runTest(playerStatusByName.Active, 'Active');
        runTest(999, '');
    })

    it('buildDisplayedColumnObject - return correct result', () => {
        const displayedColumnList = [{ key: 'status', show: true }, { key: 'username', show: false }];
        const expectedColumnObject = {
            status: true,
            username: false
        };

        const result = buildDisplayedColumnObject(displayedColumnList);

        expect(result).toEqual(expectedColumnObject);
    })

    it('getCustIdListFromLapData - return correct ID List', () => {
        const lapPlayerList = [
            { IBCCustId: 0, isArchived: false, userName: 'test01' },
            { IBCCustId: 1, isArchived: true, userName: 'test02' },
            { IBCCustId: 2, isArchived: false, userName: 'test03' },
        ]
        const expectedResult = [2];

        const result = getCustIdListFromLapData(lapPlayerList);

        expect(result).toEqual(expectedResult);
    })

    it('getErrorMessageForUpdating - return correct error message', () => {
        const runTest = (errorInfo, expectedResult) => {
            const result = getErrorMessageForUpdating(errorInfo);

            expect(result).toBe(expectedResult);
        }

        runTest(undefined, 'HttpError_Default');
        runTest({ response: null }, 'HttpError_Default');
        runTest({ response: { status: 403 } }, 'FunctionDeniedMsg');
        runTest({ response: { status: 401 } }, 'FunctionDeniedMsg');
        runTest({ response: { status: 500 } }, 'HttpError_Default');
    })
})