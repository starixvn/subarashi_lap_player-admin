import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { tabKeys, tabList, licenseeType } from '../lib/constants';
import AccountProfile from './subtab/AccountProfile';
import BankInformation from './subtab/BankInformation';
import ActivityRecord from './subtab/ActivityRecord';
import { ActionCreators } from '../actions/index.js';
import AccountKYC from './subtab/AccountKYC';
import SubTab from './SubTab';

export const TabContent = (props) => {
    const [selectedTab, setSelectedTab] = useState(null);
    const [tabListByLicenseeType, setTabListByLicenseeType] = useState([]);
    const isAPILicensee = props.licenseeType === licenseeType.api;
    useEffect(() => {
        props.getPlayerInfoSetting().then(result => {
            const tabListFilter = tabList.filter(tab => {
                if (tab.name == tabKeys.playerProfile) {
                    return result.isShowPlayerProfile;
                }
                if (tab.name == tabKeys.bankInformation) {
                    return result.isShowBankInformation && !isAPILicensee;
                }
                if (tab.name == tabKeys.activityRecord) {
                    return result.isShowActivityRecord;
                }
                if (tab.name == tabKeys.playerKYC) {
                    return true;
                }
                return false;
            });
            const initialSubtab = tabListFilter.length > 0 ? tabListFilter[0].name : null;
            setTabListByLicenseeType(tabListFilter);
            setSelectedTab(initialSubtab);
        })
    }, []);

    const getStyle = (tabKey) => ({ display: selectedTab === tabKey ? 'block' : 'none' })

    if (tabListByLicenseeType.length == 0) return null;

    return (
        <div className="outer-col-1">
            <div className="left-side">
                <div className="subtab">
                    <SubTab dataList={tabListByLicenseeType} selectedTab={selectedTab} onChangeTab={setSelectedTab} />
                </div>
                <div className="tab-panel">
                    <div style={getStyle(tabKeys.playerProfile)}><AccountProfile /></div>
                    <div style={getStyle(tabKeys.playerKYC)}><AccountKYC /></div>
                    <div style={getStyle(tabKeys.bankInformation)}><BankInformation /></div>
                    <div style={getStyle(tabKeys.activityRecord)}><ActivityRecord /></div>
                </div>
            </div>
        </div>
    )
}

function mapStateToProps(state) {
    const { licenseeType, subTabKey } = state;
    return Object.assign({}, { licenseeType, subTabKey });
}

function mapDispatchToProps(dispatch) {
    const { getPlayerInfoSetting } = ActionCreators;
    return bindActionCreators({ getPlayerInfoSetting }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(TabContent);