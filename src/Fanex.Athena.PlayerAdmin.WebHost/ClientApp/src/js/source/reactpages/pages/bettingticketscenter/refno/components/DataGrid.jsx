import React from 'react';
import { ApiUrl } from '../../common/lib/constants';
import BaseDataGrid from '../../common/components/BaseDataGrid';
import AppData from '@jsroot/lib/AppData';

var Columns = [
    'Index', 'ExternalId', 'SystemId', 'RefNo', 'TransTime', 'Choice', 'Odds',
    'OddsType', 'Stake', 'WinLoss', 'LicenseeWL', 'Status', 'Platform', 'Live'
];
var ColumnWidths = [
    40, 120, 120, 110, 150, 400, 70,
    70, 120, 100, 100, 80, 80, 80
];
var ColumnAlignments = [
    'center-align', '', '', 'center-align', 'center-align', 'right-align', 'right-align',
    'right-align', 'right-align', 'right-align', 'right-align', 'center-align', 'center-align', 'center-align'
];

const DataGrid = (props) => {
    if (AppData.isAllowTxIDColumn && !Columns.includes('LicenseeTxId')) {
        Columns.splice(4, 0, 'LicenseeTxId');
        ColumnWidths.splice(4, 0, 110);
        ColumnAlignments.splice(4, 0, 'center-align');
    }

    return (
        <BaseDataGrid
            dataUrl={ApiUrl.REFNO_DATA_URL}
            isPaging={false}
            columns={Columns}
            columnWidths={ColumnWidths}
            columnAlignments={ColumnAlignments}
            totalText=''
            totalColumns={[]}
            sortableColumns={[]}
        />
    );
}

export default DataGrid;
