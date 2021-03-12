import React from 'react';
import { Row, Cell } from '@9softstudio/react-grid';
import NumberFormat from '@jsroot/lib/numberFormat';

export const GridRow = (props) => {
    const { dataRow, index, hasCustomerComm, isAllowedBA } = props;
    const { turnover, totalTransaction, customerWL, playerComm, customerTotalWL, betEffective, licenseeWL, baComm, amtDueToBA } = dataRow || {};
    const classNameForLicWl = licenseeWL < 0 ? 'negative' : '';

    return (
        <Row>
            <Cell className="center-align">{index}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(turnover)}</Cell>
            <Cell className="right-align">{NumberFormat.format(totalTransaction)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(customerWL)}</Cell>
            {hasCustomerComm && <Cell className="right-align">{NumberFormat.formatDecimal(playerComm)}</Cell>}
            {hasCustomerComm && <Cell className="right-align">{NumberFormat.formatDecimal(customerTotalWL)}</Cell>}
            <Cell className="right-align">{NumberFormat.formatDecimal(betEffective)}</Cell>
            <Cell className={'right-align ' + classNameForLicWl}>{NumberFormat.formatDecimal(licenseeWL)}</Cell>
            {isAllowedBA && <Cell className="right-align">{NumberFormat.formatDecimal(baComm)}</Cell>}
            {isAllowedBA && <Cell className="right-align">{NumberFormat.formatDecimal(amtDueToBA)}</Cell>}
        </Row>
    )
}

export default GridRow;