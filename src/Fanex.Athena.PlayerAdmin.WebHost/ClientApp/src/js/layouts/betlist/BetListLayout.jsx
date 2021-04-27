import React, { Component } from 'react';
import DataService from '@jsroot/lib/DataService';
import BetList from './lib/BetList';
import Popup from '@jsroot/components/popup';
import PropTypes from 'prop-types';
import { SportType } from './lib/constants';

class BetListLayout extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isShowPopup: false,
            popupUrl: null,
            popupTitle: null
        };
    }

    componentDidMount() {
        window.onload = () => {
            window.logOpenBetlistEvent && window.logOpenBetlistEvent(this.props.pageName);
        };

        window.ViewResult = (matchid, raceNo, bettype, sporttype, refno,
            username, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid, transdecs) => {
            const title = this._getResource('Result');
            const defaultPlatform = 'd';
            const rootUrl = this.props.rootUrl;

            if (sporttype === SportType.RNG_GAME || sporttype === SportType.RNG_MINIGAME || sporttype === SportType.RNG_MOBILE) {
                const url = BetList.getRngCasinoResultUrl(rootUrl)
                this._showRngCasinoResult(refno, url, title);
            }
            else if (sporttype === SportType.VOIDBRIDGE) {
                const url = BetList.getVoidbridgeResultUrl(rootUrl, custid, extension2);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.PP) {
                const url = BetList.getPPResultUrl(rootUrl, custid, extension2);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.CARDCLUB) {
                const url = BetList.getCardClubResultUrl(rootUrl, custid, bettype, extension2);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.SBC || sporttype === SportType.SBC_FreeBets) {
                const url = BetList.getSabaClubResultUrl(rootUrl, custid, bettype, extension2, transdecs, sporttype);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.SG) {
                const url = BetList.getSGResultUrl(rootUrl, custid, extension2);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.VRG) {
                const url = BetList.getVRGResultUrl(rootUrl, refno, defaultPlatform);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.ALLBET) {
                const url = BetList.getAllBetResultUrl(rootUrl, transdecs, extension2, bettype);
                this._openPopup(url, title);
            }
            else if (sporttype === SportType.SAG) {
                const url = BetList.getSAGamingResultUrl(rootUrl, transdecs, extension2, bettype);
                this._openPopup(url, title);
            }
            else if (sporttype === SportType.GLOBALGAMING) {
                const url = BetList.getGlobalGamingResultUrl(rootUrl, custid, extension2);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.JK) {
                const url = BetList.getJokerResultUrl(rootUrl, custid, transdecs, extension2);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.SXG) {
                const url = BetList.getSexyGamingResultUrl(rootUrl, transdecs, extension2, bettype);
                this._openPopup(url, title);
            }
            else if (sporttype === SportType.PGS) {
                const url = BetList.getPGSoftResultUrl(rootUrl, custid, transdecs, extension2);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.Happy5LinkedGames) {
                const url = BetList.getHappy5LinkedGamesResultUrl(rootUrl, extension1, refno, defaultPlatform);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.GDG) {
                const url = BetList.getGDGResultUrl(rootUrl, custid, transdecs, extension2, league);
                this._showThirdPartyResult(url, title);
            }
            else if (sporttype === SportType.SLT) {
                const url = BetList.getSportsLotteryResultUrl(rootUrl, custid, transdecs, defaultPlatform);
                this._showThirdPartyResult(url, title);
            }
            else {
                const url = BetList.getResultUrl(rootUrl, matchid, raceNo, bettype,
                    sporttype, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid);
                this._openPopup(url, title);
            }
        };

        window.showMP = (transId) => {
            let divEvent = document.querySelector('#divEvent_' + transId);
            divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket
            this._showMixParlayDetail(transId, divEvent);
        };

        window.showCombMP = (transId, index) => {
            let divEvent = document.querySelector('#divEvent_' + transId + '' + index);
            divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket
            this._showMixParlayDetail(transId, divEvent);
        };

        // expand choice content
        window.showNewCombMP = (...args) => {
            this._showNewParlayDetail.apply(this, [this.props.rootUrl, ...args]);
        };

        // expand choice content
        window.showSingleMatchParlayDetail = (...args) => {
            this._showSingleMatchParlayDetail.apply(this, [this.props.rootUrl, ...args]);
        };

        // show detail popup (in choice column)
        window.showCombMPDetail = (transId, refNo, betId, custId, betTeam) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getParlayDetailUrl(rootUrl, transId, refNo, betId, custId, betTeam, baseCurrency);
            this._openPopup(url, betTeam);
        };

        window.ViewCashOutHistory = (transId, refNo, betType, betdaqId) => {
            const { rootUrl, baseCurrency } = this.props;

            const { url, isCashOut } = BetList.getCashOutHistoryPopupSetting(rootUrl, baseCurrency, transId, refNo, betType, betdaqId);
            const title = this._getResource(isCashOut ? 'CashOutDetails' : 'BoughtDetails');
            this._openPopup(url, title);
        }

        window.ShowBonusDetails = (refCode, transId) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getBonusDetailUrl(rootUrl, refCode, transId, baseCurrency);

            const title = this._getResource('TransactionDetail');
            this._openPopup(url, title);
        }

        window.showAGCasinoDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getAGCasinoDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('AGCasino') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showRnGCasinoDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getRnGCasinoDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductCS') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showVoidBridgeDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getVoidBridgeDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductVB') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showTodayRunningBetsDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getAllBetDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductAB') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showColossusBetsDetails = (refNo, winlostDate, transId, custId, betCheck) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getColossusBetsDetailUrl(rootUrl, baseCurrency, refNo, winlostDate, transId, custId, betCheck);
            const title = this._getResource('TicketDetailOf').replace('{0}', refNo);

            this._openPopup(url, title);
        }

        window.showGlobalGaming = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getGlobalGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductFW') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showPPDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getPPDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductPP') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showPPJackpotWonDetails = (transId, custId, betTypeId) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getPPJackpotWonDetailUrl(rootUrl, baseCurrency, transId, custId, betTypeId);
            const title = this._getResource('ProductPP') + ' - ' +
                this._getResource('JackpotWonDetails');

            this._openPopup(url, title);
        }

        window.showSabaClubJackpotWonDetails = (transId, custId, betTypeId) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getSabaClubJackpotWonDetailUrl(rootUrl, baseCurrency, transId, custId, betTypeId);
            const title = this._getResource('ProductSBC') + ' - ' +
                this._getResource('JackpotWonDetails');

            this._openPopup(url, title);
        }

        window.showVideoPokerGameDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getVideoPokerGameDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductCC') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showSGDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getSGDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductSG') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showJokerBetDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getJokerDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductJK') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showSexyGamingBetDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getSexyGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductSXG') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showSAGamingDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getSAGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductSAG') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showSabaClubDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getSBCDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductSBC') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showPGSoftDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getPGSDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductPGS') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }

        window.showGDGBetDetails = (transId, winlostDate, custId, transType) => {
            const { rootUrl, baseCurrency } = this.props;

            const url = BetList.getGDGDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);
            const title = this._getResource('ProductGDG') + ' - ' +
                this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

            this._openPopup(url, title);
        }
    }

    componentWillUnmount() {
        window.showMP = undefined;
        window.showCombMP = undefined;
        window.ViewResult = undefined;
        window.showNewCombMP = undefined;
        window.showCombMPDetail = undefined;
        window.ViewCashOutHistory = undefined;
        window.ShowBonusDetails = undefined;
        window.showAGCasinoDetails = undefined;
        window.showRnGCasinoDetails = undefined;
        window.showVoidBridgeDetails = undefined;
        window.showTodayRunningBetsDetails = undefined;
        window.showColossusBetsDetails = undefined;
        window.showGlobalGaming = undefined;
        window.showPPDetails = undefined;
        window.showPPJackpotWonDetails = undefined;
        window.showSabaClubJackpotWonDetails = undefined;
        window.showSingleMatchParlayDetail = undefined;
        window.showSGDetails = undefined;
        window.showJokerBetDetails = undefined;
        window.showSexyGamingBetDetails = undefined;
        window.showSAGamingDetails = undefined;
        window.showSabaClubDetails = undefined;
        window.showPGSoftDetails = undefined;
        window.showGDGBetDetails = undefined;
    }

    _getResource(key) {
        return this.props.getResourceFunc ? this.props.getResourceFunc(key) : key;
    }

    _formatDateString(dateString) {
        return this.props.formatDateStringFunc ? this.props.formatDateStringFunc(dateString) : dateString;
    }

    _beforeShow() {
        this.props.beforeShowFunc && this.props.beforeShowFunc();
    }

    _afterShow() {
        this.props.afterShowFunc && this.props.afterShowFunc();
    }

    _resolveClientUrl(relativeUrl) {
        return this.props.rootUrl + relativeUrl;
    }

    _showMixParlayDetail(transId, divEvent) {
        if (divEvent.style.display === 'none') {
            const url = this._resolveClientUrl('api/MixParlayBetList/Detail?transId=' + transId);
            this._beforeShow();// this.props.toggleLoader(true);

            DataService.getData(url)
                .then((result) => {
                    let choice = '';
                    const length = result.data.length;
                    for (let i = 0; i < length; i++) {
                        choice += result.data[i];
                        if (i < length - 1) {
                            choice += '<div class="line"></div>';
                        }
                    }

                    divEvent.innerHTML = choice;
                    divEvent.style.display = 'block';

                    this._afterShow();
                }).catch((error) => {
                    this._afterShow();
                    console.log(error);
                });
        }
        else {
            divEvent.style.display = 'none';
            divEvent.innerHTML = '';
        }
    }

    _showNewParlayDetail(rootUrl, transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam, custId) {
        let divEvent = document.querySelector('#divEvent_' + transId + '' + transIndex);
        let nameDiv = document.querySelector('#combinationOfferName_' + transId + transIndex);

        divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket

        if (divEvent.style.display == 'none') {
            let url = BetList.getChoiceParlayDetailUrl(rootUrl, transId, refNo, handicap1, handicap2, betcheck, betteam, custId);
            this._beforeShow();//this.props.toggleLoader(true);
            DataService.getData(url)
                .then((result) => {
                    let choice = BetList.buildChoiceParlayDetail(result.data);

                    divEvent.innerHTML = choice;
                    divEvent.style.display = 'block';
                    nameDiv.style.display = 'none';

                    this._afterShow();
                }).catch((error) => {
                    this._afterShow();
                    console.log(error);
                });
        }
        else {
            divEvent.style.display = 'none';
            divEvent.innerHTML = '';
            nameDiv.style.display = 'block';
        }
    }

    _showThirdPartyResult(url, title) {
        this._beforeShow();//this.props.toggleLoader(true);
        DataService.getData(url)
            .then((result) => {
                if (result.data !== '') {
                    this._openPopup(result.data, title);
                }

                this._afterShow();
            }).catch((error) => {
                this._afterShow();
                console.log(error);
            });
    }

    _showRngCasinoResult(refno, url, title) {
        this._beforeShow();
        DataService.postData(url, { refno })
            .then((result) => {
                if (result.data.Success) {
                    if (result.data.Result === '') {
                        return;
                    }

                    this._openPopup(result.data.Result, title);
                }
                else {
                    alert(this._getResource("ErrorProcessingMsg"));
                }

                this._afterShow();
            }).catch((error) => {
                this._afterShow();
                console.log(error);
            });
    }

    _showSingleMatchParlayDetail(rootUrl, transId, custId, status, transIndex) {
        const divEvent = document.querySelector('#divEvent_' + transId + '' + transIndex);

        if (divEvent.style.display == 'none') {
            const url = BetList.getSingleMatchParlayDetailUrl(rootUrl, transId, custId, status);
            this._beforeShow();
            DataService.getData(url)
                .then((result) => {
                    const selection = BetList.buildSingleMatchParlayDetail(result.data);

                    divEvent.innerHTML = selection;
                    divEvent.style.display = 'block';

                    this._afterShow();
                }).catch(function (error) {
                    console.log(error);
                });
        }
        else {
            divEvent.style.display = 'none';
            divEvent.innerHTML = '';
        }
    }

    _openPopup(url, title) {
        this.setState({ isShowPopup: true, popupUrl: url, popupTitle: title });
    }

    _closePopup() {
        this.setState({ isShowPopup: false });
    }

    render() {
        return (
            <div className="betlist-layout">
                {this.props.children}
                {this.state.isShowPopup &&
                    <Popup isShow={true}
                        iframeUrl={this.state.popupUrl}
                        title={this.state.popupTitle}
                        width={900} height={600}
                        onClose={() => this._closePopup()}
                    >
                    </Popup>}
            </div>
        );
    }
}

BetListLayout.propTypes = {
    pageName: PropTypes.string.isRequired,
    rootUrl: PropTypes.string.isRequired,
    getResourceFunc: PropTypes.func,
    baseCurrency: PropTypes.string,
    beforeShowFunc: PropTypes.func,
    afterShowFunc: PropTypes.func,
    beforeShowFunc: PropTypes.func,
    afterShowFunc: PropTypes.func,
    formatDateStringFunc: PropTypes.func
}

BetListLayout.defaultProps = {
    rootUrl: '/',
    baseCurrency: ''
}

export default BetListLayout