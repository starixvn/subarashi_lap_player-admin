import createReducer from '@jsroot/lib/createReducer.js';
import { combineReducers } from 'redux';
import { SET_GRID_DATA } from '../actions/types';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';

const gridData = createReducer([], {
    [SET_GRID_DATA](preState, action) {
        return [...action.data];
    }
})

const requestParam = createReducer({}, {});
const hasCustomerComm = createReducer(false, {});
const isAllowedBA = createReducer(false, {});

export default combineReducers(Object.assign({},
    loaderReducer,
    {
        gridData,
        requestParam,
        hasCustomerComm,
        isAllowedBA
    }
));