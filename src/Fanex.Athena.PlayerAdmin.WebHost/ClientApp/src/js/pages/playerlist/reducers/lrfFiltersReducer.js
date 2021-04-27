import createReducer from '@jsroot/lib/createReducer.js';
import { SET_LRF_FILTERS } from '../actions/types.js';

const defaultLRFFilters = {
    CustomerClass: [],
    CommGroups: []
};

export const lrfFilters = createReducer(defaultLRFFilters, {
    [SET_LRF_FILTERS](preState, action) {
        return { ...action.data };
    }
});
