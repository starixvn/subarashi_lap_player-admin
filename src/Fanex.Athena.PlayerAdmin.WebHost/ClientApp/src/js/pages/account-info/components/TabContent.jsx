import React, { useState } from 'react';
import { connect } from 'react-redux';

import { tabKeys, tabList, licenseeType } from '../lib/constants';
import AccountProfile from './subtab/AccountProfile';
import BankInformation from './subtab/BankInformation';
import ActivityRecord from './subtab/ActivityRecord';
import SubTab from './SubTab';

export const TabContent = (props) => {
    const initialSubtab = props.subTabKey || tabKeys.playerProfile;
    const [selectedTab, setSelectedTab] = useState(initialSubtab);
    const tabListByLicenseeType = props.licenseeType === licenseeType.api ? tabList.filter(x => x.name !== tabKeys.bankInformation) : [...tabList];

    const getStyle = (tabKey) => ({ display: selectedTab === tabKey ? 'block' : 'none' })

    return (
        <div className="outer-col-1">
            <div className="left-side">
                <div className="subtab">
                    <SubTab dataList={tabListByLicenseeType} selectedTab={selectedTab} onChangeTab={setSelectedTab} />
                </div>

                <div className="tab-panel">
                    <div style={getStyle(tabKeys.playerProfile)}><AccountProfile /></div>
                    <div style={getStyle(tabKeys.bankInformation)}><BankInformation /></div>
                    {selectedTab === tabKeys.activityRecord && <ActivityRecord />}
                </div>
            </div>
        </div>
    )
}

function mapStateToProps(state) {
    const { licenseeType, subTabKey } = state;
    return Object.assign({}, { licenseeType, subTabKey });
};

export default connect(mapStateToProps, null)(TabContent);