import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions';
import { bindActionCreators } from 'redux';
import BetListDataGrid from '@jsroot/components/betlist/BetListDataGrid';
import LangHelper from '@jsroot/lib/langHelper';
import AppData from '@jsroot/lib/AppData';
import QueryHelper from '../lib/helpers/queryHelper';
import GridHelper from '../lib/helpers/gridHelper';
import { HeaderText, Columns, ColumnWidths, ColumnAlignments } from '../lib/constants';

export const DataGrid = (props) => {
    const { gridData, selectedParams, setSelectedParams, toggleLoader, getGridData } = props;

    if (!gridData || !gridData.Data) {
        return null;
    }

    const pagingOption = React.useMemo(() => GridHelper.GetPagingOption(selectedParams, gridData), [selectedParams, gridData])

    useEffect(() => {
        toggleLoader(false);
    }, [gridData]);

    const getHeaderName = column => {
        return GridHelper.GetHeaderName(column, HeaderText, AppData.isAllowedCustomerComm)
    }

    const onPaging = (pageIndex, pageSize) => {
        const params = {
            ...selectedParams,
            ...{ pageIndex, pageSize }
        };
        setSelectedParams(params);
        QueryHelper.FetchBetList({ params, getGridData, toggleLoader });
    }

    const afterLoad = () => {
        toggleLoader(false);
    }

    const getResource = (key) => {
        return LangHelper.getSingleResource(key);
    }

    return (
        <div className="betlist-container result-content">
            <BetListDataGrid
                isPaging={true}
                pageOption={pagingOption}
                totalColumns={null}
                onPaging={onPaging}
                getHeaderNameFunc={getHeaderName}
                columns={Columns}
                columnWidths={ColumnWidths}
                columnAlignments={ColumnAlignments}
                sortableColumns={[]}
                data={gridData.Data}
                afterLoad={afterLoad}
                getResourceFunc={getResource}
                adjustedHeight={10}
            />
        </div>
    );
}

function mapDispatchToProps(dispatch) {
    const { toggleLoader, getGridData, setSelectedParams } = ActionCreators;
    return bindActionCreators({ toggleLoader, getGridData, setSelectedParams }, dispatch);
}

function mapStateToProps(state) {
    const { gridData, selectedParams } = state;
    return Object.assign({}, { gridData, selectedParams });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);