import * as types from '../actions/types'
import createReducer from '@jsroot/lib/createReducer.js'

export const alertOption = createReducer({}, {
    [types.SET_ALERT_OPTION](state, action) {
        return Object.assign({}, state, action.data);
    }
})