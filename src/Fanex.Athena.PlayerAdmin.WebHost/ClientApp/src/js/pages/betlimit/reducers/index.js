import { combineReducers } from 'redux';
import * as requestParamReducer from '@jsroot/reducers/requestParamReducer';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';
import * as alertReducer from './alertReducer';
import * as productReducer from './productReducer';

export default combineReducers(Object.assign({},
    loaderReducer,
    requestParamReducer,
    alertReducer,
    productReducer
));