import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index';
import BetListLayout from '@jsroot/layouts/betlist/BetListLayout';
import FormFilter from './FormFilter';
import DataGrid from './DataGrid';
import LangHelper from '@jsroot/lib/langHelper';
import Site from '@jsroot/lib/site';
import Service from '../lib/service';
import Loader from '@jsroot/components/Loader';
import AppLayout from '@jsroot/layouts/AppLayout';
import ReportTitle from '@jsroot/components/ReportTitle';

export class AppContainer extends Component {
    constructor(props) {
        super(props);

        this.title = Service.getReportTitle(props.requestParam.Currency);
    }

    getResource = (key) => {
        return LangHelper.getSingleResource(key);
    }

    formatDateString = (dateString) => {
        const date = new Date(dateString);
        const month = date.getMonth() + 1;
        const day = date.getDate();

        return `${month < 10 ? '0' + month : month}/${day < 10 ? '0' + day : day}/${date.getFullYear()}`;
    }

    beforeShow = () => {
        this.props.toggleLoader(true);
    }

    afterShow = () => {
        this.props.toggleLoader(false);
    }

    render() {
        return (
            <AppLayout>
                <BetListLayout
                    pageName='PlayerAdmin'
                    rootUrl={Site.getRootUrl()}
                    getResourceFunc={this.getResource}
                    formatDateStringFunc={this.formatDateString}
                    baseCurrency=''
                    beforeShowFunc={this.beforeShow}
                    afterShowFunc={this.afterShow}>
                    <Loader isLoading={this.props.isLoading} />
                    <FormFilter />
                    <div className="betlist-container result-content">
                        { this.title && <ReportTitle content={this.title} /> }
                        <DataGrid />
                    </div>
                </BetListLayout>
            </AppLayout>
        );
    }
}

function mapDispatchToProps(dispatch) {
    const { toggleLoader } = ActionCreators;
    return bindActionCreators({ toggleLoader }, dispatch);
}

function mapStateToProps(state) {
    const { isLoading, requestParam } = state;
    return Object.assign({}, { isLoading, requestParam });
}

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);