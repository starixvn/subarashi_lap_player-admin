import LrfPlayerQueryService from '../../services/lrfPlayerQueryService';
import PlayerListDataHelper from '../../helpers/playerListDataHelper';
import { apiInfo } from '../../constants';

describe('LrfPlayerQueryService', () => {
    const defaultParam = {
        currency: 1
    }

    const emptyResult = {
        playerList: [],
        totalItem: 0
    }

    describe('fetchData', () => {
        afterEach(() => {
            jest.restoreAllMocks();
        })

        it('commonService.fetchData should be called with correct parameter', () => {
            const param = {...defaultParam };
            const commonService = {
                fetchData: jest.fn(() => Promise.resolve({ data: {} }))
            }
            const { lrfSearch } = apiInfo;

            const promise = LrfPlayerQueryService.fetchData(param, commonService, apiInfo);

            return Promise.resolve(promise).then(() => {
                expect(commonService.fetchData).toHaveBeenCalledWith(lrfSearch.type, lrfSearch.action, param);
            })
        })

        it('data response is empty - return empty result', () => {
            const param = {...defaultParam };
            const commonService = {
                fetchData: jest.fn(() => Promise.resolve({ data: { DataList: [] } }))
            }

            const promise = LrfPlayerQueryService.fetchData(param, commonService, apiInfo);

            return Promise.resolve(promise).then((result) => {
                expect(result).toEqual(emptyResult);
            })
        })

        it('throw exception - should return empty result', () => {
            const param = {...defaultParam };
            const commonService = {
                fetchData: jest.fn(() => new Promise(() => {
                    throw new Error('Network Error')
                }))
            }

            const promise = LrfPlayerQueryService.fetchData(param, commonService, apiInfo);

            return Promise.resolve(promise).then((result) => {
                expect(result).toEqual(emptyResult);
            })
        })

        it('has data list response - should call _fetchFromLap with correct parameter', () => {
            const param = {...defaultParam };
            const expectedResponse = [{ CustId: 1, UserName: 'test' }];
            const expectedTotalItem = 1;
            const commonService = {
                fetchData: jest.fn(() => Promise.resolve({ data: { DataList: expectedResponse, TotalItem: expectedTotalItem } }))
            }

            const expectedApiInfo = apiInfo.lapDataById;
            const _fetchFromLap = jest.spyOn(LrfPlayerQueryService, '_fetchFromLap');

            LrfPlayerQueryService.fetchData(param, commonService, apiInfo)

            return Promise.resolve().then(() => {
                expect(_fetchFromLap).toHaveBeenCalledWith(expectedResponse, expectedApiInfo, commonService, expectedTotalItem);
            })
        })
    })

    describe('_fetchFromLap', () => {
        let originalMergePlayerList;
        beforeAll(() => {
            originalMergePlayerList = PlayerListDataHelper.mergePlayerList;
        });
        afterEach(() => {
            PlayerListDataHelper.mergePlayerList = originalMergePlayerList;
        })

        it('data response is empty - return lrfPlayerList', () => {
            const lrfPlayerList = [{ IbcCustId: 1, BaStatus: true, OddsGroup: 'A' }, { IbcCustId: 2, BaStatus: false, OddsGroup: 'B' }];
            const expectedLapParam = {
                idList: [1, 2]
            }
            const { lapDataById } = apiInfo;
            const commonService = {
                fetchData: jest.fn((type, action, lapParam) => {
                    const isMatchParam = JSON.stringify(lapParam) === JSON.stringify(expectedLapParam);
                    return type === lapDataById.type && action === lapDataById.action && isMatchParam ?
                        Promise.resolve({ data: { PlayerList: [], TotalItem: 0 } }) : null;
                })
            }
            const lrfTotalItem = 2;

            const expectedResult = {
                playerList: [],
                totalItem: 0
            }

            const promise = LrfPlayerQueryService._fetchFromLap(lrfPlayerList, lapDataById, commonService, lrfTotalItem);

            return Promise.resolve(promise).then((result) => {
                expect(result).toEqual(expectedResult);
            });
        })

        it('data list response has item - return correct result', () => {
            const lrfPlayerList = [{ IbcCustId: 1, BaStatus: true, OddsGroup: 'A' }, { IbcCustId: 2, BaStatus: false, OddsGroup: 'B' }];
            const lapPlayerList = [{ CustId: 1, UserName: 'test1', Status: 0 }, { CustId: 2, UserName: 'test2', Status: 1 }];

            const expectedLapTotalItem = 2;
            const commonService = {
                fetchData: jest.fn(() => Promise.resolve({ data: { PlayerList: lapPlayerList, TotalItem: expectedLapTotalItem } }))
            };

            const expectedMergingData = [
                { CustId: 1, UserName: 'test1', Status: 0, BaStatus: true, OddsGroup: 'A' },
                { CustId: 2, UserName: 'test2', Status: 1, BaStatus: false, OddsGroup: 'B' }
            ];
            PlayerListDataHelper.mergePlayerList = jest.fn((lrfList, lapList) => {
                return lrfList == lrfPlayerList && lapList == lapPlayerList ? expectedMergingData : [];
            });

            const expectedResult = {
                playerList: expectedMergingData,
                totalItem: expectedLapTotalItem
            }
            const lrfTotalItem = 2;
            const promise = LrfPlayerQueryService._fetchFromLap(lrfPlayerList, apiInfo.lapDataById, commonService, lrfTotalItem);

            return Promise.resolve(promise).then((result) => {
                expect(result).toEqual(expectedResult);
            })
        })

        it('throw exception - should build data with Lrf Player List', () => {
            const commonService = {
                fetchData: jest.fn(() => new Promise(() => {
                    throw new Error('Network Error')
                }))
            }
            const lrfPlayerList = [{ IbcCustId: 1, BaStatus: true, OddsGroup: 'A' }, { IbcCustId: 2, BaStatus: false, OddsGroup: 'B' }];
            const lrfTotalItem = 2;

            const expectedResult = {
                playerList: [],
                totalItem: 0
            }

            const promise = LrfPlayerQueryService._fetchFromLap(lrfPlayerList, apiInfo.lapDataById, commonService, lrfTotalItem);

            return Promise.resolve(promise).then((result) => {
                expect(result).toEqual(expectedResult);
            })
        })
    })
})