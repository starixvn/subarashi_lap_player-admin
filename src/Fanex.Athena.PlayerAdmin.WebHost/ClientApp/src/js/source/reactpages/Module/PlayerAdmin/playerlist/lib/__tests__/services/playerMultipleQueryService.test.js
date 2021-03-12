import PlayerMultipleQueryService from '../../services/playerMultipleQueryService';
import PlayerListDataHelper from '../../helpers/playerListDataHelper';
import { apiInfo } from '../../constants';
import * as utils from '../../helpers/utils';

describe('PlayerMultipleQueryService', () => {
    let originalAlert;
    beforeAll(() => {
        originalAlert = window.alert;
        window.alert = jest.fn();
    })
    afterAll(() => {
        window.alert = originalAlert;
    })
    const defaultParam = {
        currency: '',
        specialLevel: '',
        pageSize: 50,
        pageIndex: 1
    };
    const defaultLapPlayerList = [
        { IBCCustId: 1, UserName: 'test1', Status: 0, IP: '1.1.1.1', email: 'abc@gmail.com', isOnline: true, RealName: 'test1real', ActStatus: 2, isArchived: false },
        { IBCCustId: 2, UserName: 'test2', Status: 1, IP: '2.2.2.2', email: 'xyz@gmail.com', isOnline: false, RealName: 'test2real', ActStatus: 2, isArchived: false }
    ];
    const defaultLapDifferenceList = [{ IBCCustId: 4, UserName: 'test4', Status: 0, IP: '4.4.4.4', email: 'ijk@gmail.com', isOnline: true, RealName: 'test4real', ActStatus: 2, isArchived: false }];
    const defaultLrfPlayerList = [{ IbcCustId: 1, BaStatus: true, OddsGroup: 'A' }, { IbcCustId: 2, BaStatus: false, OddsGroup: 'B' }];
    afterEach(() => {
        jest.restoreAllMocks();
    })

    describe('fetchData', () => {
        afterEach(() => {
            jest.restoreAllMocks();
        })

        it('should call _getResult with correct parameter', () => {
            const _getResult = jest.spyOn(PlayerMultipleQueryService, '_getResult');
            const { lrfSearch, lapSearch } = apiInfo;
            const param = { userName: 'test' };

            const expectedPromise = Promise.resolve({});
            const commonService = {
                fetchData: jest.fn((type, action, passedParam) => {
                    const isValidParam = (type === lrfSearch.type || type === lapSearch.type) &&
                        (action === lrfSearch.action || action === lapSearch.action) &&
                        passedParam.userName === param.userName;

                    return isValidParam ? expectedPromise : null;
                }),
                fetchMultiple: jest.fn(() => ({
                    catch: () => ({})
                }))
            };

            const promise = PlayerMultipleQueryService.fetchData(param, commonService, apiInfo);

            return Promise.resolve(promise).then(() => {
                expect(_getResult).toHaveBeenCalledWith(param, expectedPromise, expectedPromise, commonService, apiInfo);
            });
        })

        const runTestForThrowingException = (lrfPromise, lapPromise, isCheckLrfPromise) => {
            const { lrfSearch } = apiInfo;
            const param = { userName: 'test' };
            const commonService = {
                fetchData: jest.fn((type, action) => {
                    return action === lrfSearch.action ? lrfPromise : lapPromise;
                }),
                fetchMultiple: jest.fn(() => ({
                    catch: () => ({})
                }))
            };

            let passedPromise = null;
            const originalGetResult = PlayerMultipleQueryService._getResult;
            PlayerMultipleQueryService._getResult = jest.fn((param, lrfPromiseParam, lapPromiseParam) => {
                passedPromise = isCheckLrfPromise ? lrfPromiseParam : lapPromiseParam;
            });
            PlayerMultipleQueryService.fetchData(param, commonService, apiInfo);

            const expectedResult = {
                data: {
                    [isCheckLrfPromise ? 'DataList' : 'PlayerList']: [],
                    TotalItem: 0
                }
            };
            return passedPromise.then((result) => {
                expect(result).toEqual(expectedResult);
                PlayerMultipleQueryService._getResult = originalGetResult;
            });
        }
        it('lrfPromise throw exception - return empty result', () => {
            const lrfPromise = new Promise(() => {
                throw new Error('LRF Internal Error')
            });
            const lapPromise = Promise.resolve({});

            return runTestForThrowingException(lrfPromise, lapPromise, true);
        })

        it('lapPromise throw exception - return empty result', () => {
            const lrfPromise = Promise.resolve({});
            const lapPromise = new Promise(() => {
                throw new Error('LAP Internal Error')
            });

            return runTestForThrowingException(lrfPromise, lapPromise, false);
        })
    })

    describe('_getResult', () => {
        let originalMergePlayerList;
        let originalGetCustIdListFromLapData;
        beforeAll(() => {
            originalMergePlayerList = PlayerListDataHelper.mergePlayerList;
            originalGetCustIdListFromLapData = utils.getCustIdListFromLapData;
        });

        let handler = null;
        let defaultCommonService = null;
        beforeEach(() => {
            defaultCommonService = {
                fetchMultiple: jest.fn((promiseList, callback) => {
                    handler = callback;

                    return { catch: jest.fn() }
                })
            }
        })
        afterEach(() => {
            handler = null;
            defaultCommonService = null;
            PlayerListDataHelper.mergePlayerList = originalMergePlayerList;
            utils.getCustIdListFromLapData = originalGetCustIdListFromLapData;
        })

        const runTestCallingCorrectFunction = (param, lrfList, lapList, functionName, checkResultFunc) => {
            const lrfResponse = { data: { DataList: lrfList, TotalItem: lrfList.length } };
            const lapResponse = { data: { PlayerList: lapList, TotalItem: lapList.length } };
            const lrfPromise = Promise.resolve(lrfResponse);
            const lapPromise = Promise.resolve(lapResponse);

            const commonService = {...defaultCommonService }

            const originalRefetchFunction = PlayerMultipleQueryService[functionName];
            PlayerMultipleQueryService[functionName] = jest.fn(() => ({ then: () => {} }));
            PlayerMultipleQueryService._getResult(param, lrfPromise, lapPromise, commonService, apiInfo);
            handler(lrfResponse, lapResponse);

            checkResultFunc();

            PlayerMultipleQueryService[functionName] = originalRefetchFunction;
        }

        it('LAP side have no differenceList - return merged common list', () => {
            const param = {};
            const lrfResponse = { data: { DataList: defaultLrfPlayerList, TotalItem: 2 } };
            const lapResponse = { data: { PlayerList: defaultLapPlayerList, TotalItem: 2 } };
            const lrfPromise = Promise.resolve(lrfResponse);
            const lapPromise = Promise.resolve(lapResponse);

            const commonService = {...defaultCommonService };

            const expectedMergedPlayerList = [
                { IbcCustId: 1, BaStatus: true, OddsGroup: 'A', Status: 0 },
                { IbcCustId: 2, BaStatus: false, OddsGroup: 'B', Status: 1 }
            ];
            PlayerListDataHelper.mergePlayerList = jest.fn(() => expectedMergedPlayerList);
            const expectedResult = {
                playerList: expectedMergedPlayerList,
                totalItem: 2
            }

            PlayerMultipleQueryService._getResult(param, lrfPromise, lapPromise, commonService, apiInfo);
            const result = handler(lrfResponse, lapResponse);

            expect(result).toEqual(expectedResult);
        })

        it('data response from Lap has different items - should call _refetchFromLrf with correct parameter', () => {
            const param = {...defaultParam };
            const lrfPlayerList = [...defaultLrfPlayerList];
            const lapPlayerList = [...defaultLapPlayerList, ...defaultLapDifferenceList];

            const functionName = '_refetchFromLrf';
            const checkResult = () => {
                const expectedLrfParam = {
                    currencyIdList: param.currencyIdList,
                    idList: '4',
                    specialLevel: param.specialLevel,
                    pageSize: param.pageSize,
                    pageIndex: param.pageIndex
                }

                const expectedLapDataObject = {
                    commonList: [...defaultLapPlayerList],
                    differenceList: [...defaultLapDifferenceList]
                }

                utils.getCustIdListFromLapData = jest.fn((dataList) => {
                    if (dataList === expectedLapDataObject.differenceList) {
                        return defaultLapDifferenceList.map(x => x.IBCCustId);
                    }

                    return [];
                })

                expect(PlayerMultipleQueryService[functionName]).toHaveBeenCalledWith(
                    expectedLrfParam,
                    expectedLapDataObject,
                    defaultCommonService,
                    apiInfo);
            }

            runTestCallingCorrectFunction(param, lrfPlayerList, lapPlayerList, functionName, checkResult);
        })
    })

    describe('_refetchFromLrf', () => {
        const runTestForReturningEmptyResult = (commonService) => {
            const param = {};
            const lapDataObject = {
                commonList: [],
                differenceList: [...defaultLapDifferenceList]
            }

            const expectedResult = {
                playerList: []
            }

            const promise = PlayerMultipleQueryService._refetchFromLrf(param, lapDataObject, commonService, apiInfo);

            return Promise.resolve(promise).then((result) => {
                expect(result).toEqual(expectedResult);
            })
        }

        it('data response is empty - return lap list data', () => {
            const commonService = {
                fetchData: jest.fn(() => Promise.resolve({ data: { DataList: [], TotalItem: 0 } }))
            }

            return runTestForReturningEmptyResult(commonService);
        })

        it('throw exception - should return lap list data', () => {
            const commonService = {
                fetchData: jest.fn(() => new Promise(() => {
                    throw new Error('Network Error')
                }))
            }

            return runTestForReturningEmptyResult(commonService);
        })

        it('has data list response - return correct data', () => {
            let originalMergePlayerList = PlayerListDataHelper.mergePlayerList;
            const param = {};
            const lapDataObject = {
                commonList: [],
                differenceList: [...defaultLapDifferenceList]
            }
            const expectedDataList = [{ IbcCustId: 4, UserName: 'test4' }];
            const commonService = {
                fetchData: jest.fn(() => Promise.resolve({ data: { DataList: expectedDataList } }))
            }

            const expectedPlayerList = [{ UserId: 1 }];
            PlayerListDataHelper.mergePlayerList = jest.fn((lrfPlayerList, differenceList) => {
                return lrfPlayerList === expectedDataList ? expectedPlayerList : null;
            });

            const promise = PlayerMultipleQueryService._refetchFromLrf(param, lapDataObject, commonService, apiInfo);

            return Promise.resolve(promise).then((result) => {
                expect(result.playerList).toBe(expectedPlayerList);

                PlayerListDataHelper.mergePlayerList = originalMergePlayerList;
            })
        })
    })
})