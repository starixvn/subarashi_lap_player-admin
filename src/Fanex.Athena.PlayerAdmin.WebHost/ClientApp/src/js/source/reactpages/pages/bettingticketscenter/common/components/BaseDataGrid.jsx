import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from './action.js';
import { bindActionCreators } from 'redux';
import BetListDataGrid from '@jsroot/components/betlist/BetListDataGrid';
import ReportTitle from '@jsroot/components/ReportTitle';
import LangHelper from '@jsroot/lib/langHelper';
import { HeaderText, DefaultPageSize } from '../lib/constants';
import AppData from '@jsroot/lib/AppData';

export const BaseDataGrid = (props) => {
    const {
        gridData, requestParam, reportTitle, toggleLoader, getGridData, setRequestParam,
        dataUrl, isPaging, columns, columnWidths, columnAlignments, totalText, totalColumns, sortableColumns
    } = props;
    const pageOption = {
        PageIndex: requestParam.PageIndex,
        PageSize: requestParam.PageSize,
        TotalItem: gridData && gridData.TotalRows ? gridData.TotalRows : DefaultPageSize,
        PageList: [DefaultPageSize, 100, 200, 300, 400, 500, 1000, 2000]
    };

    useEffect(() => {
        toggleLoader(false);
    }, [gridData]);

    const getHeaderName = (column) => {
        if (column === 'LicenseeWL') {
            return `${HeaderText.LicenseeWL}/${HeaderText.Comm}`;
        }

        if (column === 'WinLoss' && AppData.isAllowedCustomerComm) {
            return `${HeaderText.WinLoss}/${HeaderText.Comm}`;
        }

        return HeaderText[column];
    }

    const onPaging = (index, size) => {
        const param = Object.assign({}, requestParam, { PageIndex: index, PageSize: size });

        getData(param);
    }

    const afterLoad = () => {
        toggleLoader(false);
    }

    const getData = (param) => {
        toggleLoader(true);
        getGridData(dataUrl, param);
        setRequestParam(param);
    }

    const onSort = (sortBy, sortOrder) =>{
        const param = Object.assign({}, requestParam, { SortBy: sortBy === 'WinLoss' ? 'WinLost' : sortBy, SortOrder: sortOrder });

        getData(param);
    }

    const getResource = (key) => {
        return LangHelper.getSingleResource(key);
    }

    if (!gridData || !gridData.Data) {
        return null;
    }

    return (
        <div className="betlist-container result-content">
            <ReportTitle content={reportTitle} />
            <BetListDataGrid
                isPaging={isPaging}
                pageOption={pageOption}
                onPaging={onPaging}
                getHeaderNameFunc={getHeaderName}
                columns={columns}
                columnWidths={columnWidths}
                sortableColumns={sortableColumns}
                totalColumns={totalColumns}
                columnAlignments={columnAlignments}
                data={gridData.Data}
                total={gridData.Total}
                totalText={totalText}
                afterLoad={afterLoad}
                onSort={onSort}
                getResourceFunc={getResource}
                adjustedHeight={-20}
            />
        </div>
    );
}

function mapDispatchToProps(dispatch) {
    const { toggleLoader, getGridData, setRequestParam } = ActionCreators;
    return bindActionCreators({ toggleLoader, getGridData, setRequestParam }, dispatch);
}

function mapStateToProps(state) {
    const { gridData, requestParam, reportTitle } = state;
    return Object.assign({}, { gridData, requestParam, reportTitle });
};

export default connect(mapStateToProps, mapDispatchToProps)(BaseDataGrid);