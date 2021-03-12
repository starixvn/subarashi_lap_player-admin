import * as types from './types';
import { toggleLoader } from '@jsroot/actions/loaderAction';
import { queryServiceCommonFactory } from '../lib/queryService';

export const searchPlayer = (key, params, afterSuccess) => async  (dispatch) => {
    dispatch(toggleLoader(true));
    const response = await queryServiceCommonFactory[key].getPlayerList(params);
    response.isSuccess && afterSuccess && afterSuccess(response);
    dispatch(setPlayerList(response.data));
    dispatch(toggleLoader(false));
}

export const updateVipStatus = (params, afterSuccess) => async (dispatch) => {
    dispatch(toggleLoader(true));
    const response = await queryServiceCommonFactory.common.updateVipStatus(params);
    afterSuccess && afterSuccess(response);
    dispatch(toggleLoader(false));
}

export const setPlayerList = data => {
    return {
        type: types.SET_PLAYER_LIST,
        data
    }
}
