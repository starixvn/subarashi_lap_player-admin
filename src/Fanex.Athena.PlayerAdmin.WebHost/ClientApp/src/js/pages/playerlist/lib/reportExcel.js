import Excel from '@jsroot/lib/excel';
import NumberFormat from '@jsroot/lib/numberFormat';
import { headerCellStyle, bodyCellStyle } from '@jsroot/constants/excelStyle';
import { fields, headerText } from './constants';
import { getSpecialLevelText, getBaStatusText, getPlayerStatus, getCustomerClassText } from './helpers/utils';
import $ from 'jquery';

const HEADER_ROW_INDEX = 0;
const DATA_ROW_INDEX = 1;

const MAIN_SECTION_STYLE = 'mainSectionStyle';

const displayedTextByField = {
    [fields.SpecialLevel]: (value) => getSpecialLevelText(value),
    [fields.BaStatus]: (value) => getBaStatusText(value),
    [fields.Status]: (value) => getPlayerStatus(value)
}

const fieldListWithEmptyValueByStatus = [fields.CurrentBalance, fields.OutstandingBalance, fields.OddsGroup, fields.BaStatus, fields.SpecialLevel];

export default class ReportExcel {
    constructor(data, displayedColumn, hasCommPermissionRead, title) {
        this.data = data;
        this.title = title;
        this.displayedColumn = displayedColumn;
        this.hasCommPermissionRead = hasCommPermissionRead;

        this.worksheet = {};
        this.currentRowIndex = DATA_ROW_INDEX;
        this.columnConfigs = this._getcolumnConfigs();
    }

    export(onFinished) {
        Excel.process(() => {
            this._createHeader();
            this._createBodyData();
            this._createExcelConfig();

            Excel.export(this.worksheet, this.title);
            onFinished && onFinished();
        })
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
                v: headerText[field] || '',
                s: headerCellStyle
            }
        }
    }

    _createBodyData() {
        const totalColumn = this.columnConfigs.length;
        for (let i = 0; i < this.data.length; i++) {
            const item = this.data[i];

            for (let j = 0; j < totalColumn; j++) {
                const address = Excel.getCellAddress({
                    c: j,
                    r: this.currentRowIndex
                });

                this.worksheet[address] = this._buildDataRow(item, j);
            }

            this.currentRowIndex++;
        }
    }

    _buildDataRow(item, colIndex) {
        const columnConfig = this.columnConfigs[colIndex];
        const shouldHideValue = !item.IsDeposit || item.IsArchived;
        if (shouldHideValue && fieldListWithEmptyValueByStatus.indexOf(columnConfig.field) !== -1) {
            return {
                t: 's',
                v: '',
                s: columnConfig[MAIN_SECTION_STYLE]
            };
        }

        let value = item[columnConfig.field];
        if (columnConfig.field === fields.CurrentBalance
            || columnConfig.field === fields.OutstandingBalance) {
            value = NumberFormat.roundDown(value, 2);
        }

        if (columnConfig.field === fields.CommissionGroup) {
            value = $(".td-comm-" + item.IbcCustId).html();
        }

        if (columnConfig.field === fields.CustomerClass) {
            value = getCustomerClassText(value);
        }

        const displayedTextFunc = displayedTextByField[columnConfig.field];
        const displayedValue = this._shouldSetEmpty(value) ? '' :
            (displayedTextFunc ? displayedTextFunc(value) : value);

        return {
            t: Excel.getCellType(displayedValue),
            v: displayedValue,
            s: columnConfig[MAIN_SECTION_STYLE]
        }
    }

    _shouldSetEmpty(value) {
        return value === undefined || value === null;
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
    }

    _getcolumnConfigs() {
        var result = [{
            field: fields.Status,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 60
        }, {
            field: fields.UserName,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 200
        }, {
            field: fields.SystemId,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 200
        }, {
            field: fields.VendorMemberId,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 150
        }, {
            field: fields.RealName,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 150
        }, {
            field: fields.Currency,
            [MAIN_SECTION_STYLE]: bodyCellStyle.center,
            width: 80
        }, {
            field: fields.CurrentBalance,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            width: 130
        }, {
            field: fields.OutstandingBalance,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            width: 130
        }, {
            field: fields.OddsGroup,
            [MAIN_SECTION_STYLE]: bodyCellStyle.center,
            width: 70
        }, {
            field: fields.BaStatus,
            [MAIN_SECTION_STYLE]: bodyCellStyle.center,
            width: 70
        }, {
            field: fields.SpecialLevel,
            [MAIN_SECTION_STYLE]: bodyCellStyle.center,
            width: 70
        }, {
            field: fields.LastTransaction,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 150
        }, {
            field: fields.Email,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 200
        }];

        if (this.hasCommPermissionRead) {
            result.push({
                field: fields.CommissionGroup,
                [MAIN_SECTION_STYLE]: bodyCellStyle.center,
                width: 120
            });
        }

        result.push({
            field: fields.CustomerClass,
            [MAIN_SECTION_STYLE]: bodyCellStyle.center,
            width: 120
        });

        return result.filter(x => !!this.displayedColumn[x.field]);
    }
}
