import QueryString from '@jsroot/lib/QueryString';
import Site from '@jsroot/lib/site';
import { Url } from '../constants';

export default class BetListHelper {
    static buildAbnormalBetListUrl({ custId, externalId, systemId, baseCurrency }) {
        let url = Site.resolveClientUrl(Url.abnormalBetList);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'externalId', externalId);
        url = QueryString.setParam(url, 'systemId', systemId);
        url = QueryString.setParam(url, 'baseCurrency', baseCurrency);
        return url;
    }
}