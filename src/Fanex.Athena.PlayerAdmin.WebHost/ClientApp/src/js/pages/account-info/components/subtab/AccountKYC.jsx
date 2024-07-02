import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../actions/index.js';
import { bindActionCreators } from 'redux';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import ClientResourceManager from '@jsroot/lib/clientResourceManager';
import SweetAlert from '@jsroot/components/sweet-alert';
import { predefinedIcons } from '@jsroot/lib/sweetAlert';


export const AccountKYC = (props) => {
    const { userName, sweetAlertOption, setSweetAlertOption, getAccountKYC, deleteKYCImage } = props;
    const [url, setUrl] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    const hardcodeAccount = ["qatest10"]

    useEffect(() => {
        setIsLoading(true)
        const param = { username: hardcodeAccount.includes(userName) ? "timmytest" : userName };
        getAccountKYC(param).then((url) => {
            setUrl(url)
            setIsLoading(false)
        })
        return () => window.URL.revokeObjectURL(url)
    }, [userName]);


    const _showResult = (title, msg, isSuccess) => {
        const content = (<p className="center-align">{msg}</p>);

        const buttons = [
            {
                text: ClientResourceManager.getText('OK'),
                onClick: _handleCloseAlert,
                className: 'btn btn-apply btn-alone'
            }];

        setSweetAlertOption({
            isShow: true,
            title: title,
            children: content,
            onClose: _handleCloseAlert,
            icon: isSuccess ? predefinedIcons.success : predefinedIcons.error,
            height: 225,
            buttons: buttons
        });

    }

    const _showConfirmation = (onOK) => {
        const buttons = [
            {
                text: ClientResourceManager.getText('OK'),
                onClick: onOK,
                className: 'btn btn-apply'
            }, {
                text: ClientResourceManager.getText('Cancel'),
                onClick: _handleCloseAlert,
                className: 'btn btn-cancel'
            }];

        const msg = (<p className="center-align">{ClientResourceManager.getText('ConfirmDeleteKYCImage')}</p>);

        setSweetAlertOption({
            isShow: true,
            title: ClientResourceManager.getText('Confirmation'),
            children: msg,
            buttons: buttons,
            icon: '',
            onClose: _handleCloseAlert,
            height: 130
        })
    }


    const _handleCloseAlert = () => {
        setSweetAlertOption(null);
    }

    const _deleteKYCImageHandler = () => {
        _showConfirmation(() => {
            const param = { username: hardcodeAccount.includes(userName) ? "timmytest" : userName };
            deleteKYCImage(param).then((data) => {
                const Success = data && data.ErrorCode == 0
                const resultMsg = Success ? ClientResourceManager.getText('DeleteSuccessMsg') : ClientResourceManager.getText("ErrorProcessingMsg");
                const titleMsg = Success ? ClientResourceManager.getText('Success') : ClientResourceManager.getText('Error');
                _showResult(titleMsg, resultMsg, Success)

                if (Success) {
                    setUrl(null)
                }
            })
        });
    }

    const _renderLoading = () => <div className="icon-loading">
        <div className="loader-inner ball-beat">
            <div></div>
            <div></div>
            <div></div>
        </div>
    </div>

    const _renderImageKYC = () => <>
        <img className="account-kyc-image" src={url} alt="Account KYC" />
        <button className="kyc-delete-btn " onClick={_deleteKYCImageHandler} >
            <i className="icon-delete" />
        </button>
    </>

    if (isLoading) return (
        <div className="left-side-content account-kyc-container">
            {_renderLoading()}
        </div>
    )

    return (
        <div className="left-side-content account-kyc-container">
            {url ? _renderImageKYC() : <NoDataTitle title={ClientResourceManager.getText('NoInfo')} />}
            {sweetAlertOption && <SweetAlert {...sweetAlertOption} />}
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { getAccountKYC, deleteKYCImage, setSweetAlertOption } = ActionCreators;
    return bindActionCreators({ getAccountKYC, deleteKYCImage, setSweetAlertOption }, dispatch);
}

function mapStateToProps(state) {
    const { userName, sweetAlertOption } = state;
    return Object.assign({}, { userName, sweetAlertOption });
}

export default connect(mapStateToProps, mapDispatchToProps)(AccountKYC);