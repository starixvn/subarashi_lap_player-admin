import Site from '@jsroot/lib/site'

export default class ParamHelper {
    static ToAPIParams({ selectedParams, username, selectedCurrency, selectedBaseCurrency, isGetAll, dateRange, selectedReason, selectedSubsidiary, subsidiarySearch, selectedCustomerClass }) {
        return {
            ...selectedParams, 
            ...{
                username,
                currencyIdList: selectedCurrency,
                baseCurrency: selectedBaseCurrency,
                reasonGroupIdList: selectedReason,
                isGetAll,
                fromDate: Site.convertDateToString(dateRange.fromDate),
                toDate: Site.convertDateToString(dateRange.toDate),
                subsidiaryList: subsidiarySearch ? selectedSubsidiary : '',
                subsidiarySearch,
                customerClass: selectedCustomerClass
            }
        }
    }
}
