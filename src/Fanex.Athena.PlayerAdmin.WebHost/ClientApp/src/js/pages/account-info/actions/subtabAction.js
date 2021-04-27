import PlayerAccountService from "../lib/playerAccountService";

export const getAccountProfile = (param) => () => {
    const service = new PlayerAccountService();

    return service.getAccountProfile(param);
}

export const getBankInfo = (param) => () => {
    const service = new PlayerAccountService();

    return service.getBankInfo(param);
}

export const getActivityRecord = (param) => () => {
    const service = new PlayerAccountService();

    return service.getActivityRecord(param);
}