import Validatation from './validatation';
import Utility from './utility';
import LangHelper from '@jsroot/lib/langHelper';

export default class DataHandling {
    getViewData(rawData) {
        return rawData.map((rawRow) => {
            return this.getViewRowData(rawRow);
        });
    }

    getViewRowData(rawData) {
        let row = {
            Id: rawData.Id,
            SportType: rawData.SportType,
            BetType: rawData.BetType,
            BetSettingType: rawData.BetSettingType,
            MinBet: Utility.formatValue(rawData.MinBet),
            MaxBet: Utility.formatValue(rawData.MaxBet),
            MaxPerMatch: Utility.formatValue(rawData.MaxPerMatch),
            BetValueMin: rawData.BetValueMin,
            BetValueMax: rawData.BetValueMax,
            MaxPerMatchMin: rawData.MaxPerMatchMin,
            MaxPerMatchMax: rawData.MaxPerMatchMax,
            OldMinBet: rawData.MinBet,
            OldMaxBet: rawData.MaxBet,
            OldMaxPerMatch: rawData.MaxPerMatch,
        };

        row.OriginalMinBet = row.MinBet;
        row.OriginalMaxBet = row.MaxBet;
        row.OriginalMaxPerMatch = row.MaxPerMatch;

        return row;
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

    getSubmitData(rows) {
        const submitData = [];

        for (let i = 0; i < rows.length; i++) {
            const row = rows[i];
            const currentValue = this.getSubmitRowData(row);
            const invalidData = this.validate(currentValue, row);

            if (invalidData !== null) {
                return {
                    IsValid: false,
                    Data: invalidData
                };
            }

            submitData.push(currentValue);
        }

        return {
            IsValid: true,
            Data: submitData
        };
    }

    validate(currentValue, row) {
        if (!currentValue.IsUpdated) {
            return null;
        }

        const validateMinMaxBet = Validatation.validateMinMaxBet(currentValue, row);

        if (validateMinMaxBet !== null) {
            return validateMinMaxBet;
        }

        return Validatation.validateMaxPerMatch(currentValue, row);
    }

    getHeaderName(header) {
        return LangHelper.getSingleResource(header);
    }
}