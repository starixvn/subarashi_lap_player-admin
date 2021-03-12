import React from 'react';
import { shallow } from 'enzyme';

import { PlayerDetail } from '../PlayerDetail';
import { heightSlicing } from '../../lib/constants';

describe('PlayerDetail', () => {
    const defaultTabList = [{ Action: 'test1/index', SystemType: 0, Name: 'Tab1' }, { Action: 'test2/index', SystemType: 1, Name: 'Tab2' }];
    const defaultProps = {
        selectedPlayer: null,
        isAdvancedSearch: false,
        isFilterCollapsing: false,
        selectedTabName: null,
        getLink: jest.fn(),
        getTabList: jest.fn(() => [...defaultTabList]),
        setSelectedTabName: jest.fn()
    }

    it('constructor - initial state should be correct', () => {
        const expectedTabList = [...defaultTabList];
        const props = {
            ...defaultProps
        }

        const instance = new PlayerDetail(props);

        expect(instance._tabList).toEqual(expectedTabList);
    })

    it('_getDefaultSelectedTab - should return name of first item', () => {
        const instance = new PlayerDetail({ ...defaultProps });

        const result = instance._getDefaultSelectedTab();

        expect(result).toBe('Tab1');
    })

    it('handleClickMenu - setSelectedTabName should be called with correct parameters', () => {
        const props = {
            ...defaultProps,
            setSelectedTabName: jest.fn()
        }
        const instance = new PlayerDetail({ ...props });
        const event = {
            preventDefault: () => { }
        }
        const tabName = 'Tab1';

        instance.handleClickMenu(event, tabName);

        expect(props.setSelectedTabName).toHaveBeenCalledWith(tabName);
    })

    it('_buildActionUrl - props.getLink should be called with correct parameters', () => {
        const createdDate = '11/06/2008 10:35:11 AM';
        const runTest = (action, expectedUrl) => {
            const props = {
                ...defaultProps,
                selectedPlayer: {
                    ibcCustId: 1, 
                    licenseeCustId: 11, 
                    currencyId: 20, 
                    currency: 'UUS', 
                    userName: 'test',
                    createdDate
                },
                getLink: jest.fn()
            }

            const instance = new PlayerDetail(props);
            const systemType = 1;

            instance._buildActionUrl(systemType, action);

            expect(props.getLink).toHaveBeenCalledWith(systemType, expectedUrl);
        }

        const encodedCreatedDate = encodeURIComponent(createdDate);
        runTest('test/index', 'test/index?custId=1&licenseeCustId=11&currencyId=20&currencyName=UUS&userName=test');
        runTest('test/index?param1=value1', 'test/index?param1=value1&custId=1&licenseeCustId=11&currencyId=20&currencyName=UUS&userName=test');
    })

    it('_renderTabList - should match snapshot', () => {
        const runTest = (tabName) => {
            const props = {
                ...defaultProps,
                selectedPlayer: {
                    ibcCustId: 1, 
                    licenseeCustId: 11, 
                    currencyId: 20, 
                    currency: 'UUS', 
                    userName: 'test',
                    isDeposit: true,
                    isArchived: false
                },
                selectedTabName: tabName
            }
            const instance = new PlayerDetail({ ...props });
            instance._getDefaultSelectedTab = jest.fn(() => 'DefaultTab');
            const tabList = [
                { SystemType: 0, Action: 'test1/action1', Name: 'test1', Text: 'Test 1' },
                { SystemType: 1, Action: 'test1/action2', Name: 'test2', Text: 'Test 2' },
                { SystemType: 0, Action: 'test2/action2', Name: 'test3', Text: 'Test 3' }
            ]
            instance._buildActionUrl = jest.fn((systemType, action) => {
                return `${action}?systemType=${systemType}`;
            })

            const tabListContent = instance._renderTabList(tabList);
            const wrapper = shallow(<div>{tabListContent}</div>);

            expect(wrapper).toMatchSnapshot();
        }

        ['Tab2', null].forEach(tabName => runTest(tabName));
    })

    describe('_getFrameHeight', () => {
        const runTest = ({ props, omittedHeight }) => {
            const spec = `isFilterCollapsing: ${props.isFilterCollapsing} - isAdvancedSearch: ${props.isAdvancedSearch}  - should get correct omitted height`;
            it(spec, () => {
                const instance = new PlayerDetail({ ...props });

                const expected = window.innerHeight - omittedHeight;

                const result = instance._getFrameHeight();

                expect(result).toBe(expected);
            })
        }

        const testCaseList = [
            { props: { ...defaultProps, isFilterCollapsing: true }, omittedHeight: heightSlicing.whenCollapsing },
            { props: { ...defaultProps, isFilterCollapsing: false, isAdvancedSearch: true }, omittedHeight: heightSlicing.withAdvancedSearch },
            { props: { ...defaultProps, isFilterCollapsing: false, isAdvancedSearch: false }, omittedHeight: heightSlicing.default },
        ]

        testCaseList.forEach(runTest);
    })

    describe('_getActionUrl - return correct action url', () => {
        const runTest = ({ selectedTabName, expectedTabItem }) => {
            it('selectedTabName has value - should get correct tab item', () => {
                const props = {
                    ...defaultProps,
                    selectedTabName
                }

                const instance = new PlayerDetail(props);
                instance._buildActionUrl = jest.fn();

                instance._getActionUrl();

                expect(instance._buildActionUrl).toHaveBeenCalledWith(expectedTabItem.SystemType, expectedTabItem.Action);
            })

            const testCaseList = [
                { selectedTabName: defaultTabList[1].Name, expectedTabItem: defaultTabList[1] },
                { selectedTabName: null, expectedTabItem: defaultTabList[0] }
            ]

            testCaseList.forEach(runTest);
        }

    })

    describe('render', () => {
        it('selectedPlayer is null - return null', () => {
            const instance = new PlayerDetail({ ...defaultProps });

            const result = instance.render();

            expect(result).toBeNull();
        })

        it('tabList is empty - return null', () => {
            const props = {
                ...defaultProps,
                selectedPlayer: {},
                getTabList: jest.fn(() => [])
            }
            const instance = new PlayerDetail(props);

            const result = instance.render();

            expect(result).toBeNull();
        })

        it('_getActionUrl return null - return null', () => {
            const props = {
                ...defaultProps,
                selectedPlayer: {custId: 1}
            }

            const instance = new PlayerDetail(props);
            instance._getActionUrl = jest.fn(() => null);

            const result = instance.render();

            expect(result).toBeNull();
        })

        it('data is correct - match snapshot', () => {
            const props = {
                ...defaultProps,
                selectedPlayer: {custId: 1}
            }

            const instance = new PlayerDetail(props);
            instance._getActionUrl = jest.fn(() => null);
            instance._renderTabList = jest.fn(() => 'This is tab list');
            instance._getFrameHeight = jest.fn(() => 500);

            const renderedContent = instance.render();
            const wrapper = shallow(<div className="wrapper">{renderedContent}</div>);

            expect(wrapper).toMatchSnapshot();
        })
    })
})