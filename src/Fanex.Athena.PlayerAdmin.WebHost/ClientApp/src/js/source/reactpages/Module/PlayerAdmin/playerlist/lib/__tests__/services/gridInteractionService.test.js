import GridInteractionService from '../../services/gridInteractionService';
import GridHelper from '../../helpers/gridHelper';

describe('gridInteractionService', () => {
    describe('getNextDropdownPosition', () => {
        const dropdownItemName = 'statusDropdown';
        const yPos = 100;
        let originalCalculateTopPosition;

        beforeEach(() => {
            originalCalculateTopPosition = GridHelper.calculateTopPosition;
            GridHelper.calculateTopPosition = jest.fn(() => yPos);
        })

        afterEach(() => {
            GridHelper.calculateTopPosition = originalCalculateTopPosition;
        })

        it('current position is not null - dropdown hides - return null', () => {
            const currentPosition = { x: 10, y: 10 };
            const element = {
                getBoundingClientRect: () => ({ left: 0, top: 0 })
            }

            const result = GridInteractionService.getNextDropdownPosition(element, currentPosition, dropdownItemName);

            expect(result).toBeNull();
        })

        it('current position is null - dropdown shows - return correct position information', () => {
            const currentPosition = null;
            const element = {
                getBoundingClientRect: () => ({ left: 0, top: 0 })
            }

            const result = GridInteractionService.getNextDropdownPosition(element, currentPosition, dropdownItemName);

            expect(result.x).toBe(0);
            expect(result.y).toBe(yPos);
        })
    })

    describe('updatePlayerItem', () => {
        const playerList = [
            { IbcCustId: 1, LicenseeCustId: 11, field1: 'field11', field2: 'field12' },
            { IbcCustId: 2, LicenseeCustId: 22, field1: 'field21', field2: 'field22' }
        ]
        it('item in data list - update item correctly with new data list', () => {
            const testByCustIdParam = (custId) => {
                const value = 'NewValue11',
                    fieldName = 'field1';

                const expectedUpdatingItem = { IbcCustId: 1, LicenseeCustId: 11, field1: 'NewValue11', field2: 'field12' };

                const newPlayerList = GridInteractionService.updatePlayerItem(playerList, custId, value, fieldName);
                const newDataItem = newPlayerList.find(x => x.IbcCustId === expectedUpdatingItem.IbcCustId);

                expect(newPlayerList).not.toBe(playerList);
                expect(newDataItem).toEqual(expectedUpdatingItem);
            }

            testByCustIdParam(1);
            testByCustIdParam(11);
        })

        it('item match by LicenseeCustId - update item correctly with new data list', () => {
            const custId = 11,
                value = 'NewValue11',
                fieldName = 'field1';

            const expectedUpdatingItem = { IbcCustId: 1, LicenseeCustId: 11, field1: 'NewValue11', field2: 'field12' };

            const newPlayerList = GridInteractionService.updatePlayerItem(playerList, custId, value, fieldName);
            const newDataItem = newPlayerList.find(x => x.IbcCustId === expectedUpdatingItem.IbcCustId);

            expect(newPlayerList).not.toBe(playerList);
            expect(newDataItem).toEqual(expectedUpdatingItem);
        })
    })

    it('updatePlayerWithBaStatus - data item is updated correctly', () => {
        const playerList = [
            { IbcCustId: 1, LicenseeCustId: 11, field1: 'field11', field2: 'field12', BaStatus: false, OddsGroup: 'C' },
            { IbcCustId: 2, LicenseeCustId: 22, field1: 'field21', field2: 'field22', BaStatus: true, OddsGroup: 'B' }
        ]
        const ibcCustId = 1;
        const isBa = true;
        const oddsGroup = 'A';

        const expectedUpdatingItem = { IbcCustId: 1, LicenseeCustId: 11, field1: 'field11', field2: 'field12', BaStatus: true, OddsGroup: 'A' };

        const newPlayerList = GridInteractionService.updatePlayerWithBaStatus(playerList, ibcCustId, isBa, oddsGroup);
        const newDataItem = newPlayerList.find(x => x.IbcCustId === expectedUpdatingItem.IbcCustId);

        expect(newPlayerList).not.toBe(playerList);
        expect(newDataItem).toEqual(expectedUpdatingItem);
    })

    describe('sortPlayerList', () => {
        const playerList = [
            { IbcCustId: 1, LicenseeCustId: 11, UserName: 'aa', CurrentBalance: '25', OddsGroup: 'C', LastTransaction: '09/01/2019' },
            { IbcCustId: 2, LicenseeCustId: 22, UserName: 'bb', CurrentBalance: '5', OddsGroup: 'B', LastTransaction: '09/30/2019' },
            { IbcCustId: 3, LicenseeCustId: 33, UserName: '', CurrentBalance: '15', OddsGroup: 'B', LastTransaction: '09/05/2019' }
        ]

        it('sort ascending with string data type - Return correct data list', () => {
            const sortBy = 'UserName';
            const sortOrder = 'asc';

            const sortedPlayerList = GridInteractionService.sortPlayerList(playerList, sortBy, sortOrder);

            expect(sortedPlayerList[0].IbcCustId).toBe(3);
            expect(sortedPlayerList[1].IbcCustId).toBe(1);
            expect(sortedPlayerList[2].IbcCustId).toBe(2);
        })

        it('sort descending with string data type - Return correct data list', () => {
            const sortBy = 'UserName';
            const sortOrder = 'desc';

            const sortedPlayerList = GridInteractionService.sortPlayerList(playerList, sortBy, sortOrder);

            expect(sortedPlayerList[0].IbcCustId).toBe(2);
            expect(sortedPlayerList[1].IbcCustId).toBe(1);
            expect(sortedPlayerList[2].IbcCustId).toBe(3);
        })

        it('sort ascending with date time data type - Return correct data list', () => {
            const sortBy = 'LastTransaction';
            const sortOrder = 'asc';

            const sortedPlayerList = GridInteractionService.sortPlayerList(playerList, sortBy, sortOrder);

            expect(sortedPlayerList[0].IbcCustId).toBe(1);
            expect(sortedPlayerList[1].IbcCustId).toBe(3);
            expect(sortedPlayerList[2].IbcCustId).toBe(2);
        })

        it('sort ascending with date time data type - value is null - return correct data list', () => {
            const sortBy = 'LastTransaction';
            const sortOrder = 'asc';
            const entryPlayerList = [
                ...playerList,
                { IbcCustId: 4, LicenseeCustId: 44, UserName: 'Test4', CurrentBalance: '15', OddsGroup: 'B', LastTransaction: null }
            ]

            const sortedPlayerList = GridInteractionService.sortPlayerList(entryPlayerList, sortBy, sortOrder);

            expect(sortedPlayerList[0].IbcCustId).toBe(4);
            expect(sortedPlayerList[1].IbcCustId).toBe(1);
            expect(sortedPlayerList[2].IbcCustId).toBe(3);
        })

        it('data list is empty - return empty list', () => {
            const sortBy = 'UserName';
            const sortOrder = 'asc';

            const sortedPlayerList = GridInteractionService.sortPlayerList([], sortBy, sortOrder);

            expect(sortedPlayerList).toEqual([]);
        })
    })
})