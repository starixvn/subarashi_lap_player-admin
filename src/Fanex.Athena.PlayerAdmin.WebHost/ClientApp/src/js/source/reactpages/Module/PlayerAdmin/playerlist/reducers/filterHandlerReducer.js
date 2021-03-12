import createReducer from '../../../../lib/createReducer.js';
import { SET_PARAM, SET_PLAYER_LIST, SET_ADVANCED_SEARCH, SET_COLLAPSE_FILTER } from '../actions/types.js';

export const param = createReducer({}, {
    [SET_PARAM](preState, action) {
        return { ...action.data };
    }
})

export const playerList = createReducer(null, {
    [SET_PLAYER_LIST](preState, action) {
        return action.data && [...action.data];
    }
})

export const type = createReducer(null, {});

export const currencyList = createReducer([], {});

export const hasBaPermission = createReducer(false, {});
export const hasOddsGroupPermission = createReducer(false, {});
export const hasSpecialLevelPermission = createReducer(false, {});
export const hasCommPermissionEdit = createReducer(false, {});
export const hasCommPermissionRead = createReducer(false, {});

export const isSubsidiary = createReducer(false, {});

export const isAdvancedSearch = createReducer(false, {
    [SET_ADVANCED_SEARCH](preState, action) {
        return action.data;
    }
})

export const isFilterCollapsing = createReducer(false, {
    [SET_COLLAPSE_FILTER](preState, action) {
        return action.data;
    }
})
