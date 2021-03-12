import React from 'react';
import { shallow } from 'enzyme';
import { FormFilter } from '../FormFilter';
import { searchOptions, defaultPagingOption, licenseeType, baStatusList } from '../../lib/constants';
import LangHelper from '@jsroot/lib/langHelper';
import QueryString from '@jsroot/lib/QueryString';

describe('FormFilter', () => {
    let originalGetParam;
    beforeAll(() => {
        originalGetParam = QueryString.getParam;
    })
    afterAll(() => {
        QueryString.getParam = originalGetParam;
    })

    const defaultProps = {
        setAdvancedSearch: jest.fn(),
        searchPlayer: jest.fn(),
        setCollapseFilter: jest.fn(),
        getSuggestionList: jest.fn(),
        getSinglePlayer: jest.fn(),
        getSiteSetting: jest.fn(),

        type: licenseeType.nonApi,
        isAdvancedSearch: false,
        isFilterCollapsing: false,
        pagingOption: { ...defaultPagingOption },
        currencyList: [{ Id: 20, Name: 'UUS' }, { Id: 9, Name: 'AUD' }],
        siteSetting: { isAllowedBA: true },
        isSubsidiary: false
    }
    const allOddsGroupString = 'A,B,C,D,E,F';
    const allCurrencyString = '20,9';
    const allSpecialLevel = '0,1,2,3,4,5,6,7,8,9,10';

    it('constructor initial state should be correct', () => {
        const instance = new FormFilter({ ...defaultProps });
        const expectedState = {
            isValidUserName: false,
            bankAccountNumber: '',
            email: '',
            currencyIdList: allCurrencyString,
            oddsGroup: allOddsGroupString,
            specialLevel: allSpecialLevel,
            baFilter: baStatusList.All,
            searchOption: searchOptions.matchCase
        }

        const result = instance.state;

        expect(result).toEqual(expectedState);
        expect(instance.userNameRef).not.toBeNull();
    })

    describe('componentDidMount', () => {
        it('getSiteSetting should be called with correct parameter', () => {
            QueryString.getParam = jest.fn((url, name) => 0);

            const props = { ...defaultProps, getSiteSetting: jest.fn() };

            const instance = new FormFilter(props);
            instance.componentDidMount();

            expect(props.getSiteSetting).toHaveBeenCalledWith(props.type, props.isSubsidiary);
        })

        it('has query string about customer info - getSinglePlayer and setState should be called', () => {
            QueryString.getParam = jest.fn((url, name) => {
                if (name === 'custId') {
                    return 1;
                }
                else if (name === 'licenseeCustId') {
                    return 11;
                }
                else if (name === 'userName') {
                    return 'test';
                }
                else {
                    return '';
                }
            });

            const props = { ...defaultProps, getSinglePlayer: jest.fn() };
            const expectedParam = {
                ibcCustId: 1,
                licenseeCustId: 11
            }

            const instance = new FormFilter(props);
            instance.userNameRef.current = {};
            instance.setState = jest.fn();
            instance.componentDidMount();

            expect(props.getSinglePlayer).toHaveBeenCalledWith(expectedParam);
            expect(instance.setState).toHaveBeenCalledWith({ isValidUserName: true });
        })
    })

    it('componentDidUpdate - advanced search is hidding - reset params in advanced search', () => {
        const prevProps = {
            isAdvancedSearch: true
        }
        const props = { ...defaultProps, isAdvancedSearch: false };
        const expectedState = {
            currencyIdList: allCurrencyString,
            oddsGroup: allOddsGroupString,
            specialLevel: allSpecialLevel,
            baFilter: baStatusList.All
        }

        const instance = new FormFilter(props);
        instance.setState = jest.fn();
        instance.componentDidUpdate(prevProps);

        expect(instance.setState).toHaveBeenCalledWith(expectedState);
    })

    describe('searchPlayer', () => {
        it('invalid inputs - do nothing', () => {
            window.alert = jest.fn();
            const runTest = (currentState, userName) => {
                const props = { ...defaultProps, searchPlayer: jest.fn() };
                const instance = new FormFilter(props);
                instance.state = currentState;
                instance.userNameRef.current = { value: userName };

                expect(props.searchPlayer).not.toHaveBeenCalled();
            }

            runTest({ bankAccountNumber: '', email: '' }, '');
            runTest({ bankAccountNumber: '', email: 'aa.com' }, 'aa');
        })

        it('valid input - searchPlayer should be called with correct parameter', () => {
            const runTest = (state, userName, expectedParam) => {
                const props = { ...defaultProps, searchPlayer: jest.fn() };
                const instance = new FormFilter(props);
                instance.userNameRef.current = { value: userName };
                instance.state = state

                instance.searchPlayer();

                expect(props.searchPlayer).toHaveBeenCalledWith(expectedParam);
            }

            const state1 = {
                isValidUserName: true,
                bankAccountNumber: '',
                email: '',
                currencyIdList: allCurrencyString,
                oddsGroup: allOddsGroupString,
                specialLevel: allSpecialLevel,
                baFilter: baStatusList.All,
                searchOption: searchOptions.matchCase
            };
            const expectedParam1 = {
                bankAccountNumber: '',
                email: '',
                baFilter: baStatusList.All,
                searchOption: searchOptions.matchCase,
                userName: 'test',
                currencyIdList: '',
                oddsGroup: '',
                specialLevel: allSpecialLevel,
                pageSize: 50,
                pageIndex: 1
            }
            runTest(state1, 'test', expectedParam1);

            const state2 = {
                isValidUserName: true,
                bankAccountNumber: '',
                email: '',
                currencyIdList: '20',
                oddsGroup: 'A,B',
                specialLevel: '0,1',
                baFilter: baStatusList.All,
                searchOption: searchOptions.matchCase
            };
            const expectedParam2 = {
                bankAccountNumber: '',
                email: '',
                baFilter: baStatusList.All,
                searchOption: searchOptions.matchCase,
                userName: '&lt;test',
                currencyIdList: '20',
                oddsGroup: 'A,B',
                specialLevel: '0,1',
                pageSize: 50,
                pageIndex: 1
            }
            runTest(state2, '<test', expectedParam2);
        })
    })

    it('handleSubmit - should call searchPlayer function', () => {
        const props = { ...defaultProps };
        const instance = new FormFilter(props);

        instance.searchPlayer = jest.fn();

        const event = {
            preventDefault: jest.fn()
        }

        instance.handleSubmit(event);

        expect(instance.searchPlayer).toHaveBeenCalled();
    })

    it('handlePressEnter - should call searchPlayer function when submitting with enter', () => {
        const props = { ...defaultProps };
        const instance = new FormFilter(props);

        instance.searchPlayer = jest.fn();

        const event = {
            charCode: 13
        }

        instance.handlePressEnter(event);

        expect(instance.searchPlayer).toHaveBeenCalled();
    })

    it('handleChange - setState should be called with correct parameter', () => {
        const instance = new FormFilter({ ...defaultProps });
        const event = {
            target: {
                name: 'userName',
                value: 'test1'
            }
        }

        instance.setState = jest.fn();
        const expectedStateParam = {
            userName: 'test1'
        }

        instance.handleChange(event);

        expect(instance.setState).toHaveBeenCalledWith(expectedStateParam);
    })

    it('handleSpecificInputChange - setState should be called with correct parameter', () => {
        const instance = new FormFilter({ ...defaultProps });
        instance.setState = jest.fn();
        const expectedStateParam = {
            email: 'test1'
        }

        instance.handleSpecificInputChange('email', 'test1');

        expect(instance.setState).toHaveBeenCalledWith(expectedStateParam);
    })

    it('handleUserNameChange - should call setState for isValidUserName', () => {
        const instance = new FormFilter({ ...defaultProps });
        instance.userNameRef.current = {};
        instance.setState = jest.fn();

        instance.handleUserNameChange('test');

        expect(instance.setState).toHaveBeenCalledWith({isValidUserName: true});
    })

    it('_renderUserGuide - should match snapshot', () => {
        LangHelper.getSingleResource = jest.fn((name) => name);
        const instance = new FormFilter({ ...defaultProps });
        const userGuide = instance._renderUserGuide();

        const result = shallow(<div>{userGuide}</div>);

        expect(result).toMatchSnapshot();
    })

    describe('render', () => {
        it('match snapshot', () => {
            const instance = new FormFilter({ ...defaultProps });
            instance.userNameRef.current = {value: 'test'};
            const renderedContent = instance.render();
            const wrapper = shallow(<div>{renderedContent}</div>);

            expect(wrapper).toMatchSnapshot();
        })
    })
})