import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../actions/index.js';
import { bindActionCreators } from 'redux';
import AthenaProfile from './AthenaProfile';
import ApiProfile from './ApiProfile';
import { licenseeType } from '../../lib/constants';
import { SubsidiaryViewType } from '@jsroot/constants/report'

export const AccountProfile = (props) => {
    const { licenseeCustId } = props;
    const [data, setData] = useState(null);
    const [subsidiaryViewType, setSubsidiaryViewType] = useState(SubsidiaryViewType.NOT_MANAGEMENT);

    useEffect(() => {
        const param = { licenseeCustId };
        props.getAccountProfile(param).then(setData);
        props.getSubsidiaryViewType(param).then(setSubsidiaryViewType);
    }, [licenseeCustId]);
    
    return (
        <div className="left-side-content account-profile-container">
            <table className="table">
                {props.licenseeType === licenseeType.api ? 
                <ApiProfile data={data} subsidiaryViewType={subsidiaryViewType} /> : <AthenaProfile data={data} />}
            </table>
        </div>
    );
}

function mapDispatchToProps(dispatch) {
    const { getAccountProfile, getSubsidiaryViewType } = ActionCreators;
    return bindActionCreators({ getAccountProfile, getSubsidiaryViewType }, dispatch);
}

function mapStateToProps(state) {
    const { licenseeCustId, licenseeType } = state;
    return Object.assign({}, { licenseeCustId, licenseeType });
};

export default connect(mapStateToProps, mapDispatchToProps)(AccountProfile);