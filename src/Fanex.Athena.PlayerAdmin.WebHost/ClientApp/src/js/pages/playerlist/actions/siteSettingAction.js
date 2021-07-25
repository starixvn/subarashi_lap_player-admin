import { SET_SITE_SETTING } from './types';
import { setDisplayedColumnList } from './gridHandlerAction';
import { buildColumnList } from '../lib/helpers/gridColumnBuilder';
import SiteSettingService from '../lib/services/siteSettingService';

export const getSiteSetting = (licType, isSubsidiary, hasCommPermissionRead, isEnableSingleWallet) => (dispatch) => {
    const service = new SiteSettingService();

    service.getSetting().then((response) => {
        const { IsAllowedCustomerComm, IsAllowedBA } = response.data;

        const isAllowedBA = !isSubsidiary && IsAllowedBA;
        if (isAllowedBA || isEnableSingleWallet) {
            const columnList = buildColumnList(licType, isAllowedBA, hasCommPermissionRead, isEnableSingleWallet);
            dispatch(setDisplayedColumnList(columnList));
        }

        dispatch(setSiteSetting({
            isAllowedCustomerComm: IsAllowedCustomerComm,
            isAllowedBA
        }));
    }).catch(() => {
        dispatch(setSiteSetting({
            isAllowedCustomerComm: false,
            isAllowedBA: false
        }));
    });
}

const setSiteSetting = (data) => {
    return {
        type: SET_SITE_SETTING,
        data
    }
}
