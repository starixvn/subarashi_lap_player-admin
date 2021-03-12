import PlayerQueryService from '../lib/services/playerQueryService';
import { SET_SELECTED_PLAYERID_LIST, SET_SELECTED_TAB_NAME, SET_SELECTED_SUBTAB_NAME } from './types';

export const getLink = (systemType, action) => () => {
    const service = new PlayerQueryService();
    return service.getLink(systemType, action);
}

export const getTabList = () => () => {
    const service = new PlayerQueryService();
    return service.getTabList();
}

export const setSelectedPlayerIdList = (data) => {
    return {
        type: SET_SELECTED_PLAYERID_LIST,
        data
    }
}

export const setSelectedTabName = (data) => {
    return {
        type: SET_SELECTED_TAB_NAME,
        data
    }
}

export const setSelectedSubTabName = (data) => {
    return {
        type: SET_SELECTED_SUBTAB_NAME,
        data
    }
};
