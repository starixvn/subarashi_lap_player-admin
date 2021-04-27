import React from 'react';
import { HeaderRow, HeaderCell } from '@9softstudio/react-grid';
import { generateHeaderText } from '../lib/utils';

export const buildHeader = (hasCustomerComm, isAllowedBA) => {
    const rowSpan = hasCustomerComm ? 2 : 1;
    const colSpan = hasCustomerComm ? 3 : 1;
    const headerText = generateHeaderText(hasCustomerComm);

    const gridHeader = [
        <HeaderRow key="thead1" style={{ height: 32 }}>
            <HeaderCell rowSpan={rowSpan} colWidth={30}>{headerText.Index}</HeaderCell>
            <HeaderCell rowSpan={rowSpan} colWidth={150}>{headerText.Turnover}</HeaderCell>
            <HeaderCell rowSpan={rowSpan} colWidth={80}>{headerText.TotalTransaction}</HeaderCell>
            <HeaderCell colSpan={colSpan}>{headerText.CustomerWinLoss}</HeaderCell>
            <HeaderCell rowSpan={rowSpan} colWidth={80}>{headerText.BetEffective}</HeaderCell>
            <HeaderCell rowSpan={rowSpan} colWidth={80}>{headerText.LicenseeWL}</HeaderCell>
            {isAllowedBA && <HeaderCell rowSpan={rowSpan} colWidth={80}>{headerText.BAComm}</HeaderCell>}
            {isAllowedBA && <HeaderCell rowSpan={rowSpan} colWidth={80}>{headerText.AmtDueToBA}</HeaderCell>}
        </HeaderRow>
    ]

    if (hasCustomerComm) {
        gridHeader.push(
            <HeaderRow key="thead2" style={{ height: 32 }}>
                <HeaderCell colWidth={80}>{headerText.WinLoss}</HeaderCell>
                <HeaderCell colWidth={80}>{headerText.Comm}</HeaderCell>
                <HeaderCell colWidth={80}>{headerText.Total}</HeaderCell>
            </HeaderRow>
        );
    }

    return gridHeader;
}