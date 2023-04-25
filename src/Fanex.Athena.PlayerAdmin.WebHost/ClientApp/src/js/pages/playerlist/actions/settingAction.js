import { SET_SITE_SETTING, SET_DEPOSIT_SETTING, SET_SUBSIDIARY_SETTING } from './types';
import SettingService from '../lib/services/settingService';
import { setDisplayedColumnList } from './gridHandlerAction';
import { buildColumnList } from '../lib/helpers/gridColumnBuilder';

export const getSiteSetting = (licType, hasCommPermissionRead) => (dispatch) => {
    const service = new SettingService();
    service.getSiteSetting().then((response) => {
        const { isAllowedBA, isEnableSingleWallet, userLevel } = response.data;
        if (isAllowedBA || isEnableSingleWallet) {
            const columnList = buildColumnList(licType, isAllowedBA, hasCommPermissionRead, isEnableSingleWallet, userLevel);
            dispatch(setDisplayedColumnList(columnList));
        }

        dispatch(setSiteSetting({ ...response.data }));
    }).catch(() => {
        dispatch(setSiteSetting({
            isAllowedCustomerComm: false,
            isAllowedBA: false,
            isEnableSingleWallet: false,
            userLevel: 2,
        }));
    });
}

export const getDepositSetting = () => (dispatch) => {
    const service = new SettingService();

    service.getDepositSetting().then((response) => {
        dispatch(setDepositSetting(response.data));
    }).catch(() => {
        dispatch(setDepositSetting({
            RiskLevels: [],
            CommGroups: [],
            OddsGroups: []
        }));
    });
};

export const getSubsidiarySetting = () => (dispatch) => {
    const service = new SettingService();

    service.getSubsidiarySetting().then((response) => {
        dispatch(setSubsidiarySetting(response.data));
    }).catch(() => {
        dispatch(setSubsidiarySetting({
            SubsidiaryViewType: 0,
            SubsidiaryList: []
        }));
    });
};

const setSiteSetting = (data) => {
    return {
        type: SET_SITE_SETTING,
        data
    }
}

const setDepositSetting = (data) => {
    return {
        type: SET_DEPOSIT_SETTING,
        data
    }
};

const setSubsidiarySetting = (data) => {
    return {
        type: SET_SUBSIDIARY_SETTING,
        data
    }
};
