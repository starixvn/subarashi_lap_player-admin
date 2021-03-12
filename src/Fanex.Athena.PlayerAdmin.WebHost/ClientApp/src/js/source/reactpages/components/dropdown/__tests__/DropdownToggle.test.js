import React from 'react';
import Renderer from 'react-test-renderer';

import DropdownToggle from '../DropdownToggle';

describe('DropdownToggle', () => {
    const defaultProps = {
        caret: true,
        children: undefined,
        className: undefined,
        disabled: undefined,
        onClick: undefined,
        tag: 'button'
    }

    describe('render', () => {
        it('with default props - render correctly', () => {
            const component = Renderer.create(
                <DropdownToggle {...defaultProps}>Toggle Button</DropdownToggle>);

            expect(component.toJSON()).toMatchSnapshot();
        })

        it('with specific props - render correctly', () => {
            const props = {
                caret: false,
                className: 'custom-toggle',
                disabled: true,
                onClick: jest.fn(),
                tag: 'div'
            }

            const component = Renderer.create(
                <DropdownToggle {...props}>Toggle Label</DropdownToggle>);

            expect(component.toJSON()).toMatchSnapshot();
        })
    })
})