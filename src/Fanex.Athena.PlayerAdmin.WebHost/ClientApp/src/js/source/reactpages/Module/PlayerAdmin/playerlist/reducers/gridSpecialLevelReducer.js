import createReducer from '@jsroot/lib/createReducer.js'
import { SET_SPECIAL_LEVEL_POSITION } from '../actions/types.js'

export const specialLevelDropdownPosition = createReducer(null, {
    [SET_SPECIAL_LEVEL_POSITION](preState, action) {
        return action.data && {...action.data };
    }
})