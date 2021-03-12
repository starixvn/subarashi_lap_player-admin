import DataService from '@jsroot/lib/DataService';
import { errorResponse, successResponse } from '../../../lib/constants';

export default class ProblemPlayerQueryService {
    static async getPlayerList(params) {
        try {
            const response = await DataService.postData('Api/PlayerSegmentation/GetProblemPlayerList', params);
            if(response.status === 200) {
                return { 
                    ...successResponse,
                    ...response.data
                }
            }
        }
        catch(e) {
            console.log(e)
        }
        return errorResponse;
    }
}
