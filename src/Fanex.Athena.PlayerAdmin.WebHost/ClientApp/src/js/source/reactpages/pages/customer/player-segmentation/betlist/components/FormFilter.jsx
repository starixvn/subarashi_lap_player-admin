import React, { PureComponent } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import CollapsiblePanel from '@jsroot/components/collapsible-panel'
import { DateRange } from '@jsroot/components/date-range'
import LangHelper from '@jsroot/lib/langHelper'
import AppData from '@jsroot/lib/AppData'
import { ActionCreators } from '../actions'
import ParamHelper from '../lib/helpers/paramHelper';
import QueryHelper from '../lib/helpers/queryHelper';

export class FormFilter extends PureComponent {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        this._fetchData();
    }

    _submit = (event) => {
        event && event.preventDefault();
        this._fetchData();
    }

    _fetchData() {
        const { setSelectedParams, getGridData, selectedParams, dateRange, toggleLoader } = this.props;
        const params = ParamHelper.ToAPIParams({ selectedParams, dateRange, custId: AppData.custId, baseCurrency: AppData.baseCurrency });
        if (!params) {
            return;
        }
        setSelectedParams(params);
        QueryHelper.FetchBetList({ params, getGridData, toggleLoader });
    }

    _renderDateRange() {
        const { dateRange } = this.props;
        const dateConstraint = AppData.dateConstraint;
        return (<DateRange
            startDate={dateRange.fromDate} endDate={dateRange.toDate}
            minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
            toDate={dateRange.toDate} limitedMonthRange={dateConstraint.limitedMonthRange}
            onChange={this._onChangeDateRange}
        />)
    }

    _onChangeDateRange = (date, isValidMonthRange = true) => {
        if (isValidMonthRange) {
            this.props.setDateRange(date);
        }
        else {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", AppData.dateConstraint.limitedMonthRange);
            alert(message);
        }
    }

    render() {
        return (
            <CollapsiblePanel className="page-filter">
                <div className="row">
                    <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>
                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1">
                        <div className="form-group">
                            <div className="button-group">
                                <button type="button" className="btn btn-submit" onClick={this._submit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>
                </div>
            </CollapsiblePanel>
        )
    }
}

function mapDispatchToProps(dispatch) {
    const { setDateRange, setSelectedParams, getGridData, toggleLoader } = ActionCreators;
    return bindActionCreators(Object.assign({}, {
        setDateRange,
        setSelectedParams,
        getGridData,
        toggleLoader
    }), dispatch);
}

function mapStateToProps(state) {
    const { dateRange, selectedParams } = state;
    return Object.assign({}, {
        dateRange,
        selectedParams
    });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);