import Site from '@jsroot/lib/site';
import { Url } from '../constants';

export default class QueryHelper {
    static FetchBetList({ params, toggleLoader, getGridData }) {
        toggleLoader(true);
        const realUrl = Site.resolveClientUrl(Url.AbnormalBetList);
        getGridData(realUrl, params);
    }
}