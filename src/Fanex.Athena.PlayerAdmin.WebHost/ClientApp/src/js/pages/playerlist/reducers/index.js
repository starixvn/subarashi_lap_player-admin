import { combineReducers } from 'redux';
import * as filterHandlerReducer from './filterHandlerReducer.js';
import * as playerDetailHandlerReducer from './playerDetailHandlerReducer';
import * as loaderReducer from '@jsroot/reducers/loaderReducer';
import * as gridHandlerReducer from './gridHandlerReducer';
import * as gridOddsGroupReducer from './gridOddsGroupReducer';
import * as gridSpecialLevelReducer from './gridSpecialLevelReducer';
import * as gridPlayerStatusReducer from './gridPlayerStatusReducer';
import * as gridUserNameReducer from './gridUserNameReducer';
import * as settingReducer from './settingReducer';

export default combineReducers(Object.assign({},
    filterHandlerReducer,
    loaderReducer,
    playerDetailHandlerReducer,
    gridHandlerReducer,
    gridOddsGroupReducer,
    gridSpecialLevelReducer,
    gridPlayerStatusReducer,
    gridUserNameReducer,
    settingReducer
));
