import { SET_CONTEXT_MENU } from './types';

export const setContextMenu = (data) => {
    return {
        type: SET_CONTEXT_MENU,
        data
    }
}