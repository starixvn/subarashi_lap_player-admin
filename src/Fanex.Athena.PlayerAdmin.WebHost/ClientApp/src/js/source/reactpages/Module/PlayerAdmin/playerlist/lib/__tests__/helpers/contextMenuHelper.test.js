import { getActiveTabList } from '../../helpers/contextMenuHelper';
import { playerStatusByName, contextMenuKeys, licenseeType } from '../../constants';

describe('contextMenuHelper', () => {
    const tabNames = {
        playerInfo: 'lblPlayerAdminPlayerInfo',
        statement: 'lblPlayerAdminPlayerStatement',
        betlist: 'lblPlayerAdminBetList',
        betLimit: 'lblPlayerAdminBetLimit',
        note: 'lblPlayerAdminNote',
        password: 'lblPlayerAdminSetting',
        bankInfo: 'lblPlayerAdminPlayerInfo',
        fundTransfer: 'lblPlayerAdminFundTransfer',
        personalMessage: 'lblPlayerAdminMessage'
    }

    it('getActiveTabList - return correct result', () => {
        const runTest = (player, tabList, licType, expectedResult) => {
            const result = getActiveTabList(player, tabList, licType).map(x => x.key);

            expect(result).toEqual(expectedResult);
        }

        const commonTabList = Object.keys(tabNames).map(k => ({ Name: tabNames[k] }));

        const testCaseList = [{
                player: { isDeposit: true, isArchived: false, status: playerStatusByName.Active },
                tabList: commonTabList,
                licType: licenseeType.api,
                expectedResult: [contextMenuKeys.note, contextMenuKeys.fundtransferLimit, contextMenuKeys.betLimit, contextMenuKeys.statement]
            },
            {
                player: { isDeposit: false, isArchived: false, status: playerStatusByName.Active },
                tabList: commonTabList,
                licType: licenseeType.nonApi,
                expectedResult: [contextMenuKeys.note, contextMenuKeys.password, contextMenuKeys.bankInformation, contextMenuKeys.fundtransferLimit, contextMenuKeys.personalMessage]
            },
            {
                player: { isDeposit: true, isArchived: false, status: playerStatusByName.Closed },
                tabList: commonTabList,
                licType: licenseeType.nonApi,
                expectedResult: [contextMenuKeys.note, contextMenuKeys.bankInformation, contextMenuKeys.fundtransferLimit, contextMenuKeys.betLimit, contextMenuKeys.statement, contextMenuKeys.personalMessage]
            },
            {
                player: { isDeposit: true, isArchived: true, status: playerStatusByName.Active },
                tabList: commonTabList,
                licType: licenseeType.nonApi,
                expectedResult: [contextMenuKeys.note, contextMenuKeys.bankInformation, contextMenuKeys.fundtransferLimit, contextMenuKeys.personalMessage]
            }
        ]

        testCaseList.forEach(t => runTest(t.player, t.tabList, t.licType, t.expectedResult));
    })
})