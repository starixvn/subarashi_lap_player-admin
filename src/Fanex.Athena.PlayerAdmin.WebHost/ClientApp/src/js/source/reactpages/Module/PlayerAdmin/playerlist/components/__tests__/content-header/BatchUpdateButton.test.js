import React from 'react';
import { shallow } from 'enzyme';
import BatchUpdateButton from '../../content-header/BatchUpdateButton';

describe('BatchUpdateButton', () => {
    describe('render', () => {
        it('render correctly with default state', () => {
            const wrapper = shallow( < BatchUpdateButton / > );

            expect(wrapper).toMatchSnapshot();
        })

        it('render correctly when toggle isOpen', () => {
            const wrapper = shallow( < BatchUpdateButton / > );
            expect(wrapper).toMatchSnapshot();

            const props = wrapper.props();
            props.onToggle();

            expect(wrapper).toMatchSnapshot();

            props.onClose();
            expect(wrapper).toMatchSnapshot();
        })
    })
})