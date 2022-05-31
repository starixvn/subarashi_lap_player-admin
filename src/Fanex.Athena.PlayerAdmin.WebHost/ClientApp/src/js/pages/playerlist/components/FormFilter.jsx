import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js';
import { bindActionCreators } from 'redux';
import CheckBox from '@jsroot/components/CheckBox';
import AdvancedSearch from './filter/AdvancedSearch';
import SearchOption from './filter/SearchOption';
import AthenaSearch from './filter/AthenaSearch';
import { licenseeType } from '../lib/constants';
import CollapsiblePanel from '@jsroot/components/collapsible-panel';
import UserGuide from '@jsroot/components/UserGuide';
import LangHelper from '@jsroot/lib/langHelper';
import {
    treeViewOption, searchOptions, baStatusList, specialLevelListWithColor,
    emailPattern, isDepositedList, responsiveFilterColumn
} from '../lib/constants';
import UserNameInput from './filter/UserNameInput';
import QueryString from '@jsroot/lib/QueryString';
import { encode } from '@jsroot/lib/htmlHelper';
import LicAthenaUserGuide from '../../userGuide/LicAthenaUserGuide';
import LicApiUserGuide from '../../userGuide/LicApiUserGuide';
import Select from 'react-select';
import { TreeViewSelect } from '@9softstudio/react-multiselect'
import { SubsidiaryViewType } from '@jsroot/constants/report';
import { getOddsGroupPdf } from '../lib/helpers/utils'
import Site from '@jsroot/lib/site';

export class FormFilter extends PureComponent {
    constructor(props) {
        super(props);

        this.allCurrencyString = props.currencyList.map(c => c.Id).join(',');
        this.allSpecialLevel = Object.keys(specialLevelListWithColor).map(item => item).join(',');
        this.isDepositedList = Object.keys(isDepositedList).map(item => ({ value: isDepositedList[item], label: LangHelper.getSingleResource(item) }));
        this.userNameRef = React.createRef();

        this.state = {
            isArchivedPlayersSearch: false,
            bankAccountNumber: "",
            email: "",
            ipLogin: "",
            isDeposited: isDepositedList["All"],
            currencyIdList: this.allCurrencyString,
            oddsGroup: "",
            specialLevel: this.allSpecialLevel,
            baFilter: baStatusList.All,
            searchOption: searchOptions.matchPrecise,
            isMatchPrecise: true,
            customerClass: "",
            commGroups: "",
            subsidiarySearch: false,
            selectedSubsidiaryList: "",
        };
    }

    componentDidMount() {
        const ibcCustId = Number(QueryString.getParam(window.location.href, 'custId'));
        const licenseeCustId = QueryString.getParam(window.location.href, 'licenseeCustId');
        if (ibcCustId || licenseeCustId) {
            const param = { ibcCustId, licenseeCustId };
            this.props.getSinglePlayer(param);

            const userName = QueryString.getParam(window.location.href, 'userName');
            this.userNameRef.current.value = userName;
        }

        const { type, hasCommPermissionRead } = this.props;
        this.props.getSubsidiarySetting();
        this.props.getDepositSetting();
        this.props.getSiteSetting(type, hasCommPermissionRead);
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevProps.depositSetting !== this.props.depositSetting 
            || (prevProps.isAdvancedSearch && !this.props.isAdvancedSearch)) {
            this.setState({
                currencyIdList: this.allCurrencyString,
                oddsGroup: this.props.depositSetting.OddsGroups.join(','),
                specialLevel: this.allSpecialLevel,
                baFilter: baStatusList.All,
                customerClass: this.props.depositSetting.CustomerClass.join(','),
                commGroups: this.props.depositSetting.CommGroups.map(c => c.Id).join(',')
            });
        }
    }

    searchPlayer() {
        const { email } = this.state;
        if (email && !(new RegExp(emailPattern).test(email))) {
            window.alert(LangHelper.getSingleResource('InvalidEmail'));
            return;
        }

        const userName = this.userNameRef.current.value;
        const { pagingOption, searchPlayer, currencyList } = this.props;
        const { specialLevel, currencyIdList, oddsGroup, customerClass, commGroups, subsidiarySearch, selectedSubsidiaryList, ...restParam } = this.state;

        const actualSpecialLevel = specialLevel.trim() === '' ? this.allSpecialLevel : specialLevel;
        const actualCurrencyIdList = currencyIdList.trim().split(',').length === currencyList.length ? '' : currencyIdList;
        const actualOddsGroup = oddsGroup.trim().split(',').length === this.props.depositSetting.OddsGroups.length ? '' : oddsGroup;
        const actualCustomerClass = customerClass.trim().split(',').length === this.props.depositSetting.CustomerClass.length ? '' : customerClass;
        const actualCommGroups = commGroups;

        const param = {
            ...restParam,
            userName: encode(userName),
            currencyIdList: actualCurrencyIdList,
            oddsGroup: actualOddsGroup,
            specialLevel: actualSpecialLevel,
            customerClass: actualCustomerClass,
            changeOnCommGroups: actualCommGroups.split(',').length != this.props.depositSetting.CommGroups.length,
            commGroups: actualCommGroups,
            subsidiarySearch,
            subsidiaryList: selectedSubsidiaryList,
            pageSize: pagingOption.PageSize,
            pageIndex: 1
        };

        searchPlayer(param);
    }

    handleSubmit = (event) => {
        event.preventDefault();
        if (this.state.isArchivedPlayersSearch) {
            const { subsidiarySearch, selectedSubsidiaryList, searchOption } = this.state;
            this.props.setParam({
                isArchivedPlayersSearch: true,
                userName: encode(this.userNameRef.current.value),
                searchOption,
                subsidiarySearch,
                subsidiaryList: selectedSubsidiaryList
            });
        } else {
            this.searchPlayer();
        }
    };

    handlePressEnter = (event) => {
        if (event.charCode === 13) {
            this.searchPlayer();
        }
    };

    handleChange = (event) => {
        const { name, value } = event.target;

        if (name === "searchOption") {
            this.setState({ isMatchPrecise: value == 4 });
        }

        this.setState({ [name]: value });
    };

    handleSpecificInputChange = (name, value) => {
        this.setState({ [name]: value });
    };

    handleUserNameChange = (value) => {
        this.userNameRef.current.value = value;
    };

    handleExportUserOddsGroup = (event) => {
        event.preventDefault();
        var fileName = this.props.siteSetting.isAllowedBA ? "Odds Group Feature_Licensee_BA.pdf" : "Odds Group Feature_Licensee.pdf";

        getOddsGroupPdf(fileName).then(response => {
            Site.downloadFile(response.data, fileName);
        })
        .catch(err => {
            console.log(err.message);
        });
    }

    _renderContentUserGuide() {
        const currentLang = LangHelper.currentLanguage;
        return this.props.type === licenseeType.nonApi ? <LicAthenaUserGuide lang={currentLang} exportOddsGroup={this.handleExportUserOddsGroup}/>
            : <LicApiUserGuide lang={currentLang} isSingleWallet={this.props.siteSetting.isEnableSingleWallet} exportOddsGroup={this.handleExportUserOddsGroup}/>
    }

    _renderUserGuide() {
        return (<UserGuide
            title={LangHelper.getSingleResource("UserGuide")}
            contentTitle={LangHelper.getSingleResource("PlayerAdministration")}
            isHtml={false}
            content={this._renderContentUserGuide()} />)
    }

    _renderSubsidiaryTreeView() {
        return <TreeViewSelect
            language={LangHelper.currentLanguage}
            id="SubsidiaryTreeView"
            dataSource={this.props.subsidiarySetting.SubsidiaryList}
            keyField="Value"
            valueField="Name"
            statusField="Checked"
            texts={{ SelectOptions: "Please select", All: "All", SelectedItemCount: "# selected" }}
            isAllTextShown={false}
            hasSearchBox={false}
            treeViewOption={treeViewOption}
            onChange={(currentItem, selectedItemsKey, treeViewList) => this.handleSpecificInputChange('selectedSubsidiaryList', selectedItemsKey)} />;
    }

    _getUserNameLabel() {
        return this.props.type === licenseeType.nonApi || this.state.isArchivedPlayersSearch ?
            LangHelper.getSingleResource("Username") :
            `${LangHelper.getSingleResource("Username")}/${LangHelper.getSingleResource("VendorMemberID")}`
    }

    render() {
        const { bankAccountNumber, email, searchOption, ipLogin, isDeposited, subsidiarySearch, isArchivedPlayersSearch, selectedSubsidiaryList, ...advancedSearchParam } = this.state;
        const { type, isSubsidiary, isAdvancedSearch, isFilterCollapsing, currencyList,
            getSuggestionList, siteSetting, subsidiarySetting } = this.props;
        const { currencyIdList, oddsGroup, specialLevel, customerClass, commGroups } = advancedSearchParam;

        const userGuide = this._renderUserGuide();

        const isValidParam = !!currencyIdList && !!oddsGroup && !!specialLevel 
            && !!customerClass && !!commGroups &&
            (!subsidiarySearch || selectedSubsidiaryList);

        const iconToggle = {
            tag: 'div',
            className: isFilterCollapsing ? 'chevron down' : 'chevron up',
            style: {}
        };

        return (
            <CollapsiblePanel className="page-filter" iconToggle={iconToggle} stickyFooter={userGuide}
                onCollapse={() => this.props.setCollapseFilter(true)} onExpand={() => this.props.setCollapseFilter(false)}>
                <div className="row">
                    {subsidiarySetting.isAllowedSubsidiarySearch && subsidiarySearch && (<div className="col-12 col-md-4 col-sm-6 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">
                                {subsidiarySetting.SubsidiaryViewType === SubsidiaryViewType.MASTER_MANAGEMENT ?
                                    LangHelper.getSingleResource("WhiteLabel") : LangHelper.getSingleResource("Subsidiary")}
                            </label>
                            {this._renderSubsidiaryTreeView()}
                        </div>
                    </div>)}
                    <div className={responsiveFilterColumn["username"][type]}>
                        <div className="form-group">
                            <label className="label-text">
                                {this._getUserNameLabel()}<span className={this.state.isMatchPrecise ? "" : "hide"}>/{LangHelper.getSingleResource("SystemId")}</span>
                                <div className="search-option-container">
                                    <SearchOption value={searchOption} onChange={this.handleChange} />
                                </div>
                            </label>
                            <UserNameInput ref={this.userNameRef} onSelectItem={(value) => this.handleUserNameChange(value)}
                                minChar={3} getSuggestionList={isArchivedPlayersSearch ? null : getSuggestionList} searchOption={searchOption}
                                onKeyPress={this.handlePressEnter} onChange={(value) => this.handleUserNameChange(value)} />
                        </div>
                    </div>
                    {!isArchivedPlayersSearch && type === licenseeType.nonApi && <AthenaSearch onChange={this.handleChange}
                        bankAccountNumber={bankAccountNumber}
                        email={email}
                        ipLogin={ipLogin}
                        onKeyPress={this.handlePressEnter} />}
                    {!isArchivedPlayersSearch && <div className={responsiveFilterColumn["deposited"][type]}>
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Deposited")}</label>
                            <Select instanceId='ddlDeposited'
                                name='form-field-name'
                                value={isDeposited}
                                options={this.isDepositedList}
                                onChange={(item) => this.handleSpecificInputChange('isDeposited', item.value)} />
                        </div>
                    </div>}
                    {!isArchivedPlayersSearch && isAdvancedSearch && <AdvancedSearch
                        currencyList={currencyList}
                        param={advancedSearchParam}
                        isAllowedBA={siteSetting.isAllowedBA}
                        isSubsidiary={isSubsidiary}
                        onChange={this.handleSpecificInputChange}
                        type={type}
                        depositSetting={this.props.depositSetting} />
                    }
                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1">
                        <div className="form-group">
                            <div className="button-group">
                                <button type="button" className="btn btn-submit" disabled={!isValidParam}
                                    onClick={this.handleSubmit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>
                    <div className={`col-12 col-md-7 col-sm-6 col-lg-${subsidiarySetting.isAllowedSubsidiarySearch ? 6 : 4} col-xl-${subsidiarySetting.isAllowedSubsidiarySearch ? 5 : 3}`} >
                        <div className="form-group">
                            {!isArchivedPlayersSearch && <CheckBox id="isAdvancedSearch"
                                checked={isAdvancedSearch}
                                onChange={this.props.setAdvancedSearch}
                                label={LangHelper.getSingleResource("AdvancedSearch")} />}
                            {subsidiarySetting.isAllowedSubsidiarySearch &&
                                <CheckBox id="SubsidiarySearch" className="SubsidiarySearch"
                                    checked={subsidiarySearch}
                                    onChange={checked => {
                                        this.handleSpecificInputChange('subsidiarySearch', checked);
                                        this.handleSpecificInputChange('selectedSubsidiaryList', checked ? subsidiarySetting.defaultSelectedSubsidiaryList : '');
                                    }}
                                    label={LangHelper.getSingleResource("SubsidiarySearch")} />
                            }
                            {<CheckBox id="isArchivedPlayersSearch"
                                checked={isArchivedPlayersSearch}
                                onChange={checked => this.handleSpecificInputChange('isArchivedPlayersSearch', checked)}
                                label={LangHelper.getSingleResource("DormantPlayersSearch")} />}
                        </div>
                    </div>
                </div>
            </CollapsiblePanel>
        );
    }
}

function mapDispatchToProps(dispatch) {
    const { searchPlayer, setAdvancedSearch, setParam, setCollapseFilter, getSuggestionList,
        getSinglePlayer, getSiteSetting, getDepositSetting, getSubsidiarySetting } = ActionCreators;
    return bindActionCreators({
        searchPlayer, setAdvancedSearch, setParam, setCollapseFilter, getSuggestionList,
        getSinglePlayer, getSiteSetting, getDepositSetting, getSubsidiarySetting
    }, dispatch);
}

function mapStateToProps(state) {
    const { type, isSubsidiary, hasCommPermissionRead, isAdvancedSearch, isFilterCollapsing, pagingOption, currencyList, siteSetting, depositSetting, subsidiarySetting } = state;
    return Object.assign({}, { type, isSubsidiary, hasCommPermissionRead, isAdvancedSearch, isFilterCollapsing, pagingOption, currencyList, siteSetting, depositSetting, subsidiarySetting });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);
