import Excel from '../../../lib/excel'
import LangHelper from '../../../lib/langHelper'
import { titleCellStyle, headerCellStyle, bodyCellStyle, totalCellStyle, grandTotalCellStyle } from '../../../constants/excelStyle'
import { HeaderText, TotalText } from '../lib/service'

const TITLE_ROW_INDEX = 0;
const HEADER_ROW_INDEX = 1;
const DATA_ROW_INDEX = 2;

const MAIN_SECTION_STYLE = 'mainSectionStyle';
const TOTAL_SECTION_STYLE = 'totalSectionStyle';
const GRAND_TOTAL_SECTION_STYLE = 'grandTotalSectionStyle';

const fields = {
    whiteLabel: 'WhiteLabel',
    subsidiary: 'Subsidiary',
    currencyName: 'CurrencyName',
    product: 'Product',
    betCount: 'BetCount',
    turnover: 'Turnover',
    customerWinLoss: 'CustomerWinLoss',
    subsidiaryWinLoss: 'SubsidiaryWinLoss',
    externalId: 'ExternalId',
    systemId: 'SystemId'
}

export default class ReportExcel {
    constructor(gridData = {}, title, isMaster = false, isCustomerLevel = false, baseCurrency = '', fileName = 'SubsidiaryPerformance') {
        this.worksheet = {};
        this.title = title;
        this.fileName = fileName;
        this.gridData = gridData;
        this.currentRowIndex = DATA_ROW_INDEX;
        this.mergeConfig = [];
        this.columnConfigs = this._getcolumnConfigs(isMaster, isCustomerLevel);
        this.baseCurrency = baseCurrency;
        this.isCustomerLevel = isCustomerLevel;
        this.isMaster = isMaster;
        this.columnPosition = this._getcolumnPosition(isMaster, isCustomerLevel);
    }

    export (onFinished) {
        if (!this.gridData.Data || !this.gridData.Data.length) {
            onFinished && onFinished();
            return;
        }

        Excel.process(() => { 
            this._createTitle();
            this._createHeader();
            this._createSheetData();
            this._createExcelConfig();

            Excel.export(this.worksheet, this.fileName);
            onFinished && onFinished();
        });
    }

    _createTitle() {
        this.mergeConfig.push({ s: { r: TITLE_ROW_INDEX, c: 0 }, e: { r: TITLE_ROW_INDEX, c: this.columnPosition.SubsidiaryWL } });
        const address = Excel.getCellAddress({ c: 0, r: TITLE_ROW_INDEX });
        this.worksheet[address] = { t: 's', v: this.title, s: titleCellStyle }
    }

    _createHeader() {
        for (let i = 0; i < this.columnConfigs.length; i++) {
            const fieldName = this.columnConfigs[i].field;
            const address = Excel.getCellAddress({ c: i, r: HEADER_ROW_INDEX });
            const header = HeaderText[fieldName];
            this.worksheet[address] = { t: 's', v: header, s: headerCellStyle }
        }
    }

    _createSheetData() {
        if(this.isCustomerLevel) {
            this._createBodyDetailData();
            this._createFooterDetailData();
            this._createGrandTotalDetailData();
        }
        else {
            this._createBodyData();
            this._createFooterData();
            this._createGrandTotalData();
        }
    }

    _createBodyData() {
        const mainData = this.gridData.Data;

        for (let w = 0; w < mainData.length; w++) {
            const subData = mainData[w];

            if (this.isMaster) {
                this._createRowByColumnIndex({ WhiteLabel: subData.WhiteLabel }, MAIN_SECTION_STYLE, this.columnPosition.WhiteLabel, this.columnPosition.WhiteLabel);
                this.mergeConfig.push({ s: { r: this.currentRowIndex, c: this.columnPosition.WhiteLabel }, e: { r: this.currentRowIndex + subData.TotalItem - 1, c: this.columnPosition.WhiteLabel } });
            }

            for (let i = 0; i < subData.Details.length; i++) {
                const item = subData.Details[i];
                const details = item.Details;

                this._createRowByColumnIndex({ Subsidiary: item.Subsidiary }, MAIN_SECTION_STYLE, this.columnPosition.Subsidiary, this.columnPosition.Subsidiary);
                this.mergeConfig.push({ s: { r: this.currentRowIndex, c: this.columnPosition.Subsidiary }, e: { r: this.currentRowIndex + item.TotalItem - 1, c: this.columnPosition.Subsidiary } });

                const currencies = Object.keys(item.Details);
                for (let j = 0; j < currencies.length; j++) {
                    const currency = currencies[j];
                    const products = item.Details[currency];

                    this._createRowByColumnIndex({ CurrencyName: currency }, MAIN_SECTION_STYLE, this.columnPosition.Currency, this.columnPosition.Currency);
                    this.mergeConfig.push({ s: { r: this.currentRowIndex, c: this.columnPosition.Currency }, e: { r: this.currentRowIndex + products.length - 1, c: this.columnPosition.Currency } });

                    for (let k = 0; k < products.length; k++) {
                        const product = products[k];

                        this._createRowByColumnIndex(product, MAIN_SECTION_STYLE, this.columnPosition.Product, this.columnPosition.SubsidiaryWL);
                        this.currentRowIndex++;
                    }
                }
            }
        }
    }

    _createBodyDetailData() {
        const mainData = this.gridData.Data;

        for (let i = 0; i < mainData.length; i++) {
            const item = mainData[i];
            item.Index = i + 1;

            this._createRowByColumnIndex(item, MAIN_SECTION_STYLE, this.columnPosition.Index, this.columnPosition.SubsidiaryWL);
            this.currentRowIndex++;
        }
    }

    _createFooterData() {
        const totalDataList = this.gridData.Total;
        const subs = Object.keys(totalDataList);

        for (let i = 0; i < subs.length; i++) {
            const item = totalDataList[subs[i]];
           
            if (this.isMaster) {
                this._createRowByColumnIndex({ WhiteLabel: TotalText.Total + ' ' + subs[i], CurrencyName: TotalText.Based + ' ' + this.baseCurrency }, TOTAL_SECTION_STYLE, this.columnPosition.WhiteLabel, this.columnPosition.Currency);
                this.mergeConfig.push({ s: { r: this.currentRowIndex, c: this.columnPosition.WhiteLabel }, e: { r: this.currentRowIndex, c: this.columnPosition.Subsidiary } });
            }
            else {
                this._createRowByColumnIndex({ Subsidiary: TotalText.Total + ' ' + subs[i], CurrencyName: TotalText.Based + ' ' + this.baseCurrency }, TOTAL_SECTION_STYLE, this.columnPosition.Subsidiary, this.columnPosition.Currency);
            }
          
            this.mergeConfig.push({ s: { r: this.currentRowIndex, c: this.columnPosition.Currency }, e: { r: this.currentRowIndex, c: this.columnPosition.Product } });
            this._createRowByColumnIndex(item, TOTAL_SECTION_STYLE, this.columnPosition.BetCount, this.columnPosition.SubsidiaryWL);
            this.currentRowIndex++;
        }
    }

    _createFooterDetailData() {
        const totalDataList = this.gridData.Total;
        const subs = Object.keys(totalDataList);
        const rowSpan = subs.length;

        this._createRowByColumnIndex({ Index: TotalText.TotalIn + ' ' + this.baseCurrency }, TOTAL_SECTION_STYLE, this.columnPosition.Index, this.columnPosition.Index);
        this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 0 }, e: { r: this.currentRowIndex + rowSpan - 1, c: 2 } });

        for (let i = 0; i < rowSpan; i++) {
            const item = totalDataList[subs[i]];

            this._createRowByColumnIndex(item, TOTAL_SECTION_STYLE, this.columnPosition.Currency, this.columnPosition.SubsidiaryWL);
            this.currentRowIndex++;
        }
    }

    _createGrandTotalData() {
        const grandTotal = this.gridData.GrandTotal;
      
        if (this.isMaster) {
            this.mergeConfig.push({ s: { r: this.currentRowIndex, c: this.columnPosition.WhiteLabel }, e: { r: this.currentRowIndex, c: this.columnPosition.Subsidiary } });
            this._createRowByColumnIndex({ WhiteLabel: TotalText.Total, CurrencyName: TotalText.Based + ' ' + this.baseCurrency }, GRAND_TOTAL_SECTION_STYLE, this.columnPosition.WhiteLabel, this.columnPosition.Currency);
        }
        else {
            this._createRowByColumnIndex({ Subsidiary: TotalText.Total, CurrencyName: TotalText.Based + ' ' + this.baseCurrency }, GRAND_TOTAL_SECTION_STYLE, this.columnPosition.Subsidiary, this.columnPosition.Currency);
        }

        this.mergeConfig.push({ s: { r: this.currentRowIndex, c: this.columnPosition.Currency }, e: { r: this.currentRowIndex, c: this.columnPosition.Product } });
        this._createRowByColumnIndex(grandTotal, GRAND_TOTAL_SECTION_STYLE, this.columnPosition.BetCount, this.columnPosition.SubsidiaryWL);
    }

    _createGrandTotalDetailData() {
        this.mergeConfig.push({ s: { r: this.currentRowIndex, c: 0 }, e: { r: this.currentRowIndex, c: 3 } });
        const grandTotal = this.gridData.GrandTotal;

        this._createRowByColumnIndex({ Index: TotalText.TotalIn + ' ' + this.baseCurrency }, GRAND_TOTAL_SECTION_STYLE, this.columnPosition.Index, this.columnPosition.Index);
        this._createRowByColumnIndex(grandTotal, GRAND_TOTAL_SECTION_STYLE, this.columnPosition.BetCount, this.columnPosition.SubsidiaryWL);
    }

    _createExcelConfig() {
        const cellStart = Excel.getCellAddress({ r: 0, c: 0 });
        const cellEnd = Excel.getCellAddress({ r: this.currentRowIndex, c: this.columnPosition.SubsidiaryWL });

        this.worksheet['!merges'] = this.mergeConfig;
        this.worksheet['!ref'] = `${cellStart}:${cellEnd}`;
        const wscols = this.isCustomerLevel 
            ? [{ wpx: 30 }, { wpx: 180 }, { wpx: 180 }, { wpx: 180 }, { wpx: 180 }, { wpx: 250 }, { wpx: 250 }, { wpx: 250 }]
            : (this.isMaster
                ? [{ wpx: 150 }, { wpx: 120 }, { wpx: 180 }, { wpx: 120 }, { wpx: 250 }, { wpx: 250 }, { wpx: 250 }, { wpx: 250 }]
                : [{ wpx: 150 }, { wpx: 120 }, { wpx: 180 }, { wpx: 120 }, { wpx: 250 }, { wpx: 250 }, { wpx: 250 }]);

        this.worksheet['!cols'] = wscols;
    }

    _createRowByColumnIndex(item, sectionStyle, startIndex, endIndex) {
        for (let i = startIndex; i <= endIndex; i++) {
            const { cellAddress, type, value, style } = this._getCellObject(item, sectionStyle, i);
            
            if (cellAddress) { 
                this.worksheet[cellAddress] = { t: type, v: value, s: style };
            }
        }
    }

    _getCellObject(item, sectionStyle, columnIndex) {
        const columnConfig = this.columnConfigs[columnIndex];
        if (!columnConfig) {
            return { cellAddress: null };
        }

        const field = columnConfig.field;

        const cellAddress = Excel.getCellAddress({ c: columnIndex, r: this.currentRowIndex });
        const type = typeof item[columnConfig.field] === 'number' ? 'n' : 's';
        const value = item[field] !== null ? item[field] : '';

        let style = columnConfig[sectionStyle];
        if (field === fields.subsidiaryWinLoss) {
            style = value < 0 ? style.negative : style.positive;
        }

        return { cellAddress, type, value, style };
    }

    _getcolumnConfigs(isMaster, isCustomerLevel) {
        var columns = [];

        if (isCustomerLevel) {
            columns.push(
                {
                    field: 'Index',
                    [MAIN_SECTION_STYLE]: bodyCellStyle.center,
                    [TOTAL_SECTION_STYLE]: totalCellStyle.center,
                    [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.center
                },
                {
                    field: fields.externalId,
                    [MAIN_SECTION_STYLE]: bodyCellStyle.left
                },
                {
                    field: fields.systemId,
                    [MAIN_SECTION_STYLE]: bodyCellStyle.left
                },
                {
                    field: fields.currencyName,
                    [MAIN_SECTION_STYLE]: bodyCellStyle.center,
                    [TOTAL_SECTION_STYLE]: totalCellStyle.center
                },
            );
        }
        else
        {
            if (isMaster) {
                columns.push(
                    {
                        field: fields.whiteLabel,
                        [MAIN_SECTION_STYLE]: bodyCellStyle.rowspan,
                        [TOTAL_SECTION_STYLE]: totalCellStyle.default,
                        [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.default
                    },
                    {
                        field: fields.subsidiary,
                        [MAIN_SECTION_STYLE]: bodyCellStyle.rowspan,
                    },
                );
            }
            else {
                columns.push(
                    {
                        field: fields.subsidiary,
                        [MAIN_SECTION_STYLE]: bodyCellStyle.rowspan,
                        [TOTAL_SECTION_STYLE]: totalCellStyle.default,
                        [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.default
                    }
                );
            }

            columns.push(
                {
                    field: fields.currencyName,
                    [MAIN_SECTION_STYLE]: bodyCellStyle.center,
                    [TOTAL_SECTION_STYLE]: totalCellStyle.center,
                    [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.center
                },
                {
                    field: fields.product,
                    [MAIN_SECTION_STYLE]: bodyCellStyle.default
                },
            );
        }


        columns.push(
            {
                field: fields.betCount,
                [MAIN_SECTION_STYLE]: bodyCellStyle.number,
                [TOTAL_SECTION_STYLE]: totalCellStyle.number,
                [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.number
            },
            {
                field: fields.turnover,
                [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
                [TOTAL_SECTION_STYLE]: totalCellStyle.decimal,
                [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.decimal
            },
            {
                field: fields.customerWinLoss,
                [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
                [TOTAL_SECTION_STYLE]: totalCellStyle.decimal,
                [GRAND_TOTAL_SECTION_STYLE]: grandTotalCellStyle.decimal
            },
            {
                field: fields.subsidiaryWinLoss,
                [MAIN_SECTION_STYLE]: { positive: bodyCellStyle.decimal, negative: bodyCellStyle.negativeDecimal },
                [TOTAL_SECTION_STYLE]: { positive: totalCellStyle.decimal, negative: totalCellStyle.negativeDecimal },
                [GRAND_TOTAL_SECTION_STYLE]: { positive: grandTotalCellStyle.decimal, negative: grandTotalCellStyle.negativeDecimal }
            }   
        );

        return columns;
    }

    _getcolumnPosition(isMaster, isCustomerLevel) {
        if (isCustomerLevel) {
            return {
                Index: 0,
                Currency: 3,
                BetCount: 4,
                SubsidiaryWL: 7
            };
        }

        if (isMaster) {
            return {
                WhiteLabel: 0,
                Subsidiary: 1,
                Currency: 2,
                Product: 3,
                BetCount: 4,
                SubsidiaryWL: 7
            };
        }

        return {
            Subsidiary: 0,
            Currency: 1,
            Product: 2,
            BetCount: 3,
            SubsidiaryWL: 6
        };
    }
}