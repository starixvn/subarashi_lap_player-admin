import { fields, columnList, licenseeType } from '../constants';

const excludedKeyForApiType = [fields.RealName, fields.Email, fields.LastTransaction];
const excludedKeyForAthenaType = [fields.VendorMemberId];

export const buildDefaultColumnList = (licType, hasCommPermissionRead) => {
    const isApiType = licType === licenseeType.api;

    return columnList.filter(item => {
        const show = item.key === fields.CommissionGroup ? hasCommPermissionRead : item.key !== fields.BaStatus;
        const showByLicenseeType = isApiType ? !excludedKeyForApiType.includes(item.key) : !excludedKeyForAthenaType.includes(item.key);
        return show && showByLicenseeType;
    }).map(item => ({ ...item }))
}

export const buildColumnList = (licType, isAllowedBA, hasCommPermissionRead, isEnableSingleWallet) => {
    const isApiType = licType === licenseeType.api;

    return columnList.filter(item => {
        if (item.key === fields.BaStatus) {
            return isAllowedBA;
        } else if (item.key === fields.CommissionGroup) {
            return hasCommPermissionRead;
        } else if (item.key === fields.CurrentBalance) {
            return !isEnableSingleWallet;
        } else {
            return isApiType ? !excludedKeyForApiType.includes(item.key) : !excludedKeyForAthenaType.includes(item.key);
        }
    }).map(item => ({ ...item }))
};
