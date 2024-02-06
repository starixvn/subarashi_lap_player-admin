import createReducer from '@jsroot/lib/createReducer.js';
import { SET_DISPLAYED_COLUMN_LIST, SET_CURRENT_ACTIVE_PLAYER, SET_PAGING, SET_IS_FETCHING_DATA, SET_SWEET_ALERT_OPTION, SET_FETCH_DATA_TIME } from '../actions/types';
import { defaultPagingOption } from '../lib/constants.js';

export const displayedColumnList = createReducer([], {
    [SET_DISPLAYED_COLUMN_LIST](preState, action) {
        return [...action.data];
    }
})

export const currentActivePlayer = createReducer(null, {
    [SET_CURRENT_ACTIVE_PLAYER](preState, action) {
        return action.data && {...action.data };
    }
})

export const pagingOption = createReducer(defaultPagingOption, {
    [SET_PAGING](preState, action) {
        return {...action.data };
    }
})

export const isFetchingData = createReducer(false, {
    [SET_IS_FETCHING_DATA](preState, action) {
        return action.data;
    }
})

export const sweetAlertOption = createReducer(null, {
    [SET_SWEET_ALERT_OPTION](preState, action) {
        return action.data && {...action.data };
    }
})

export const fetchDataTime = createReducer(null, {
    [SET_FETCH_DATA_TIME](preState, action) {
        return action.data;
    }
})