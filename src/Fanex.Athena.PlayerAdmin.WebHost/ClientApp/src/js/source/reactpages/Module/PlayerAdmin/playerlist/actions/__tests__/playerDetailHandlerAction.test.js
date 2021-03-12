import * as playerDetailHandlerAction from '../playerDetailHandlerAction';
import { SET_SELECTED_PLAYERID_LIST, SET_SELECTED_TAB_NAME } from '../types';

const mockGetLink = jest.fn();
const mockGetTabList = jest.fn();
jest.mock('../../lib/services/playerQueryService', () => {
    return jest.fn().mockImplementation(() => {
        return {
            getLink: mockGetLink,
            getTabList: mockGetTabList
        };
    })
});

describe('playerDetailHandlerAction', () => {
    it('getLink - should call service with correct parameter', () => {
        const systemType = 0;
        const action = 'test/index';

        playerDetailHandlerAction.getLink(systemType, action)();

        expect(mockGetLink).toHaveBeenCalledWith(systemType, action);
    })

    it('getTabList - should call service', () => {
        playerDetailHandlerAction.getTabList()();

        expect(mockGetTabList).toHaveBeenCalled();
    })

    it('setSelectedPlayerIdList - should return correct data', () => {
        const data = [1, 2];
        const expectedResult = {
            type: SET_SELECTED_PLAYERID_LIST,
            data
        }

        const result = playerDetailHandlerAction.setSelectedPlayerIdList(data);

        expect(result).toEqual(expectedResult);
    })

    it('setSelectedTabName - should return correct data', () => {
        const data = 'playerInfo';
        const expectedResult = {
            type: SET_SELECTED_TAB_NAME,
            data
        }

        const result = playerDetailHandlerAction.setSelectedTabName(data);

        expect(result).toEqual(expectedResult);
    })
})
