import createReducer from '@jsroot/lib/createReducer.js';
import { SET_IS_FETCHING_DATA, SET_REQUEST_PARAM, SET_DETAIL_TRANSACTION_REPORT } from '../actions/action-types.js';

export const requestParam = createReducer({}, {
    [SET_REQUEST_PARAM](preState, action) {
        return { ...action.data };
    }
});

export const detailTransactionReport = createReducer(null, {
    [SET_DETAIL_TRANSACTION_REPORT](preState, action) {
        return { ...action.data };
    }
});

export const isFetchingData = createReducer(false, {
    [SET_IS_FETCHING_DATA](preState, action) {
        return action.data;
    }
});

export const type = createReducer(null, {});
