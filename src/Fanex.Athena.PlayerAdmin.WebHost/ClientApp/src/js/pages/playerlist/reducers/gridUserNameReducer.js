import createReducer from '@jsroot/lib/createReducer.js'
import { SET_CONTEXT_MENU } from '../actions/types.js'

export const contextMenu = createReducer({}, {
    [SET_CONTEXT_MENU](preState, action) {
        return {...action.data };
    }
})