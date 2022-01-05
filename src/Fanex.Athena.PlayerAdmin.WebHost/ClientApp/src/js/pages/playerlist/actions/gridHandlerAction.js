import {
    SET_DISPLAYED_COLUMN_LIST, SET_PLAYER_LIST, SET_CURRENT_ACTIVE_PLAYER, SET_PAGING,
    SET_IS_FETCHING_DATA, SET_SWEET_ALERT_OPTION
} from './types';

import PlayerQueryService from '../lib/services/playerQueryService';

export const setDisplayedColumnList = (data) => {
    return {
        type: SET_DISPLAYED_COLUMN_LIST,
        data
    }
}

export const setPlayerList = (data) => {
    return {
        type: SET_PLAYER_LIST,
        data
    }
}

export const setCurrentActivePlayer = (data) => {
    return {
        type: SET_CURRENT_ACTIVE_PLAYER,
        data
    }
}

export const setPagingOption = (data) => {
    return {
        type: SET_PAGING,
        data
    }
}

export const exportExcel = (playerList, displayedColumn) => () => {
    const service = new PlayerQueryService();
    service.exportExcel(playerList, displayedColumn);
}

export const setIsFetchingData = (data) => {
    return {
        type: SET_IS_FETCHING_DATA,
        data
    }
}

export const setSweetAlertOption = (data) => {
    return {
        type: SET_SWEET_ALERT_OPTION,
        data
    }
}
