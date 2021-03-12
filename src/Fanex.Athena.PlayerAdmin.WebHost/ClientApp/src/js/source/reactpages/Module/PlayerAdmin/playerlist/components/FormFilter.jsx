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
import { searchOptions, baStatusList, specialLevelListWithColor, emailPattern, oddsGroupListWithStyle } from '../lib/constants';

import UserNameInput from './filter/UserNameInput';
import QueryString from '@jsroot/lib/QueryString';
import { encode } from '@jsroot/lib/htmlHelper';
import LicAthenaUserGuide from '../../userGuide/LicAthenaUserGuide';
import LicApiUserGuide from '../../userGuide/LicApiUserGuide';

export class FormFilter extends PureComponent {
    constructor(props) {
        super(props);

        this.allOddsGroupString = Object.keys(oddsGroupListWithStyle).join(',');
        this.allCurrencyString = props.currencyList.map(c => c.Id).join(',');
        this.allSpecialLevel = Object.keys(specialLevelListWithColor).map(item => item).join(',');

        this.userGuide = this._renderUserGuide();
        this.userNameRef = React.createRef();

        this.state = {
            bankAccountNumber: '',
            email: '',
            currencyIdList: this.allCurrencyString,
            oddsGroup: this.allOddsGroupString,
            specialLevel: this.allSpecialLevel,
            baFilter: baStatusList.All,
            searchOption: searchOptions.matchPrecise,
            isMatchPrecise: true,
            customerClass: "",
            changeOnCustomerClass: false
        }
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

        const { type, isSubsidiary, hasCommPermissionRead } = this.props;
        this.props.getSiteSetting(type, isSubsidiary, hasCommPermissionRead);
        this.props.getLRFFilters();
    }

    componentDidUpdate(prevProps, prevState) {
        const { isAdvancedSearch } = this.props;
        if (prevProps.isAdvancedSearch !== isAdvancedSearch && !isAdvancedSearch) {
            this.setState({
                currencyIdList: this.allCurrencyString,
                oddsGroup: this.allOddsGroupString,
                specialLevel: this.allSpecialLevel,
                baFilter: baStatusList.All,
                customerClass: this.props.lrfFilters.CustomerClass.join(','),
                changeOnCustomerClass: false
            });
        }
    }

    searchPlayer() {
        const { email } = this.state
        if (email && !(new RegExp(emailPattern).test(email))) {
            window.alert(LangHelper.getSingleResource('InvalidEmail'));
            return;
        }
        const userName = this.userNameRef.current.value;
        const { pagingOption, searchPlayer, currencyList } = this.props;
        const { specialLevel, currencyIdList, oddsGroup, ...restParam } = this.state;

        const actualSpecialLevel = specialLevel.trim() === '' ? this.allSpecialLevel : specialLevel;
        const actualCurrencyIdList = currencyIdList.trim().split(',').length === currencyList.length ? '' : currencyIdList;
        const actualOddsGroup = oddsGroup.trim().split(',').length === Object.keys(oddsGroupListWithStyle).length ? '' : oddsGroup;
        const actualCustomerClass = restParam.customerClass.trim().split(',').length === this.props.lrfFilters.CustomerClass.length
            ? '' : restParam.customerClass;

        const param = {
            ...restParam,
            userName: encode(userName),
            currencyIdList: actualCurrencyIdList,
            oddsGroup: actualOddsGroup,
            specialLevel: actualSpecialLevel,
            customerClass: actualCustomerClass,
            pageSize: pagingOption.PageSize,
            pageIndex: 1
        };

        searchPlayer(param);
    }

    handleSubmit = (event) => {
        event.preventDefault();
        this.searchPlayer();
    }

    handlePressEnter = (event) => {
        if (event.charCode === 13) {
            this.searchPlayer();
        }
    }

    handleChange = (event) => {
        const { name, value } = event.target;

        if (name === "searchOption") {
            this.setState({ isMatchPrecise: value == 4 });
        }

        this.setState({ [name]: value });
    }

    handleSpecificInputChange = (name, value) => {
        if (name === "customerClass") {
            this.setState({ changeOnCustomerClass: true });
        }

        this.setState({ [name]: value });
    }

    handleUserNameChange = (value) => {
        this.userNameRef.current.value = value;
    }

    _renderContentUserGuide() {
        const currentLang = LangHelper.currentLanguage;
        return this.props.type === licenseeType.nonApi ? <LicAthenaUserGuide lang={currentLang} />
            : <LicApiUserGuide lang={currentLang} />
    }

    _renderUserGuide() {
        return (<UserGuide
            title={LangHelper.getSingleResource("UserGuide")}
            contentTitle={LangHelper.getSingleResource("PlayerAdministration")}
            isHtml={false}
            content={this._renderContentUserGuide()} />)
    }

    _getUserNameLabel() {
        return this.props.type === licenseeType.nonApi ?
            LangHelper.getSingleResource("Username") :
            `${LangHelper.getSingleResource("Username")}/${LangHelper.getSingleResource("VendorMemberID")}`
    }

    render() {
        const { bankAccountNumber, email, searchOption, ...advancedSearchParam } = this.state;
        const { type, isAdvancedSearch, isFilterCollapsing, currencyList, getSuggestionList, siteSetting } = this.props;
        const { currencyIdList, oddsGroup, specialLevel } = advancedSearchParam;

        const isValidParam = !!currencyIdList && !!oddsGroup && !!specialLevel &&
			!(this.state.changeOnCustomerClass && !this.state.customerClass); 
        const iconToggle = {
            tag: 'div',
            className: isFilterCollapsing ? 'chevron down' : 'chevron up',
            style: {}
        };

        return (
            <CollapsiblePanel className="page-filter" iconToggle={iconToggle} stickyFooter={this.userGuide}
                onCollapse={() => this.props.setCollapseFilter(true)} onExpand={() => this.props.setCollapseFilter(false)}>
                <div className="row">
                    <div className="col-12 col-md-4 col-sm-4 col-lg-3 col-xl-3">
                        <div className="form-group">
                            <label className="label-text">
                                {this._getUserNameLabel()}<span className={this.state.isMatchPrecise ? "" : "hide"}>/{LangHelper.getSingleResource("SystemId")}</span>
                                {(<SearchOption value={searchOption} onChange={this.handleChange} />)}
                            </label>
                            <UserNameInput ref={this.userNameRef} onSelectItem={(value) => this.handleUserNameChange(value)}
                                minChar={3} getSuggestionList={getSuggestionList} searchOption={searchOption}
                                onKeyPress={this.handlePressEnter} onChange={(value) => this.handleUserNameChange(value)} />
                        </div>
                    </div>

                    {type === licenseeType.nonApi && <AthenaSearch onChange={this.handleChange}
                        bankAccountNumber={bankAccountNumber}
                        email={email}
                        onKeyPress={this.handlePressEnter} />}
                    {isAdvancedSearch && <AdvancedSearch
                        currencyList={currencyList}
                        param={advancedSearchParam}
                        isAllowedBA={siteSetting.isAllowedBA}
                        onChange={this.handleSpecificInputChange}
                        lrfFilters={this.props.lrfFilters} />
                    }

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1">
                        <div className="form-group">
                            <div className="button-group">
                                <button type="button" className="btn btn-submit" disabled={!isValidParam}
                                    onClick={this.handleSubmit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <CheckBox id="isAdvancedSearch"
                                checked={isAdvancedSearch}
                                onChange={checked => this.props.setAdvancedSearch(checked)}
                                label={LangHelper.getSingleResource("AdvancedSearch")} />
                        </div>
                    </div>
                </div>
            </CollapsiblePanel>
        );
    }
}
function mapDispatchToProps(dispatch) {
    const { searchPlayer, setAdvancedSearch, setCollapseFilter, getSuggestionList, getSinglePlayer, getSiteSetting, getLRFFilters } = ActionCreators;
    return bindActionCreators({ searchPlayer, setAdvancedSearch, setCollapseFilter, getSuggestionList, getSinglePlayer, getSiteSetting, getLRFFilters }, dispatch);
}
function mapStateToProps(state) {
    const { type, isAdvancedSearch, isFilterCollapsing, pagingOption, currencyList, siteSetting, isSubsidiary, lrfFilters } = state;
    return Object.assign({}, { type, isAdvancedSearch, isFilterCollapsing, pagingOption, currencyList, siteSetting, isSubsidiary, lrfFilters });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);
