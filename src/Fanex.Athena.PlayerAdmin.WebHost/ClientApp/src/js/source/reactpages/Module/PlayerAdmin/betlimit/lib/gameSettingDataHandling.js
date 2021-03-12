import Validatation from './validatation';
import DataHandling from './dataHandling';

export default class GameSettingDataHandling extends DataHandling {
    validate(currentValue, row) {
        if (!currentValue.IsUpdated) {
            return null;
        }

        const validateMinMaxBet = Validatation.validateMinMaxBet(currentValue, row);

        if (validateMinMaxBet !== null) {
            return validateMinMaxBet;
        }

        return Validatation.validateMaxBetPerDraw(currentValue, row);
    }

    getHeaderName(header) {
        if (header === 'Sport') {
            return super.getHeaderName('GameType');
        }

        if (header === 'MaxPerMatch') {
            return super.getHeaderName('MaxBetPerDraw');
        }

        return super.getHeaderName(header);
    }
}