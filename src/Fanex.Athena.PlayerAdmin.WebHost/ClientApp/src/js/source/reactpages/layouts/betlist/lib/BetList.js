import QueryString from '@jsroot/lib/QueryString'
import { BetType } from './constants'

class BetList {
    _isFastMarket(bettype) {
        return bettype >= BetType.MIN_FASTMARKET && bettype <= BetType.MAX_FASTMARKET;
    }

    _getMixParlayResultUrl(rootUrl, refno, winlostdate, bettype) {
        let url = rootUrl + 'ResultReport/ResultPopup';
        url = QueryString.setParam(url, 'refno', refno);
        url = QueryString.setParam(url, 'winlossdate', winlostdate);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'matchid', 1);

        return url;
    }

    _getSportBookResultUrl(rootUrl, matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2) {
        let url = rootUrl + 'ResultReport/ResultPopup';
        url = QueryString.setParam(url, 'matchid', matchid);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'raceno', raceNo);
        url = QueryString.setParam(url, 'sporttype', sporttype);
        url = QueryString.setParam(url, 'isoutright', isOutright);

        if (this._isFastMarket(bettype)) {
            url = QueryString.setParam(url, 'handicap1', extension1);
            url = QueryString.setParam(url, 'handicap2', extension2);
        } else {
            url = QueryString.setParam(url, 'betid', extension1);
        }

        return url;
    }

    getResultUrl(rootUrl, matchid, raceNo, bettype, sporttype, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid) {
        bettype = Number(bettype);

        let isParlay = bettype === BetType.MIXPARLAY || bettype === BetType.SYSTEMPARLAY || bettype === BetType.MIXPARLAY_BETRADAR;
        let url = isParlay
            ? this._getMixParlayResultUrl(rootUrl, refnoMixParlay, winlostdate, bettype)
            : this._getSportBookResultUrl(rootUrl, matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid);

        return url;
    }

    getChoiceParlayDetailUrl(rootUrl, transId, refNo, handicap1, handicap2, betcheck, betteam, custId) {
        var url = rootUrl + 'api/MixParlayBetList/NewParlayDetail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'refNo', refNo);
        url = QueryString.setParam(url, 'handicap1', handicap1);
        url = QueryString.setParam(url, 'handicap2', handicap2);
        url = QueryString.setParam(url, 'betcheck', betcheck);
        url = QueryString.setParam(url, 'betteam', betteam);
        url = QueryString.setParam(url, 'custId', custId);

        return url;
    }

    buildChoiceParlayDetail(response) {
        let choice = '';
        let length = response.Data === null ? 0 : response.Data.length;
        if (length > 0) {
            for (let i = 0; i < response.Texts.length; i++) {
                choice += response.Texts[i];
            }

            choice += '<div class="line"></div>';

            for (let i = 0; i < response.Data.length; i++) {
                let item = response.Data[i];
                choice += item;
                if (i < length - 1) {
                    choice += '<div class="line"></div>';
                }
            }
        }

        return choice;
    }

    buildSingleMatchParlayDetail(data) {
        let selection = '';
        let length = data === null ? 0 : data.length;
        if (length > 0) {
            selection = '<div class="single-match-parlay-detail">';
            for (let i = 0; i < data.length; i++) {
                selection += data[i];
            }
            selection += '</div>';
        }

        return selection;
    }

    getParlayDetailUrl(rootUrl, transId, refNo, betId, custId, betTeam, baseCurrency) {
        if (Number(transId) === 0) { return; }

        var url = rootUrl + 'SystemParlayBetList/Detail';
        url = QueryString.setParam(url, 'refno', refNo);
        url = QueryString.setParam(url, 'betteam', betTeam);
        url = QueryString.setParam(url, 'basecurrency', baseCurrency);

        return url;
    }

    getSingleMatchParlayDetailUrl(rootUrl, transId, custId, status) {
        if (Number(transId) === 0) { return; }

        var url = rootUrl + 'api/SingleMatchParlayBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, "status", status);

        return url;
    }

    getCashOutHistoryPopupSetting(rootUrl, baseCurrency, transId, refNo, betType, betdaqId) {
        var isCashOut = betdaqId < 1 || betType === BetType.CASHOUT;
        var url = rootUrl + 'CashOutBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'refNo', refNo);
        url = QueryString.setParam(url, 'betType', betType);
        url = QueryString.setParam(url, 'betdaqId', betdaqId);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);
        url = QueryString.setParam(url, 'isCashout', isCashOut);
        return {
            url: url,
            isCashOut: isCashOut
        };
    }

    getBonusDetailUrl(rootUrl, refCode, transId, baseCurrency) {
        let url = rootUrl + 'BonusBetList/Detail';
        url = QueryString.setParam(url, 'RefCode', refCode);
        url = QueryString.setParam(url, 'TransId', transId);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getAGCasinoDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'AGCasinoBetList/Detail';
        url = QueryString.setParam(url, 'transid', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custid', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getRnGCasinoDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'RnGCasinoBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getVoidBridgeDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'VoidbridgeBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getAllBetDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'AllBetBetList/Detail';
        url = QueryString.setParam(url, 'transid', transId);
        url = QueryString.setParam(url, 'custid', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getColossusBetsDetailUrl(rootUrl, baseCurrency, refNo, winlostDate, transId, custId, betCheck) {
        let url = rootUrl + 'ColossusBetBetList/Detail';
        url = QueryString.setParam(url, 'refNo', refNo);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'transid', transId);
        url = QueryString.setParam(url, 'custid', custId);
        url = QueryString.setParam(url, 'betCheck', betCheck);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getGlobalGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'GlobalGamingBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getPPDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'PragmaticPlayBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getPPJackpotWonDetailUrl(rootUrl, baseCurrency, transId, custId, betTypeId) {
        let url = rootUrl + 'JackpotWonBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'bettype', betTypeId);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getSabaClubJackpotWonDetailUrl(rootUrl, baseCurrency, transId, custId, betTypeId) {
        let url = rootUrl + 'SabaClubJackpotWonBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'bettype', betTypeId);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getVideoPokerGameDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'CardClubBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getSGDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'SGBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getJokerDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'JokerBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getSexyGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'SexyGamingBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getSAGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'SAGamingBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getSBCDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'SabaClubBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getPGSDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'PGSoftBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getPGSDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'PGSoftBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getGDGDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'GDGBetList/Detail';
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'BaseCurrency', baseCurrency);

        return url;
    }

    getRngCasinoResultUrl(rootUrl) {
        return rootUrl + 'ResultReport/ShowRngCasinoSingleWalletResult';
    }

    getVoidbridgeResultUrl(rootUrl, custid, gamePlayID) {
        let url = rootUrl + 'ResultReport/VoidbridgeResult';
        url = QueryString.setParam(url, 'custid', custid);
        url = QueryString.setParam(url, 'gamePlayID', gamePlayID);

        return url;
    }

    getPPResultUrl(rootUrl, custid, gamePlayId) {
        let url = rootUrl + 'PPResultPopup/Index';
        url = QueryString.setParam(url, 'custid', custid);
        url = QueryString.setParam(url, 'gamePlayId', gamePlayId);

        return url;
    }

    getCardClubResultUrl(rootUrl, custid, gameTypeId, transId3rd) {
        let url = rootUrl + 'CardClubResultPopup/Index';
        url = QueryString.setParam(url, 'custid', custid);
        url = QueryString.setParam(url, 'gameTypeId', gameTypeId);
        url = QueryString.setParam(url, 'transId3rd', transId3rd);

        return url;
    }

    getSabaClubResultUrl(rootUrl, custid, gameId, siteId, gameRoundId, sportType) {
        let url = rootUrl + 'SabaClubResultPopup/Index';
        url = QueryString.setParam(url, 'custid', custid);
        url = QueryString.setParam(url, 'gameId', gameId);
        url = QueryString.setParam(url, 'siteId', siteId);
        url = QueryString.setParam(url, 'gameRoundId', gameRoundId);
        url = QueryString.setParam(url, 'sportType', sportType);

        return url;
    }

    getSGResultUrl(rootUrl, custid, gamePlayId) {
        let url = rootUrl + 'SGResultPopup/Index';
        url = QueryString.setParam(url, 'custid', custid);
        url = QueryString.setParam(url, 'gamePlayId', gamePlayId);

        return url;
    }

    getVRGResultUrl(rootUrl, gamePlayId, platform) {
        let url = rootUrl + 'VirtualGamesResultPopup/Index';
        console.log(gamePlayId)
        url = QueryString.setParam(url, 'gamePlayId', gamePlayId);
        url = QueryString.setParam(url, 'platform', platform);

        return url;
    }

    getAllBetResultUrl(rootUrl, transdesc, betcheck, bettype) {
        let url = rootUrl + 'ResultReport/AllBetResult';
        url = QueryString.setParam(url, 'betcheck', betcheck);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'transdesc', encodeURIComponent(transdesc));

        return url;
    }

    getSAGamingResultUrl(rootUrl, transdesc, matchCode, bettype) {
        let url = rootUrl + 'ResultReport/SAGaming';
        url = QueryString.setParam(url, 'matchCode', matchCode);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'transdesc', encodeURIComponent(transdesc));

        return url;
    }

    getJokerResultUrl(rootUrl, custId, gamePlayId, gameCode) {
        let url = rootUrl + 'ResultReport/Joker';
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'gamePlayId', gamePlayId);
        url = QueryString.setParam(url, 'gameCode', gameCode);

        return url;
    }

    getSexyGamingResultUrl(rootUrl, transdesc, matchCode, bettype) {
        let url = rootUrl + 'ResultReport/SexyGaming';
        url = QueryString.setParam(url, 'matchCode', matchCode);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'transdesc', encodeURIComponent(transdesc));

        return url;
    }

    getGlobalGamingResultUrl(rootUrl, custid, gameId) {
        let url = rootUrl + 'ResultReport/GlobalGamingResult';
        url = QueryString.setParam(url, 'custid', custid);
        url = QueryString.setParam(url, 'gameId', gameId);

        return url;
    }

    getPGSoftResultUrl(rootUrl, custid, gamePlayId, gameId3rd) {
        let url = rootUrl + 'PGSoftResultPopup/Index';
        url = QueryString.setParam(url, 'custid', custid);
        url = QueryString.setParam(url, 'gamePlayId', gamePlayId);
        url = QueryString.setParam(url, 'gameId3rd', gameId3rd);

        return url;
    }
    
    getHappy5LinkedGamesResultUrl(rootUrl, custId, transId3rd, platform) {
        let url = rootUrl + 'Happy5LinkedGamesResultPopup/Index';
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transId3rd', transId3rd);
        url = QueryString.setParam(url, 'platform', platform);

        return url;
    }

    getGDGResultUrl(rootUrl, custId, transId3rd, gameId3rd, siteId) {
        let url = rootUrl + 'ResultReport/GDG';
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transId3rd', transId3rd);
        url = QueryString.setParam(url, 'gameId3rd', gameId3rd);
        url = QueryString.setParam(url, 'siteId', siteId);

        return url;
    }
}

export default new BetList();