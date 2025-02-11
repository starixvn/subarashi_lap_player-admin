import React, { useEffect, useMemo } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import { Table } from '@9softstudio/react-grid';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import { renderHeader, renderDataSection, renderFooter } from "../lib/gridHelper";
import { BreakdownByMode, GroupByMode } from '../lib/constants';

export const DataGrid = (props) => {
    const { requestParam, gridData, getData } = props;
    const { data, sumTotal } = gridData;

    useEffect(() => {
        const param = { ...requestParam, GroupMode: GroupByMode.Player, BreakdownMode: BreakdownByMode.Product };
        getData(param);
    }, []);

    const renderOption = {
        isGroupByPlayerMode: true,
        isBreakDownPlayerMode: false
    }

    const header = useMemo(() => renderHeader(renderOption), [requestParam])
    const body = useMemo(() => renderDataSection(data, renderOption), [data])
    const footer = useMemo(() => renderFooter(sumTotal, renderOption), [sumTotal])

    if(!data || data.length === 0){
        return (<NoDataTitle />);
    }

    return (
        <Table minWidth={650}
            autoWidth={true}
            autoHeight={true}
            header={header}
            body={body}
            footer={footer}
            containerPadding={30}
            isPaging={false}
            shouldResetScrollPosition={props.reloadReport} />
    );
}

function mapDispatchToProps(dispatch) {
    const { getData } = ActionCreators;
    return bindActionCreators({ getData }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam, gridData, hasCustomerComm } = state;
    return Object.assign({}, { requestParam, gridData, hasCustomerComm });
}

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);