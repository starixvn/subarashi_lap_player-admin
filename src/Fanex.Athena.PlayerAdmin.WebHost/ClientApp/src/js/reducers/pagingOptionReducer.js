import { SET_PAGING_OPTION } from '../actions/types'
import createReducer from '../lib/createReducer.js'

const defaultPagingOption = {
    PageIndex: 1,
    PageSize: 50,
    TotalItem: 0,
    PageList: [50, 100, 200, 500]
};

export const pagingOption = createReducer(defaultPagingOption, {
    [SET_PAGING_OPTION](prevState, action) {
        return { ...prevState, ...action.data }
    }
});