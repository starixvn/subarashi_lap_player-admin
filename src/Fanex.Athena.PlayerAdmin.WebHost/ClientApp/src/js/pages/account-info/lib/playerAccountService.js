import FactoryCommonService from '../../../lib/factoryCommonService';
import { lapAccountProfile, lapBankInfo, lrfActivityRecord, lrfSubsidiaryViewType, custBettingStatus, playerInfoSettings, lapAccountKYC } from './apiInfo';

export default class PlayerAccountService {
    constructor() {
        this.commonService = FactoryCommonService.createInstance();
    }

    getPlayerInfoSetting() {
        return this._getData(playerInfoSettings.type, playerInfoSettings.action, {});
    }

    getAccountProfile(param) {
        return this._getData(lapAccountProfile.type, lapAccountProfile.action, param);
    }

    getSubsidiaryViewType() {
        return this._getData(lrfSubsidiaryViewType.type, lrfSubsidiaryViewType.action, {});
    }

    getBankInfo(param) {
        return this._getData(lapBankInfo.type, lapBankInfo.action, param);
    }

    getActivityRecord(param) {
        return this._getData(lrfActivityRecord.type, lrfActivityRecord.action, param);
    }

    getAccountKYC(param) {
        const ajaxConfig = {
            method: 'get',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'content-type': 'application/json;charset=UTF-8'
            },
            transformResponse: [],
            withCredentials: false,
            responseType: 'arraybuffer',
        }

        return this.commonService.fetchData(lapAccountKYC.type, lapAccountKYC.action, param, ajaxConfig)
            .then(response => {
                const { data, headers } = response;
                if (!headers.get("content-type").includes("application/json")) {
                    return data;
                }
                return null;
            })
            .catch(error => {
                console.log(error.message);
                return null;
            });
    }

    _getData(licType, action, param) {
        return this.commonService.fetchData(licType, action, param)
            .then(response => {
                const { data } = response;
                return data;
            })
            .catch(error => {
                console.log(error.message);
                return null;
            });
    }

    getCustomerBettingStatus(param) {
        const params = { ...param };

        return this.commonService.fetchData(custBettingStatus.type, custBettingStatus.action, params);
    }
}