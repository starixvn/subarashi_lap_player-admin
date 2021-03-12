import { combineReducers } from 'redux';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';
import * as gridDataReducer from '@jsroot/reducers/gridDataReducer.js'
import * as filterReducer from './filterReducer';
import { selectedParams } from '../../reducers/paramHandlerReducer';

export default combineReducers(Object.assign(
    gridDataReducer,
    loaderReducer,
    filterReducer,
    { selectedParams }
));