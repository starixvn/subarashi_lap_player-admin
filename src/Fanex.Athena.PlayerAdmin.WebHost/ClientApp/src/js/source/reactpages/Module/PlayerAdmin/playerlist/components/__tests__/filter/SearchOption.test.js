import React from 'react';
import { shallow } from 'enzyme';

import SearchOption from '../../filter/SearchOption';

describe('SearchOption', () => {
    it('with initial state - should match snapshot', () => {
        const wrapper = shallow(<SearchOption />);

        expect(wrapper).toMatchSnapshot();
    })

    it('handleToggleDropdown - isOpen should be changed - match snapshot', () => {
        const wrapper = shallow(<SearchOption />);
        expect(wrapper).toMatchSnapshot();

        const props = wrapper.props();

        props.onToggle();
        expect(wrapper).toMatchSnapshot();
    })

    it('handleClose - isOpen should be false - match snapshot', () => {
        const wrapper = shallow(<SearchOption />);
        const props = wrapper.props();

        props.onToggle();
        expect(wrapper).toMatchSnapshot();

        props.onClose();
        expect(wrapper).toMatchSnapshot();
    })
})