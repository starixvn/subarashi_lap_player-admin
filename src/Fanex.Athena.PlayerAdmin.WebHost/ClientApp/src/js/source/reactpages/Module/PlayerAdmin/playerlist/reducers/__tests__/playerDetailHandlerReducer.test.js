import * as playerDetailHandlerReducer from '../playerDetailHandlerReducer';
import { SET_SELECTED_PLAYER, SET_SELECTED_PLAYERID_LIST } from '../../actions/types.js'

describe('playerDetailHandlerReducer', () => {
    const runTestReducer = ({ name, value, action, expectedResult }) => {
        it(`should handle ${name} reducer with type ${action.type} - return correct next state`, () => {
            const result = playerDetailHandlerReducer[name](value, action);
            expect(result).toEqual(expectedResult);
        });
    }

    const testCaseList = [{
            name: 'selectedPlayer',
            value: undefined,
            action: {
                type: SET_SELECTED_PLAYER,
                data: { custId: 1, userName: 'test1' }
            },
            expectedResult: { custId: 1, userName: 'test1' }
        },
        {
            name: 'selectedPlayerIdList',
            value: undefined,
            action: {
                type: SET_SELECTED_PLAYERID_LIST,
                data: [1, 2]
            },
            expectedResult: [1, 2]
        }
    ]

    testCaseList.forEach((item) => {
        runTestReducer(item);
    })
})