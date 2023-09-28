import Validatation from './validatation';
import DataHandling from './dataHandling';
import Utility from './utility';

export default class VS3DataHandling extends DataHandling {
    validate(currentValue, row) {
        if (!currentValue.IsUpdated) {
            return null;
        }
        
        const validateMinMaxBet =  Validatation.validateMinMaxBet(currentValue, row);

        if (validateMinMaxBet !== null) {
            return validateMinMaxBet;
        }

        return Validatation.validateMaxPerMatch(currentValue, row);
    }

    getSubmitRowData(row) {
        return {
            Id: row.Id,
            BetType: row.BetType,
            SportType: row.SportType,
            BetSettingType: row.BetSettingType,
            MinBet: Utility.getValueAsNumber(row.MinBet),
            MaxBet: Utility.getValueAsNumber(row.MaxBet),
            MaxPerMatch: Utility.getValueAsNumber(row.MaxPerMatch),
            OldMinBet: row.OldMinBet,
            OldMaxBet: row.OldMaxBet,
            OldMaxPerMatch: row.OldMaxPerMatch,
            IsUpdated: row.MinBet !== row.OriginalMinBet
                || row.MaxBet !== row.OriginalMaxBet
                || row.MaxPerMatch !== row.OriginalMaxPerMatch
        };
    }

    getHeaderName(header) {
        return LangHelper.getSingleResource(header);
    }
}