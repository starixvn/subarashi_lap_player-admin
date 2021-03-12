import createReducer from '../../../../lib/createReducer.js';
import { SET_SELECTED_PLAYER, SET_SELECTED_PLAYERID_LIST, SET_SELECTED_TAB_NAME, SET_SELECTED_SUBTAB_NAME } from '../actions/types.js';

export const selectedPlayer = createReducer(null, {
    [SET_SELECTED_PLAYER](preState, action) {
        return action.data && { ...action.data };
    }
})

export const selectedPlayerIdList = createReducer([], {
    [SET_SELECTED_PLAYERID_LIST](preState, action) {
        return [...action.data];
    }
})

export const selectedTabName = createReducer('', {
    [SET_SELECTED_TAB_NAME](preState, action) {
        return action.data;
    }
})

export const selectedSubTabName = createReducer('', {
    [SET_SELECTED_SUBTAB_NAME](preState, action) {
        return action.data;
    }
});
