import createReducer from '@jsroot/lib/createReducer.js';
import { SET_SITE_SETTING, SET_DEPOSIT_SETTING, SET_SUBSIDIARY_SETTING } from '../actions/types.js';
import { SubsidiaryViewType } from '@jsroot/constants/report'

const defaultSiteSetting = {
    isAllowedBA: false,
    isAllowedCustomerComm: false,
    userLevel: 2,
    isEnableSingleWallet: false
}

export const siteSetting = createReducer(defaultSiteSetting, {
    [SET_SITE_SETTING](preState, action) {
        return {...action.data };
    }
})

export const depositSetting = createReducer({ CustomerClass: [], CommGroups: [] }, {
    [SET_DEPOSIT_SETTING](preState, action) {
        return { ...action.data };
    }
});

export const subsidiarySetting =  createReducer({ SubsidiaryViewType: 0, SubsidiaryList: [] }, {
    [SET_SUBSIDIARY_SETTING](preState, action) {
        const response = action.data;
        let defaultSelectedSubsidiaryList = '';
        let isAllowedSubsidiarySearch = true;
        if(response.SubsidiaryViewType === SubsidiaryViewType.NOT_MANAGEMENT) { //do not support subsidiary search
            defaultSelectedSubsidiaryList = '';
            isAllowedSubsidiarySearch = false;
        } else if(response.SubsidiaryViewType === SubsidiaryViewType.NORMAL_MANAGEMENT) { // subsidiary list
            defaultSelectedSubsidiaryList = response.SubsidiaryList.map(_ => _.Value).join(',');
        } else { //whitelabel list
            defaultSelectedSubsidiaryList = response.SubsidiaryList.reduce((result, whitelabel) => {
                return [ ...result, ...whitelabel.Children.map(_ => _.Value)];
            }, []).join(',');
        }
        return { ...action.data, isAllowedSubsidiarySearch, defaultSelectedSubsidiaryList };
    }
});
