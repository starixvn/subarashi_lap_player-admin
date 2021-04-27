import FactoryCommonService from '../../../../lib/factoryCommonService';
import { apiSuggestion } from '../constants';

export default class PlayerSuggestionService {
    constructor() {
        this.commonService = FactoryCommonService.createInstance();
    }

    getSuggestionList(param) {
        const configOption = {
            method: 'get'
        }

        return this.commonService.fetchData(apiSuggestion.type, apiSuggestion.action, {...param }, configOption);
    }
}