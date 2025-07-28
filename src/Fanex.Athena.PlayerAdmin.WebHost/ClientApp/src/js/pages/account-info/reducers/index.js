import { combineReducers } from 'redux';
import * as commonReducer from './commonReducer';
import * as subtabReducer from './subtabReducer';

export default combineReducers({
    ...commonReducer,
    ...subtabReducer
});