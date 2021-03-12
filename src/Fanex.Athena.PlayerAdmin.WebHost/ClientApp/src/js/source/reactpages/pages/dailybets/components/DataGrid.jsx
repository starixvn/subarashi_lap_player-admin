import React, { useEffect, useRef } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';

import { Table } from '@9softstudio/react-grid';
import { renderHeader, renderDataSection } from '../lib/gridUtils';
import NoDataTitle from '@jsroot/components/NoDataTitle';

export const DataGrid = (props) => {
    const isFirstRun = useRef(true);

    useEffect(() => {
        if (isFirstRun.current) {
            isFirstRun.current = false;
            return;
        }

        const { getData } = props;
        const param = { ...props.requestParam };
        getData(param);
    }, [props.requestParam])

    const { gridData } = props;
    const { dataList, total } = gridData;
    if (!dataList) {
        return null;
    }

    if (!dataList.length) {
        return <NoDataTitle />;
    }

    const { pagingOption, requestParam } = props;

    return (
        <Table minWidth={650}
            autoWidth={true}
            autoHeight={true}
            header={renderHeader()}
            body={renderDataSection(dataList)}
            footer={renderDataSection([total], true)}
            containerPadding={30}
            onSort={(sortBy, sortOrder) => props.setRequestParam({ ...requestParam, sortBy, sortOrder })}
            isPaging={true}
            pageOption={pagingOption}
            onPaging={(pageIndex, pageSize) => props.setRequestParam({ ...requestParam, pageSize, pageIndex })} />
    );
}

function mapDispatchToProps(dispatch) {
    const { setRequestParam, getData } = ActionCreators;
    return bindActionCreators({ setRequestParam, getData }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam, pagingOption, gridData } = state;
    return Object.assign({}, { requestParam, pagingOption, gridData });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);