import Validatation from './validatation';
import Utility from './utility';
import DataHandling from './dataHandling';

export default class NGDataHandling extends DataHandling {
    getViewRowData(rawData) {
        let row = super.getViewRowData(rawData);
        row.MaxPerBall = Utility.formatValue(rawData.MaxPerBall);
        row.OldMaxPerBall = rawData.MaxPerBall;
        row.MaxPerBallMax = rawData.MaxPerBallMax;
        row.OriginalMaxPerBall = row.MaxPerBall;

        return row;
    }

    getSubmitRowData(row) {
        let data = super.getSubmitRowData(row);
        data.MaxPerBall = Utility.getValueAsNumber(row.MaxPerBall);
        data.OldMaxPerBall = row.OldMaxPerBall;
        data.IsUpdated = data.IsUpdated || row.MaxPerBall !== row.OriginalMaxPerBall;

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

        return Validatation.validateMaxPerBall(currentValue, row);
    }
}