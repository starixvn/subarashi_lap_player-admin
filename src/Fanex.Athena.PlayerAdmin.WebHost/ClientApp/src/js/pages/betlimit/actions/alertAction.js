import * as types from './types'

export const setAlertOption = (data) => {
    return {
        type: types.SET_ALERT_OPTION,
        data: data
    }
}