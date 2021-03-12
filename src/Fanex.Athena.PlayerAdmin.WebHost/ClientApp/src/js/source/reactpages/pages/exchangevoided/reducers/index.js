import { combineReducers } from 'redux';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';
import * as requestParamReducer from '@jsroot/reducers/requestParamReducer';
import * as gridDataReducer from '@jsroot/reducers/gridDataReducer';

import createReducer from '@jsroot/lib/createReducer.js';

export const dateConstraint = createReducer({}, {});

export default combineReducers(Object.assign(
    loaderReducer,
    requestParamReducer,
    gridDataReducer, { dateConstraint }
));