import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js';
import { bindActionCreators } from 'redux';
import LangHelper from '@jsroot/lib/langHelper';
import Service from '../lib/service';

export class FormFilter extends Component {
    constructor(props) {
        super(props);
    }

    submit = () => {
        let param = { ...this.props.requestParam };
        param.TimeStamp = new Date().getTime();

        this.props.setRequestParam(param);
    }

    exportExcel = (event) => {
        event.preventDefault();

        const requestParam = this.props.requestParam;

        this.props.toggleLoader(true);
        Service.export(requestParam, () => { this.props.toggleLoader(false) });
    }

    render() {
        return (
            <div className='pa-running-betlist-filter'>
                <button type='button' className='btn btn-submit' onClick={this.submit}>{LangHelper.getSingleResource('Refresh')}</button>
                <div className='pa-running-betlist-export'>
                    <i className='icon-excel' onClick={this.exportExcel} title={LangHelper.getSingleResource('ExportToExcel')}></i>
                </div>
            </div>
        )
    }
}
function mapDispatchToProps(dispatch) {
    const { setRequestParam, toggleLoader } = ActionCreators;
    return bindActionCreators({ setRequestParam, toggleLoader }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);