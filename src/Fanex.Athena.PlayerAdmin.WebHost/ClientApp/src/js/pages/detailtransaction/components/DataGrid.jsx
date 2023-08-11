import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Table, HeaderRow, HeaderCell, Row, Cell, Pager } from '@9softstudio/react-grid';
import LangHelper from '@jsroot/lib/langHelper';
import ReportTitle from '@jsroot/components/ReportTitle';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import AppData from '@jsroot/lib/AppData';
import { ActionCreators } from '../actions/index.js';

export class DataGrid extends PureComponent {
    constructor(props) {
        super(props);
    }

    _renderHeader() {
        return (
            <HeaderRow style={{ height: 32 }}>
                <HeaderCell colWidth={60}>{LangHelper.getSingleResource('TransactionID')}</HeaderCell>
                <HeaderCell colWidth={80}>{LangHelper.getSingleResource('TransactionTime')}</HeaderCell>
                <HeaderCell colWidth={120}>{LangHelper.getSingleResource('Transaction')}</HeaderCell>
                <HeaderCell colWidth={60}>{LangHelper.getSingleResource('BalanceChange')}</HeaderCell>
                {!AppData.isEnableSingleWallet && <HeaderCell colWidth={60}>{LangHelper.getSingleResource('AfterBalance')}</HeaderCell>}
                <HeaderCell colWidth={60}>{LangHelper.getSingleResource('ResettledRecovered')}</HeaderCell>
                <HeaderCell colWidth={160}>{LangHelper.getSingleResource('Remark')}</HeaderCell>
            </HeaderRow>
        )
    }

    _renderDataSection(detailTransactionReport) {
        const dataRows = [];
        var transactions = detailTransactionReport.DetailTransactions;
        for (let i = 0; i < transactions.length; i++) {
            const dataItem = transactions[i];
            dataRows.push(
                <Row key={dataItem.TransactionId + "_" + i}>
                    <Cell title={dataItem.TransactionTimeStr}>{dataItem.TransactionId}</Cell>
                    <Cell className="center-text">{dataItem.TransactionTimeStr}</Cell>
                    <Cell>{dataItem.TransactionType}</Cell>
                    <Cell className="right-text">{dataItem.BalanceChangeStr}</Cell>
                    {!AppData.isEnableSingleWallet && <Cell className="right-text">{dataItem.AfterBalanceStr}</Cell>}
                    <Cell className="center-text">
                        {dataItem.IsResettled ? <div class="icon-red-check"></div> : ""}
                    </Cell>
                    <Cell>{dataItem.Remark}</Cell>
                </Row>
            );
        }

        return dataRows;
    }

    _handlePaging(pageIndex, pageSize, props) {
        var oldParam = props.detailTransactionReport;
        var newRequestParam = {
            PageIndex: pageIndex,
            PageSize: pageSize,
            CustId: oldParam.CustId,
            CurrencyId: oldParam.CurrencyId,
            FromDate: oldParam.FromDateStr,
            ToDate: oldParam.ToDateStr,
            TransactionId: oldParam.TransactionId,
            IsResettled: oldParam.IsResettled
        };

        props.getDetailTransactionReport(newRequestParam);
    }

    render() {
        const { detailTransactionReport } = this.props;

        if (!detailTransactionReport || !detailTransactionReport.DetailTransactions) {
            return null;
        }

        const reportTitle = <>{LangHelper.getSingleResource('DetailTransaction')}, {detailTransactionReport.FromDateStr} -> {detailTransactionReport.ToDateStr}</>;

        if (!detailTransactionReport.DetailTransactions.length) {
            return (
                <>
                    <ReportTitle tag="div" content={reportTitle} />
                    <NoDataTitle />
                </>
            );
        }

        const pagingOption = {
            TotalItem: detailTransactionReport.TotalRow,
            PageSize: detailTransactionReport.PageSize,
            PageIndex: detailTransactionReport.PageIndex,
            PageList: [500, 1000, 1500, 2000]
        };

        return (
            <>
                <Pager pageOption={pagingOption} onPaging={(i, s) => this._handlePaging(i, s, this.props)} />
                <ReportTitle tag="div" content={reportTitle} />
                <Table minWidth={650}
                    autoWidth={true}
                    autoHeight={true}
                    header={this._renderHeader()}
                    body={this._renderDataSection(detailTransactionReport)}
                    containerPadding={30}
                />
            </>
        )
    }
}

function mapDispatchToProps(dispatch) {
    const { setDetailTransactionReport, getDetailTransactionReport, setRequestParam } = ActionCreators;
    return bindActionCreators({ setDetailTransactionReport, getDetailTransactionReport, setRequestParam }, dispatch);
}

function mapStateToProps(state) {
    const { detailTransactionReport } = state;
    return Object.assign({}, { detailTransactionReport });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);
