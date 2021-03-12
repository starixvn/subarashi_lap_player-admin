import { setSelectedPlayer } from '../commonAction';
import { SET_SELECTED_PLAYER } from '../types';

describe('commonAction', () => {
    it('setSelectedPlayer - should be return correct data', () => {
        const data = {
            custId: 1,
            userName: 'test'
        }
        const expectedResult = {
            type: SET_SELECTED_PLAYER,
            data
        }

        const result = setSelectedPlayer(data);

        expect(result).toEqual(expectedResult);
    })
})