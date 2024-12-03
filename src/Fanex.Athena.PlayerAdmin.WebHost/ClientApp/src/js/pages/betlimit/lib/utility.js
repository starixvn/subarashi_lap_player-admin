import LangHelper from '@jsroot/lib/langHelper';
import NumberFormat from '@jsroot/lib/numberFormat';
import { BetLimitProduct } from './constants';

const ConfirmationMsg = {
    [BetLimitProduct.SB]: { Msg: 'UpdateSportsbookBetSettings' },
    [BetLimitProduct.VS]: { Msg: 'UpdateProductBetSettings', Product: 'ProductVS' },
    [BetLimitProduct.VS2]: { Msg: 'UpdateProductBetSettings', Product: 'ProductVS2' },
    [BetLimitProduct.LT]: { Msg: 'UpdateProductBetSettings', Product: 'ProductLT' },
    [BetLimitProduct.NG]: { Msg: 'UpdateNumberGameBetSettings' },
    [BetLimitProduct.KN]: { Msg: 'UpdateProductBetSettings', Product: 'ProductKN' },
    [BetLimitProduct.AGC]: { Msg: 'UpdateAgCasinoBetSettings' },
    [BetLimitProduct.TG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductTG' },
    [BetLimitProduct.I4D]: { Msg: 'UpdateProductBetSettings', Product: 'ProductI4D' },
    [BetLimitProduct.SLT]: { Msg: 'UpdateProductBetSettings', Product: 'ProductSLT' },
    [BetLimitProduct.CG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductCG' },
    [BetLimitProduct.AB]: { Msg: 'UpdateProductBetSettings', Product: 'ProductAB' },
    [BetLimitProduct.VB]: { Msg: 'UpdateProductBetSettings', Product: 'ProductVB' },
    [BetLimitProduct.CO]: { Msg: 'UpdateProductBetSettings', Product: 'ProductCO' },
    [BetLimitProduct.BX]: { Msg: 'UpdateProductBetSettings', Product: 'ProductBX' },
    [BetLimitProduct.PP]: { Msg: 'UpdateProductBetSettings', Product: 'ProductPP' },
    [BetLimitProduct.SXG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductSXG' },
    [BetLimitProduct.HB]: { Msg: 'UpdateProductBetSettings', Product: 'ProductHB' },
    [BetLimitProduct.JK]: { Msg: 'UpdateProductBetSettings', Product: 'ProductJK' },
    [BetLimitProduct.SAG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductSAG' },
    [BetLimitProduct.PGS]: { Msg: 'UpdateProductBetSettings', Product: 'ProductPGS' },
    [BetLimitProduct.SBC]: { Msg: 'UpdateProductBetSettings', Product: 'ProductSBC' },
    [BetLimitProduct.WM]: { Msg: 'UpdateProductBetSettings', Product: 'ProductWM' },
    [BetLimitProduct.SG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductSG' },
    [BetLimitProduct.VRG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductVRG' },
    [BetLimitProduct.CC]: { Msg: 'UpdateProductBetSettings', Product: 'ProductCC' },
    [BetLimitProduct.CS]: { Msg: 'UpdateProductBetSettings', Product: 'ProductCS' },
    [BetLimitProduct.VGA]: { Msg: 'UpdateProductBetSettings', Product: 'ProductVGA' },
    [BetLimitProduct.LG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductLG' },
    [BetLimitProduct.BTC]: { Msg: 'UpdateProductBetSettings', Product: 'ProductBTC' },
    [BetLimitProduct.FGGL]: { Msg: 'UpdateProductBetSettings', Product: 'ProductFGGL' },
    [BetLimitProduct.MXG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductMXG' },
    [BetLimitProduct.JIG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductJIG' },
    [BetLimitProduct.YB]: { Msg: 'UpdateProductBetSettings', Product: 'ProductYB' },
    [BetLimitProduct.MGRNG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductMGRNG' },
    [BetLimitProduct.SEA]: { Msg: 'UpdateProductBetSettings', Product: 'ProductSEA' }
}

const ConfirmationUpdateProductStatusMsg = {
    [BetLimitProduct.CG]: { Msg: 'UpdateProductStatus', Product: 'ProductCG' },
    [BetLimitProduct.AGC]: { Msg: 'UpdateProductStatus', Product: 'ProductAGC' },
    [BetLimitProduct.I4D]: { Msg: 'UpdateProductStatus', Product: 'ProductI4D' },
    [BetLimitProduct.SLT]: { Msg: 'UpdateProductStatus', Product: 'ProductSLT' },
    [BetLimitProduct.AB]: { Msg: 'UpdateProductStatus', Product: 'ProductAB' },
    [BetLimitProduct.VB]: { Msg: 'UpdateProductStatus', Product: 'ProductVB' },
    [BetLimitProduct.CO]: { Msg: 'UpdateProductStatus', Product: 'ProductCO' },
    [BetLimitProduct.BX]: { Msg: 'UpdateProductStatus', Product: 'ProductBX' },
    [BetLimitProduct.PP]: { Msg: 'UpdateProductStatus', Product: 'ProductPP' },
    [BetLimitProduct.SXG]: { Msg: 'UpdateProductStatus', Product: 'ProductSXG' },
    [BetLimitProduct.HB]: { Msg: 'UpdateProductStatus', Product: 'ProductHB' },
    [BetLimitProduct.JK]: { Msg: 'UpdateProductStatus', Product: 'ProductJK' },
    [BetLimitProduct.SAG]: { Msg: 'UpdateProductStatus', Product: 'ProductSAG' },
    [BetLimitProduct.PGS]: { Msg: 'UpdateProductStatus', Product: 'ProductPGS' },
    [BetLimitProduct.SBC]: { Msg: 'UpdateProductStatus', Product: 'ProductSBC' },
    [BetLimitProduct.WM]: { Msg: 'UpdateProductStatus', Product: 'ProductWM' },
    [BetLimitProduct.SG]: { Msg: 'UpdateProductStatus', Product: 'ProductSG' },
    [BetLimitProduct.VRG]: { Msg: 'UpdateProductStatus', Product: 'ProductVRG' },
    [BetLimitProduct.CC]: { Msg: 'UpdateProductStatus', Product: 'ProductCC' },
    [BetLimitProduct.CS]: { Msg: 'UpdateProductStatus', Product: 'ProductCS' },
    [BetLimitProduct.KN]: { Msg: 'UpdateProductStatus', Product: 'ProductKN' },
    [BetLimitProduct.TG]: { Msg: 'UpdateProductStatus', Product: 'ProductTG' },
    [BetLimitProduct.LT]: { Msg: 'UpdateProductStatus', Product: 'ProductLT' },
    [BetLimitProduct.VGA]: { Msg: 'UpdateProductStatus', Product: 'ProductVGA' },
    [BetLimitProduct.AP]: { Msg: 'UpdateProductStatus', Product: 'ProductAP' },
    [BetLimitProduct.APM]: { Msg: 'UpdateProductStatus', Product: 'ProductAPM' },
    [BetLimitProduct.BTC]: { Msg: 'UpdateProductStatus', Product: 'ProductBTC' },
    [BetLimitProduct.FGGL]: { Msg: 'UpdateProductStatus', Product: 'ProductFGGL' },
    [BetLimitProduct.MXG]: { Msg: 'UpdateProductStatus', Product: 'ProductMXG' },
    [BetLimitProduct.JIG]: { Msg: 'UpdateProductStatus', Product: 'ProductJIG' },
    [BetLimitProduct.YB]: { Msg: 'UpdateProductStatus', Product: 'ProductYB' },
    [BetLimitProduct.MGRNG]: { Msg: 'UpdateProductStatus', Product: 'ProductMGRNG' },
    [BetLimitProduct.SEA]: { Msg: 'UpdateProductStatus', Product: 'ProductSEA' },
    [BetLimitProduct.WE]: { Msg: 'UpdateProductStatus', Product: 'ProductWE' },
    [BetLimitProduct.L22]: { Msg: 'UpdateProductStatus', Product: 'ProductL22' },
    [BetLimitProduct.UUSL]: { Msg: 'UpdateProductStatus', Product: 'ProductUUSL' },
    [BetLimitProduct.FS]: { Msg: 'UpdateProductStatus', Product: 'ProductFS' },
    [BetLimitProduct.FAC]: { Msg: 'UpdateProductStatus', Product: 'ProductFAC' },
    [BetLimitProduct.NS]: { Msg: 'UpdateProductStatus', Product: 'ProductNS' },
    [BetLimitProduct.ON]: { Msg: 'UpdateProductStatus', Product: 'ProductON' },
    [BetLimitProduct.PTRNG]: { Msg: 'UpdateProductStatus', Product: 'ProductPTRNG' },
    [BetLimitProduct.GPIRNG]: { Msg: 'UpdateProductStatus', Product: 'ProductGPIRNG' },
    [BetLimitProduct.GPILC]: { Msg: 'UpdateProductStatus', Product: 'ProductGPILC' },
}

const ConfirmationUpdateProductChargeCommMsg = {
    [BetLimitProduct.BX]: { Msg: 'UpdateProductChargeComm', Product: 'ProductBX' },
}

export default class Utility {
    static getConfirmationMsg(productId) {
        const message = LangHelper.getSingleResource(ConfirmationMsg[productId].Msg);

        if (!ConfirmationMsg[productId].Product) {
            return message;
        }

        return message.replace('{0}', LangHelper.getSingleResource(ConfirmationMsg[productId].Product));
    }

    static getConfirmationUpdateStatusMsg(productId) {
        const message = LangHelper.getSingleResource(ConfirmationUpdateProductStatusMsg[productId].Msg);

        if (!ConfirmationUpdateProductStatusMsg[productId].Product) {
            return message;
        }

        return message.replace('{0}', LangHelper.getSingleResource(ConfirmationUpdateProductStatusMsg[productId].Product));
    }

    static getConfirmationUpdateChargeCommMsg(productId) {
        const { Msg, Product } = ConfirmationUpdateProductChargeCommMsg[productId];
        const message = LangHelper.getSingleResource(Msg);
        return message.replace('{0}', LangHelper.getSingleResource(Product));
    }

    static formatValue(number) {
        return NumberFormat.parseToString(number);
    }

    static getValueAsNumber(text) {
        const number = parseFloat(text.replace(/,/g, ''));

        return !isNaN(number) && isFinite(number) ? NumberFormat.round(number, 2) : NaN;
    }
}