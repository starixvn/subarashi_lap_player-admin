import * as filterHandlerReducer from '../filterHandlerReducer';
import { SET_PARAM, SET_PLAYER_LIST } from '../../actions/types.js'

describe('filterHandlerReducer', () => {
    const runTestReducer = ({ name, value, action, expectedResult }) => {
        it(`should handle ${name} reducer with type ${action.type} - return correct next state`, () => {
            const result = filterHandlerReducer[name](value, action);
            expect(result).toEqual(expectedResult);
        });
    }

    const testCaseList = [{
            name: 'playerList',
            value: undefined,
            action: {
                type: SET_PLAYER_LIST,
                data: [{ custId: 1, userName: 'test1' }]
            },
            expectedResult: [{ custId: 1, userName: 'test1' }]
        },
        {
            name: 'param',
            value: undefined,
            action: {
                type: SET_PARAM,
                data: { userName: 'abc' }
            },
            expectedResult: { userName: 'abc' }
        }
    ]

    testCaseList.forEach((item) => {
        runTestReducer(item);
    })
})