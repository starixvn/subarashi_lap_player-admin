import React from 'react';
import { shallow } from 'enzyme';

import { DataGrid } from '../DataGrid';
import PlayerRow from '../grid/PlayerRow';
import GridHelper from '../../lib/helpers/gridHelper';
import GridInteractionService from '../../lib/services/gridInteractionService';

describe('DataGrid', () => {
    let originalRenderHeader;
    let originalSortPlayerList;

    beforeAll(() => {
        originalRenderHeader = GridHelper.renderHeader;
        originalSortPlayerList = GridInteractionService.sortPlayerList;
    })
    afterAll(() => {
        GridHelper.renderHeader = originalRenderHeader;
        GridInteractionService.sortPlayerList = originalSortPlayerList;
    })
    const defaultProps = {
        playerList: null,
        selectedPlayer: null,
        selectedPlayerIdList: [],
        displayedColumnList: [
            { key: 'Status', value: 'Status', show: true },
            { key: 'Username', value: 'Username', show: true },
            { key: 'SystemID', value: 'SystemID', show: true }
        ],
        currentActivePlayer: null,
        setSelectedPlayerIdList: jest.fn(),
        setPlayerList: jest.fn(),
        setCurrentActivePlayer: jest.fn()
    }

    let originalClientWidth;
    beforeEach(() => {
        originalClientWidth = Math.min(document.body.clientWidth, 1920);
    })

    it('constructor - initial state should be correct', () => {
        const instance = new DataGrid({ ...defaultProps });

        expect(instance.maxWidth).toBe(originalClientWidth);
    })

    it('_renderDataSection - row list should be correct', () => {
        const props = { ...defaultProps };
        const instance = new DataGrid({ ...props });
        const dataList = [
            { CustId: 1, LicenseeCustId: 11, userName: 'test1' },
            { CustId: 2, LicenseeCustId: 22, userName: 'test2' }
        ];

        const displayedColumn = {
            Status: true,
            Username: true,
            SystemID: true
        }

        const firstRowProps = {
            dataItem: dataList[0],
            displayedColumn
        }
        const secondRowProps = {
            dataItem: dataList[1],
            displayedColumn
        }
        const expectedResult = [
            <PlayerRow key={11} {...firstRowProps} />,
            <PlayerRow key={22} {...secondRowProps} />
        ];

        const result = instance._renderDataSection(dataList, displayedColumn);

        expect(result).toEqual(expectedResult);
    })

    it('handleSort - playerList should be sorted', () => {
        const props = {
            ...defaultProps,
            playerList: [
                { CustId: 1, UserName: 'test1', Currency: 'USD' },
                { CustId: 2, UserName: 'test2', Currency: 'USD' }
            ],
            setPlayerList: jest.fn()
        }
        const expectedSortedPlayerList = [
            { CustId: 2, UserName: 'test2', Currency: 'USD' },
            { CustId: 1, UserName: 'test1', Currency: 'USD' }
        ];
        GridInteractionService.sortPlayerList = jest.fn(() => expectedSortedPlayerList);

        const sortBy = 'UserName';
        const sortOrder = 'desc';
        const instance = new DataGrid(props);

        instance.handleSort(sortBy, sortOrder);

        expect(GridInteractionService.sortPlayerList).toHaveBeenCalledWith(props.playerList, sortBy, sortOrder);
        expect(props.setPlayerList).toHaveBeenCalledWith(expectedSortedPlayerList);
    })

    describe('render', () => {
        const renderProps = {
            ...defaultProps,
            playerList: [
                { CustId: 1, UserName: 'test1', Currency: 'USD' },
                { CustId: 2, UserName: 'test2', Currency: 'USD' }
            ],
            displayedColumnList: [{key: 'UserName', show: true}, {key: 'SystemId', show: true}]
        };

        it('selectedPlayer has value - should render only one row', () => {
            const props = {
                ...renderProps,
                selectedPlayer: {
                    custId: 1,
                    userName: 'test1',
                    currency: 'USD'
                }
            };

            const instance = new DataGrid(props);
            GridHelper.renderHeader = jest.fn(() => 'Header');

            let dataList;
            instance._renderDataSection = jest.fn((data) => {
                dataList = data;
                return 'Data List';
            });

            const renderedResult = instance.render();
            const wrapper = shallow(<div>{renderedResult}</div>).getElement();

            expect(dataList.length).toBe(1);
            expect(wrapper).toMatchSnapshot();
        })

        it('selectedPlayer has no value - should render all items in dataList', () => {
            const props = {
                ...renderProps
            };

            const instance = new DataGrid(props);
            GridHelper.renderHeader = jest.fn(() => 'Header');

            let dataList;
            instance._renderDataSection = jest.fn((data) => {
                dataList = data;
                return 'Data List';
            });

            const renderedResult = instance.render();
            const wrapper = shallow(<div>{renderedResult}</div>).getElement();

            expect(dataList).toEqual(renderProps.playerList);
            expect(wrapper).toMatchSnapshot();
        })
    })
})