import BaseCommonService from './baseCommonService';
import { systemType } from '../constants/config';
import axios from 'axios';
import asyncLock from 'async-lock';

const locker = new asyncLock();

const defaultAjaxConfig = {
    method: 'post',
    headers: {
        'X-Requested-With': 'XMLHttpRequest',
        'content-type': 'application/json;charset=UTF-8'
    },
    withCredentials: false,
    responseType: 'json',
    cancelToken: new axios.CancelToken(function(cancel) {})
}

class LapCommonService extends BaseCommonService {
    constructor(initialConfig) {
        super();

        const { baseUrl, applyLinkTokenUrl, applyApiTokenUrl, tabList } = initialConfig;
        this._applyLinkTokenUrl = applyLinkTokenUrl;
        this._applyApiTokenUrl = applyApiTokenUrl;
        this._baseUrl = baseUrl;
        this._tabList = tabList || [];
    }

    getTabList() {
        return [...this._tabList];
    }

    _buildApplyTokenQueryString(sysType, action) {
        return `ActionLink=${encodeURIComponent(action)}&Type=${sysType}`;
    }

    getLink(sysType, action) {
        return sysType === systemType.newAdmin ? this._baseUrl + action :
            `${this._applyLinkTokenUrl}?${this._buildApplyTokenQueryString(sysType, action)}`;
    }

    _buildCommonAjaxConfig(configOption, apiActionUrl, params) {
        const commonConfig = {
            ...configOption,
            url: apiActionUrl
        };

        const configWithParam = this._getConfigParam(commonConfig, params);

        return configWithParam;
    }

    _getConfigParam(config, params) {
        const isGetMethod = config.method.toLowerCase() === 'get';
        const paramConfig = isGetMethod ? { params } : { data: params };

        return {
            ...config,
            ...paramConfig
        }
    }

    _fetchAccessToken(sysType, action) {
        return locker.acquire(sysType, async () => {
            if(!window['playeradmin_' + sysType]) {
                const { ApiUrl, Token } = await this.fetchTokenFromLap(sysType, action);
                const response = await  axios.post(`${ApiUrl}/api/Auth/Token`, { token: Token })
                const { status, data } = response;
                if(status === 200 && data.IsSuccess) {
                    window['playeradmin_' + sysType] = {
                        accessToken: data.AccessToken,
                        apiUrl: ApiUrl
                    }
                }
            }
            return window['playeradmin_' + sysType];
        })
    }

    async _fetchDataWithToken(sysType, action, params, configOption, isRetry = false) {
        const { accessToken, apiUrl } = await this._fetchAccessToken(sysType, action);
        const apiActionUrl = `${apiUrl}/${action}`;
        const commonAjaxConfig = this._buildCommonAjaxConfig(configOption, apiActionUrl, params);
        const headerConfig = {
            ...configOption.headers,
            'Authorization': `Bearer ${accessToken}`
        }
        const configWithXToken = { ...commonAjaxConfig, headers: { ...headerConfig } };
        try {
            const response = await axios(configWithXToken)
            return response;
        }
        catch (error) {
            if (error.response.status === 401 && !isRetry) {
                window['playeradmin_' + sysType] = null;
                return this._fetchDataWithToken(sysType, action, params, configOption, true);
            }
        }
    }

    async fetchTokenFromLap(sysType, action)  {
        const apiToken = `${this._applyApiTokenUrl}?${this._buildApplyTokenQueryString(sysType, action)}`;
        const tokenFromLap = await axios.get(apiToken);
        return tokenFromLap.data;
    }

    fetchData(sysType, action, params, configOption = defaultAjaxConfig) {
        const config = {...defaultAjaxConfig, ...configOption };
        if (sysType === systemType.newAdmin) {
            const relativeUrl = this._baseUrl + action;
            const ajaxConfig = this._buildCommonAjaxConfig(config, relativeUrl, params);
            return axios(ajaxConfig);
        } else {
            return this._fetchDataWithToken(sysType, action, params, config);
        }
    }

    fetchMultiple(promiseList, callback) {
        if (promiseList && promiseList.length) {
            return axios.all(promiseList)
                .then(axios.spread(callback));
        }
    }
}

export default LapCommonService