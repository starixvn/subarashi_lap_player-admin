import ReportExcel from '../reportExcel';
import { headerCellStyle, bodyCellStyle } from '@jsroot/constants/excelStyle';
import Excel from '@jsroot/lib/excel';
import { fields } from '../constants';

describe('reportExcel', () => {
    let originalProcessExcel;
    let originalExportExcel;
    let originalGetCellAddress;
    beforeAll(() => {
        originalProcessExcel = Excel.process;
        originalExportExcel = Excel.export;
        originalGetCellAddress = Excel.getCellAddress;
    })
    afterAll(() => {
        Excel.process = originalProcessExcel;
        Excel.export = originalExportExcel;
        Excel.getCellAddress = originalGetCellAddress;
    })

    const MAIN_SECTION_STYLE = 'mainSectionStyle';
    it('constructor - initial values should be correct', () => {
        const data = [];
        const displayedColumn = {
            [fields.Status]: true
        };
        const expectedColumnConfig = [{
            field: fields.Status,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            width: 60
        }]

        const instance = new ReportExcel(data, displayedColumn, 'title');

        expect(instance.data).toBe(data);
        expect(instance.title).toBe('title');
        expect(instance.displayedColumn).toBe(displayedColumn);
        expect(instance.currentRowIndex).toBe(1);
        expect(instance.columnConfigs).toEqual(expectedColumnConfig);
    })

    it('export - Excel.export should be called with correct parameter', () => {
        let processHandler;
        Excel.process = jest.fn(cb => processHandler = cb);

        Excel.export = jest.fn();

        const instance = new ReportExcel([], {}, 'title');
        instance._createHeader = jest.fn();
        instance._createBodyData = jest.fn();
        instance._createExcelConfig = jest.fn();

        instance.export(() => {});
        processHandler();

        expect(instance._createHeader).toHaveBeenCalled();
        expect(instance._createBodyData).toHaveBeenCalled();
        expect(instance._createExcelConfig).toHaveBeenCalled();
        expect(Excel.export).toHaveBeenCalledWith({}, 'title');
    })

    it('_createHeader - worksheet should has correct value', () => {
        const displayedColumn = {
            [fields.Status]: true
        };
        Excel.getCellAddress = jest.fn(addr => `${addr.c}:${addr.r}`);
        const expectedWorkSheet = {
            '0:0': { t: 's', v: 'Status', s: headerCellStyle }
        }

        const instance = new ReportExcel([], displayedColumn, '');
        instance._createHeader();

        expect(instance.worksheet).toEqual(expectedWorkSheet);
    })

    it('_createBodyData - worksheet should has correct value', () => {
        const displayedColumn = {
            [fields.UserName]: true,
            [fields.BaStatus]: true
        };
        Excel.getCellAddress = jest.fn(addr => `${addr.c}:${addr.r}`);
        const data = [
            { UserName: 'test01', BaStatus: true, IsDeposit: true, IsArchived: true },
            { UserName: 'test02', BaStatus: true, IsDeposit: true, IsArchived: false }
        ]

        const expectedWorkSheet = {
            '0:1': { t: 's', v: 'test01', s: bodyCellStyle.left },
            '1:1': { t: 's', v: '', s: bodyCellStyle.center },
            '0:2': { t: 's', v: 'test02', s: bodyCellStyle.left },
            '1:2': { t: 's', v: 'On', s: bodyCellStyle.center }
        }

        const instance = new ReportExcel(data, displayedColumn, '');
        instance._createBodyData();

        expect(instance.worksheet).toEqual(expectedWorkSheet);
    })

    it('_createExcelConfig - worksheet should has correct config values', () => {
        const displayedColumn = {
            [fields.UserName]: true,
            [fields.BaStatus]: true
        };
        Excel.getCellAddress = jest.fn(addr => `${addr.c}:${addr.r}`);

        const expectedWorkSheet = {
            '!ref': '0:0:2:1',
            '!cols': [{ wpx: 200 }, { wpx: 70 }]
        }

        const instance = new ReportExcel([], displayedColumn, '');
        instance.currentRowIndex = 1;
        instance._createExcelConfig();

        expect(instance.worksheet).toEqual(expectedWorkSheet);
    })
})