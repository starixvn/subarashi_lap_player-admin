import LangHelper from '@jsroot/lib/langHelper';
import { BetLimitType, BetLimitSport } from './constants';

export default class Validatation {
    static validateMinMaxBet(current, limitData) {
        const { MinBet, MaxBet, MaxPerMatch } = current;
        const Id = current.Id === BetLimitSport.I4D ? current.Id : current.BetType;

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
                Message: LangHelper.getSingleResource('MinBetGreaterThanOrEqual') + ' ' + limitData.BetValueMin
            };
        }

        if (limitData.BetValueMax > 0 && MaxBet > limitData.BetValueMax) {
            return {
                Id: Id,
                Type: BetLimitType.MaxBet,
                Message: LangHelper.getSingleResource('MaxBetLessThanOrEqual') + ' ' + limitData.BetValueMax
            };
        }

        if (MinBet > MaxBet) {
            return {
                Id: Id,
                Type: BetLimitType.MinBet,
                Message: LangHelper.getSingleResource('MinBetCanNotGreaterThanMaxBet') + ' ' + MaxBet
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
                Message: LangHelper.getSingleResource('MaxBetMatchMoreThanOrEqual') + ' ' + limitData.MaxPerMatchMax
            };
        }

        if (limitData.MaxPerMatchMin > 0 && MaxPerMatch < limitData.MaxPerMatchMin) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerMatch,
                Message: LangHelper.getSingleResource('MaxBetMatchLessThanOrEqual') + ' ' + limitData.MaxPerMatchMin
            };
        }

        if (MaxBet > MaxPerMatch) {
            return {
                Id: Id,
                Type: BetLimitType.MaxBet,
                Message: LangHelper.getSingleResource('MaxBetCanNotGreaterThanMaxPerMatch') + ' ' + MaxPerMatch
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
                Message: LangHelper.getSingleResource('MaxBetPerDrawMoreThanOrEqual') + ' ' + limitData.MaxPerMatchMax
            };
        }

        if (limitData.MaxPerMatchMin > 0 && MaxPerMatch < limitData.MaxPerMatchMin) {
            return {
                Id: Id,
                Type: BetLimitType.MaxPerMatch,
                Message: LangHelper.getSingleResource('MaxBetPerDrawLessThanOrEqual') + ' ' + limitData.MaxPerMatchMin
            };
        }

        if (MaxBet > MaxPerMatch) {
            return {
                Id: Id,
                Type: BetLimitType.MaxBet,
                Message: LangHelper.getSingleResource('MaxBetCanNotGreaterThanMaxBetPerDraw') + ' ' + MaxPerMatch
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
                Message: LangHelper.getSingleResource('MaxPerBallLessThanOrEqual') + ' ' + limitData.MaxPerBallMax
            };
        }

        return null;
    }
}