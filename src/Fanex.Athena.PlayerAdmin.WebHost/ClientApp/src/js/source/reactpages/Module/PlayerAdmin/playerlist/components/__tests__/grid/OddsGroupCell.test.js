import React from 'react';
import { shallow } from 'enzyme';
import { OddsGroupCell } from '../../grid/OddsGroupCell';
import GridInteractionService from '../../../lib/services/gridInteractionService';

describe('OddsGroupCell', () => {
    let originalGetNextDropdownPosition;
    beforeAll(() => {
        originalGetNextDropdownPosition = GridInteractionService.getNextDropdownPosition;
        GridInteractionService.getNextDropdownPosition = jest.fn();
    })
    afterAll(() => {
        GridInteractionService.getNextDropdownPosition = originalGetNextDropdownPosition;
    })
    const defaultProps = {
        oddsGroupDropdownPosition: {top: 100, left: 100},
        dataItem: {OddsGroup: 'A', BaStatus: true},
        hasOddsGroupPermission: true, 
        setCurrentActivePlayer: jest.fn(),
        setOddsGroupDropdownPosition: jest.fn()
    }

    it('should match snapshot', () => {
        const wrapper = shallow(<OddsGroupCell {...defaultProps} />);

        expect(wrapper).toMatchSnapshot();
    })
})