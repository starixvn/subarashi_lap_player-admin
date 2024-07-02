import PlayerAccountService from "../lib/playerAccountService";
import { SET_SWEET_ALERT_OPTION } from "./types"

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

export const getAccountKYC = (param) => () => {
    const service = new PlayerAccountService();
    return service.getAccountKYC(param).then(result => {
        if (result) {
            const url = window.URL.createObjectURL(new Blob([result]));
            return url;
        }
        return null
    });
}

export const deleteKYCImage = (param) => () => {
    const service = new PlayerAccountService();
    return service.deleteKYCImage(param);
}

export const setSweetAlertOption = (data) => {
    return {
        type: SET_SWEET_ALERT_OPTION,
        data
    }
}