import { SET_SELECTED_PARAMS  } from './types';
export const setSelectedParams = options => {
    return {
        type: SET_SELECTED_PARAMS,
        data: options
    }
}