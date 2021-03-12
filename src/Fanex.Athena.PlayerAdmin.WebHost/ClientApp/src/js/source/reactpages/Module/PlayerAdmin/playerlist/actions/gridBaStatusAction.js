import PlayerCommandService from '../lib/services/playerCommandService';

export const updateBaStatus = (param) => () => {
    const commandService = new PlayerCommandService();

    return commandService.updateBaStatus(param);
}