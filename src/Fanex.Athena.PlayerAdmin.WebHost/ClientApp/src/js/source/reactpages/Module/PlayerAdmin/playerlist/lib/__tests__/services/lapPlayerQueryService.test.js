import LapPlayerQueryService from '../../services/lapPlayerQueryService';
import PlayerListDataHelper from '../../helpers/playerListDataHelper';
import { apiInfo } from '../../constants';
import { buildLapPlayerList } from '../../helpers/playerDataBuilder';

describe('LapPlayerQueryService', () => {
    const defaultParam = {
        bankAccountNumber: '123',
        email: 'test@gmail.com'
    }

    const emptyResult = {
        playerList: [],
        totalItem: 0
    }

    const createDefaultLapPlayerList = () => {
        return [{
                IBCCustId: 1,
                UserName: 'test1',
                Status: 0,
                IP: '1.1.1.1',
                email: 'abc@gmail.com',
                isOnline: true,
                RealName: 'test1real',
                ActStatus: 2,
                isArchived: false
            },
            {
                IBCCustId: 2,
                UserName: 'test2',
                Status: 1,
                IP: '2.2.2.2',
                email: 'xyz@gmail.com',
                isOnline: false,
                RealName: 'test2real',
                ActStatus: 2,
                isArchived: false
            }
        ]
    }

    const createDefaultMergedPlayerList = () => {
        return [{
                CustId: 1,
                UserName: 'test1',
                Status: 0,
                RealName: 'test1real',
                LastLoginIp: '1.1.1.1',
                Email: 'abc@gmail.com',
                IsOnline: true,
                IsArchived: false,
                IsDeposit: true
            },
            {
                CustId: 2,
                UserName: 'test2',
                Status: 1,
                RealName: 'test2real',
                LastLoginIp: '2.2.2.2',
                Email: 'xyz@gmail.com',
                IsOnline: false,
                IsArchived: false,
                IsDeposit: true
            }
        ]
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

            const expectedApiInfo = apiInfo.lapSearch;

            const promise = LapPlayerQueryService.fetchData(param, commonService, apiInfo);

            return Promise.resolve(promise).then(() => {
                expect(commonService.fetchData).toHaveBeenCalledWith(expectedApiInfo.type, expectedApiInfo.action, param);
            })
        })

        it('return empty list - should return correct result', () => {
            const param = {...defaultParam };
            const commonService = {
                fetchData: jest.fn(() => Promise.resolve({ data: { PlayerList: [], TotalItem: 0 } }))
            }

            const promise = LapPlayerQueryService.fetchData(param, commonService, apiInfo);

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

            const promise = LapPlayerQueryService.fetchData(param, commonService, apiInfo);

            return Promise.resolve(promise).then((result) => {
                expect(result).toEqual(emptyResult);
            })
        })

        it('has player list response - should call _fetchFromLrf with correct parameter', () => {
            const param = {...defaultParam };
            const expectedResponse = [{ CustId: 1, UserName: 'test' }];
            const expectedTotalItem = 1;
            const commonService = {
                fetchData: jest.fn(() => Promise.resolve({ data: { PlayerList: expectedResponse, TotalItem: expectedTotalItem } }))
            }

            const expectedApiInfo = apiInfo.lrfDataById;
            const _fetchFromLrf = jest.spyOn(LapPlayerQueryService, '_fetchFromLrf');

            const promise = LapPlayerQueryService.fetchData(param, commonService, apiInfo)

            return Promise.resolve(promise).then(() => {
                expect(_fetchFromLrf).toHaveBeenCalledWith(param, expectedApiInfo, expectedResponse, commonService, expectedTotalItem);
            })
        })
    })

    describe('_fetchFromLrf', () => {
        let originalMergePlayerList;
        beforeAll(() => {
            originalMergePlayerList = PlayerListDataHelper.mergePlayerList;
        });
        afterEach(() => {
            PlayerListDataHelper.mergePlayerList = originalMergePlayerList;
        });
        const defaultLrfParam = {
            currencyIdList: '',
            specialLevel: '',
            pageSize: 50,
            pageIndex: 1,
            oddsGroup: 'A,B',
            baFilter: 2
        }

        it('lapPlayerList do not includes deposit player - return correct non-deposit player list', () => {
            const lapPlayerList = [{
                IBCCustId: 0,
                UserName: 'test1',
                Status: 0,
                IP: '1.1.1.1',
                email: 'abc@gmail.com',
                isOnline: true,
                RealName: 'test1real',
                ActStatus: 2,
                isArchived: false
            }];
            const lapTotalItem = 1;
            const param = {...defaultLrfParam };
            const { lrfDataById } = apiInfo;
            const commonService = {};

            const expectedResult = {
                playerList: buildLapPlayerList(lapPlayerList),
                totalItem: lapTotalItem
            }

            const result = LapPlayerQueryService._fetchFromLrf(param, lrfDataById, lapPlayerList, commonService, lapTotalItem);

            expect(result).toEqual(expectedResult);
        })

        it('DataList response is empty - return Lap Player List', () => {
            const lapPlayerList = createDefaultLapPlayerList();
            const lapTotalItem = 2;
            const param = {...defaultLrfParam };

            const expectedLrfParam = {
                currencyIdList: param.currencyIdList,
                idList: '1,2',
                specialLevel: param.specialLevel,
                oddsGroup: param.oddsGroup,
                baFilter: param.baFilter,
                pageSize: param.pageSize,
                pageIndex: param.pageIndex
            }
            const { lrfDataById } = apiInfo;
            const commonService = {
                fetchData: jest.fn((type, action, lrfParam) => {
                    const isMatchParam = JSON.stringify(lrfParam) === JSON.stringify(expectedLrfParam);
                    return type === lrfDataById.type && action === lrfDataById.action && isMatchParam ?
                        Promise.resolve({ data: { DataList: [], TotalItem: 0 } }) : null;
                })
            }

            const expectedResult = {
                playerList: [],
                totalItem: 0
            }

            const promise = LapPlayerQueryService._fetchFromLrf(param, lrfDataById, lapPlayerList, commonService, lapTotalItem);

            return Promise.resolve(promise)
                .then((result) => {
                    expect(result).toEqual(expectedResult);
                })
        })

        it('return correct data result', () => {
            const lapPlayerList = createDefaultLapPlayerList();;
            const lapTotalItem = 2;
            const param = {...defaultLrfParam };

            const expectedResponse = [{ IbcCustId: 1, BaStatus: true, OddsGroup: 'A' }, { IbcCustId: 2, BaStatus: false, OddsGroup: 'B' }];
            const commonService = {
                fetchData: jest.fn(() => Promise.resolve({ data: { DataList: expectedResponse, TotalItem: 2 } }))
            }
            const expectedMergingData = createDefaultMergedPlayerList();
            PlayerListDataHelper.mergePlayerList = jest.fn(() => expectedMergingData);

            const expectedResult = {
                playerList: expectedMergingData,
                totalItem: lapTotalItem
            }
            const response = LapPlayerQueryService._fetchFromLrf(param, apiInfo.lrfDataById, lapPlayerList, commonService, lapTotalItem);

            return Promise.resolve(response).then((result) => {
                expect(result).toEqual(expectedResult);
            })
        })

        it('throw exception - should build data with Lap Player List', () => {
            const lapPlayerList = createDefaultLapPlayerList();
            const lapTotalItem = 2;
            const param = {
                currencyIdList: '',
                specialLevel: '',
                pageSize: 50,
                pageIndex: 1,
                oddsGroup: 'A,B',
                baFilter: 2
            }
            const commonService = {
                fetchData: jest.fn(() => new Promise(() => {
                    throw new Error('Network Error')
                }))
            }
            const { lrfDataById } = apiInfo;
            const expectedResult = {
                playerList: [],
                totalItem: 0
            }
            const promise = LapPlayerQueryService._fetchFromLrf(param, lrfDataById, lapPlayerList, commonService, lapTotalItem);

            return Promise.resolve(promise)
                .then((result) => {
                    expect(result).toEqual(expectedResult);
                })
        })
    })
})