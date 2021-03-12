import React, { useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { ActionCreators } from '../actions';
import LangHelper from '@jsroot/lib/langHelper';
import AppData from '@jsroot/lib/AppData';
import { DatePicker } from '@9softstudio/react-datepicker';
import { getFormattedDate } from '../lib/utils';
import { renderProductSelectList, renderCurrencySelectList } from '../lib/formFilterUtils';

export const FormFilter = (props) => {
    const { requestParam } = props;
    const { productCodeList, currencyIdList, date, pageSize, sortBy, sortOrder } = requestParam;

    const [selectedProduct, setSelectedProduct] = useState(productCodeList);
    const [selectedCurrency, setSelectedCurrency] = useState(currencyIdList);
    const [selectedDate, setSelectedDate] = useState(date);

    const handleChangeProduct = (currentItem, selectedItemsKey) => setSelectedProduct(selectedItemsKey);
    const handleChangeCurrency = (currentItem, selectedItemsKey) => setSelectedCurrency(selectedItemsKey);

    const buildParam = () => ({
        pageSize: pageSize,
        pageIndex: 1,
        sortBy,
        sortOrder,
        currencyIdList: selectedCurrency,
        productCodeList: selectedProduct,
        date: selectedDate
    });

    const submit = (event) => {
        event.preventDefault();
        const params = buildParam();

        props.setRequestParam(params);
    }

    const exportExcel = (event) => {
        event.preventDefault();
        const params = buildParam();
        params.isExportExcel = true;

        props.exportExcel(params);
    }

    const { productOptions, currencyList } = AppData;
    const { dateConstraint } = props;
    const dateString = getFormattedDate(selectedDate);

    const isValidParam = !!selectedProduct && !!selectedCurrency;

    return (
        <div className="page-filter">
            <div className="row">
                <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                        <DatePicker format="DD/MM/YYYY"
                            minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate} date={dateString}
                            onChange={(momentDate) => setSelectedDate(momentDate.format("MM/DD/YYYY"))} />
                    </div>
                </div>
                {
                    productOptions && productOptions.length ? (
                        <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                            <div className="form-group">
                                <label className="label-text">{LangHelper.getSingleResource("Product")}</label>
                                {renderProductSelectList(productOptions, handleChangeProduct)}
                            </div>
                        </div>
                    ) : null
                }

                {
                    currencyList && currencyList.length ? (
                        <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                            <div className="form-group">
                                <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                                {renderCurrencySelectList(currencyList, handleChangeCurrency)}
                            </div>
                        </div>
                    ) : null
                }

                <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1">
                    <div className="form-group">
                        <div className="button-group">
                            <button type="button" className="btn btn-submit" disabled={!isValidParam}
                                onClick={submit}>{LangHelper.getSingleResource("Submit")}</button>
                        </div>
                    </div>
                </div>

                <div className="caret-block">
                    <i className="icon-excel right" onClick={exportExcel} title={LangHelper.getSingleResource('ExportToExcel')}></i>
                </div>
            </div>
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { setRequestParam, exportExcel } = ActionCreators;
    return bindActionCreators({ setRequestParam, exportExcel }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam, dateConstraint } = state;
    return Object.assign({}, { requestParam, dateConstraint });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);