import React, { useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../../actions';
import { systemType } from '@jsroot/constants/config';
import Popup from '@jsroot/components/popup';
import ClientResourceManager from '@jsroot/lib/clientResourceManager';

const defaultPopupInfo = {url: null, title: null, width: 0, height: 0};

export const FastCrossLink = (props) => {
    const { userName, currencyName, getLink } = props;

    const singleCustomerActionLink = `PlayerBtcSingleCustomer?userName=${userName}&currencyName=${currencyName}`;
    const singleCustomerUrl = getLink(systemType.playerAdmin, singleCustomerActionLink);

    const winLossAnalysisActionLink = `PlayerWinLoss?userName=${userName}&currencyName=${currencyName}`;
    const winLossAnalysisUrl = getLink(systemType.playerAdmin, winLossAnalysisActionLink);

    const [popupInfo, setPopupInfo] = useState(defaultPopupInfo);
    const isShowPopup = !!popupInfo.url;

    const handleClickLink = (event, popup) => {
        event.preventDefault();
        setPopupInfo(popup);
    }

    const popupSingleCustomer = {
        url: singleCustomerUrl,
        title: ClientResourceManager.getText('BettingTicketCenter') + ' - ' + ClientResourceManager.getText('SinglePlayer'),
        width: window.innerWidth - 10,
        height: window.innerHeight - 10
    };

    const popupWinLossAnalysis = {
        url: winLossAnalysisUrl, 
        title: ClientResourceManager.getText('WinLossAnalysis'),
        width: window.innerWidth - 10,
        height: 430
    };

    return (
        <div className="panel">
            <div className="panel-header">{ClientResourceManager.getText('FastCrossLink')}</div>
            <div className="panel-body">
                <div>
                    <span>{ClientResourceManager.getText('BettingTicketCenter')}</span>
                    <strong className="divider">{'>'}</strong>
                    <span><a className="link" onClick={e => handleClickLink(e, popupSingleCustomer)}>{ClientResourceManager.getText('SinglePlayer')}</a></span>
                </div>
                <div><span><a className="link" onClick={e => handleClickLink(e, popupWinLossAnalysis)}>{ClientResourceManager.getText('WinLossAnalysis')}</a></span></div>
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