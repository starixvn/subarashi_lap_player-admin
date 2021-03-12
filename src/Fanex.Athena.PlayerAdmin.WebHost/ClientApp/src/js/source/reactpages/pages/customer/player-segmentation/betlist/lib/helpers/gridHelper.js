import { DefaultPagingOption } from '../constants';

export default class GridHelper {
    static GetPagingOption(selectedParams, gridData) {
        return {
            ...DefaultPagingOption,
            ...{
                PageIndex: selectedParams.pageIndex,
                PageSize: selectedParams.pageSize,
                TotalItem: gridData && gridData.TotalRows ? gridData.TotalRows : selectedParams.PageSize
            }
        };
    }

    static GetHeaderName = (column, headerText, isAllowedCustomerComm) => {
        if (column === 'LicenseeWL') {
            return `${headerText.LicenseeWL}/${headerText.Comm}`;
        }

        if (column === 'WinLoss' && isAllowedCustomerComm) {
            return `${headerText.WinLoss}/${headerText.Comm}`;
        }

        return headerText[column];
    }
}