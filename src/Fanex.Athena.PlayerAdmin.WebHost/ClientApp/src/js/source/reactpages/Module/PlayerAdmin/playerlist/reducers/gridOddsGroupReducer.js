import createReducer from '@jsroot/lib/createReducer.js'
import { SET_ODDSGROUP_POSITION } from '../actions/types.js'

export const oddsGroupDropdownPosition = createReducer(null, {
    [SET_ODDSGROUP_POSITION](preState, action) {
        return action.data && {...action.data };
    }
})