import createReducer from '../../../../lib/createReducer.js';
import { SET_SITE_SETTING } from '../actions/types.js';

const defaultSiteSetting = {
    isAllowedBA: false,
    isAllowedCustomerComm: false
}
export const siteSetting = createReducer(defaultSiteSetting, {
    [SET_SITE_SETTING](preState, action) {
        return {...action.data };
    }
})