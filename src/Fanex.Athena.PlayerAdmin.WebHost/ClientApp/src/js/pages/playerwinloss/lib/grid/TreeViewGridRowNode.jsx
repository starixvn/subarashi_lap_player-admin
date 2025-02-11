import React, { useEffect, useState } from 'react';
import { Row, Cell } from '@9softstudio/react-grid';
import NumberFormat from '@jsroot/lib/numberFormat';
import BreakdownRow from './BreakdownRow';

export const TreeViewGridRowNode = (props) => {
    const [isBreakdown, setIsBreakDown] = useState(false);
    const { data, option } = props;
    const { isGroupByPlayerMode, isBreakDownPlayerMode } = option;
    if (!data) {
        return null;
    }

    const _onclick = () => {
        setIsBreakDown(!isBreakdown);
    }
    const renderDrillDownCell = (text) => {
        return <Cell className="left-align drill-down-cell" colSpan={groupByColumnColSpan}>
        <div className='flex w100' onClick={_onclick} style={{cursor: 'pointer'}}>
            <span className='w100 main-text' title={text}>{text}</span>
            <span className={`arrow-icon ${arrowClass}`}></span>
        </div>
    </Cell>
    }

    const parentData = data.keyItem;
    const children = data.children;
    const { rowIndex, displayRowIndex, groupBy, groupByFieldText, betCount, turnover, netTurnover, playerWinloss, playerComm, betEffective, licWinloss, licComm, margin, externalId } = parentData;
    const arrowClass = isBreakdown ? 'down-arrow' : 'right-arrow';
    const groupByColumnColSpan = isBreakDownPlayerMode ? 2 : 1;
    return (
        <>
            <Row className="group-row" key={`row-${rowIndex}-${groupBy}`}>
                <Cell className="center-align">{displayRowIndex}</Cell>
                { !isGroupByPlayerMode && renderDrillDownCell(groupByFieldText) }
                { isGroupByPlayerMode &&
                    <>
                        <Cell className="left-align">{groupByFieldText}</Cell>
                        {renderDrillDownCell(externalId)}
                    </>
                }
                <Cell className="right-align">{NumberFormat.format(betCount)}</Cell>
                <Cell className="right-align">{NumberFormat.formatDecimal(turnover)}</Cell>
                <Cell className="right-align">{NumberFormat.formatDecimal(netTurnover)}</Cell>
                <Cell className="right-align">{NumberFormat.formatDecimal(playerWinloss)}</Cell>
                { isGroupByPlayerMode && <Cell className="right-align">{NumberFormat.formatDecimal(betEffective)}</Cell> }
                <Cell className="right-align">{NumberFormat.formatDecimal(playerComm)}</Cell>
                <Cell className={`right-align ${licWinloss < 0 ? "negative" : ""}`}>{NumberFormat.formatDecimal(licWinloss)}</Cell>
                <Cell className="right-align">{NumberFormat.formatDecimal(licComm)}</Cell>
                <Cell className={`right-align ${margin  < 0 ? "negative" : ""}`}>{NumberFormat.formatDecimal(margin * 100)}%</Cell>
            </Row>
            {isBreakdown && children && children.length > 0 && children.map(item => (
                <BreakdownRow key={`break-down-row-${item.groupBy}-${item.breakBy}`}
                    data={item}
                    option={option}/>
            ))}
        </>
    );
}

export default TreeViewGridRowNode;

