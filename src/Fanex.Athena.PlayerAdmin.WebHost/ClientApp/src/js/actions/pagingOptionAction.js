import { SET_PAGING_OPTION } from './types'

export const setPagingOption = (data) => {
    return {
        type: SET_PAGING_OPTION,
        data
    }
}