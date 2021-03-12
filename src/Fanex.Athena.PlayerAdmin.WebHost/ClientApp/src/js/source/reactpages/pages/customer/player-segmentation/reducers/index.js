import { combineReducers } from 'redux';
import * as tabHandlerReducer from './tabHandlerReducers';

export default combineReducers({
    ...tabHandlerReducer,
})
