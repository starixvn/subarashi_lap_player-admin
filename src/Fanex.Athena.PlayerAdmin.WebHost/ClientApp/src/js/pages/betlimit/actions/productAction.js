import * as types from './types'

export const setProductLabel = (data) => {
    return {
        type: types.SET_PRODUCT_LABEL,
        data: data
    }
}