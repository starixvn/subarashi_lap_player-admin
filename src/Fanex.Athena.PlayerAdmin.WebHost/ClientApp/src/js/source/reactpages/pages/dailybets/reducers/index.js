import { combineReducers } from 'redux';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';
import * as requestParamReducer from '@jsroot/reducers/requestParamReducer';
import * as reportTitleReducer from '@jsroot/reducers/reportTitleReducer';
import * as pagingOptionReducer from '@jsroot/reducers/pagingOptionReducer';
import * as gridDataReducer from '@jsroot/reducers/gridDataReducer';

import createReducer from '@jsroot/lib/createReducer.js';

export const dateConstraint = createReducer({}, {});

export default combineReducers(Object.assign(
    loaderReducer,
    requestParamReducer,
    reportTitleReducer,
    pagingOptionReducer,
    gridDataReducer, { dateConstraint }
));