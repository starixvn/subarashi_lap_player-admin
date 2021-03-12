import { combineReducers } from 'redux';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';
import * as userGuideReducer from '@jsroot/reducers/userGuideReducer';
import * as filterReducer from './filterReducer';
import * as playerDataReducer from '../../../reducers/playerDataReducer';
import * as gridHandlerReducer from '../../../reducers/gridHandlerReducer';
import * as tabHandlerReducers from '../../../reducers/tabHandlerReducers';

export default combineReducers(Object.assign(
    loaderReducer,
    filterReducer,
    playerDataReducer, 
    gridHandlerReducer,
    tabHandlerReducers,
    userGuideReducer
));