export default class ParamHelper {
    static ToAPIParams({ selectedParams, username, selectedCurrency, selectedVipOption, selectedSubsidiary, subsidiarySearch, selectedCustomerClass } ) {
        return {
            ...selectedParams,
            ...{
                username,
                currencyIdList: selectedCurrency,
                vipOption: selectedVipOption,
                subsidiaryList: subsidiarySearch ? selectedSubsidiary : '',
                subsidiarySearch,
                customerClass: selectedCustomerClass
            }
        }
    }
}
