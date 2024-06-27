import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../actions/index.js';
import { bindActionCreators } from 'redux';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import ClientResourceManager from '@jsroot/lib/clientResourceManager';

export const AccountKYC = (props) => {
    const { userName } = props;
    const [URL, setURL] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        setIsLoading(true)
        const param = { username: userName === "qatest10" ? "timmytest" : userName };
        props.getAccountKYC(param).then((url) => {
            setURL(url)
            setIsLoading(false)
        })
        return () => window.URL.revokeObjectURL(URL)
    }, [userName]);

    if (isLoading) return null;

    return (
        <div className="left-side-content account-kyc-container">
            {URL ? <img className="account-kyc-image" src={URL} alt="Account KYC" /> : <NoDataTitle title={ClientResourceManager.getText('NoInfo')} />}
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { getAccountKYC } = ActionCreators;
    return bindActionCreators({ getAccountKYC }, dispatch);
}

function mapStateToProps(state) {
    const { userName } = state;
    return Object.assign({}, { userName });
}

export default connect(mapStateToProps, mapDispatchToProps)(AccountKYC);