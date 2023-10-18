import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';

import Site from '@jsroot/lib/site';
import DataService from '@jsroot/lib/DataService';
import LangHelper from '@jsroot/lib/langHelper';
import { formatDateString } from '@jsroot/lib/dateUtils';

import AppData from '@jsroot/lib/AppData';
import DataGrid from './DataGrid';
import BetListLayout from '@jsroot/layouts/betlist/BetListLayout';
import { usePagingOption } from '@jsroot/customHooks/pagingHooks';
import { HeaderText } from '../lib/constants';

export const Content = (props) => {
    const { requestParam, setRequestParam } = props;
    const [gridData, setGridData] = useState(null);
    const { pagingOption, setPagingOption, onChangePaging } = usePagingOption();

    const handleSort = (sortBy, sortOrder) => {
        setRequestParam({ ...requestParam, sortBy: sortBy === 'WinLoss' ? 'WinLost' : sortBy, sortOrder });
    }

    useEffect(() => {
        const param = { ...requestParam, pageSize: pagingOption.PageSize, pageIndex: pagingOption.PageIndex };
        const url = Site.resolveClientUrl('PlayerBtcSingleCustomer/Index');
        props.toggleLoader(true);
        DataService.postData(url, param)
            .then(response => {
                const result = response.data;

                setGridData({ data: result.Data || [], total: result.Total });
                setPagingOption({ ...pagingOption, TotalItem: result.TotalRows });
                props.toggleLoader(false);
            })
            .catch(error => {
                console.log(error.message);
                setGridData({});
                setPagingOption({ PageSize: 50, PageIndex: 1, TotalItem: 0 });
                props.toggleLoader(false);
            });
    }, [requestParam, pagingOption.PageSize, pagingOption.PageIndex]);

    const getHeaderName = (column) => {
        if (column === 'WinLoss') {
            return AppData.isAllowedCustomerComm ? `${HeaderText.WinLoss}/${HeaderText.Comm}` : HeaderText.WinLoss;
        }

        return HeaderText[column];
    }

    if (!gridData) {
        return null;
    }

    const dataGridProps = {
        data: gridData.data,
        total: gridData.total,
        pagingOption,
        onPaging: onChangePaging,
        getResource: LangHelper.getSingleResource,
        totalText: LangHelper.getSingleResource('Total'),
        getHeaderName,
        onSort: handleSort
    }

    return (
        <BetListLayout
            pageName='PlayerAdmin'
            rootUrl={Site.getRootUrl()}
            getResourceFunc={dataGridProps.getResource}
            formatDateStringFunc={formatDateString}
            baseCurrency=''
            beforeShowFunc={() => props.toggleLoader(true)}
            afterShowFunc={() => props.toggleLoader(false)}>
            <DataGrid {...dataGridProps} />
        </BetListLayout>
    )
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
}

export default connect(mapStateToProps, mapDispatchToProps)(Content);