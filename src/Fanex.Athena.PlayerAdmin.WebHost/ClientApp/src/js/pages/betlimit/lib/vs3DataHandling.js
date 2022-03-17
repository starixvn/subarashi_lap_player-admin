import Validatation from './validatation';
import DataHandling from './dataHandling';
import Utility from './utility';

export default class I4DDataHandling extends DataHandling {
    validate(currentValue, row) {
        if (!currentValue.IsUpdated) {
            return null;
        }
        
        return Validatation.validateMinMaxBet(currentValue, row);
    }

    getSubmitRowData(row) {
        return {
            Id: row.Id,
            BetType: row.BetType,
            BetSettingType: row.BetSettingType,
            MinBet: Utility.getValueAsNumber(row.MinBet),
            MaxBet: Utility.getValueAsNumber(row.MaxBet),
            MaxPerMatch: Utility.getValueAsNumber(row.MaxBet),
            OldMinBet: row.OldMinBet,
            OldMaxBet: row.OldMaxBet,
            OldMaxPerMatch: row.OldMaxPerMatch,
            IsUpdated: row.MinBet !== row.OriginalMinBet
                || row.MaxBet !== row.OriginalMaxBet
        };
    }

    getHeaderName(header) {
        return LangHelper.getSingleResource(header);
    }
}