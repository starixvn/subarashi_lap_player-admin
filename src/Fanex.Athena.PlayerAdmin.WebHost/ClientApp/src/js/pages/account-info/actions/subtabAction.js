import PlayerAccountService from "../lib/playerAccountService";

export const getPlayerInfoSetting = () => () => {
    const service = new PlayerAccountService();
    return service.getPlayerInfoSetting();
}

export const getAccountProfile = (param) => () => {
    const service = new PlayerAccountService();
    return service.getAccountProfile(param);
}

export const getSubsidiaryViewType = () => () => {
    const service = new PlayerAccountService();
    return service.getSubsidiaryViewType().then(result => result.SubsidiaryViewType);
}

export const getBankInfo = (param) => () => {
    const service = new PlayerAccountService();
    return service.getBankInfo(param);
}

export const getActivityRecord = (param) => () => {
    const service = new PlayerAccountService();
    return service.getActivityRecord(param);
}