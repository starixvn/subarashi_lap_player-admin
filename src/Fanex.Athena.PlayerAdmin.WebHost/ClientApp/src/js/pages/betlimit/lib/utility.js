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
}

export default class Utility {
    static getConfirmationMsg(productId) {
        const message = LangHelper.getSingleResource(ConfirmationMsg[productId].Msg);

        if (!ConfirmationMsg[productId].Product) {
            return message;
        }

        return message.replace('{0}', LangHelper.getSingleResource(ConfirmationMsg[productId].Product));
    }

    static formatValue(number) {
        return NumberFormat.parseToString(number);
    }

    static getValueAsNumber(text) {
        const number = parseFloat(text.replace(/,/g, ''));

        return !isNaN(number) && isFinite(number) ? NumberFormat.round(number, 2) : NaN;
    }
}