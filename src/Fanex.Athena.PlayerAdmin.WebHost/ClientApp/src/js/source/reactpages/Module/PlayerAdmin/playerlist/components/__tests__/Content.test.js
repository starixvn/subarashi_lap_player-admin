import React from 'react';
import { shallow } from 'enzyme';
import { Pager } from '@9softstudio/react-grid';
import { Content } from '../Content';

describe('Content', () => {
    const defaultPagingOption = {
        PageIndex: 1,
        PageSize: 50,
        PageList: [50, 100, 200, 500],
        TotalItem: null
    }

    const defaultProps = {
        pagingOption: defaultPagingOption,
        param: {},
        searchPlayer: jest.fn()
    }

    describe('render', () => {
        it('pagingOption.TotalItem is null - return null', () => {
            const wrapper = shallow(<Content {...defaultProps} />);

            expect(wrapper).toMatchSnapshot();
        })

        it('pagingOption.TotalItem is not null - should match snapshot', () => {
            const props = { ...defaultProps, pagingOption: { ...defaultPagingOption, TotalItem: 1000 } }
            const wrapper = shallow(<Content {...props} />);

            expect(wrapper).toMatchSnapshot();
        })

        it('handlePaging - should call action props.searchPlayer', () => {
            const props = { ...defaultProps, pagingOption: { ...defaultPagingOption, TotalItem: 1000 } }
            const wrapper = shallow(<Content {...props} />);

            const pageIndex = 2;
            const pageSize = 100;
            const expectedParam = { ...props.param, pageIndex, pageSize };

            const pagerProps = wrapper.find(Pager).props();
            pagerProps.onPaging(pageIndex, pageSize);

            expect(props.searchPlayer).toHaveBeenCalledWith(expectedParam);
        })
    })
})