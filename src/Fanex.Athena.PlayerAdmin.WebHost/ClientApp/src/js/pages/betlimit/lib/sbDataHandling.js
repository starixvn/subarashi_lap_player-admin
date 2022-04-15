import Validatation from './validatation';
import Utility from './utility';
import DataHandling from './dataHandling';

export default class NGDataHandling extends DataHandling {
    getViewRowData(rawData) {
        let row = super.getViewRowData(rawData);
        row.MaxPayoutPerMatch = Utility.formatValue(rawData.MaxPayoutPerMatch);
        row.OldMaxPayoutPerMatch = rawData.MaxPayoutPerMatch;
        row.MaxPayoutPerMatchMax = rawData.MaxPayoutPerMatchMax;
        row.OriginalMaxPayoutPerMatch = row.MaxPayoutPerMatch;

        return row;
    }

    getSubmitRowData(row) {
        let data = super.getSubmitRowData(row);
        data.MaxPayoutPerMatch = Utility.getValueAsNumber(row.MaxPayoutPerMatch);
        data.OldMaxPayoutPerMatch = row.OldMaxPayoutPerMatch;
        data.IsUpdated = data.IsUpdated || row.MaxPayoutPerMatch !== row.OriginalMaxPayoutPerMatch;

        return data;
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
        const validatationResult = super.validate(currentValue, row);

        if (validatationResult !== null) {
            return validatationResult;
        }

        return Validatation.validateMaxPayoutPerMatch(currentValue, row);
    }
}