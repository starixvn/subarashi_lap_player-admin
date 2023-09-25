import React from 'react';
import { Row, Cell, HeaderRow, HeaderCell } from '@9softstudio/react-grid';
import { HeaderText, SortBy } from './constants';
import NumberFormat from '@jsroot/lib/numberFormat';
import TreeViewGridRowNode from './grid/TreeViewGridRowNode';
import LangHelper from '@jsroot/lib/langHelper'

export const renderHeader = (option) => {
    const { isGroupByPlayerMode, isShowBAField, isBreakDownPlayerMode } = option;

    const _renderDefaultHeader = () => {
        const prefixKey = 'hearder-default';
        let headerCells = [
            <HeaderCell className='border-left-0' key={`${prefixKey}-#`} colWidth={30}>#</HeaderCell>,
            <HeaderCell key={`${prefixKey}-${HeaderText.GroupBy}`} colWidth={200}>{HeaderText.GroupBy}</HeaderCell>,
            ...getCommonHeaderCells(1, prefixKey)
        ]
        return (
            <HeaderRow>
                {headerCells.map(elem => elem)}
            </HeaderRow>
        )
    }

    const _renderGroupByPlayerModeHeaderCell = () => {
        const prefixKey = 'hearder-player';
        let headerCells = [
            <HeaderCell key={`${prefixKey}-#`} className='border-left-0' rowSpan={2} colWidth={30}>#</HeaderCell>,
            <HeaderCell key={`${prefixKey}-${HeaderText.GroupBy}`} rowSpan={1} colSpan={2} colWidth={200}>{HeaderText.GroupBy}</HeaderCell>,
            ...getCommonHeaderCells(2)
        ]
        return [
            <HeaderRow key={'header-row-1'}>
                {headerCells.map(elem => elem)}
            </HeaderRow>,
            <HeaderRow key={'header-row-2'}>
                <HeaderCell key={`${prefixKey}-${HeaderText.SystemId}`} colWidth={120}>{HeaderText.SystemId}</HeaderCell>
                <HeaderCell key={`${prefixKey}-${HeaderText.ExternalId}`} colWidth={120}>{HeaderText.ExternalId}</HeaderCell>
            </HeaderRow>
        ]

    }

    const getCommonHeaderCells = (rowSpan, prefixKey) => {
        let firstGroup = [
            <HeaderCell key={`${prefixKey}-${HeaderText.BetCount}`} rowSpan={rowSpan} colWidth={80}>{HeaderText.BetCount}</HeaderCell>,
            <HeaderCell key={`${prefixKey}-${HeaderText.TurnOver}`} rowSpan={rowSpan} colWidth={110}>{HeaderText.TurnOver}</HeaderCell>,
            <HeaderCell key={`${prefixKey}-${HeaderText.NetTurnOver}`} rowSpan={rowSpan} colWidth={110}>{HeaderText.NetTurnOver}</HeaderCell>,
            <HeaderCell key={`${prefixKey}-${HeaderText.PlayerWL}`} rowSpan={rowSpan} colWidth={110}>{HeaderText.PlayerWL}</HeaderCell>,
        ]
        let secondGroup = [
            <HeaderCell key={`${prefixKey}-${HeaderText.PlayerComm}`} rowSpan={rowSpan} colWidth={110}>{HeaderText.PlayerComm}</HeaderCell>,
            <HeaderCell key={`${prefixKey}-${HeaderText.LicenseeWL}`} rowSpan={rowSpan} colWidth={110}>{HeaderText.LicenseeWL}</HeaderCell>,
            <HeaderCell key={`${prefixKey}-${HeaderText.LicenseeComm}`} rowSpan={rowSpan} colWidth={110}>{HeaderText.LicenseeComm}</HeaderCell>,
            isShowBAField && <HeaderCell key={`${prefixKey}-${HeaderText.AmountDueToBA}`} rowSpan={rowSpan} colWidth={110}>{HeaderText.AmountDueToBA}</HeaderCell>,
            <HeaderCell key={`${prefixKey}-${HeaderText.Margin}`} rowSpan={rowSpan} colWidth={110}>{HeaderText.Margin}</HeaderCell>
        ]
        let effectiveBetCell = <HeaderCell key={`${prefixKey}-${HeaderText.EffectiveBettingValue}`} rowSpan={rowSpan} colWidth={110}>{HeaderText.EffectiveBettingValue}</HeaderCell>;
        return isGroupByPlayerMode  ? [...firstGroup, effectiveBetCell, ...secondGroup] : [...firstGroup, ...secondGroup];
    }

    return isGroupByPlayerMode || isBreakDownPlayerMode ? _renderGroupByPlayerModeHeaderCell() : _renderDefaultHeader();
}

export const renderDataSection = (dataList, option) => {
    if (!dataList)
    {
        return null;
    }
    let dataRows = [];
    dataRows = dataList.map(item =>
        <TreeViewGridRowNode
            key={`row-groupby-${item.keyItem.rowIndex}-${item.keyItem.groupBy}`}
            data={item}
            option={option} />)

    return dataRows;
}


export const renderFooter = (dataItem, option) => {
    if (!dataItem)
    {
        return null;
    }
    const { isGroupByPlayerMode, isShowBAField, isBreakDownPlayerMode } = option;
    const { betCount, turnover, netTurnover, playerWinloss, playerComm, betEffective, licWinloss, licComm, amtDueToBA, margin } = dataItem;
    const totalCellColSpan = isGroupByPlayerMode || isBreakDownPlayerMode ? 3 : 2;
    return (
        <Row className="grant-total-row" key="grantTotal-row">
            <Cell className="center-align" colSpan={totalCellColSpan}>{LangHelper.getSingleResource("Total")}</Cell>
            <Cell className="right-align">{NumberFormat.format(betCount)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(turnover)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(netTurnover)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(playerWinloss)}</Cell>
            {isGroupByPlayerMode && <Cell className="right-align">{NumberFormat.formatDecimal(betEffective)}</Cell>}
            <Cell className="right-align">{NumberFormat.formatDecimal(playerComm)}</Cell>
            <Cell className={`right-align ${licWinloss < 0 ? "negative" : ""}`}>{NumberFormat.formatDecimal(licWinloss)}</Cell>
            <Cell className="right-align">{NumberFormat.formatDecimal(licComm)}</Cell>
            {isShowBAField && <Cell className="right-align">{NumberFormat.formatDecimal(amtDueToBA)}</Cell>}
            <Cell className={`right-align ${margin < 0 ? "negative" : ""}`}>{NumberFormat.formatDecimal(margin * 100)}%</Cell>
        </Row>
    )
}