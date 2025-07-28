import { playerStatus } from '../constants';
import LangHelper from '@jsroot/lib/langHelper';
import { decode } from '@jsroot/lib/htmlHelper';
import FactoryCommonService from '../../../../lib/factoryCommonService';
import axios from 'axios'

export const getSpecialLevelText = (value) => {
    return value === undefined || value === null ? '' : (Number(value) === 0 ? LangHelper.getSingleResource('None') : value);
}

export const getPlayerStatus = (value) => {
    return playerStatus[value] || '';
}

export const buildDisplayedColumnObject = (displayedColumnList) => {
    return displayedColumnList.reduce((accumulator, currentItem) => {
        accumulator[currentItem.key] = currentItem.show;
        return accumulator;
    }, {});
}

export const isNonDepositOrArchived = (lapItem) => lapItem.IBCCustId === 0 || lapItem.isArchived;

export const getNoneDepositOrArchivedList = (lapPlayerList) => {
    return !!lapPlayerList && !!lapPlayerList.length ? lapPlayerList.filter(player => isNonDepositOrArchived(player)) : [];
}

export const getCustIdListFromLapData = (lapPlayerList) => {
    return lapPlayerList.reduce((result, item) => {
        if (!isNonDepositOrArchived(item)) {
            result.push(item.IBCCustId);
        }

        return result;
    }, []);
}

export const getErrorMessageForUpdating = (errorInfo) => {
    const defaultKey = 'HttpError_Default';
    if (!errorInfo) {
        return LangHelper.getSingleResource(defaultKey);
    }

    const resourceKey = isUnauthorizedResponse(errorInfo) ? 'FunctionDeniedMsg' : defaultKey;

    return decode(LangHelper.getSingleResource(resourceKey));
}

const isUnauthorizedResponse = (errorInfo) => {
    if (!errorInfo.response) {
        return false;
    }

    const { status } = errorInfo.response;

    return status === 403 || status === 401;
}

export const getCustomerClassText = (value) => {
    return value === "0" || value == "" || value == undefined ? LangHelper.getSingleResource('None') : value;
};

export const getOddsGroupPdf = (fileName) => {
    const playerAdminSystemId = 5;
    const url = "api/UserGuide/OddsGroupPdf?fileName=" + fileName;
    const commonService = FactoryCommonService.createInstance();

    const ajaxConfig = {
        method: 'post',
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            'content-type': 'application/json;charset=UTF-8'
        },
        transformResponse: [],
        withCredentials: false,
        responseType: 'arraybuffer',
        transformResponse: [],
        cancelToken: new axios.CancelToken(function (cancel) { })
    }

    return commonService.fetchData(playerAdminSystemId, url, {}, ajaxConfig);
}