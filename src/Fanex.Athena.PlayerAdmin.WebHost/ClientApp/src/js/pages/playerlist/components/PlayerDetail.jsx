import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import { heightSlicing, excludedTabNameWithNonDepositAndArchived, tabNames, excludedTabNameWithClosed } from '../lib/constants';

export class PlayerDetail extends PureComponent {
    constructor(props) {
        super(props);
        this._tabList = this.props.getTabList();
    }

    _getDefaultSelectedTab() {
        return this._tabList && this._tabList.length && this._tabList[0].Name;
    }

    handleClickMenu = (event, tabName) => {
        event.preventDefault();
        this.props.setSelectedTabName(tabName);
        this.props.setSelectedSubTabName(null);
    }

    _buildActionUrl(systemType, action, tabName) {
        const { selectedPlayer } = this.props;
        const { ibcCustId, licenseeCustId, currencyId, currency, userName, createdDate } = selectedPlayer;
        const custIdQueryString = `custId=${ibcCustId}&licenseeCustId=${licenseeCustId}`;
        const nameValuePairString = custIdQueryString + `&currencyId=${currencyId}&currencyName=${currency}&userName=${userName}`;
        const queryString = (action.indexOf('?') !== -1 ? '&' : '?') + nameValuePairString;

        let urlByPlayer = action + queryString;
        if (tabName === tabNames.playerInfo) {
            urlByPlayer += `&createdDate=${encodeURIComponent(createdDate)}`;
        }

        return this.props.getLink(systemType, urlByPlayer);
    }

    _renderTabList(tabList) {
        const { selectedPlayer } = this.props;
        const selectedTabName = this.props.selectedTabName || this._getDefaultSelectedTab();
        const { isDeposit, isArchived } = selectedPlayer;

        let renderedTabList = !isDeposit || isArchived ?
            tabList.filter(item => !excludedTabNameWithNonDepositAndArchived.includes(item.Name)) : tabList;

        if (selectedPlayer.isClosed) {
            renderedTabList = renderedTabList.filter(item => !excludedTabNameWithClosed.includes(item.Name));
        }

        return (
            renderedTabList.map((item) => {
                const actionUrl = this._buildActionUrl(item.SystemType, item.Action, item.Name);
                const isActive = selectedTabName === item.Name;

                return (
                    <li className="tab" key={item.Name}>
                        <a className={isActive ? "active" : ""}
                            href={actionUrl} onClick={(event) => this.handleClickMenu(event, item.Name)}>
                            {item.Text}
                        </a>
                    </li>
                )
            })
        )
    }

    _getFrameHeight() {
        const { isAdvancedSearch, isFilterCollapsing } = this.props;
        let omittedHeight;

        if (isFilterCollapsing) {
            omittedHeight = heightSlicing.whenCollapsing;
        }
        else {
            const key = isAdvancedSearch ? 'withAdvancedSearch' : 'default';
            omittedHeight = heightSlicing[key];
        }

        return window.innerHeight - omittedHeight;
    }

    _getActionUrl() {
        const { selectedTabName, selectedSubTabName } = this.props;
        const tabItem = selectedTabName ? this._tabList.find(t => t.Name === selectedTabName) : this._tabList[0];
        const action = selectedSubTabName
            ? `${tabItem.Action}?timestamp=${new Date().getTime()}&subTab=${selectedSubTabName}`
            : `${tabItem.Action}?timestamp=${new Date().getTime()}`;
        return tabItem && this._buildActionUrl(tabItem.SystemType, action, tabItem.Name);
    }

    render() {
        const { selectedPlayer } = this.props;

        if (!selectedPlayer || this._tabList.length === 0) {
            return null;
        }

        const src = this._getActionUrl();
        if (!src) {
            return null;
        }

        const frameHeight = this._getFrameHeight();

        return (
            <div className="pa-detail-container">
                <div className="page-filter-tab">
                    <div className="header-tab">
                        <div className="content-tab">
                            <ul className="tabs">
                                {this._renderTabList(this._tabList)}
                            </ul>
                        </div>
                    </div>
                    <div className="tab-panel">
                        <div className="pa-frame-container">
                            <iframe id="paDetailFrame" style={{ height: frameHeight }} name="paDetailFrame" frameBorder="0" src={src}></iframe>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

function mapDispatchToProps(dispatch) {
    const { getLink, getTabList, setSelectedTabName, setSelectedSubTabName } = ActionCreators;
    return bindActionCreators({ getLink, getTabList, setSelectedTabName, setSelectedSubTabName }, dispatch);
}

function mapStateToProps(state) {
    const { selectedPlayer, isAdvancedSearch, isFilterCollapsing, selectedTabName, selectedSubTabName } = state;
    return Object.assign({}, { selectedPlayer, isAdvancedSearch, isFilterCollapsing, selectedTabName, selectedSubTabName });
};

export default connect(mapStateToProps, mapDispatchToProps)(PlayerDetail);
