import createReducer from '@jsroot/lib/createReducer.js';
import { SET_LRF_FILTERS } from '../actions/types.js';

export const lrfFilters = createReducer({}, {
    [SET_LRF_FILTERS](preState, action) {
        return { ...action.data };
    }
});
