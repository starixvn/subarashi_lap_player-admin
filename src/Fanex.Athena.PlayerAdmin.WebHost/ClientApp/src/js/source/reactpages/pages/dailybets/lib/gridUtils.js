import React from 'react';
import { Row, Cell, HeaderRow, HeaderCell } from '@9softstudio/react-grid';
import { headerText } from './constants';
import NumberFormat from '@jsroot/lib/numberFormat';

export const renderHeader = () => {
    return (
        <HeaderRow style={{ height: 32 }}>
            <HeaderCell colWidth={40}>#</HeaderCell>
            <HeaderCell colWidth={100}>{headerText.playerName}</HeaderCell>
            <HeaderCell colWidth={50}>{headerText.currency}</HeaderCell>
            <HeaderCell colWidth={100}>{headerText.refNo}</HeaderCell>
            <HeaderCell colWidth={100}>{headerText.transTime}</HeaderCell>
            <HeaderCell colWidth={80}>{headerText.product}</HeaderCell>
            <HeaderCell colWidth={150}>{headerText.betType}</HeaderCell>
            <HeaderCell colWidth={80} sortBy={'turnover'}>{headerText.turnover}</HeaderCell>
            <HeaderCell colWidth={80} sortBy={'winLoss'}>{headerText.winloss}</HeaderCell>
            <HeaderCell colWidth={50}>{headerText.status}</HeaderCell>
        </HeaderRow>
    )
}

export const renderDataSection = (dataList, isTotalRow) => {
    if (isTotalRow) {
        return buildTotalRow(dataList[0]);
    }

    const dataRows = [];
    for (let i = 0; i < dataList.length; i++) {
        dataRows.push(buildDataRow(dataList[i]));
    }

    return dataRows;
}

const buildDataRow = (dataItem) => {
    const { index, playerName, currencyName, refNo, transTimeString, productName, betType, turnover, winLoss, status } = dataItem;

    return (
        <Row key={index}>
            <Cell className="center-align">{index}</Cell>
            <Cell>{playerName}</Cell>
            <Cell className="center-align">{currencyName}</Cell>
            <Cell className="center-align">{refNo}</Cell>
            <Cell className="center-align">{transTimeString}</Cell>
            <Cell>{productName}</Cell>
            <Cell>{betType}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(turnover)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(winLoss)}</Cell>
            <Cell className="center-align">{status}</Cell>
        </Row>
    )
}

const buildTotalRow = (dataItem) => {
    const { turnover, winLoss } = dataItem;
    return (
        <Row key="totalRowUSD">
            <Cell className="right-align" colSpan="7">Total In USD</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(turnover)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(winLoss)}</Cell>
            <Cell></Cell>
        </Row>
    )
}