import React, { useState } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js';
import { bindActionCreators } from 'redux';
import LangHelper from '@jsroot/lib/langHelper';
import AppData from '@jsroot/lib/AppData';
import MonthPicker from '@jsroot/components/month-picker/index';

const FormFilter = (props) => {
    const [dateRange, setDateRange] = useState({ fromDate: props.requestParam.fromDate, toDate: props.requestParam.toDate });
    const dateConstraint = AppData.dateConstraint;

    const onSubmit = () => {
        const param = Object.assign({}, props.requestParam, dateRange);

        props.setRequestParam(param);
    }

    const onChange = (dateRange) => {
        setDateRange(dateRange);
    }

    const renderMonthPicker = () => {
        const monthOption = {
            label: LangHelper.getSingleResource('ViewDataInMonth')
        };

        return <MonthPicker
            minDate={dateConstraint.minDate}
            maxDate={dateConstraint.maxDate}
            fromDate={dateRange.fromDate}
            toDate={dateRange.toDate}
            onChange={onChange}
            monthOption={monthOption}
            showDate={false} />;
    }

    return (
        <div className='page-filter'>
            <div className="row">
                <div className="col-12 col-md-5 col-sm-8 col-lg-4 col-xl-3">
                    <div className="form-group">
                        {renderMonthPicker()}
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