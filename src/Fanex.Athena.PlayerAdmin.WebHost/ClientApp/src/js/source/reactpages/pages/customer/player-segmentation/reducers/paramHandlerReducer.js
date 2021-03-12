
import createReducer from '@jsroot/lib/createReducer.js'
import { SET_SELECTED_PARAMS } from '../actions/types.js'

export const selectedParams = createReducer(null, {
    [SET_SELECTED_PARAMS](prevState, action) {
        return action.data && {...action.data };
    }
})
