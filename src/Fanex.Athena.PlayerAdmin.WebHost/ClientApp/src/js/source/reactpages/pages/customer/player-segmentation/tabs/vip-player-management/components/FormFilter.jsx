import React, { PureComponent } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import Select from 'react-select'
import MultipleSelect, { TreeViewSelect } from '@9softstudio/react-multiselect'
import AutoComplete from '@jsroot/components/auto-complete'
import CheckBox from '@jsroot/components/CheckBox'
import CollapsiblePanel from '@jsroot/components/collapsible-panel'
import UserGuide from '@jsroot/components/UserGuide'
import LangHelper from '@jsroot/lib/langHelper'
import StringHelper from '@jsroot/lib/stringHelper'
import { decode } from '@jsroot/lib/htmlHelper'
import Site from '@jsroot/lib/site'
import AppData from '@jsroot/lib/AppData'
import DataService from '@jsroot/lib/DataService.js'
import { ActionCreators } from '../actions'
import { Url } from '../lib/constants';
import { ViewType, TreeViewOption } from '../../../lib/constants';
import DataSourceHelper from '../../../lib/helpers/dataSourceHelper';
import ParamHelper from "../lib/helpers/paramHelper";
import GridHelper from "../../../lib/helpers/gridHelper";

export class FormFilter extends PureComponent {
    constructor(props) {
        super(props);
    }

    _submit = (event) => {
        event.preventDefault();
        const params = ParamHelper.ToAPIParams(this.props);
        if (!params) {
            return;
        }
        this.props.setSelectedParams(params);
        this.props.searchPlayer(this.props.selectedTabKey, params, GridHelper.callBackToUpdatePaging(this.props));
    }

    exportExcel = (event) => {
        event.preventDefault();
        const { selectedParams, pagingOption } = this.props;
        if (!selectedParams) {
            return;
        }
        const params = {
            ...selectedParams,
            ...pagingOption
        }

        DataService.getFile(Url.exportVipPlayerList, params).then((response) => {
            const NoContentStatusCode = 204;
            if (response.status === NoContentStatusCode) {
                alert(LangHelper.getSingleResource("NoInfo"));
            } else {
                Site.downloadFile(response.data, 'VipPlayerList.xls');
            }
        });
    }

    _renderVipOptionSelectList() {
        const { selectedVipOption, setSelectedVipOption, selectedTabKey } = this.props;
        const vipOptions = AppData[selectedTabKey].vipOptions;
        return vipOptions && vipOptions.length && (
            <Select instanceId="VipOptions"
                isSearchable={false}
                value={selectedVipOption}
                options={vipOptions}
                onChange={(item) => setSelectedVipOption(item.value)} />
        );
    }

    _renderCurrencySelectList() {
        const { selectedCurrency, selectedTabKey } = this.props;
        const currencyList = AppData[selectedTabKey].currencyList;
        const dataSource = DataSourceHelper.CreateDataSourceFrom(currencyList, selectedCurrency);
        return currencyList && currencyList.length && (
            <MultipleSelect id="permissionCurrency"
                language={LangHelper.currentLanguage}
                dataSource={dataSource}
                keyField={"key"}
                valueField={"value"}
                statusField={"status"}
                onChange={(_, selectedItemsKey) => this.props.setSelectedCurrency(selectedItemsKey)} />);
    }

    showUserGuide = () => {
        const userGuideContent = this.props.userGuideContent;
        if (!userGuideContent || userGuideContent.length === 0) {
            this.props.getUserGuideContent('PlayerSegmentationVIP');
        }
    }

    _renderUserGuide() {
        const userGuideContent = this.props.userGuideContent;
        return (<UserGuide
            title={LangHelper.getSingleResource("UserGuide")}
            contentTitle={`${LangHelper.getSingleResource("PlayerSegmentation")} / ${LangHelper.getSingleResource('VipPlayerManagement')}`}
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
            StringHelper.isEmptyOrWhiteSpace(this.props.selectedCustomerClass);

        return (
            <CollapsiblePanel className="page-filter" stickyFooter={this._renderStickyFooter()}>
                <div className="row">
                    {AppData.viewType !== ViewType.Normal && this.props.subsidiarySearch && (<div className="col-12 col-md-2 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">
                                {AppData.viewType === ViewType.WhiteLabel ? LangHelper.getSingleResource("WhiteLabel") : LangHelper.getSingleResource("Subsidiary")}
                            </label>
                            {this._renderSubsidiaryTreeView()}
                        </div>
                    </div>)}
                    {AppData.viewType !== ViewType.Normal && <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <CheckBox id="SubsidiarySearch"
                                checked={this.props.subsidiarySearch}
                                onChange={checked => this.props.setSubsidiarySearch(checked)}
                                label={LangHelper.getSingleResource("SubsidiarySearch")} />
                        </div>
                    </div>}
                    <div className="col-12 col-md-2 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Vip")}</label>
                            {this._renderVipOptionSelectList()}
                        </div>
                    </div>
                    <div className="col-12 col-md-4 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Username")}</label>
                            <AutoComplete
                                onSelect={(value) => this.props.setUsername(value)}
                                onInput={(value) => this.props.setUsername(value)}
                                htmlAttributes={{ placeholder: LangHelper.getSingleResource("ExternalIdOrSystemId") }} />
                        </div>
                    </div>
                    <div className="col-12 col-md-2 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                            {this._renderCurrencySelectList()}
                        </div>
                    </div>
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("CustomerClass")}</label>
                            {this._renderCustomerClassSelectList()}
                        </div>
                    </div>
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-1">
                        <div className="form-group">
                            <div className="button-group">
                                <button type="button" className="btn btn-submit" disabled={isDisabledSubmit} onClick={this._submit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-12 text-bold">
                        {`${decode(LangHelper.getSingleResource("NotAllowedBAPlayerTurnonVip"))}`}
                    </div>
                </div>
            </CollapsiblePanel>
        )
    }
}

function mapDispatchToProps(dispatch) {
    const {
        setDateRange, setSelectedVipOption, setUsername, setSelectedCurrency,
        setSelectedParams, searchPlayer, setPagingOption, setSubsidiaryList,
        getUserGuideContent, setSubsidiarySearch, setSelectedCustomerClass } = ActionCreators;

    return bindActionCreators(Object.assign({}, {
        setSelectedVipOption, setDateRange, setUsername, setSelectedCurrency,
        setSelectedParams, searchPlayer, setPagingOption, setSubsidiaryList,
        getUserGuideContent, setSubsidiarySearch, setSelectedCustomerClass
    }), dispatch);
}

function mapStateToProps(state) {
    const {
        dateRange, selectedVipOption, selectedCurrency, selectedParams,
        username, pagingOption, selectedTabKey, userGuideContent,
        selectedSubsidiary, subsidiarySearch, selectedCustomerClass } = state;

    return Object.assign({}, {
        selectedVipOption,
        dateRange,
        selectedCurrency,
        username,
        selectedParams,
        pagingOption,
        selectedTabKey,
        userGuideContent,
        selectedSubsidiary,
        subsidiarySearch,
        selectedCustomerClass
    });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);
