import { combineReducers } from 'redux';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';
import * as requestParamReducer from '@jsroot/reducers/requestParamReducer';

export default combineReducers(Object.assign({},
    loaderReducer,
    requestParamReducer
));