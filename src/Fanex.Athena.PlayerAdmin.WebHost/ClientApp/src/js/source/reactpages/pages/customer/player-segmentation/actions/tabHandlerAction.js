import { SET_SELECTED_TAB } from './types';
export const setSelectedTab = selectedTabKey => {
    return {
        type: SET_SELECTED_TAB,
        data: selectedTabKey
    }
}