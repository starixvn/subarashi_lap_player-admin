import PlayerItemQueryService from '../../services/playerItemQueryService';
import { specialLevelListWithColor, defaultPagingOption, apiInfo } from '../../constants';

describe('playerItemQueryService', () => {
    describe('getPlayerById', () => {
        const runTestForGetPlayerById = (param) => {
            const originalGetResult = PlayerItemQueryService._getResult;
            const lrfPromise = Promise.resolve({});
            const lapPromise = Promise.resolve({});
            const { lrfDataById, lapGetPlayerByLicenseeId } = apiInfo;

            const commonService = {
                fetchData: jest.fn((licType, action) => {
                    if (action === lrfDataById.action) {
                        return lrfPromise;
                    }

                    if (action === lapGetPlayerByLicenseeId.action) {
                        return lapPromise;
                    }

                    return null;
                })
            }

            PlayerItemQueryService._getResult = jest.fn();

            PlayerItemQueryService.getPlayerById(param, commonService, apiInfo);

            PlayerItemQueryService._getResult = originalGetResult;

            return commonService;
        }

        it('getPlayerById - has data - return correct playerList', () => {
            const { lrfDataById, lapGetPlayerByLicenseeId } = apiInfo;
            const param = {
                ibcCustId: 1,
                licenseeCustId: 11
            }
            const lrfParam = {
                idList: param.ibcCustId,
                specialLevel: Object.keys(specialLevelListWithColor).map(item => item).join(','),
                pageSize: defaultPagingOption.PageSize,
                pageIndex: 1
            }

            const lapParam = {
                custId: param.licenseeCustId
            }
            const expectedLapPromiseParam = [lapGetPlayerByLicenseeId.type, lapGetPlayerByLicenseeId.action, lapParam];
            const expectedLrfPromiseParam = [lrfDataById.type, lrfDataById.action, lrfParam];
            const commonService = runTestForGetPlayerById(param);

            expect(commonService.fetchData.mock.calls[0]).toEqual(expectedLrfPromiseParam);
            expect(commonService.fetchData.mock.calls[1]).toEqual(expectedLapPromiseParam);
        })

        it('getPlayerById - has data - return correct playerList', () => {
            const { lapGetPlayerByLicenseeId } = apiInfo;
            const param = {
                ibcCustId: 0,
                licenseeCustId: 11
            }
            const lapParam = {
                custId: param.licenseeCustId
            }
            const expectedLapPromiseParam = [lapGetPlayerByLicenseeId.type, lapGetPlayerByLicenseeId.action, lapParam];

            const commonService = runTestForGetPlayerById(param);

            expect(commonService.fetchData.mock.calls[0]).toEqual(expectedLapPromiseParam);
        })
    })

    describe('_getResult', () => {
        it('has data for both sides - Player List should be returned with only one item', () => {
            let handler;
            const commonService = {
                fetchMultiple: jest.fn((promiseList, callback) => {
                    handler = callback;

                    return {
                        catch: jest.fn()
                    }
                })
            }

            const lrfResponse = { data: { DataList: [{ IbcCustId: 1 }] } };
            const lapResponse = { data: { Player: { CustId: 11, IBCCustId: 1, UserName: 'test' } } };

            PlayerItemQueryService._getResult({}, {}, commonService);
            const result = handler(lrfResponse, lapResponse);

            expect(result).not.toBeNull();
        })

        it('Non-deposit case - Player List should be returned from only LAP side', () => {
            let handler;
            const commonService = {
                fetchMultiple: jest.fn((promiseList, callback) => {
                    handler = callback;

                    return {
                        catch: jest.fn()
                    }
                })
            }

            const lrfResponse = null;
            const lapResponse = { data: { Player: { CustId: 11, IBCCustId: 0, UserName: 'test' } } };

            PlayerItemQueryService._getResult({}, {}, commonService);
            const result = handler(lrfResponse, lapResponse);

            expect(result.LicenseeCustId).toBe(11);
            expect(result.IbcCustId).toBe(0);
            expect(result.UserName).toBe('test');
        })
    })
})