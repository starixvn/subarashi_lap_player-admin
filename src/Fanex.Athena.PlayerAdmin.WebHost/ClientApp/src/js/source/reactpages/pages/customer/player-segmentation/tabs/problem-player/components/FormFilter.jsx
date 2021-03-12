import React, { PureComponent } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import Select from 'react-select'
import MultipleSelect, { TreeViewSelect } from '@9softstudio/react-multiselect'
import AutoComplete from '@jsroot/components/auto-complete'
import CollapsiblePanel from '@jsroot/components/collapsible-panel'
import UserGuide from '@jsroot/components/UserGuide'
import CheckBox from '@jsroot/components/CheckBox'
import { DateRange } from '@jsroot/components/date-range'
import LangHelper from '@jsroot/lib/langHelper'
import StringHelper from '@jsroot/lib/stringHelper'
import { decode } from '@jsroot/lib/htmlHelper'
import Site from '@jsroot/lib/site';
import AppData from '@jsroot/lib/AppData'
import DataService from '@jsroot/lib/DataService.js'
import { ActionCreators } from '../actions'
import { Url } from '../lib/constants';
import { ViewType, TreeViewOption } from '../../../lib/constants';
import ParamHelper from '../lib/helpers/paramHelper';
import { bindDataUserGuideWithPermission } from '../lib/helpers/userGuideHelper';
import DataSourceHelper from '../../../lib/helpers/dataSourceHelper';
import GridHelper from '../../../lib/helpers/gridHelper';

export class FormFilter extends PureComponent {
    constructor(props) {
        super(props);
    }

    _submit = (event) => {
        event.preventDefault();
        const { setSelectedParams, searchPlayer, selectedTabKey } = this.props;
        const params = ParamHelper.ToAPIParams(this.props);
        if (!params) {
            return;
        }
        setSelectedParams(params);
        searchPlayer(selectedTabKey, params, GridHelper.callBackToUpdatePaging(this.props));
    }

    exportExcel = (event) => {
        event.preventDefault();
        const { selectedParams, pagingOption } = this.props;
        if (!selectedParams) {
            return;
        }
        const sortInfo = GridHelper.getSortInfoFromPagingOption(pagingOption);
        const params = {
            ...selectedParams,
            ...sortInfo
        }

        DataService.getFile(Url.exportProblemPlayerList, params)
            .then((response) => {
                const NoContentStatusCode = 204;
                if (response.status === NoContentStatusCode) {
                    alert(LangHelper.getSingleResource("NoInfo"));
                } else {
                    Site.downloadFile(response.data, 'ProblemPlayerList.xls');
                }
            });
    }

    _renderDateRange() {
        const { isGetAll, dateRange, selectedTabKey } = this.props;
        const dateConstraint = AppData[selectedTabKey].dateConstraint;
        return (
            <div className={isGetAll ? 'datepicker-disabled' : ''}>
                <DateRange
                    startDate={dateRange.fromDate} endDate={dateRange.toDate}
                    minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                    toDate={dateRange.toDate} limitedMonthRange={dateConstraint.limitedMonthRange}
                    onChange={this._onChangeDateRange}
                />
            </div>
        )
    }

    _onChangeDateRange = (date, isValidMonthRange = true) => {
        if (isValidMonthRange) {
            this.props.setDateRange(date);
        }
        else {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", 1);
            alert(message);
        }
    }

    _renderCurrencySelectList() {
        const { selectedCurrency, selectedTabKey } = this.props;
        const currencyList = AppData[selectedTabKey].currencyList;
        const dataSource = DataSourceHelper.CreateDataSourceFrom(currencyList, selectedCurrency);
        return currencyList && currencyList.length && (
            <MultipleSelect id="ddlCurrency"
                language={LangHelper.currentLanguage}
                dataSource={dataSource}
                keyField={"key"}
                valueField={"value"}
                statusField={"status"}
                onChange={(_, selectedItemsKey) => this.props.setSelectedCurrency(selectedItemsKey)} />);
    }

    _renderBaseCurrencySelectList() {
        const baseCurrencyList = AppData.baseCurrencyList;
        return baseCurrencyList && baseCurrencyList.length && (
            <Select instanceId="ddlBased"
                value={this.props.selectedBaseCurrency}
                options={baseCurrencyList}
                onChange={(item) => this.props.setSelectedBaseCurrency(item.value)} />
        );
    }

    _renderReasonSelectList() {
        const { selectedReason, selectedTabKey } = this.props;
        const reasonList = AppData[selectedTabKey].reasonList;
        const dataSource = DataSourceHelper.CreateDataSourceFrom(reasonList, selectedReason);
        return reasonList && reasonList.length && (
            <MultipleSelect id="ddlReason"
                language={LangHelper.currentLanguage}
                dataSource={dataSource}
                keyField={"key"}
                valueField={"value"}
                statusField={"status"}
                onChange={(_, selectedItemsKey) => this.props.setReason(selectedItemsKey)} />);
    }

    showUserGuide = () => {
        const userGuideContent = this.props.userGuideContent;
        if (!userGuideContent || userGuideContent.length === 0) {
            this.props.getUserGuideContent('PlayerSegmentationProblem');
        }
    }

    _renderUserGuide() {
        const userGuideContent = bindDataUserGuideWithPermission(this.props.userGuideContent, AppData.HasVipManagement);
        return (<UserGuide
            title={LangHelper.getSingleResource("UserGuide")}
            contentTitle={`${LangHelper.getSingleResource("PlayerSegmentation")} / ${LangHelper.getSingleResource('ProblemPlayer')}`}
            onClick={this.showUserGuide}
            content={userGuideContent} />)
    }

    _renderStickyFooter() {
        return (<div>
            <i className="icon-excel right" onClick={this.exportExcel} title={LangHelper.getSingleResource("ExportToExcel")}></i>
            {this._renderUserGuide()}
        </div>);
    }

    _onChangeSubsidiaryList = (currentItem, selectedItemsKey, treeViewList) => {
        this.props.setSubsidiaryList(selectedItemsKey);
    }

    _renderSubsidiaryTreeView() {
        return <TreeViewSelect
            language={LangHelper.currentLanguage}
            id="SubsidiaryTreeView"
            dataSource={AppData.subsidiaryList}
            keyField="Value"
            valueField="Name"
            statusField="Checked"
            texts={{ SelectOptions: "Please select", All: "All", SelectedItemCount: "# selected" }}
            isAllTextShown={false}
            hasSearchBox={false}
            treeViewOption={TreeViewOption}
            onChange={this._onChangeSubsidiaryList} />;
    }

    _renderCustomerClassSelectList() {
        const customerClassList = AppData.customerClassList;
        const dataSource = DataSourceHelper.CreateDataSourceFrom(customerClassList, this.props.selectedCustomerClass);

        return customerClassList && customerClassList.length && (
            <MultipleSelect id="ddlCustomerClass"
                language={LangHelper.currentLanguage}
                dataSource={dataSource}
                keyField={"key"}
                valueField={"value"}
                statusField={"status"}
                onChange={(_, selectedItemsKey) => this.props.setSelectedCustomerClass(selectedItemsKey)} />);
    }

    render() {
        const isDisabledSubmit = StringHelper.isEmptyOrWhiteSpace(this.props.selectedCurrency) ||
            StringHelper.isEmptyOrWhiteSpace(this.props.selectedReason)||
            StringHelper.isEmptyOrWhiteSpace(this.props.selectedCustomerClass);

        return (
            <CollapsiblePanel className="page-filter" stickyFooter={this._renderStickyFooter()}>
                <div className="row">
                    {AppData.viewType !== ViewType.Normal && this.props.subsidiarySearch && (<div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">
                                {AppData.viewType === ViewType.WhiteLabel ? LangHelper.getSingleResource("WhiteLabel") : LangHelper.getSingleResource("Subsidiary")}
                            </label>
                            {this._renderSubsidiaryTreeView()}
                        </div>
                    </div>)}
                    {AppData.viewType !== ViewType.Normal && <div className="col-12 col-md-3 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <CheckBox id="SubsidiarySearch"
                                checked={this.props.subsidiarySearch}
                                onChange={checked => this.props.setSubsidiarySearch(checked)}
                                label={LangHelper.getSingleResource("SubsidiarySearch")} />
                        </div>
                    </div>}
                    <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>
                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <CheckBox id="IsGetAll"
                                checked={this.props.isGetAll}
                                onChange={checked => this.props.changeIsGetAll(checked)}
                                label={LangHelper.getSingleResource("GetAll")} />
                        </div>
                    </div>
                    <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Reason")}</label>
                            {this._renderReasonSelectList()}
                        </div>
                    </div>
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("CustomerClass")}</label>
                            {this._renderCustomerClassSelectList()}
                        </div>
                    </div>
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Username")}</label>
                            <AutoComplete
                                onSelect={(value) => this.props.setUsername(value)}
                                onInput={(value) => this.props.setUsername(value)}
                                htmlAttributes={{ placeholder: LangHelper.getSingleResource("ExternalIdOrSystemId") }} />
                        </div>
                    </div>
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                            {this._renderCurrencySelectList()}
                        </div>
                    </div>
                    <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("BaseCurrency")}</label>
                            {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>
                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1">
                        <div className="form-group">
                            <div className="button-group">
                                <button type="button" className="btn btn-submit" disabled={isDisabledSubmit} onClick={this._submit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>
                </div>
                {AppData.HasVipManagement && <div className="row">
                    <div className="col-12 text-bold">
                        {`${decode(LangHelper.getSingleResource("NotAllowedBAPlayerTurnonVip"))}`}
                    </div>
                </div>}
            </CollapsiblePanel>
        )
    }
}

function mapDispatchToProps(dispatch) {
    const {
        setDateRange, changeIsGetAll, setUsername, setSelectedCurrency,
        setSelectedParams, searchPlayer, setPagingOption, getUserGuideContent,
        setSelectedBaseCurrency, setReason, setSubsidiaryList, setSubsidiarySearch,
        setSelectedCustomerClass } = ActionCreators;

    return bindActionCreators(Object.assign({}, {
        setDateRange,
        changeIsGetAll,
        setUsername,
        setReason,
        setSelectedCurrency,
        setSelectedBaseCurrency,
        setSelectedParams,
        searchPlayer,
        setPagingOption,
        setSubsidiaryList,
        setSubsidiarySearch,
        getUserGuideContent,
        setSelectedCustomerClass
    }), dispatch);
}

function mapStateToProps(state) {
    const {
        dateRange, isGetAll, selectedCurrency, selectedParams,
        username, pagingOption, selectedTabKey, userGuideContent,
        selectedBaseCurrency, selectedReason, selectedSubsidiary, subsidiarySearch,
        selectedCustomerClass
    } = state;

    return Object.assign({}, {
        dateRange,
        isGetAll,
        selectedReason,
        selectedCurrency,
        selectedBaseCurrency,
        selectedSubsidiary,
        username,
        selectedParams,
        pagingOption,
        selectedTabKey,
        subsidiarySearch,
        userGuideContent,
        selectedCustomerClass
    });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);
