import React, { Component } from 'react';
import { Table, HeaderRow, Row, HeaderCell, Cell } from '@9softstudio/react-grid';
import PropTypes from 'prop-types';

class BetListDataGrid extends Component {
    constructor(props) {
        super(props);
    }

    componentDidUpdate(prevProps) {
        if (this.props.data !== prevProps.data) {
            this.props.afterLoad && this.props.afterLoad();
        }
    }

    _getResource(key) {
        return this.props.getResourceFunc ? this.props.getResourceFunc(key) : key;
    }
    
    _getTotalCellClassName(column, value) {
        if (this.props.getTotalCellClassNameFunc) {
            return this.props.getTotalCellClassNameFunc(column, value);
        }

        return column === 'LicenseeWL' && value.substring(0, 1) === '-' ? 'negative' : '';
    }

    _getHeaderName(column) {
        return this.props.getHeaderNameFunc ? this.props.getHeaderNameFunc(column) : column;
    }

    _mapData(data) {
        return this.props.mappingDataFunc ? this.props.mappingDataFunc(data) : data;
    }

    _buildHeader() {
        let rows = [];
        const { columns, columnWidths, sortableColumns } = this.props;

        for (let i = 0; i < columns.length; i++) {
            const column = columns[i];
            const isSortable = sortableColumns.indexOf(column) >= 0;
            const headerName = this._getHeaderName(column);

            rows.push(
                <HeaderCell key={column} colWidth={columnWidths[i]} sortBy={isSortable ? column : null}>{headerName}</HeaderCell>
            )
        }

        return <HeaderRow style={{ height: 32 }}>{rows}</HeaderRow>;
    }

    _buildBody() {
        const { columns, columnAlignments, data } = this.props;
        let rows = [];

        for (let i = 0; i < data.length; i++) {
            const item = this._mapData(data[i]);
            let cells = [];

            for (let j = 0; j < columns.length; j++) {
                const column = columns[j];
                cells.push(
                    <Cell key={`${column}-${i}`} className={columnAlignments[j]} dangerouslySetInnerHTML={{ __html: item[column] }}></Cell>
                )
            }

            rows.push(<Row key={i}>{cells}</Row>);
        }

        return rows;
    }

    _buildFooter() {
        if (!this.props.totalColumns || !this.props.totalColumns.length) {
            return false;
        }

        const { columns, totalColumns, total, totalText, columnAlignments } = this.props;
        const startTotalIndex = columns.indexOf(totalColumns[0]);
        let rows = [];
        let cells = [];

        cells.push(<Cell key="total" className="right-align" colSpan={startTotalIndex}>{totalText}</Cell>);

        const item = this._mapData(total);

        for (let i = 0; i < totalColumns.length; i++) {
            const column = totalColumns[i];
            const columnIndex = columns.indexOf(column);
            const className = this._getTotalCellClassName(column, item[column]);

            cells.push(
                <Cell key={`Sum${column}`} className={`${columnAlignments[columnIndex]} ${className}`} dangerouslySetInnerHTML={{ __html: item[column] }}></Cell>
            )
        }

        const blankRows = columns.length - totalColumns.length - startTotalIndex;
        if (blankRows) {
            cells.push(<Cell key="blank" colSpan={blankRows}></Cell>);
        }

        rows.push(<Row className="fixed-table-grant-total" key="totalRow">{cells}</Row>);

        return rows;
    }

    _renderNodata() {
        return (
            <div className="result-content">
                <p className="no-info-row center-align">{this._getResource('NoInfo')}</p>
            </div>
        )
    }

    onSort = (sortBy, sortOrder) => {
        this.props.onSort && this.props.onSort(sortBy, sortOrder);
    }

    onPaging = (index, size) => {
        this.props.onPaging && this.props.onPaging(index, size);
    }

    render() {
        if (!this.props.data) {
            return null;
        }

        const { data } = this.props;
        
        if (!data.length) {
            return this._renderNodata();
        }

        const { isPaging, pageOption, adjustedHeight } = this.props;

        return (
            <div className="betlist-container result-content">
                <Table
                    onSort={this.onSort}
                    minWidth={980}
                    autoWidth={true}
                    bodyHeight={300}
                    header={this._buildHeader()}
                    body={this._buildBody()}
                    footer={this._buildFooter()}
                    isPaging={isPaging}
                    pageOption={pageOption}
                    onPaging={this.onPaging}
                    adjustedHeight={adjustedHeight}
                />
            </div>
        );
    }
}

BetListDataGrid.propTypes = {
    isPaging: PropTypes.bool,
    pageOption: PropTypes.shape({
        PageIndex: PropTypes.number,
        PageSize: PropTypes.number,
        TotalItem: PropTypes.number,
        PageList: PropTypes.arrayOf(PropTypes.number)
    }),
    columns: PropTypes.arrayOf(PropTypes.string),
    columnWidths: PropTypes.arrayOf(PropTypes.number),
    mappingDataFunc: PropTypes.func,
    sortableColumns: PropTypes.arrayOf(PropTypes.string),
    totalColumns: PropTypes.arrayOf(PropTypes.string),
    getHeaderNameFunc: PropTypes.func,
    columnAlignments: PropTypes.arrayOf(PropTypes.string),
    data: PropTypes.arrayOf(PropTypes.object),
    total: PropTypes.object,
    getTotalCellClassNameFunc: PropTypes.func,
    totalText: PropTypes.string,
    onSort: PropTypes.func,
    onPaging: PropTypes.func,
    afterLoad: PropTypes.func,
    adjustedHeight: PropTypes.number,
    getResourceFunc: PropTypes.func
}

BetListDataGrid.defaultProps = {
    isPaging: false,
    pageOption: {
        PageIndex: 1,
        PageSize: 50,
        TotalItem: 0,
        PageList: [50, 100, 200, 300, 400, 500, 1000, 2000]
    },
    columns: ['Index', 'ExternalId', 'SystemId', 'RefNo', 'TransTime', 'Choice', 'Odds', 'OddsType', 'Stake', 'WinLoss', 'LicenseeWL', 'Status', 'Platform', 'Live'],
    columnWidths: [40, 120, 120, 110, 150, 400, 70, 70, 100, 100, 100, 80, 80, 80],
    sortableColumns: ['Stake', 'WinLoss', 'LicenseeWL'],
    totalColumns: ['Stake', 'WinLoss', 'LicenseeWL'],
    columnAlignments: [
        'center-align', '', '', 'center-align', 'center-align',
        'right-align', 'right-align', 'right-align', 'right-align',
        'right-align', 'right-align', 'center-align', 'center-align',
        'center-align'
    ],
    data: null,
    total: null,
    totalText: 'Total',
    adjustedHeight: 0
}

export default BetListDataGrid;