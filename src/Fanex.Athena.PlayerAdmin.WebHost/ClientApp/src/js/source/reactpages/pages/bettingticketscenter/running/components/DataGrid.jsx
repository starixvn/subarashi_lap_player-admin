import React from 'react';
import { connect } from 'react-redux';
import { ApiUrl } from '../../common/lib/constants';
import BaseDataGrid from '../../common/components/BaseDataGrid';
import LangHelper from '@jsroot/lib/langHelper';
import AppData from '@jsroot/lib/AppData';

var Columns = [
    'Index', 'ExternalId', 'SystemId', 'RefNo', 'TransTime', 'Choice', 'Odds',
    'OddsType', 'Stake', 'Status', 'Platform', 'Live'
];
var ColumnWidths = [
    40, 120, 120, 110, 150, 400, 70,
    70, 120, 80, 80, 80
];
var ColumnAlignments = [
    'center-align', '', '', 'center-align', 'center-align', 'right-align', 'right-align',
    'right-align', 'right-align', 'center-align', 'center-align', 'center-align'
];
const SortableColumns = ['Stake'];

export const DataGrid = (props) => {
    if (AppData.isAllowTxIDColumn && !Columns.includes('LicenseeTxId')) {
        Columns.splice(4, 0, 'LicenseeTxId');
        ColumnWidths.splice(4, 0, 110);
        ColumnAlignments.splice(4, 0, 'center-align');
    }

    const { BaseCurrencyName } = props.requestParam;

    return (
        <BaseDataGrid
            dataUrl={ApiUrl.RUNNING_DATA_URL}
            isPaging={true}
            columns={Columns}
            columnWidths={ColumnWidths}
            columnAlignments={ColumnAlignments}
            totalText={`${LangHelper.getSingleResource('Totalin')} ${BaseCurrencyName}`}
            totalColumns={SortableColumns}
            sortableColumns={SortableColumns}
        />
    );
}

function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps)(DataGrid);
