import React from 'react';
import { Row, Cell, HeaderRow, HeaderCell } from '@9softstudio/react-grid';
import { headerText, VOIDED_STATUS } from './constants';

export const renderHeader = () => {
    return (
        <HeaderRow style={{ height: 32 }}>
            <HeaderCell colWidth={50}>#</HeaderCell>
            <HeaderCell colWidth={100}>{headerText.refNo}</HeaderCell>
            <HeaderCell colWidth={100}>{headerText.eventType}</HeaderCell>
            <HeaderCell colWidth={200}>{headerText.event}</HeaderCell>
            <HeaderCell colWidth={70}>{headerText.status}</HeaderCell>
        </HeaderRow>
    )
}

export const renderDataSection = (dataList) => {
    const dataRows = [];
    for (let i = 0; i < dataList.length; i++) {
        dataRows.push(buildDataRow(i + 1, dataList[i]));
    }

    return dataRows;
}

const buildDataRow = (index, dataItem) => {
    const { refNo, eventType, event } = dataItem;

    return (
        <Row key={index}>
            <Cell className="center-align">{index}</Cell>
            <Cell>{refNo}</Cell>
            <Cell>{eventType}</Cell>
            <Cell>{event}</Cell>
            <Cell className="center-align">{VOIDED_STATUS}</Cell>
        </Row>
    )
}