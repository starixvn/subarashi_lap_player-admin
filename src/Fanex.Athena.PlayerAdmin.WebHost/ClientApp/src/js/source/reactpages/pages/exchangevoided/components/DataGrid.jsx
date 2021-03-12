import React, { useEffect, useRef, useState } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions';
import { bindActionCreators } from 'redux';
import { renderHeader, renderDataSection } from '../lib/gridUtils';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import { Table } from '@9softstudio/react-grid';
import ReportTitle from '@jsroot/components/ReportTitle';
import LangHelper from '@jsroot/lib/langHelper';
import SimplePagination from '@jsroot/components/SimplePagination';

const DataGrid = (props) => {
    const isFirstRun = useRef(true);
    const [pagingOption, setPagingOption] = useState({pageIndex: 1, totalPage: 0});

    useEffect(() => {
        if (isFirstRun.current) {
            isFirstRun.current = false;
            return;
        }

        const { getData } = props;
        const param = { ...props.requestParam };
        getData(param);
    }, [props.requestParam])

    useEffect(() => {
        const { gridData, requestParam } = props;

        if(gridData && gridData.totalPage !== undefined){
            setPagingOption({ pageIndex: requestParam.pageIndex, totalPage: gridData.totalPage });
        }
    }, [props.gridData])

    const handleChangePaging = pageIndex => {
        setPagingOption({ ...pagingOption, pageIndex });
        props.setRequestParam({...props.requestParam, pageIndex});
    }

    const { dataList } = props.gridData;
    if (!dataList) {
        return null;
    }

    if (!dataList.length) {
        return <NoDataTitle />;
    }

    const { requestParam } = props;

    const reportTitle = `${LangHelper.getSingleResource('BettingTicketCenter')}, ${requestParam.selectedDate}`;

    const {pageIndex, totalPage} = pagingOption;

    return (
        <div>
            <ReportTitle content={reportTitle} />
            <Table minWidth={650}
                autoWidth={true}
                autoHeight={true}
                header={renderHeader()}
                body={renderDataSection(dataList)}
                containerPadding={30} />
            {totalPage > 1 && <SimplePagination pageIndex={pageIndex} totalPage={totalPage} 
                onPageChanged={handleChangePaging} />}
        </div>
    );
}

function mapDispatchToProps(dispatch) {
    const { setRequestParam, getData } = ActionCreators;
    return bindActionCreators({ setRequestParam, getData }, dispatch);
};

function mapStateToProps(state) {
    const { gridData, requestParam } = state;
    return Object.assign({}, { gridData, requestParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);