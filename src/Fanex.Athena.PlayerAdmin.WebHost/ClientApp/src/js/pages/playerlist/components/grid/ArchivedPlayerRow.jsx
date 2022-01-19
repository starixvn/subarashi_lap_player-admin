import React from 'react';
import { fields } from '../../lib/constants';
import { Row, Cell } from '@9softstudio/react-grid';
import NumberFormat from '@jsroot/lib/numberFormat';
import LangHelper from '@jsroot/lib/langHelper';

export const ArchivedPlayerRow = (props) => {
    const {
        dataItem,
        displayedColumn,
    } = props;

    const {
        IbcCustId,
        SystemId,
        UserName,
        Currency,
        CurrentBalance,
    } = dataItem;

    return (
        <Row key={IbcCustId}>
            {displayedColumn[fields.Status] && <Cell>{LangHelper.getSingleResource('Archived')}</Cell>}
            {displayedColumn[fields.UserName] && <Cell title={UserName}>{UserName}</Cell>}
            {displayedColumn[fields.SystemId] && <Cell title={SystemId}>{SystemId}</Cell>}
            {displayedColumn[fields.Currency] && <Cell className="center-align">{Currency}</Cell>}
            {displayedColumn[fields.CurrentBalance] && <Cell className="right-align" title={NumberFormat.roundDownFormatDecimal(CurrentBalance)}>{NumberFormat.roundDownFormatDecimal(CurrentBalance)}</Cell>}
        </Row>
    )
}

export default ArchivedPlayerRow;
