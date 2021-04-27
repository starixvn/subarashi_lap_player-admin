import { SET_ODDSGROUP_POSITION } from './types';
import PlayerCommandService from '../lib/services/playerCommandService';

export const setOddsGroupDropdownPosition = (data) => {
    return {
        type: SET_ODDSGROUP_POSITION,
        data
    }
}

export const updateOddsGroup = (param) => () => {
    const commandService = new PlayerCommandService();

    return commandService.updateOddsGroup(param);
}