import React from 'react';
import { shallow } from 'enzyme';

import { licenseeType } from '../../../lib/constants';
import { ContextMenu } from '../../grid/ContextMenu';
import { DropdownMenu } from '@jsroot/components/dropdown';

describe('ContextMenu', () => {
    const defaultProps = {
        currentActivePlayer: { custId: 1 },
        contextMenu: {
            position: null,
            isOpen: false
        },
        type: licenseeType.nonApi,
        setSelectedPlayer: jest.fn(),
        setSelectedTabName: jest.fn(),
        getTabList: jest.fn(() => {
            return [
                { key: 'note', name: 'Note', tabName: 'Note' },
                { key: 'password', name: 'Password', tabName: 'Setting' }
            ]
        })
    }   

    it('with default props and inital state - should match snapshot', () => {
        const wrapper = shallow(<ContextMenu {...defaultProps} />);

        expect(wrapper).toMatchSnapshot();
    })

    it('onMouseEnter - isOpen should be true', () => {
        const props = { ...defaultProps };
        const wrapper = shallow(<ContextMenu {...props} />);
        const menuProps = wrapper.find(DropdownMenu).props();
        menuProps.onMouseEnter();

        expect(wrapper).toMatchSnapshot();
    })

    it('onMouseLeave - isOpen should rely on props.isOpenContextMenu', () => {
        const runTest = (initialProps) => {
            const wrapper = shallow(<ContextMenu {...initialProps} />);

            const menuProps = wrapper.find(DropdownMenu).props();
            menuProps.onMouseLeave();

            expect(wrapper).toMatchSnapshot();
        }

        const testCaseList = [{
            ...defaultProps,
            position: null,
            isOpenContextMenu: true
        }, {
            ...defaultProps,
            position: null,
            isOpenContextMenu: false
        }];

        testCaseList.forEach(item => {
            runTest(item);
        })
    })
})