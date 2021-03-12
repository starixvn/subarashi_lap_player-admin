import React from 'react';
import Renderer from 'react-test-renderer';

import DropdownButton from '../DropdownButton';

describe('DropdownButton', () => {
    const defaultProps = {
        buttonLabel: undefined,
        children: undefined,
        className: undefined,
        isOpen: undefined,
        onOpen: undefined,
        onClose: undefined,
        onToggle: undefined,
        toggleButton: {
            caret: undefined,
            className: 'toggle-default',
            disabled: undefined,
            onClick: undefined,
            tag: undefined
        },
        dropdownMenu: {
            tag: undefined,
            children: 'children',
            className: undefined
        }
    }

    describe('render', () => {
        it('with default props - render correctly', () => {
            const component = Renderer.create(
                <DropdownButton {...defaultProps}>
                    <div>Item 1</div>
                    <div>Item 2</div>
                </DropdownButton>);

            expect(component.toJSON()).toMatchSnapshot();
        })

        it('with specific props - render correctly', () => {
            const props = {
                buttonLabel: 'toggle label',
                className: 'custom-button-dropdown',
                isOpen: true,
                onOpen: jest.fn(),
                onClose: jest.fn(),
                onToggle: jest.fn(),
                toggleButton: {
                    caret: true,
                    className: 'custom-toggle-button',
                    disabled: true,
                    onClick: jest.fn(),
                    tag: 'div'
                },
                dropdownMenu: {
                    tag: 'ul',
                    className: 'custom-dropdown-menu',
                    children: 'children'
                }
            }

            const component = Renderer.create(
                <DropdownButton {...props}>
                    <li>Item 1</li>
                    <li>Item 2</li>
                </DropdownButton>);

            expect(component.toJSON()).toMatchSnapshot();
        })
    })
})