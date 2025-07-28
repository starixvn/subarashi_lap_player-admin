import createReducer from '@jsroot/lib/createReducer.js';
import { SET_SWEET_ALERT_OPTION, } from '../actions/types';


export const sweetAlertOption = createReducer(null, {
    [SET_SWEET_ALERT_OPTION](preState, action) {
        return action.data && { ...action.data };
    }
})