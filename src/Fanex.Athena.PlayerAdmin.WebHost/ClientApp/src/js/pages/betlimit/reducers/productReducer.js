import * as types from '../actions/types'
import createReducer from '@jsroot/lib/createReducer.js'

export const productLabel = createReducer({}, {
    [types.SET_PRODUCT_LABEL](prevState, action) {
        return action.data;
    }
})