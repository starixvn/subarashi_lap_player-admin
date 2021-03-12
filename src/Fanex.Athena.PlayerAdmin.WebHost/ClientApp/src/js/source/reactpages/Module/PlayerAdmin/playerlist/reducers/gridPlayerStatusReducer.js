import createReducer from '@jsroot/lib/createReducer.js'
import { SET_STATUS_POSITION } from '../actions/types.js'

export const statusDropdownPosition = createReducer(null, {
    [SET_STATUS_POSITION](preState, action) {
        return action.data && {...action.data };
    }
})