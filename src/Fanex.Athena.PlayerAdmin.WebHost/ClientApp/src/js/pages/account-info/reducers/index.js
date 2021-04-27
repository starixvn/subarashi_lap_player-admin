import { combineReducers } from 'redux';
import * as commonReducer from './commonReducer';

export default combineReducers({
    ...commonReducer
});