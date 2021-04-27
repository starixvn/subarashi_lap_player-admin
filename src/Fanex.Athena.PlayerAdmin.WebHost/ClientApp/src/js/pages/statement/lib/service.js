import DataService from '@jsroot/lib/DataService';
import Site from '@jsroot/lib/site';
import LangHelper from '@jsroot/lib/langHelper';

const RelativeUrl = 'PlayerStatement/GetStatementUrl';

export default class Service {
    static getData(param, callBack) {
        const url = Site.resolveClientUrl(RelativeUrl);
        DataService.postData(url, param)
            .then(response => {
                callBack && callBack(response.data);
            })
            .catch(error => {
                console.log(error.message);
                callBack && callBack(null);
            });
    }
}