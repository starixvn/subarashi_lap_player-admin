import { combineReducers } from 'redux';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';
import * as userGuideReducer from '@jsroot/reducers/userGuideReducer';
import * as filterReducer from './filterReducer';
import * as playerDataReducer from '../../../reducers/playerDataReducer';
import * as gridHandlerReducer from '../../../reducers/gridHandlerReducer';
import * as tabHandlerReducers from '../../../reducers/tabHandlerReducers';
import * as playerReducer from './playerReducer';

export default combineReducers(Object.assign(
    loaderReducer,
    userGuideReducer,
    filterReducer,
    playerDataReducer, 
    gridHandlerReducer,
    tabHandlerReducers,
    playerReducer
));