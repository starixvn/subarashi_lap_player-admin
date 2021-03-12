import { toggleLoader } from '@jsroot/actions/loaderAction';
import { setGridData } from '@jsroot/actions/gridDataAction';

import Site from '@jsroot/lib/site';
import DataService from '@jsroot/lib/DataService';
import ReportExcel from '../lib/reportExcel';

const SUCCESS_CODE = 1;
const UM_ERR_CODE = 101;
const UM_MESSAGE = 'Exchange product is under maintenance now';
const ERROR_MESSAGE = 'Your request cannot be processed now, please try again later';
const MAX_REFNO = 2147483647;

const dispatchResult = (dispatch, data) => {
    dispatch(setGridData(data));
    dispatch(toggleLoader(false));
}

const apiUrl = Site.resolveClientUrl('ExchangeVoided/Index');
export const getData = (param) => (dispatch) => {
    dispatch(toggleLoader(true));
    if (isNaN(param.refNo) || Number(param.refNo) > MAX_REFNO) {
        dispatchResult(dispatch, { dataList: [], totalPage: 0 });
    } else {
        DataService.postData(apiUrl, param)
            .then(response => {
                const { dataList, totalPage, errCode } = response.data;
                const isSuccess = errCode === SUCCESS_CODE;
                const data = {
                    dataList: isSuccess ? dataList : [],
                    totalPage: param.refNo ? 0 : totalPage
                };
                dispatchResult(dispatch, data);

                !isSuccess && window.alert(errCode === UM_ERR_CODE ? UM_MESSAGE : ERROR_MESSAGE);
            })
            .catch(error => {
                console.log(error.message);
                dispatchResult(dispatch, { dataList: [], totalPage: 0 });
            });
    }
}

export const exportExcel = (dataList, title) => () => {
    const service = new ReportExcel(dataList, title);
    service.export();
}