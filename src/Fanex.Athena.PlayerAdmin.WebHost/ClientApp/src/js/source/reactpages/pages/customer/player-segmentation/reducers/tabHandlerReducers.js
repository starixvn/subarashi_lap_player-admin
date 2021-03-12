import createReducer from '@jsroot/lib/createReducer.js'
import { SET_SELECTED_TAB } from '../actions/types.js'

export const selectedTabKey = createReducer(null, {
    [SET_SELECTED_TAB](prevState, action) {
        return action.data;
    }
})