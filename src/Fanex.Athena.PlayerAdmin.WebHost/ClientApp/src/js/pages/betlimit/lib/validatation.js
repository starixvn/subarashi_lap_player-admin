import LangHelper from '@jsroot/lib/langHelper';
import { BetLimitType, BetLimitSport } from './constants';
import NumberFormat from "@jsroot/lib/numberFormat";

export default class Validatation {
    static validateMinMaxBet(current, limitData) {
        const { MinBet, MaxBet, MaxPerMatch } = current;
        const Id = current.SportType === BetLimitSport.I4D ? current.SportType : current.Id;

        if (isNaN(MinBet)) {
            return {
                Id: Id,
                Type: BetLimitType.MinBet,
                Message: LangHelper.getSingleResource('IncorrectMinBet')
            };
        }
        if (isNaN(MaxBet)) {
            return {
                Id: Id,
                Type: BetLimitType.MaxBet,
                Message: LangHelper.getSingleResource('IncorrectMaxBet')
            };
        }

        if (limitData.BetValueMin > 0 && MinBet < limitData.BetValueMin) {
            return {
                Id: Id,
                Type: BetLimitType.MinBet,
                Message: LangHelper.getSingleResource('MinBetGreaterThanOrEqual') + ' ' + NumberFormat.formatInteger(limitData.BetValueMin)
            };
        }

        if (limitData.BetValueMax > 0 && MaxBet > limitData.BetValueMax) {
            return {
                Id: Id,
                Type: BetLimitType.MaxBet,
                Message: LangHelper.getSingleResource('MaxBetLessThanOrEqual') + ' ' + NumberFormat.formatInteger(limitData.BetValueMax)
            };
        }

        if (MinBet > MaxBet) {
            return {
                Id: Id,
                Type: BetLimitType.MinBet,
                Message: LangHelper.getSingleResource('MinBetCanNotGreaterThanMaxBet') + ' ' + NumberFormat.formatInteger(MaxBet)
            };
        }

        return null;
    }

    static validateMaxPerMatch(current, limitData) {
        const { MaxBet, MaxPerMatch, Id } = current;

        if (isNaN(MaxPerMatch)) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerMatch,
                Message: LangHelper.getSingleResource('IncorrectMaxPerMatch')
            };
        }

        if (limitData.MaxPerMatchMax > 0 && MaxPerMatch > limitData.MaxPerMatchMax) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerMatch,
                Message: LangHelper.getSingleResource('MaxBetMatchLessThanOrEqual') + ' ' + NumberFormat.formatInteger(limitData.MaxPerMatchMax)
            };
        }

        if (limitData.MaxPerMatchMin > 0 && MaxPerMatch < limitData.MaxPerMatchMin) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerMatch,
                Message: LangHelper.getSingleResource('MaxBetMatchMoreThanOrEqual') + ' ' + NumberFormat.formatInteger(limitData.MaxPerMatchMin)
            };
        }

        if (MaxBet > MaxPerMatch) {
            return {
                Id: Id,
                Type: BetLimitType.MaxBet,
                Message: LangHelper.getSingleResource('MaxBetCanNotGreaterThanMaxPerMatch') + ' ' + NumberFormat.formatInteger(MaxPerMatch)
            };
        }

        return null;
    }

    static validateMaxBetPerDraw(current, limitData) {
        const { MaxBet, MaxPerMatch, Id } = current;

        if (isNaN(MaxPerMatch)) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerMatch,
                Message: LangHelper.getSingleResource('IncorrectMaxBetPerDraw')
            };
        }

        if (limitData.MaxPerMatchMax > 0 && MaxPerMatch > limitData.MaxPerMatchMax) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerMatch,
                Message: LangHelper.getSingleResource('MaxBetPerDrawLessThanOrEqual') + ' ' + NumberFormat.formatInteger(limitData.MaxPerMatchMax)
            };
        }

        if (limitData.MaxPerMatchMin > 0 && MaxPerMatch < limitData.MaxPerMatchMin) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerMatch,
                Message: LangHelper.getSingleResource('MaxBetPerDrawMoreThanOrEqual') + ' ' + NumberFormat.formatInteger(limitData.MaxPerMatchMin)
            };
        }

        if (MaxBet > MaxPerMatch) {
            return {
                Id: Id,
                Type: BetLimitType.MaxBet,
                Message: LangHelper.getSingleResource('MaxBetCanNotGreaterThanMaxBetPerDraw') + ' ' + NumberFormat.formatInteger(MaxPerMatch)
            };
        }

        return null;
    }

    static validateMaxPerBall(current, limitData) {
        const { MaxPerBall, Id } = current;

        if (isNaN(MaxPerBall)) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerBall,
                Message: LangHelper.getSingleResource('IncorrectMaxPerBall')
            };
        }

        if (limitData.MaxPerBallMax > 0 && MaxPerBall > limitData.MaxPerBallMax) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerBall,
                Message: LangHelper.getSingleResource('MaxPerBallLessThanOrEqual') + ' ' + NumberFormat.formatInteger(limitData.MaxPerBallMax)
            };
        }

        return null;
    }

    static validateMaxPayoutPerMatch(current, limitData){
        const { MaxPayoutPerMatch, Id, MaxPerMatch } = current;

        if (isNaN(MaxPayoutPerMatch)) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPayoutPerMatch,
                Message: LangHelper.getSingleResource('IncorrectMaxPayoutPerMatch')
            };
        }

        if (MaxPerMatch > MaxPayoutPerMatch){
            return{
                Id: Id,
                Type: BetLimitType.MaxPerMatch,
                Message: LangHelper.getSingleResource("MaxPerMatchNotGreaterMaxPayoutPerMatch")
            }
        }

        if (limitData.MaxPayoutPerMatchMax > 0 && MaxPayoutPerMatch > limitData.MaxPayoutPerMatchMax) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPayoutPerMatch,
                Message: LangHelper.getSingleResource('MaxPayoutPerMatchLessThanOrEqual') + ' ' + NumberFormat.formatInteger(limitData.MaxPayoutPerMatchMax)
            };
        }

        return null;
    }
}