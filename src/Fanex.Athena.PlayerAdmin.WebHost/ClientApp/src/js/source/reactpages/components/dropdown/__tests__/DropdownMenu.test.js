import React from 'react';
import Renderer from 'react-test-renderer';

import DropdownMenu from '../DropdownMenu';

describe('DropdownMenu', () => {
    const defaultProps = {
        tag: 'div',
        children: undefined,
        className: undefined,
        isOpen: undefined,
        onOpen: undefined,
        onClose: undefined
    }

    describe('render', () => {
        it('with default props - render correctly', () => {
            const component = Renderer.create(
            <DropdownMenu {...defaultProps}>
                <div>MenuItem 1</div>
                <div>MenuItem 2</div>
            </DropdownMenu>);

            expect(component.toJSON()).toMatchSnapshot();
        })

        it('with specific props - render correctly', () => {
            const props = {
                tag: 'ul',
                className: 'custom-dropdown-menu',
                isOpen: true,
                onOpen: jest.fn(),
                onClose: jest.fn()
            }

            const component = Renderer.create(
            <DropdownMenu {...props}>
                <li>MenuItem 1</li>
                <li>MenuItem 2</li>
            </DropdownMenu>);

            expect(component.toJSON()).toMatchSnapshot();
        })
    })
})