import { buildDefaultColumnList, buildColumnList } from '../../helpers/gridColumnBuilder';
import { fields, licenseeType } from '../../constants';

describe('gridColumnBuilder', () => {
    const {
        Status,
        UserName,
        SystemId,
        RealName,
        Currency,
        CurrentBalance,
        OutstandingBalance,
        OddsGroup,
        BaStatus,
        SpecialLevel,
        LastTransaction,
        Email
    } = fields;

    it('buildDefaultColumnList - return correct result', () => {
        const runTest = (licType, expectedResult) => {
            const result = buildDefaultColumnList(licType).map(x => x.key);

            expect(result).toEqual(expectedResult);
        }

        runTest(licenseeType.api, [Status, UserName, SystemId, Currency, CurrentBalance,
            OutstandingBalance, OddsGroup, SpecialLevel
        ])

        runTest(licenseeType.nonApi, [Status, UserName, SystemId, RealName, Currency, CurrentBalance,
            OutstandingBalance, OddsGroup, SpecialLevel, LastTransaction, Email
        ])
    })

    it('buildColumnList - return correct result', () => {
        const runTest = (licType, isAllowedBA, expectedResult) => {
            const result = buildColumnList(licType, isAllowedBA).map(x => x.key);

            expect(result).toEqual(expectedResult);
        }

        runTest(licenseeType.api, true, [Status, UserName, SystemId, Currency, CurrentBalance,
            OutstandingBalance, OddsGroup, BaStatus, SpecialLevel
        ])

        runTest(licenseeType.nonApi, false, [Status, UserName, SystemId, RealName, Currency, CurrentBalance,
            OutstandingBalance, OddsGroup, SpecialLevel, LastTransaction, Email
        ])
    })
})