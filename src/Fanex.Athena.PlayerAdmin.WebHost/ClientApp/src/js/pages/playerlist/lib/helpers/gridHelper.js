import React from 'react';
import { HeaderRow, HeaderCell } from '@9softstudio/react-grid';
import { fields, headerText } from '../constants';
import HeaderCustomerClassCell from '../../components/grid/HeaderCustomerClassCell';

export default class GridHelper {
    static renderHeader(displayedColumn, canSelectPlayers, hasCommPermissionRead) {
        const { Index, Status, UserName, SystemId, VendorMemberId, RealName, Currency, CurrentBalance, OutstandingBalance,
            OddsGroup, BaStatus, SpecialLevel, LastTransaction, Email, CommissionGroup, CustomerClass, RiskLevel } = fields;

        return (
            <HeaderRow style={{ height: 32 }}>
                {canSelectPlayers && displayedColumn[Index] && <HeaderCell colWidth={30}><input type="checkbox" id="chk-cust-0" data-username="" /></HeaderCell>}
                {displayedColumn[Status] && <HeaderCell colWidth={80} sortBy={Status}>{headerText.Status}</HeaderCell>}
                {displayedColumn[UserName] && <HeaderCell colWidth={120} sortBy={UserName}>{headerText.UserName}</HeaderCell>}
                {displayedColumn[SystemId] && <HeaderCell colWidth={100}>{headerText.SystemId}</HeaderCell>}
                {displayedColumn[VendorMemberId] && <HeaderCell colWidth={70}>{headerText.VendorMemberId}</HeaderCell>}
                {displayedColumn[RealName] && <HeaderCell colWidth={80} sortBy={RealName}>{headerText.RealName}</HeaderCell>}
                {displayedColumn[Currency] && <HeaderCell colWidth={60} sortBy={Currency}>{headerText.Currency}</HeaderCell>}
                {displayedColumn[CurrentBalance] && <HeaderCell colWidth={80} sortBy={CurrentBalance}>{headerText.CurrentBalance}</HeaderCell>}
                {displayedColumn[OutstandingBalance] && <HeaderCell colWidth={80} sortBy={OutstandingBalance}>{headerText.OutstandingBalance}</HeaderCell>}
                {displayedColumn[RiskLevel] && <HeaderCell colWidth={100}>{headerText.RiskLevel}</HeaderCell>}
                {displayedColumn[OddsGroup] && <HeaderCell colWidth={50} sortBy={OddsGroup}>{headerText.OddsGroup}</HeaderCell>}
                {displayedColumn[BaStatus] && <HeaderCell colWidth={50} sortBy={BaStatus}>{headerText.BaStatus}</HeaderCell>}
                {displayedColumn[SpecialLevel] && <HeaderCell colWidth={50} sortBy={SpecialLevel}>{headerText.SpecialLevel}</HeaderCell>}
                {displayedColumn[LastTransaction] && <HeaderCell colWidth={60} sortBy={LastTransaction}>{headerText.LastTransaction}</HeaderCell>}
                {displayedColumn[Email] && <HeaderCell colWidth={100}>{headerText.Email}</HeaderCell>}
                {hasCommPermissionRead && displayedColumn[CommissionGroup] && <HeaderCell colWidth={50}>{headerText.CommissionGroup}</HeaderCell>}
                {displayedColumn[CustomerClass] && <HeaderCell colWidth={80} sortBy={CustomerClass}><HeaderCustomerClassCell text={headerText.CustomerClass} /></HeaderCell>}
            </HeaderRow>
        )
    }

    static renderHeaderForArchived(displayedColumn) {
        const { Status, UserName, SystemId, Currency, CurrentBalance } = fields;

        return (
            <HeaderRow style={{ height: 32 }}>
                {displayedColumn[Status] && <HeaderCell colWidth={60} >{headerText.Status}</HeaderCell>}
                {displayedColumn[UserName] && <HeaderCell colWidth={120} sortBy={UserName}>{headerText.UserName}</HeaderCell>}
                {displayedColumn[SystemId] && <HeaderCell colWidth={100}>{headerText.SystemId}</HeaderCell>}
                {displayedColumn[Currency] && <HeaderCell colWidth={60} sortBy={Currency}>{headerText.Currency}</HeaderCell>}
                {displayedColumn[CurrentBalance] && <HeaderCell colWidth={80} sortBy={CurrentBalance}>{headerText.CurrentBalance}</HeaderCell>}
            </HeaderRow>
        )
    }

    static calculateTopPosition(dropdownHeight = 0, currentTop) {
        const innerHeight = window.innerHeight;
        const expectedHeight = dropdownHeight + currentTop + 20;

        return expectedHeight > innerHeight ? currentTop - dropdownHeight : currentTop;
    }
}
