import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../actions/index.js';
import { bindActionCreators } from 'redux';
import NoDataTitle from '@jsroot/components/NoDataTitle';

export const AccountKYC = (props) => {
    const { userName } = props;
    const [data, setData] = useState(null);

    useEffect(() => {
        const param = { username: userName };
        props.getAccountKYC(param).then(setData)
    }, [userName]);

    return (
        <div className="left-side-content account-kyc-container">
            {data ? <img className="account-kyc-image" src={data} alt="Account KYC" /> : <NoDataTitle />}
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