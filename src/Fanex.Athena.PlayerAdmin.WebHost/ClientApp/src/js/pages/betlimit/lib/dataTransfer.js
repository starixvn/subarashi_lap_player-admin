import LangHelper from '@jsroot/lib/langHelper';
import DataService from '@jsroot/lib/DataService';
import Site from '@jsroot/lib/site';
import { BetLimitProduct } from './constants';

const Url = {
    [BetLimitProduct.SB]: 'SportsBookBetLimit',
    [BetLimitProduct.VS]: 'DcsBetLimit',
    [BetLimitProduct.VS2]: 'DcsBetLimit',
    [BetLimitProduct.LT]: 'DcsBetLimit',
    [BetLimitProduct.NG]: 'DcsBetLimit',
    [BetLimitProduct.KN]: 'DcsBetLimit',
    [BetLimitProduct.AGC]: 'AGCasinoBetLimit',
    [BetLimitProduct.TG]: 'DcsBetLimit',
    [BetLimitProduct.I4D]: 'DcsBetLimit',
    [BetLimitProduct.SLT]: 'DcsBetLimit',
    [BetLimitProduct.CG]: 'DcsBetLimit',
    [BetLimitProduct.AB]: 'DcsBetLimit',
    [BetLimitProduct.VB]: 'DcsBetLimit',
    [BetLimitProduct.CO]: 'ThirdPartyBetLimit',
    [BetLimitProduct.BX]: 'ExchangeDcsBetLimit',
    [BetLimitProduct.PP]: 'DcsBetLimit',
    [BetLimitProduct.SXG]: 'DcsBetLimit',
    [BetLimitProduct.HB]: 'DcsBetLimit',
    [BetLimitProduct.JK]: 'DcsBetLimit',
    [BetLimitProduct.SAG]: 'DcsBetLimit',
    [BetLimitProduct.PGS]: 'DcsBetLimit',
    [BetLimitProduct.SBC]: 'DcsBetLimit',
    [BetLimitProduct.WM]: 'DcsBetLimit',
    [BetLimitProduct.SG]: 'DcsBetLimit',
    [BetLimitProduct.VRG]: 'DcsBetLimit',
    [BetLimitProduct.CC]: 'DcsBetLimit',
    [BetLimitProduct.CS]: 'DcsBetLimit',
    [BetLimitProduct.VGA]: 'DcsBetLimit',
    [BetLimitProduct.AP]: 'DcsBetLimit',
    [BetLimitProduct.APM]: 'DcsBetLimit',
    [BetLimitProduct.LG]: 'DcsBetLimit',
    [BetLimitProduct.BTC]: 'DcsBetLimit',
    [BetLimitProduct.FGGL]: 'DcsBetLimit',
    [BetLimitProduct.MXG]: 'DcsBetLimit',
    [BetLimitProduct.JIG]: 'DcsBetLimit',
    [BetLimitProduct.YB]: 'DcsBetLimit',
    [BetLimitProduct.MGRNG]: 'DcsBetLimit',
    [BetLimitProduct.SEA]: 'DcsBetLimit',
    [BetLimitProduct.WE]: 'DcsBetLimit',
    [BetLimitProduct.L22]: 'DcsBetLimit',
    [BetLimitProduct.UUSL]: 'DcsBetLimit',
    [BetLimitProduct.FS]: 'DcsBetLimit'
};

export default class DataTransfer {
    static getControllerName = ProductId =>  !!Url[ProductId] ? Url[ProductId] : 'DcsBetLimit';

    static getData(param, callBack) {
        const url = Site.resolveClientUrl(this.getControllerName(param.ProductId) + '/GetSetting');
        DataService.postData(url, param)
            .then(response => {
                const { Data, ...setting } = response.data;

                callBack && callBack(Data, setting);
            })
            .catch(error => {
                console.log(error.message);
                callBack && callBack([], {});
            });
    }

    static updateData(param, callBack) {
        const url = Site.resolveClientUrl(this.getControllerName(param.ProductId) + '/UpdateSetting');
        DataService.postData(url, param)
            .then(response => {
                const result = response.data;
                const isSuccess = Number(result.Code) === 0;
                const title = isSuccess ?
                    LangHelper.getSingleResource('MessageSuccess') :
                    LangHelper.getSingleResource('Fail');

                callBack && callBack(title, result.Message, isSuccess);
            })
            .catch(error => {
                console.log(error.message);
                callBack(LangHelper.getSingleResource('Error'), LangHelper.getSingleResource('ErrorProcessingMsg'), false);
            });
    }

    static updateStatus(param, callBack) {
        const url = Site.resolveClientUrl(this.getControllerName(param.ProductId) + '/UpdateStatus');
        DataService.postData(url, param)
            .then(response => {
                const result = response.data;
                const isSuccess = Number(result.Code) === 0;
                const title = isSuccess ?
                    LangHelper.getSingleResource('MessageSuccess') :
                    LangHelper.getSingleResource('Fail');

                callBack && callBack(title, result.Message, isSuccess);
            })
            .catch(error => {
                console.log(error.message);
                callBack(LangHelper.getSingleResource('Error'), LangHelper.getSingleResource('ErrorProcessingMsg'), false);
            });
    }

    //Exchange only
    static updateExchangeChargeComm(param, callBack) {
        const url = Site.resolveClientUrl(Url[BetLimitProduct.BX] + '/UpdateChargeComm');
        DataService.postData(url, param)
            .then(response => {
                const result = response.data;
                const isSuccess = Number(result.Code) === 0;
                const title = isSuccess ?
                    LangHelper.getSingleResource('MessageSuccess') :
                    LangHelper.getSingleResource('Fail');

                callBack && callBack(title, result.Message, isSuccess);
            })
            .catch(error => {
                console.log(error.message);
                callBack(LangHelper.getSingleResource('Error'), LangHelper.getSingleResource('ErrorProcessingMsg'), false);
            });
    }
}