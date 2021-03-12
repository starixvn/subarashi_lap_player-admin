import LangHelper from '@jsroot/lib/langHelper'
import Site from '@jsroot/lib/site'

export default class Service {
    static getTitle(fromDate, toDate, baseCurrency) {
        const dateRangeText = fromDate && toDate 
            ? `, ${Site.convertDateToString(fromDate)} → ${Site.convertDateToString(toDate)}` 
            : '';

        const baseCurrencyText = baseCurrency ? `, ${LangHelper.getSingleResource("BaseCurrency")}: ${baseCurrency}` : '';

        return LangHelper.getSingleResource("BettingTicketCenter") + dateRangeText + baseCurrencyText;
    }
}