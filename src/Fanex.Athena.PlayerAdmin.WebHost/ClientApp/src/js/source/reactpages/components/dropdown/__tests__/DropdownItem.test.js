import React from 'react';
import Renderer from 'react-test-renderer';

import DropdownItem from '../DropdownItem';

describe('DropdownItem', () => {
    const defaultProps = {
        children: undefined,
        active: undefined,
        disabled: false,
        tag: 'button',
        onClick: undefined,
        className: undefined
    }

    const getDefaultProps = () => Object.assign({}, defaultProps);

    describe('handleClick', () => {
        it('item is disabled - should not call onClick function', () => {
            const props = { ...getDefaultProps(), disabled: true, onClick: jest.fn() };
            const instance = new DropdownItem(props);
            const event = { preventDefault: () => { } };

            instance.handleClick(event);

            expect(props.onClick).not.toHaveBeenCalled();
        })

        it('item is enabled - should call onClick function', () => {
            const props = { ...getDefaultProps(), onClick: jest.fn() };
            const instance = new DropdownItem(props);
            const event = { preventDefault: () => { } };

            instance.handleClick(event);

            expect(props.onClick).toHaveBeenCalled();
        })
    })

    describe('render', () => {
        it('with default props - render snapshot correctly', () => {
            const component = Renderer.create(<DropdownItem {...getDefaultProps()}>Button Text</DropdownItem>);

            expect(component.toJSON()).toMatchSnapshot();
        })

        it('with tag is button and has onClick function in props - render snapshot with specific type is button', () => {
            const props = {...getDefaultProps(), onClick: jest.fn()};
            const component = Renderer.create(<DropdownItem {...props}>Button Text</DropdownItem>);

            expect(component.toJSON()).toMatchSnapshot();
        })

        it('with specific props - render snapshot correctly', () => {
            const props = {
                active: true,
                disabled: false,
                tag: 'div',
                onClick: jest.fn(),
                className: 'custom-class'
            }

            const component = Renderer.create(<DropdownItem {...props}>Button Text</DropdownItem>);

            expect(component.toJSON()).toMatchSnapshot();
        })
    })
})