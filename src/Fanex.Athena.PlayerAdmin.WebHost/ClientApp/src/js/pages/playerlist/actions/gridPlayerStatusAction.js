import { SET_STATUS_POSITION } from './types';
import PlayerCommandService from '../lib/services/playerCommandService';

export const setStatusDropdownPosition = (data) => {
    return {
        type: SET_STATUS_POSITION,
        data
    }
}

export const updatePlayerStatus = (param) => () => {
    const commandService = new PlayerCommandService();

    return commandService.updatePlayerStatus(param);
}