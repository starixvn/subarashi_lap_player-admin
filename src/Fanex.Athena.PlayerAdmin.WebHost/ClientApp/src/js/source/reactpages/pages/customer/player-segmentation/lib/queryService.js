import DataService from '@jsroot/lib/DataService';
import ProblemPlayerService from '../tabs/problem-player/lib/queryService';
import VipPlayerService from '../tabs/vip-player-management/lib/queryService';
import { tabKeys, errorResponse, successResponse } from './constants';

class CommonQueryService {
    static async updateVipStatus(params) {
        try {
            const response = await DataService.postData('Api/PlayerSegmentation/UpdateVipStatus', params);
            if(response.status === 200) {
                return { 
                    ...successResponse,
                    ...{ data: response.data }
                }
            }
        }
        catch(e) {
            console.log(e)
        }
        return errorResponse
    }
}

export const queryServiceCommonFactory = {
    [tabKeys.problemPlayer]: {
        getPlayerList: ProblemPlayerService.getPlayerList, 
    },
    [tabKeys.vipPlayerManagement]: {
        getPlayerList: VipPlayerService.getPlayerList
    },
    common: {
        updateVipStatus: CommonQueryService.updateVipStatus
    }
}
