import React from 'react';
import { connect } from 'react-redux';
import ReportTitle from '@jsroot/components/ReportTitle';
import AppData from '@jsroot/lib/AppData';
import LangHelper from '@jsroot/lib/langHelper';
import Site from '@jsroot/lib/site';
import DataService from '@jsroot/lib/DataService';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';

export const Title = (props) => {
    const handleExportExcel = () => {
        const { requestParam, toggleLoader } = props;
        const param = { ...requestParam, isExport: true };
        const url = Site.resolveClientUrl('PlayerBtcSingleCustomer/Index');
        toggleLoader(true);
        DataService.getFile(url, param)
            .then(response => {
                const NoContentStatusCode = 204;

                if (response.status === NoContentStatusCode) {
                    alert(LangHelper.getSingleResource("NoInfo"));
                } else {
                    Site.downloadFile(response.data, 'BettingTicketsCenter_SingleCustomer.xls');
                }
                toggleLoader(false);
            })
            .catch(error => {
                console.log(error.message);
                toggleLoader(false);
                alert(LangHelper.getSingleResource("NoInfo"));
            });
    }

    const content = (
        <>
            <span className="caret-block">
                <span className="icon-excel-wrapper" onClick={handleExportExcel}>
                    <i className="icon-excel" title={LangHelper.getSingleResource('ExportToExcel')}></i>
                </span>
            </span>
            <span>{AppData.reportTitle}</span>
        </>
    )

    return (
        <ReportTitle tag="div" content={content} />
    )
}

function mapDispatchToProps(dispatch) {
    const { toggleLoader } = ActionCreators;

    return bindActionCreators({ toggleLoader }, dispatch);
}

function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(Title);