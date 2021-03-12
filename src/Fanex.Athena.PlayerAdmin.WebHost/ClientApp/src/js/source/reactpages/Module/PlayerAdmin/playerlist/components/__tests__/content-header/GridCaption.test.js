import React from 'react';
import { shallow } from 'enzyme';

import { GridCaption } from '../../content-header/GridCaption';

jest.mock('../../../lib/reportExcel');

describe('GridCaption', () => {
    const defaultProps = {
        playerCount: 0,
        selectedPlayer: null,
        selectedPlayerIdList: [],
        displayedColumnList: [],
        type: 0,
        param: {},
        setSelectedPlayer: undefined,
        setDisplayedColumnList: undefined,
        exportExcel: undefined
    };

    it('handleChangeDisplayedColumn - setDisplayedColumnList should be called with correct parameter', () => {
        const props = {
            ...defaultProps,
            displayedColumnList: [
                { key: 'Status', value: 'Status', show: true },
                { key: 'Username', value: 'Username', show: true }
            ],
            setDisplayedColumnList: jest.fn()
        }

        const expectedColumnList = [
            { key: 'Status', value: 'Status', show: true },
            { key: 'Username', value: 'Username', show: false }
        ];
        const instance = new GridCaption({ ...props });

        instance.handleChangeDisplayedColumn({}, 'Status');

        expect(props.setDisplayedColumnList).toHaveBeenCalledWith(expectedColumnList);

    })

    it('_buildDisplayedColumnList - should match snapshot', () => {
        const props = {
            ...defaultProps,
            displayedColumnList: [
                { key: 'Status', value: 'Status', show: true },
                { key: 'Username', value: 'Username', show: true }
            ]
        }
        const instance = new GridCaption({ ...props });
        const dropdownColumn = instance._buildDisplayedColumnList();

        const wrapper = shallow(<div>{dropdownColumn}</div>);

        expect(wrapper).toMatchSnapshot();
    })

    describe('_buildLeftPart', () => {
        const runTest = ({ desciption, props }) => {
            it(desciption, () => {
                const instance = new GridCaption({ ...props });
                const leftPart = instance._buildLeftPart();

                const wrapper = shallow(<div>{leftPart}</div>);

                expect(wrapper.getElement()).toMatchSnapshot();
            })
        }

        const testCaseList = [
            {
                desciption: 'selectedPlayer is not null - should render correct breadcrumb information',
                props: {
                    ...defaultProps,
                    selectedPlayer: {
                        userName: 'test',
                        currency: 'USD'
                    }
                }
            },
            // {
            //     desciption: 'selectedPlayer is null and selectedPlayerIdList has value - should render buttons for batch update',
            //     props: {
            //         ...defaultProps,
            //         selectedPlayerIdList: [1, 2]
            //     }
            // },
            {
                desciption: 'have not selected any item yet - should render player count information',
                props: {
                    ...defaultProps,
                    playerList: [{}, {}, {}]
                }
            }
        ]

        testCaseList.forEach(runTest);
    })

    it('_buildRightPart - should render correctly with dropdown column and excel icon', () => {
        const instance = new GridCaption({ ...defaultProps });
        instance._buildDropdownColumn = jest.fn(() => 'Dropdown Column List');
        const rightPart = instance._buildRightPart();
        const wrapper = shallow(<div>{rightPart}</div>);

        expect(wrapper).toMatchSnapshot();
    })

    it('handleExportExcel - exportExcel action should be called with correct parameter', () => {
        const type = 1;
        const param = {
            userName: 'test',
        }
        const props = {
            ...defaultProps,
            type,
            param,
            exportExcel: jest.fn(),
            playerList: [{userName: 'test1'}, {userName: 'test2'}],
            displayedColumnList: [{key: 'userName', show: true}]
        }

        const displayedColumn = {
            userName: true
        }

        const instance = new GridCaption({ ...props });

        instance.handleExportExcel();

        expect(props.exportExcel).toHaveBeenCalledWith(props.playerList, param, displayedColumn);
    })

    it('render - should match snapshot', () => {
        const instance = new GridCaption({ ...defaultProps });
        instance._buildLeftPart = jest.fn(() => "Left Part");
        instance._buildRightPart = jest.fn(() => "Right Part");

        const wrapper = shallow(<div>{instance.render()}</div>);

        expect(wrapper).toMatchSnapshot();
    })
})