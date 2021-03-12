import { toggleLoader } from '@jsroot/actions/loaderAction';
import DetailTransactionReportService from '../services/detail-transaction-service';
import { SET_IS_FETCHING_DATA, SET_REQUEST_PARAM, SET_DETAIL_TRANSACTION_REPORT } from './action-types';

const processBeforeGettingData = (dispatch) => {
    dispatch(toggleLoader(true));
    dispatch({ type: SET_IS_FETCHING_DATA, data: true });
};

const processAfterGettingData = (dispatch) => {
    dispatch({ type: SET_IS_FETCHING_DATA, data: false });
    dispatch(toggleLoader(false));
};

export const getDetailTransactionReport = (param) => (dispatch) => {
    processBeforeGettingData(dispatch);
    dispatch({ type: SET_REQUEST_PARAM, data: param });

    const service = new DetailTransactionReportService();
    service.getData(param, result => {
        dispatch({ type: SET_DETAIL_TRANSACTION_REPORT, data: result });
        processAfterGettingData(dispatch);
    });
};
