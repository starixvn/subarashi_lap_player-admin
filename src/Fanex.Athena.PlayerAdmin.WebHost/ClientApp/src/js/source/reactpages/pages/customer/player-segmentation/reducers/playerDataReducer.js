import * as types from '../actions/types';
import createReducer from '@jsroot/lib/createReducer.js';

export const playerList = createReducer({}, {
    [types.SET_PLAYER_LIST](state, action) {
        return action.data
    }
});