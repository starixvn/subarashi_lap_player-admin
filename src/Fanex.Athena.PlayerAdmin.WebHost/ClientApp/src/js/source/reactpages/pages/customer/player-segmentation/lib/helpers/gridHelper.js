import React from 'react';
import { HeaderRow, HeaderCell } from '@9softstudio/react-grid';
import AppData from '@jsroot/lib/AppData';
import { SortMapper } from '../constants';
import { ViewType } from '../constants';
import CustomerClassCell from '../../tabs/problem-player/components/grid/CustomerClassCell';

export default class GridHelper {
    static renderHeader(dataHeaders) {
        return (
            <HeaderRow style={{ height: 32 }}>
                {dataHeaders && dataHeaders.length > 0 &&
                    dataHeaders.map(_ =>
                        <HeaderCell colWidth={_.colWidth} sortBy={_.sortBy}>
                            {this.renderHeaderCell(_)}
                        </HeaderCell>)}
            </HeaderRow>
        )
    }

    static renderHeaderCell(cell) {
        var isCustomerClass = cell.key == "customerClass";
        if (isCustomerClass) {
            return (<CustomerClassCell cell={cell}></CustomerClassCell>);
        }

        return <>{cell.childComponent ? cell.childComponent : cell.text}</>;
    }

    static calculateTopPosition(dropdownHeight = 0, currentTop) {
        const innerHeight = window.innerHeight;
        const expectedHeight = dropdownHeight + currentTop + 20;
        return expectedHeight > innerHeight ? currentTop - dropdownHeight : currentTop;
    }

    static getSortInfo(orderCol, sortType) {
        const sortInfo = {
            orderCol: SortMapper.sortCol[orderCol],
            sortType: SortMapper.sortType[sortType]
        }
        return sortInfo;
    }

    static getSortInfoFromPagingOption(options) {
        const { orderCol, sortType } = options;
        return {
            orderCol,
            sortType
        }
    }

    static callBackToUpdatePaging = ({ pagingOption, setPagingOption }) => response => {
        const { totalItem, pageIndex, pageSize, orderCol, sortType } = response;
        const newPagingOption = {
            ...pagingOption,
            ...{
                PageIndex: pageIndex,
                PageSize: pageSize,
                orderCol,
                sortType,
                TotalItem: totalItem
            }
        }
        setPagingOption(newPagingOption);
    }

    static isShowSubsidiary = subsidiarySearch => AppData.viewType !== ViewType.Normal && subsidiarySearch;
}
