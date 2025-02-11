import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../../actions';

import { fields, playerStatusByName } from '../../lib/constants';
import { Row, Cell } from '@9softstudio/react-grid';
import NumberFormat from '@jsroot/lib/numberFormat';

import UserNameCell from './UserNameCell';
import OddsGroupCell from './OddsGroupCell';
import SpecialLevelCell from './SpecialLevelCell';
import StatusCell from './StatusCell';
import RiskLevelCell from './RiskLevelCell';
import LangHelper from '@jsroot/lib/langHelper';

export const PlayerRow = (props) => {
    const {
        dataItem,
        displayedColumn,
        canSelectPlayers,
        hasCommPermissionRead,
        isShowSabaIntelligence
    } = props;

    const {
        LicenseeCustId,
        SystemId,
        RealName,
        Currency,
        CurrentBalance,
        OutstandingBalance,
        LastTransaction,
        Email,
        OddsGroup,
        SpecialLevel,
        IsDeposit,
        IsArchived,
        CommissionGroup,
        VendorMemberId,
        RiskLevelKey,
        RiskLevel,
        CurrencyId
    } = dataItem;
    const canShowMainColumn = IsDeposit && !IsArchived;
    const commissionGroup = CommissionGroup || LangHelper.getSingleResource('None');
    const riskLevelKey = RiskLevelKey || 'Uncategorized';
    const riskLevel = RiskLevel || LangHelper.getSingleResource('Uncategorized');

    const playerIsEditable = IsDeposit && !IsArchived && dataItem.Status != playerStatusByName.Closed;
    const customerClass = dataItem.CustomerClass == "0" || !dataItem.CustomerClass
        ? LangHelper.getSingleResource('None') : dataItem.CustomerClass;

    return (
        <Row key={LicenseeCustId}>
            {canSelectPlayers && displayedColumn[fields.Index] && !playerIsEditable && <Cell className="center-align"></Cell>}
            {canSelectPlayers && displayedColumn[fields.Index] && playerIsEditable &&
                <Cell className="center-align">
                    <input type="checkbox" id={"chk-cust-" + LicenseeCustId} data-username={dataItem.UserName}
                        data-custid={dataItem.IbcCustId} className="chk-cust" defaultChecked={dataItem.Selected} />
                </Cell>}
            {displayedColumn[fields.Status] && <Cell><StatusCell dataItem={dataItem} /></Cell>}
            {displayedColumn[fields.UserName] && <Cell><UserNameCell dataItem={dataItem} hasCommPermissionRead={hasCommPermissionRead} isShowSabaIntelligence={isShowSabaIntelligence} /></Cell>}
            {displayedColumn[fields.SystemId] && <Cell title={SystemId}>{SystemId}</Cell>}
            {displayedColumn[fields.VendorMemberId] && <Cell>{VendorMemberId}</Cell>}
            {displayedColumn[fields.RealName] && <Cell>{RealName}</Cell>}
            {displayedColumn[fields.Currency] && <Cell className="center-align">{Currency}</Cell>}
            {displayedColumn[fields.CurrentBalance] && <Cell className="right-align" title={canShowMainColumn ? NumberFormat.roundDownFormatDecimal(CurrentBalance) : undefined}>{canShowMainColumn && NumberFormat.roundDownFormatDecimal(CurrentBalance)}</Cell>}
            {displayedColumn[fields.OutstandingBalance] && <Cell className="right-align" title={canShowMainColumn ? NumberFormat.roundDownFormatDecimal(OutstandingBalance) : undefined}>{canShowMainColumn && NumberFormat.roundDownFormatDecimal(OutstandingBalance)}</Cell>}
            {displayedColumn[fields.RiskLevel] && <RiskLevelCell riskLevel={riskLevel} riskLevelKey={riskLevelKey} customerClass={dataItem.CustomerClass} currencyId={CurrencyId} custId={dataItem.IbcCustId} />}
            {displayedColumn[fields.OddsGroup] && <Cell className="center-align">{canShowMainColumn && OddsGroup !== undefined && <OddsGroupCell dataItem={dataItem} />}</Cell>}
            {displayedColumn[fields.SpecialLevel] && <Cell className="center-align">{canShowMainColumn && SpecialLevel !== undefined && <SpecialLevelCell dataItem={dataItem} />}</Cell>}
            {displayedColumn[fields.LastTransaction] && <Cell>{LastTransaction}</Cell>}
            {displayedColumn[fields.Email] && <Cell>{Email}</Cell>}
            {hasCommPermissionRead && displayedColumn[fields.CommissionGroup] && <Cell className={"text-center td-comm-" + dataItem.IbcCustId} title={commissionGroup}>{commissionGroup}</Cell>}
            {displayedColumn[fields.CustomerClass] && <Cell className="text-center">{customerClass}</Cell>}
        </Row>
    )
}

function mapDispatchToProps(dispatch) {
    const { setSelectedPlayer } = ActionCreators;
    return bindActionCreators({ setSelectedPlayer }, dispatch);
}

export default connect(null, mapDispatchToProps)(PlayerRow);
