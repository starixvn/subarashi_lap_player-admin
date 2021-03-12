
import createReducer from '@jsroot/lib/createReducer'
import { SET_SUBSIDIARY_LIST, SET_SUBSIDIARY_SEARCH } from '../actions/types'

export const selectedSubsidiary = createReducer(null, {
    [SET_SUBSIDIARY_LIST](prevState, action) {
        return action.data;
    }
})

export const subsidiarySearch = createReducer(null, {
    [SET_SUBSIDIARY_SEARCH](prevState, action) {
        return action.data;
    }
})