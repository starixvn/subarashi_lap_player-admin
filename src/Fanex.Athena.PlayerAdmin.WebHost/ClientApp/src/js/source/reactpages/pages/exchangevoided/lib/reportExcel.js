import Excel from '@jsroot/lib/excel';
import { titleCellStyle, headerCellStyle, bodyCellStyle } from '@jsroot/constants/excelStyle';
import { headerText, fields, VOIDED_STATUS } from './constants';

const TITLE_ROW_INDEX = 0;
const HEADER_ROW_INDEX = 1;
const DATA_ROW_INDEX = 2;

const MAIN_SECTION_STYLE = 'mainSectionStyle';

export default class ReportExcel {
    constructor(dataList, title) {
        this._dataList = dataList;
        this._title = title;

        this.worksheet = {};
        this.currentRowIndex = DATA_ROW_INDEX;
        this.columnConfigs = this._getcolumnConfigs();
        this.mergeConfig = [];
    }

    export (onFinished) {
        if (!this._dataList || this._dataList.length === 0) {
            return;
        }

        Excel.process(() => {
            this._createTitle();
            this._createHeader();
            this._createBodyData();
            this._createExcelConfig();

            const fileName = 'ExchangeVoidedTicket';
            Excel.export(this.worksheet, fileName);
            onFinished && onFinished();
        })
    }

    _createTitle() {
        this.mergeConfig.push({ s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: this.columnConfigs.length - 1 } });

        const address = Excel.getCellAddress({
            c: 0,
            r: TITLE_ROW_INDEX
        });
        this.worksheet[address] = {
            t: "s",
            v: this._title,
            s: titleCellStyle
        }
    }

    _createHeader() {
        const totalColumn = this.columnConfigs.length;
        for (let i = 0; i < totalColumn; i++) {
            const field = this.columnConfigs[i].field;
            const address = Excel.getCellAddress({
                c: i,
                r: HEADER_ROW_INDEX
            });
            this.worksheet[address] = {
                t: "s",
                v: headerText[field] || '#',
                s: headerCellStyle
            }
        }
    }

    _createBodyData() {
        const totalColumn = this.columnConfigs.length;
        for (let i = 0; i < this._dataList.length; i++) {
            const item = this._dataList[i];

            for (let j = 0; j < totalColumn; j++) {
                const address = Excel.getCellAddress({
                    c: j,
                    r: this.currentRowIndex
                });

                this.worksheet[address] = this._buildDataRow(item, j, i);
            }

            this.currentRowIndex++;
        }
    }

    _buildDataRow(item, colIndex, rowIndex) {
        const columnConfig = this.columnConfigs[colIndex];

        let value = item[columnConfig.field];
        if (!value) {
            value = columnConfig.field === fields.index ? rowIndex + 1 : VOIDED_STATUS;
        }

        return {
            t: Excel.getCellType(value),
            v: value,
            s: columnConfig[MAIN_SECTION_STYLE]
        }
    }

    _createExcelConfig() {
        const cellStart = Excel.getCellAddress({
            r: 0,
            c: 0
        });
        const cellEnd = Excel.getCellAddress({
            r: this.currentRowIndex,
            c: this.columnConfigs.length
        });

        this.worksheet["!ref"] = `${cellStart}:${cellEnd}`;
        this.worksheet['!cols'] = this.columnConfigs.map(x => ({ wpx: x.width }));
        this.worksheet["!merges"] = this.mergeConfig;
    }

    _getcolumnConfigs() {
        return [{
            field: fields.index,
            [MAIN_SECTION_STYLE]: bodyCellStyle.center,
            width: 80
        }, {
            field: fields.refNo,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 150
        }, {
            field: fields.eventType,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 200
        }, {
            field: fields.event,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 350
        }, {
            field: fields.status,
            [MAIN_SECTION_STYLE]: bodyCellStyle.center,
            width: 100
        }];
    }
}