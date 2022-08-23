import React from 'react';
import BetListDataGrid from '@jsroot/components/betlist/BetListDataGrid';
import AppData from '@jsroot/lib/AppData';

export const DataGrid = (props) => {
    const { data, total, pagingOption, onPaging, getResource, totalText, getHeaderName, onSort } = props;

    let Columns = ['Index', 'RefNo', 'TransTime', 'Choice', 'Odds', 'OddsType', 'Stake', 'WinLoss', 'LicenseeWL', 'Status', 'Platform', 'Live'];
    let ColumnWidths = [40, 160, 150, 370, 70, 70, 80, 80, 80, 90, 60, 60];
    let ColumnAlignments = [
        'center-align', 'center-align tooltip', 'center-align',
        'right-align', 'right-align', 'right-align', 'right-align', 'right-align', 'right-align',
        'center-align', 'center-align', 'center-align'
    ]

    if (AppData.isAllowTxIDColumn && !Columns.includes('LicenseeTxId')) {
        Columns.splice(2, 0, 'LicenseeTxId');
        ColumnWidths.splice(2, 0, 110);
        ColumnAlignments.splice(2, 0, 'center-align tooltip');
    }

    return (
        <BetListDataGrid
            isPaging={true}
            pageOption={pagingOption}
            onPaging={onPaging}
            getHeaderNameFunc={getHeaderName}
            columns={Columns}
            columnWidths={ColumnWidths}
            sortableColumns={['Stake', 'WinLoss', 'LicenseeWL']}
            totalColumns={['Stake', 'WinLoss', 'LicenseeWL']}
            columnAlignments={ColumnAlignments}
            data={data}
            total={total}
            totalText={totalText}
            adjustedHeight={-20}
            getResourceFunc={getResource}
            onSort={onSort}
        />
    )
}

export default DataGrid;