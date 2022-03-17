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
    [BetLimitProduct.GDG]: { Msg: 'UpdateProductBetSettings', Product: 'ProductGDG' },
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
    [BetLimitProduct.CB]: { Msg: 'UpdateProductBetSettings', Product: 'ProductCB' },
    [BetLimitProduct.VGA]: { Msg: 'UpdateProductBetSettings', Product: 'ProductVGA' },
    [BetLimitProduct.VS3]: { Msg: 'UpdateProductBetSettings', Product: 'ProductVS3' }
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
    [BetLimitProduct.GDG]: { Msg: 'UpdateProductStatus', Product: 'ProductGDG' },
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
    [BetLimitProduct.CB]: { Msg: 'UpdateProductStatus', Product: 'ProductCB' },
    [BetLimitProduct.KN]: { Msg: 'UpdateProductStatus', Product: 'ProductKN' },
    [BetLimitProduct.TG]: { Msg: 'UpdateProductStatus', Product: 'ProductTG' },
    [BetLimitProduct.LT]: { Msg: 'UpdateProductStatus', Product: 'ProductLT' },
    [BetLimitProduct.VGA]: { Msg: 'UpdateProductStatus', Product: 'ProductVGA' },
    [BetLimitProduct.AP]: { Msg: 'UpdateProductStatus', Product: 'ProductAP' },
    [BetLimitProduct.APM]: { Msg: 'UpdateProductStatus', Product: 'ProductAPM' }
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

    static formatValue(number) {
        return NumberFormat.parseToString(number);
    }

    static getValueAsNumber(text) {
        const number = parseFloat(text.replace(/,/g, ''));

        return !isNaN(number) && isFinite(number) ? NumberFormat.round(number, 2) : NaN;
    }
}