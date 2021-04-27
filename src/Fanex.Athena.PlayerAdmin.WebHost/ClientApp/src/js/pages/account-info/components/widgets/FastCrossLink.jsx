import React, { useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../../actions';
import { systemType } from '@jsroot/constants/config';
import Popup from '@jsroot/components/popup';
import ClientResourceManager from '@jsroot/lib/clientResourceManager';
import FactoryCommonService from '@jsroot/lib/factoryCommonService';

const defaultPopupInfo = {url: null, title: null, width: 0, height: 0};

export const FastCrossLink = (props) => {
    const { userName, currencyName, getLink } = props;

    const singleCustomerActionLink = `PlayerBtcSingleCustomer?userName=${userName}&currencyName=${currencyName}&lapToken={0}`;

    const customerWinLossActionLink = `PlayerWinLoss?userName=${userName}&currencyName=${currencyName}`;
    const customerWinLossUrl = getLink(systemType.playerAdmin, customerWinLossActionLink);

    const [popupInfo, setPopupInfo] = useState(defaultPopupInfo);
    const isShowPopup = !!popupInfo.url;

    const handleClickLink = (event, popup) => {
        event.preventDefault();
        setPopupInfo(popup);
    }

    const handlClickSingleCustomer = async event => {
        event.preventDefault();
        let token = '';
        var lapService = FactoryCommonService.createInstance();
        try {
            const { Token } = await lapService.fetchTokenFromLap(0, '/');
            token = Token;
        } catch(error) { console.log(error) }

        const singleCustomerUrl = getLink(systemType.playerAdmin, singleCustomerActionLink.replace('{0}', token));
        const popupInfo = { ...popupSingleCustomer, url: singleCustomerUrl }
        setPopupInfo(popupInfo);
    }

    const popupSingleCustomer = {
        title: ClientResourceManager.getText('BettingTicketCenter') + ' - ' + ClientResourceManager.getText('SingleCustomer'),
        width: window.innerWidth - 10,
        height: window.innerHeight - 10
    };

    const popupCustomerWinLoss = {
        url: customerWinLossUrl, 
        title: ClientResourceManager.getText('CustomerWinLoss'),
        width: window.innerWidth - 10,
        height: 300
    };

    return (
        <div className="panel">
            <div className="panel-header">{ClientResourceManager.getText('FastCrossLink')}</div>
            <div className="panel-body">
                <div>
                    <span>{ClientResourceManager.getText('BettingTicketCenter')}</span>
                    <strong className="divider">{'>'}</strong>
                    <span><a className="link" onClick={handlClickSingleCustomer}>{ClientResourceManager.getText('SingleCustomer')}</a></span>
                </div>
                <div><span><a className="link" onClick={(e) => handleClickLink(e, popupCustomerWinLoss)}>{ClientResourceManager.getText('CustomerWinLoss')}</a></span></div>
            </div>

            <Popup isShow={isShowPopup} 
                            iframeUrl={popupInfo.url}
                            title={popupInfo.title}
                            width={popupInfo.width} height={popupInfo.height}
                            onClose={() => setPopupInfo(defaultPopupInfo)} />
        </div>
    );
}

function mapDispatchToProps(dispatch) {
    const { getLink } = ActionCreators;
    return bindActionCreators({ getLink }, dispatch);
}

function mapStateToProps(state) {
    const { userName, currencyName } = state;
    return Object.assign({}, { userName, currencyName });
};

export default connect(mapStateToProps, mapDispatchToProps)(FastCrossLink);