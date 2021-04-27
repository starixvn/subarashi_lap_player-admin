import { combineReducers } from 'redux';
import * as requestParamReducer from '@jsroot/reducers/requestParamReducer';

export default combineReducers(Object.assign({},
    requestParamReducer
));