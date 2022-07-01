import React, { useState } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js';
import { bindActionCreators } from 'redux';
import LangHelper from '@jsroot/lib/langHelper';
import AppData from '@jsroot/lib/AppData';
import { DateRange } from '@jsroot/components/date-range';
import Site from '@jsroot/lib/site'

const FormFilter = (props) => {
    const [dateRange, setDateRange] = useState({ startDate: props.requestParam.fromDate, endDate: props.requestParam.toDate });
    const dateConstraint = AppData.dateConstraint;

    const onSubmit = () => {
        const param = Object.assign({}, props.requestParam, {fromDate: Site.convertDateToString(dateRange.startDate), toDate: Site.convertDateToString(dateRange.endDate)});

        props.setRequestParam(param);
    }

    const onChangeDateRange = (range, isValid = true) => {
        if (isValid) {
            setDateRange(range);
        } else {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", AppData.dateConstraint.limitedMonthRange);
            alert(message);
        }
    }

    const _renderDateFilter = () => {
        return <DateRange
                startDate={dateRange.startDate} endDate={dateRange.endDate}
                minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                today={dateRange.endDate} limitedMonthRange={dateConstraint.limitedMonthRange}
                onChange={onChangeDateRange}
            />;
    }

    return (
        <div className='page-filter'>
            <div className="row">
                <div className="col-12 col-md-3 col-sm-6 col-lg-4 col-xl-2">
                    <div className="form-group">
                        <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                        {_renderDateFilter()}
                    </div>
                </div>
                <div className="col-12 col-md-3 col-sm-4 col-lg-2 col-xl-2">
                    <div className="form-group">
                        <div className="button-group">
                            <button type="button" className="btn btn-submit" onClick={onSubmit}>{LangHelper.getSingleResource("Submit")}</button>
                        </div>
                    </div>
                </div>
                <div className="col-12">
                    <div><span className="circle"></span> {LangHelper.getSingleResource("PlayerStatementSupportMonths")}</div>
                    <div><span className="circle"></span> {LangHelper.getSingleResource("GrandPrizeNote")}</div>
                </div>
            </div>
        </div>
    );
}

function mapDispatchToProps(dispatch) {
    const { setRequestParam } = ActionCreators;
    return bindActionCreators({ setRequestParam }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(FormFilter));