import { fields, columnList, licenseeType } from '../constants';

const excludedKeyForApiType = [fields.RealName, fields.Email, fields.LastTransaction];
const excludedKeyForAthenaType = [fields.VendorMemberId];

export const buildDefaultColumnList = (licType, hasCommPermissionRead) => {
    const isApiType = licType === licenseeType.api;
    return columnList.filter(item => {
        const show = item.key === fields.CommissionGroup ? hasCommPermissionRead : item.key !== fields.BaStatus;
        return show && _showByLicenseeType(isApiType, item.key);
    }).map(item => ({ ...item }))
}

export const buildColumnList = (licType, isAllowedBA, hasCommPermissionRead, isEnableSingleWallet) => {
    const isApiType = licType === licenseeType.api;
    return columnList.filter(item => {
        if (item.key === fields.BaStatus) {
            return isAllowedBA;
        } else if (item.key === fields.CommissionGroup) {
            return  hasCommPermissionRead;
        } else if (item.key === fields.CurrentBalance) {
            return !isEnableSingleWallet;
        } else {
            return _showByLicenseeType(isApiType, item.key);
        }
    }).map(item => ({ ...item }))
};

const _showByLicenseeType = (isApiType, colKey) => !((isApiType ? excludedKeyForApiType : excludedKeyForAthenaType).includes(colKey));
