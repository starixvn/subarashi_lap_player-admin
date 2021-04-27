import React from 'react';
import CustomerBettingStatus from './widgets/CustomerBettingStatus';
import FastCrossLink from './widgets/FastCrossLink';

export const WidgetContent = () => {
    return (
        <div className="outer-col-2">
            <div className="cust-betting-container"><CustomerBettingStatus /></div>
            <div className="fast-cross-link-container"><FastCrossLink /></div>
        </div>
    )
}

export default WidgetContent;