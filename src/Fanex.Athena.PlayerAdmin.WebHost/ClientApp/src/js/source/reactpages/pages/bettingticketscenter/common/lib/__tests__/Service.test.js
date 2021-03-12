import LangHelper from '@jsroot/lib/langHelper'
import Site from '@jsroot/lib/site'
import Report from '@jsroot/lib/report'

import Service from '../Service'

describe('Service ', () => {
    const fromDate = "01/01/2017";
    const toDate = "10/10/2017";
    const currency = "USD";
    const baseCurrency = "EUR";

    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn(resource => resource);

        Site.convertDateToString = jest.fn((date) => date);
    })
    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('getTitle', () => {
        it('return correct content', () => {
            const expectedResult = `BettingTicketCenter, ${fromDate} → ${toDate}, BaseCurrency: ${baseCurrency}`;
            
            const result = Service.getTitle(fromDate, toDate, baseCurrency);

            expect(result).toBe(expectedResult);
        });

        it('fromDate, toDate are null - return correct content', () => {
            const expectedResult = `BettingTicketCenter, BaseCurrency: ${baseCurrency}`;
            
            const result = Service.getTitle(null, null, baseCurrency);

            expect(result).toBe(expectedResult);
        });

        it('BaseCurrency is null - return correct content', () => {
            const expectedResult = `BettingTicketCenter, ${fromDate} → ${toDate}`;
            
            const result = Service.getTitle(fromDate, toDate, null);

            expect(result).toBe(expectedResult);
        })
    })
})