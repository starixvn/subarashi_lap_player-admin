import Site from '@jsroot/lib/site'

export default class ParamHelper {
    static ToAPIParams({ selectedParams, dateRange, custId, baseCurrency }) {
        return {
            ...selectedParams, 
            ...{
                custId,
                baseCurrency,
                fromDate: Site.convertDateToString(dateRange.fromDate),
                toDate: Site.convertDateToString(dateRange.toDate)
            }
        }
    }
}