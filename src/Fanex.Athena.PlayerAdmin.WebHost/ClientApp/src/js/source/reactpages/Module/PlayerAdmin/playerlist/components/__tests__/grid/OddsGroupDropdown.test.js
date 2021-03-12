import React from 'react';
import { shallow } from 'enzyme';
import { OddsGroupDropdown } from '../../grid/OddsGroupDropdown';
import { DropdownItem } from '@jsroot/components/dropdown';

describe('OddsGroupDropdown', () => {
    const defaultProps = {
        oddsGroupDropdownPosition: {top: 100, left: 100},
        currentActivePlayer: {oddsGroup: 'A', ibcCustId: 1, licenseeCustId: 11},
        playerList: [{oddsGroup: 'A', ibcCustId: 1, licenseeCustId: 11}],

        setOddsGroupDropdownPosition: jest.fn(),
        updateOddsGroup: jest.fn(),
        setPlayerList: jest.fn(),
        toggleLoader: jest.fn(),
        setSweetAlertOption: jest.fn()
    }

    describe('render', () => {
        const runTest = (position) => {
            const isOpen = !!position;
            const spec = `isOpen: ${isOpen} - should match snapshot`;
            it(spec, () => {
                const props = {
                    ...defaultProps,
                    position
                }
                const wrapper = shallow(<OddsGroupDropdown {...props} />);

                expect(wrapper).toMatchSnapshot();
            })
        }

        [null, { x: 10, y: 50 }].forEach(runTest);
    })

    it('click on menu item - props.onChange should be called with correct parameter', () => {
        const props = {
            ...defaultProps,
            onChange: jest.fn(),
            updateOddsGroup: jest.fn(() => {
                return{
                    then: jest.fn(),
                    catch: jest.fn()
                }
            })
        }
        const wrapper = shallow(<OddsGroupDropdown {...props} />);
        const dropdownItem = wrapper.find(DropdownItem).get(0);

        const event = {preventDefault: jest.fn()};
        dropdownItem.props.onClick(event);

        expect(props.setSweetAlertOption).toHaveBeenCalled();
    })
})