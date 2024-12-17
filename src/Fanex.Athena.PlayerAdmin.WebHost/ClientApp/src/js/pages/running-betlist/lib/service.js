import LangHelper from '@jsroot/lib/langHelper';
import Site from '@jsroot/lib/site';
import DataService from '@jsroot/lib/DataService';
import axios from 'axios';

const HeaderText = {
    Index: "#",
    Platform: LangHelper.getSingleResource("Platform"),
    ExternalId: LangHelper.getSingleResource("ExternalId"),
    SystemId: LangHelper.getSingleResource("SystemId"),
    RefNo: LangHelper.getSingleResource("RefNo"),
    LicenseeTxId: LangHelper.getSingleResource("LicenseeTxId"),
    TransTime: LangHelper.getSingleResource("TransTime"),
    Choice: LangHelper.getSingleResource("Choice"),
    Odds: LangHelper.getSingleResource("Odds"),
    OddsType: LangHelper.getSingleResource("OddsType"),
    Stake: LangHelper.getSingleResource("Stake"),
    Status: LangHelper.getSingleResource("Status"),
    Live: LangHelper.getSingleResource("Live")
}

const DataApi = "PlayerBetList/GetRunningBetList";

export default class Service {
    static getHeaderText() {
        return HeaderText;
    }

    static getReportTitle(currency) {
        return currency ? `${LangHelper.getSingleResource('Currency')}: ${currency.toUpperCase()}` : '';
    }

    static getTotalText() {
        return LangHelper.getSingleResource('Total');
    }

    static export(param, callback) {
        const url = Site.resolveClientUrl(DataApi);
        const exportParam = Object.assign({}, param, { IsExport: true });
        DataService.getFile(url, exportParam).then((response) => {
            const NoContentStatusCode = 204;

            if (response.status === NoContentStatusCode) {
                alert(LangHelper.getSingleResource("NoInfo"));
            } else {
                Site.downloadFile(response.data, 'PlayerAdministratorBetList.xlsx');
            }

            callback();
        })
            .catch(error => {
                console.log(error.message);
                if (error.response) {
                    alert(LangHelper.getSingleResource("ErrorProcessingMsg"));
                    callback();
                }
            });
    }

    static getData(param, callBack) {
        const url = Site.resolveClientUrl(DataApi);
        DataService.postData(url, param)
            .then(response => {
                const result = response.data;

                const data = result.Data;
                if (data && data.length > 0) {
                    callBack && callBack(data, result.Total, result.TotalRows);
                }
                else {
                    callBack && callBack([], {}, 0);
                }
            })
            .catch(error => {
                console.log(error.message);
                callBack && callBack([], {}, 0);
            });
    }
}