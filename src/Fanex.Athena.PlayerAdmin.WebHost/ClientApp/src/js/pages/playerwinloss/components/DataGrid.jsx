import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import { Table } from '@9softstudio/react-grid';
import { buildHeader } from '../lib/gridHelper';
import GridRow from './GridRow';
import NoDataTitle from '@jsroot/components/NoDataTitle';

export const DataGrid = (props) => {
    const { requestParam, gridData, getData, hasCustomerComm, isAllowedBA } = props;

    useEffect(() => {
        const param = { ...requestParam };
        getData(param);
    }, []);

    if (!gridData) {
        return null;
    }

    if(gridData.length === 0){
        return (<NoDataTitle />);
    }

    const renderBodyData = () => {
        const dataRows = [];
        for (let i = 0; i < gridData.length; i++) {
            const dataRow = gridData[i];
            const rowProps = {
                index: i + 1,
                dataRow,
                hasCustomerComm,
                isAllowedBA
            }
            dataRows.push(<GridRow key={i} {...rowProps} />);
        }

        return dataRows;
    }

    return (
        <Table minWidth={650}
            autoWidth={true}
            autoHeight={true}
            header={buildHeader(hasCustomerComm, isAllowedBA)}
            body={renderBodyData()}
            containerPadding={30} />
    );
}

function mapDispatchToProps(dispatch) {
    const { getData } = ActionCreators;
    return bindActionCreators({ getData }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam, gridData, hasCustomerComm, isAllowedBA } = state;
    return Object.assign({}, { requestParam, gridData, hasCustomerComm, isAllowedBA });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);