import { toggleLoader } from '@jsroot/actions/loaderAction';
import { setPagingOption } from '@jsroot/actions/pagingOptionAction';
import { setGridData } from '@jsroot/actions/gridDataAction';

import Site from '@jsroot/lib/site';
import DataService from '@jsroot/lib/DataService';
import LangHelper from '@jsroot/lib/langHelper';

const emptyData = { dataList: [], total: {} };
const defaultPaingOption = { TotalItem: 0 };

const dispatchResult = (dispatch, data, pagingOption) => {
    dispatch(setGridData(data));
    dispatch(setPagingOption(pagingOption));
    dispatch(toggleLoader(false));
}

const apiUrl = Site.resolveClientUrl('DailyBetsReport/Index');
export const getData = (param) => (dispatch) => {
    dispatch(toggleLoader(true));

    DataService.postData(apiUrl, param)
        .then(response => {
            const { dataList, total, totalRows } = response.data;
            const data = {
                dataList: dataList || [],
                total: total || {}
            };
            const newPagingOption = {
                TotalItem: totalRows,
                PageSize: param.pageSize,
                PageIndex: param.pageIndex
            }
            dispatchResult(dispatch, data, newPagingOption);
        })
        .catch(error => {
            console.log(error.message);
            dispatchResult(dispatch, {...emptyData }, {...defaultPaingOption });
        });
}

export const exportExcel = (param) => (dispatch) => {
    dispatch(toggleLoader(true));

    DataService.getFile(apiUrl, param)
        .then(response => {
            if (response.data.size === 0) {
                alert(LangHelper.getSingleResource("NoInfo"));
            } else {
                Site.downloadFile(response.data, 'DailyBetsReport.zip');
            }
            dispatch(toggleLoader(false));
        })
        .catch(error => {
            console.log(error.message);
            dispatch(toggleLoader(false));
        });
}