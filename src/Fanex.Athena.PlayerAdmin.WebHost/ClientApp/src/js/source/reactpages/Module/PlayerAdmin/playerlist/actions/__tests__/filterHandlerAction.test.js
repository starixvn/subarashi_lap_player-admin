import * as filterHandlerAction from '../filterHandlerAction';
import {
    SET_PARAM,
    SET_PLAYER_LIST,
    SET_SELECTED_PLAYER,
    SET_PAGING,
    SET_IS_FETCHING_DATA,
    SET_SELECTED_TAB_NAME
} from '../types';
import { TOGGLE_LOADER } from '@jsroot/actions/types'

jest.mock('../../lib/services/playerQueryService', () => {
    return jest.fn().mockImplementation(() => {
        return {
            getPlayerList: jest.fn((param) => {
                const playerListByCaseId = {
                    '1': {
                        playerList: [{
                            IbcCustId: 1,
                            UserName: 'test1',
                            Currency: 'UUS',
                            IsDeposit: true,
                            IsArchived: false,
                            LicenseeCustId: 11,
                            CurrencyId: 20
                        }, {
                            IbcCustId: 2,
                            UserName: 'test2',
                            Currency: 'UUS',
                            IsDeposit: true,
                            IsArchived: false,
                            LicenseeCustId: 22,
                            CurrencyId: 20
                        }],
                        totalItem: 2
                    },
                    '2': {
                        playerList: [],
                        totalItem: 0
                    },
                    '3': {
                        playerList: [{
                            IbcCustId: 1,
                            UserName: 'test1',
                            Currency: 'UUS',
                            IsDeposit: true,
                            IsArchived: false,
                            LicenseeCustId: 11,
                            CurrencyId: 20
                        }],
                        totalItem: 1
                    }
                }

                const response = playerListByCaseId[param.caseId];
                return Promise.resolve(response);
            })
        };
    })
});

describe('filterHandlerAction', () => {
    it('setParam - return correct data', () => {
        const data = {
            userName: 'test',
            currency: 'USD'
        };
        const result = filterHandlerAction.setParam(data);
        const expectedResult = {
            type: SET_PARAM,
            data
        };

        expect(result).toEqual(expectedResult);
    })

    describe('searchPlayer', () => {
        it('should dispatch necessary actions first', (done) => {
            const mockDispatch = jest.fn();
            const param = {
                caseId: 1,
                userName: 'test',
                currency: 'USD'
            };
            filterHandlerAction.searchPlayer(param)(mockDispatch);

            expect(mockDispatch.mock.calls[0][0]).toEqual({ type: SET_SELECTED_PLAYER, data: null });
            expect(mockDispatch.mock.calls[1][0]).toEqual({ type: SET_IS_FETCHING_DATA, data: true });
            expect(mockDispatch.mock.calls[2][0]).toEqual({ type: TOGGLE_LOADER, data: true });
            expect(mockDispatch.mock.calls[3][0]).toEqual({ type: SET_SELECTED_TAB_NAME, data: null });
            expect(mockDispatch.mock.calls[4][0]).toEqual({ type: SET_PARAM, data: param });
            done();
        })

        it('call ajax request successfully - should dispatch actions correctly', (done) => {
            const playerList = [{
                IbcCustId: 1,
                UserName: 'test1',
                Currency: 'UUS',
                IsDeposit: true,
                IsArchived: false,
                LicenseeCustId: 11,
                CurrencyId: 20
            }, {
                IbcCustId: 2,
                UserName: 'test2',
                Currency: 'UUS',
                IsDeposit: true,
                IsArchived: false,
                LicenseeCustId: 22,
                CurrencyId: 20
            }];
            const expectedSetPlayerListParam = {
                type: SET_PLAYER_LIST,
                data: playerList
            }

            const mockDispatch = jest.fn();
            const param = {
                caseId: 1,
                userName: 'test',
                currency: 'USD',
                pageIndex: 1,
                pageSize: 100
            };

            const expectedSetPagingOption = {
                type: SET_PAGING,
                data: {
                    PageIndex: param.pageIndex,
                    PageSize: param.pageSize,
                    TotalItem: 2,
                    PageList: [50]
                }
            }

            filterHandlerAction.searchPlayer(param)(mockDispatch);

            Promise.resolve().then(() => {
                expect(mockDispatch).toHaveBeenCalledTimes(9);
                expect(mockDispatch.mock.calls[5][0]).toEqual(expectedSetPlayerListParam);
                expect(mockDispatch.mock.calls[6][0]).toEqual({ type: SET_IS_FETCHING_DATA, data: false });
                expect(mockDispatch.mock.calls[7][0]).toEqual(expectedSetPagingOption);
                expect(mockDispatch.mock.calls[8][0]).toEqual({ type: TOGGLE_LOADER, data: false });
                done();
            })
        })

        it('playerList has 1 item - should dispatch setSelectedPlayer action correctly', (done) => {
            const expectedSetPlayerListParam = {
                type: SET_SELECTED_PLAYER,
                data: {
                    ibcCustId: 1,
                    licenseeCustId: 11,
                    userName: 'test1',
                    currency: 'UUS',
                    isDeposit: true,
                    isArchived: false,
                    currencyId: 20
                }
            }

            const mockDispatch = jest.fn();
            const param = {
                caseId: 3,
                userName: 'test',
                currency: 'USD'
            };
            filterHandlerAction.searchPlayer(param)(mockDispatch);

            Promise.resolve().then(() => {
                expect(mockDispatch).toHaveBeenCalledTimes(10);
                expect(mockDispatch.mock.calls[8][0]).toEqual(expectedSetPlayerListParam);
                done();
            })
        })
    })
})