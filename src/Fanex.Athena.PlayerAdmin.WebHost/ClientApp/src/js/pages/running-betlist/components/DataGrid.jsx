import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import BetListDataGrid from '@jsroot/components/betlist/BetListDataGrid';
import AppData from '@jsroot/lib/AppData';
import Service from '../lib/service';
import LangHelper from '@jsroot/lib/langHelper';

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.headerText = Service.getHeaderText();
        this.totalText = Service.getTotalText();

        this.state = {
            data: null,
            total: null,
            totalRows: 0
        };
    }

    componentDidUpdate(prevProps) {
        if (prevProps.requestParam.TimeStamp !== this.props.requestParam.TimeStamp) {
            this._getData(this.props.requestParam);
        }
    }

    componentDidMount() {
        this._getData(this.props.requestParam);
    }

    getHeaderName = (column) => {
        return this.headerText[column];
    }

    getResource = (key) => {
        return LangHelper.getSingleResource(key);
    }

    afterLoad = () => {
        this.props.toggleLoader(false);
    }

    onSort = (sortBy, sortOrder) => {
        const param = Object.assign({}, this.props.requestParam, { SortBy: sortBy, SortOrder: sortOrder });

        this._getData(param);
        this.props.setRequestParam(param);
    }

    onPaging = (index, size) => {
        const param = Object.assign({}, this.props.requestParam, { PageIndex: index, PageSize: size });

        this._getData(param);
        this.props.setRequestParam(param);
    }

    _updateState(data, total, totalRows) {
        this.setState({
            data: data,
            total: total,
            totalRows: totalRows
        });

        this.props.toggleLoader(false);
    }

    _getData(param) {
        this.props.toggleLoader(true);

        Service.getData(param, this._updateState.bind(this));
    }

    render() {
        const { data, total, totalRows } = this.state;
        const requestParam = this.props.requestParam;

        const pageOption = {
            PageIndex: requestParam.PageIndex,
            PageSize: requestParam.PageSize,
            TotalItem: totalRows,
            PageList: [50, 100, 200, 300, 400, 500, 1000, 2000]
        };

        let Columns = ['Index', 'RefNo', 'TransTime', 'Choice', 'Odds', 'OddsType', 'Stake', 'Status', 'Platform', 'Live'];
        let ColumnWidths = [40, 160, 150, 370, 70, 70, 100, 110, 80, 80];
        let ColumnAlignments = [
            'center-align', 'center-align tooltip', 'center-align',
            'right-align', 'right-align', 'right-align', 'right-align',
            'center-align', 'center-align', 'center-align'
        ]

        if (AppData.isAllowTxIDColumn && !Columns.includes('LicenseeTxId')) {
            Columns.splice(2, 0, 'LicenseeTxId');
            ColumnWidths.splice(2, 0, 110);
            ColumnAlignments.splice(2, 0, 'center-align tooltip');
        }

        return <BetListDataGrid
            isPaging={true}
            pageOption={pageOption}
            onPaging={this.onPaging}
            getHeaderNameFunc={this.getHeaderName}
            columns={Columns}
            columnWidths={ColumnWidths}
            sortableColumns={['Stake']}
            totalColumns={['Stake']}
            columnAlignments={ColumnAlignments}
            data={data}
            total={total}
            totalText={this.totalText}
            afterLoad={this.afterLoad}
            onSort={this.onSort}
            adjustedHeight={-20}
            getResourceFunc={this.getResource}
        />
    }
}

function mapDispatchToProps(dispatch) {
    const { toggleLoader, setRequestParam } = ActionCreators;
    return bindActionCreators({ toggleLoader, setRequestParam }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);