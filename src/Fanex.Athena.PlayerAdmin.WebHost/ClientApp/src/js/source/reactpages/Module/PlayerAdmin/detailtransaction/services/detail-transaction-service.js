import DataService from '@jsroot/lib/DataService';
import Site from '@jsroot/lib/site';

export default class DetailTransactionReportService {
    getData(param, callBack) {
        const relativeUrl = 'PlayerDetailTransaction/GetDetailTransactionReport';
        const url = Site.resolveClientUrl(relativeUrl);

        DataService.postData(url, param)
            .then(response => {
                callBack && callBack(response.data);
            })
            .catch(error => {
                console.log(error.message);
                callBack && callBack(null);
            });
    }
};
