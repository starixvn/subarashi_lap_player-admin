import GridHelper from '../helpers/gridHelper';
import { gridItemDropdownHeight, stringTypeFields, fields } from '../constants';
import { updateObjectInArray } from '@jsroot/lib/dataReducer';

export default class GridInteractionService {
    static getNextDropdownPosition(element, currentPosition, dropdownItemName) {
        if (!element || currentPosition) {
            return null;
        }
        const dropdownHeight = gridItemDropdownHeight[dropdownItemName];
        const rect = element.getBoundingClientRect();
        return {
            x: rect.left,
            y: GridHelper.calculateTopPosition(dropdownHeight, rect.top + 20)
        };
    }
    static updatePlayerItem(playerList, custId, value, fieldName) {
        let itemIndex = playerList.findIndex(x => x.IbcCustId === custId);
        if (itemIndex === -1) {
            itemIndex = playerList.findIndex(x => x.LicenseeCustId === custId);
        }

        if (itemIndex !== -1) {
            const newPlayerItem = { ...playerList[itemIndex], [fieldName]: value };
            return updateObjectInArray(playerList, itemIndex, newPlayerItem);
        }

        return [...playerList];
    }

    static sortPlayerList(dataList, sortBy, sortOrder) {
        if (!dataList || dataList.length === 0) {
            return [];
        }

        const isSortAscending = sortOrder.toLowerCase() === 'asc';
        const newDataList = [...dataList];
        const isTringType = stringTypeFields.includes(sortBy);
        const isDateTimeField = sortBy === fields.LastTransaction;
        newDataList.sort((currentItem, nextItem) => {
            const currentValue = currentItem[sortBy];
            const nextValue = nextItem[sortBy];
            if (isTringType) {
                return this.sortStringValue(currentValue || '', nextValue || '', isSortAscending);
            }

            if (isDateTimeField) {
                const currentItemDate = currentValue ? new Date(currentValue) : 0;
                const nextItemDate = nextValue ? new Date(nextValue) : 0;
                const comparingResult = (this.isValidDate(currentItemDate) ? currentItemDate : 0) - (this.isValidDate(nextItemDate) ? nextItemDate : 0);

                return isSortAscending ? comparingResult : -comparingResult;
            }

            var cEmpty = currentValue === undefined || currentValue === null || currentValue === '';
            var nEmpty = nextValue === undefined || nextValue === null || nextValue === '';
            if (cEmpty || nEmpty) {
                var c = cEmpty ? 0 : 1;
                var n = nEmpty ? 0 : 1;
                return isSortAscending ? c - n : n - c;
            }

            var diff = currentValue - nextValue;
            if (isNaN(diff)) {
                return this.sortStringValue(currentValue.toString(), nextValue.toString(), isSortAscending);
            }

            return isSortAscending ? diff : -diff;
        });

        return newDataList;
    }

    static sortStringValue(current, next, isSortAscending) {
        const currentUpperCase = current.toUpperCase();
        const nextUpperCase = next.toUpperCase();

        const sortResult = currentUpperCase > nextUpperCase ? 1 : (currentUpperCase < nextUpperCase ? -1 : 0);

        return isSortAscending ? sortResult : -sortResult;
    }

    static isValidDate(d) {
        return d instanceof Date && !isNaN(d);
    }
}
