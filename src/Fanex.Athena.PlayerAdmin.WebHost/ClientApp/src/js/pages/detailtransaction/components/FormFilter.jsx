import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import moment from 'moment'
import LangHelper from '@jsroot/lib/langHelper';
import { DateRange } from '@jsroot/components/date-range/index';
import AppData from '@jsroot/lib/AppData';
import { ActionCreators } from '../actions/index.js';
import Site from '@jsroot/lib/site';
import { ViewTypeOptions } from '../../../lib/constants'
import $ from 'jquery';

const RANGE_MONTH_CONSTRAINT = 3;
const dateConstraint = AppData.dateConstraint;
const DEFAULT_PAGE_SIZE = AppData.defaultPageSize;
const DEFAULT_PAGE_INDEX = AppData.defaultPageIndex;

export class FormFilter extends PureComponent {
    constructor(props) {
        super(props);
        this.state = {
            requestParam: this.props.requestParam,
            viewType: ViewTypeOptions.MainWallet
        };

        this.exportForm = React.createRef();
    }

    _renderDateRange = () => {
        return (
            <DateRange startDate={this.props.requestParam.FromDate} endDate={this.props.requestParam.ToDate}
                minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                today={this.props.requestParam.ToDate} limitedMonthRange={RANGE_MONTH_CONSTRAINT + 1}
                onChange={this.dateRangeChanged} />
        );
    };

    dateRangeChanged = (date, isValidMonthRange = true) => {
        if (!isValidMonthRange) {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", RANGE_MONTH_CONSTRAINT);
            alert(message);
            return;
        }

        const dateFormatStr = "MM/DD/YYYY";
        var newRequestParam = {
            FromDate: moment(date.startDate).format(dateFormatStr),
            ToDate: moment(date.endDate).format(dateFormatStr)
        };

        this.setState(state => ({
            ...state,
            requestParam: Object.assign(this.props.requestParam, newRequestParam)
        }));
    };
    changeResettled = (checked) => {
        this.setState(state => ({
            ...state,
            requestParam: Object.assign(this.props.requestParam, { IsResettled: checked })
        }));
    };
    changeTransactionId = (value) => {
        this.setState(state => ({
            ...state,
            requestParam: Object.assign(this.props.requestParam, { TransactionId: value })
        }));
    };
    onSubmit = () => {
        const param = Object.assign({}, this.props.requestParam, { PageSize: DEFAULT_PAGE_SIZE, PageIndex: DEFAULT_PAGE_INDEX, ViewType: this.state.viewType });

        if (!this.isValidTransactionId(param.TransactionId)) {
            return;
        }

        this.props.getDetailTransactionReport(param);
    };

    isValidTransactionId = (value) => {
        var transactionId = parseInt(value, 10);

        return !value || (!isNaN(transactionId) && transactionId == value);
    };

    exportExcel = (event) => {
        event.preventDefault();

        const param = Object.assign({}, this.props.requestParam);
        if (!this.isValidTransactionId(param.TransactionId)) {
            return;
        }

        const excelForm = $("#form-excel-detail-transaction");
        excelForm.find("#FromDate").val(param.FromDate);
        excelForm.find("#ToDate").val(param.ToDate);
        excelForm.find("#TransactionId").val(param.TransactionId);
        excelForm.find("#IsResettled").val(param.IsResettled);

        this.exportForm.current.submit();
    };

    onViewTypeChange = (value) => {
        this.setState(state => ({
            ...state,
            viewType: value
        }));
    }

    render() {
        const relativeUrl = 'PlayerDetailTransaction/ExportDetailTransactionReport';
        const url = Site.resolveClientUrl(relativeUrl);

        return (
            <div className='page-filter'>
                <div className='row'>
                    {AppData.isShowSabaCoins && 
                    <div className="col-12">
                        <div className="form-group radio-group">
                            <input type="radio" value={ViewTypeOptions.MainWallet} checked={this.state.viewType  == ViewTypeOptions.MainWallet} name="ViewType" id="viewtypebymain"
                                    onChange={event => this.onViewTypeChange(ViewTypeOptions.MainWallet)}/>
                            <label htmlFor="viewtypebymain">{LangHelper.getSingleResource("MainWallet")}</label>
                            <input type="radio" value={ViewTypeOptions.SabaCoins} name="ViewType" id="viewtypebysabacoins" checked={this.state.viewType  == ViewTypeOptions.SabaCoins}
                                onChange={event => this.onViewTypeChange(ViewTypeOptions.SabaCoins)}/>
                            <label htmlFor="viewtypebysabacoins">{LangHelper.getSingleResource("SabaCoins")}</label>
                        </div>
                    </div>}

                    <div className='col-12 col-sm-7 col-md-4 col-xl-3'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource("TransactionDate")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>
                    <div className='col-8 col-sm-5 col-md-3 col-xl-2'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource("TransactionID")}</label>
                            <input className="input-text" id="txt-transction-id" name="TransactionId" type="text" maxLength="19"
                                autoComplete="off"
                                onChange={event => this.changeTransactionId(event.target.value)} />
                        </div>
                    </div>
                    <div className='col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2'>
                        <div className='form-group'>
                            <div class="chk-group">
                                <input id="chk-resettled" name="IsResettled" type="checkbox"
                                    class="chk-resettled" autoComplete="off"
                                    onChange={event => this.changeResettled(event.target.checked)} />
                                <label for="chk-resettled" class="lbl-resettled">{LangHelper.getSingleResource("ResettledRecovered")}</label>
                            </div>
                        </div>
                    </div>
                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1">
                        <div className="form-group">
                            <div className="button-group">
                                <button type="button" className="btn btn-submit" onClick={this.onSubmit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>
                    <div className="caret-block">
                        <i className="icon-excel right" onClick={this.exportExcel} title={LangHelper.getSingleResource("ExportToExcel")}></i>
                        <form ref={this.exportForm} id="form-excel-detail-transaction" action={url}>
                            <input type="hidden" id="ViewType" name="ViewType" value={this.state.viewType} />
                            <input type="hidden" id="CustId" name="CustId" value={this.state.requestParam.CustId} />
                            <input type="hidden" id="FromDate" name="FromDate" value={this.state.requestParam.FromDate} />
                            <input type="hidden" id="ToDate" name="ToDate" value={this.state.requestParam.ToDate} />
                            <input type="hidden" id="CurrencyId" name="CurrencyId" value={this.state.requestParam.CurrencyId} />
                            <input type="hidden" id="PageSize" name="PageSize" value={this.state.requestParam.PageSize} />
                            <input type="hidden" id="PageIndex" name="PageIndex" value={this.state.requestParam.PageIndex} />
                            <input type="hidden" id="TransactionId" name="TransactionId" value={this.props.requestParam.TransactionId} />
                            <input type="hidden" id="IsResettled" name="IsResettled" value={this.state.requestParam.IsResettled} />
                        </form>
                    </div>
                </div>
            </div>
        );
    }
};

function mapDispatchToProps(dispatch) {
    const { setRequestParam, getDetailTransactionReport } = ActionCreators;
    return bindActionCreators({ setRequestParam, getDetailTransactionReport }, dispatch);
}

function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
}

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);
