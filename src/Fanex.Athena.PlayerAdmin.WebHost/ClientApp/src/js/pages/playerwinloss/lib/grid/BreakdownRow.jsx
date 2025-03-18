import React from 'react';
import { Row, Cell } from '@9softstudio/react-grid';
import NumberFormat from '@jsroot/lib/numberFormat';

export const BreakdownRow = (props) => {

    const { data, option } = props;
    const { betCount, turnover, netTurnover, playerWinloss, playerComm, betEffective, licWinloss, licComm, margin, breakBy, groupBy, breakDownFieldText, externalId } = data;
    const { isGroupByPlayerMode, isBreakDownPlayerMode } = option;
    const colSpan = isGroupByPlayerMode ? 2 : 1;
    return (
        <Row className="detail-row" key={`detail-row-${groupBy}-${breakBy}`}>
            <Cell ></Cell>
            <Cell className="left-align break-by-cell" colSpan={colSpan}>{breakDownFieldText}</Cell>
            { isBreakDownPlayerMode &&  <Cell className="left-align">{externalId}</Cell> }
            <Cell className="right-align">{NumberFormat.format(betCount)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(turnover)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(netTurnover)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(playerWinloss)}</Cell>
            { isGroupByPlayerMode && <Cell className="right-align">{NumberFormat.formatDecimal(betEffective)}</Cell> }
            <Cell className="right-align">{NumberFormat.formatDecimal(playerComm)}</Cell>
            <Cell className={`right-align ${licWinloss < 0 ? "negative" : ""}`}>{NumberFormat.formatDecimal(licWinloss)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(licComm)}</Cell>
            <Cell className={`right-align ${margin < 0 ? "negative" : ""}`}>{NumberFormat.formatDecimal(margin*100)}%</Cell>
        </Row>
    );
}

export default BreakdownRow;

