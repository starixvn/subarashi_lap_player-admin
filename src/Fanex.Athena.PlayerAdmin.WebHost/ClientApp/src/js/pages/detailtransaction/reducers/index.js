import { combineReducers } from 'redux';
import * as requestParamReducer from '@jsroot/reducers/requestParamReducer';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';
import * as filterReducers from './filter-reducer.js';

export default combineReducers(Object.assign({},
    requestParamReducer,
    loaderReducer,
    filterReducers
));
