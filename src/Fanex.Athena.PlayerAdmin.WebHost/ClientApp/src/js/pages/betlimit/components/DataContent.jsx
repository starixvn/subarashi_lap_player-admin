import React from 'react';
import { connect } from 'react-redux';
import { BetLimitProduct } from '../lib/constants';
import SportDataContent from './grid/data-contents/SportDataContent';
import SBDataContent from './grid/data-contents/SBDataContent';
import GameDataContent from './grid/data-contents/GameDataContent';
import NGDataContent from './grid/data-contents/NGDataContent';
import ACGDataContent from './grid/data-contents/AGCDataContent';
import I4DDataContent from './grid/data-contents/I4DDataContent';
import VS3DataContent from './grid/data-contents/VS3DataContent';
import ThirdPartyDataContent from './grid/data-contents/ThirdPartyDataContent';
import ExchangeDataContent from './grid/data-contents/ExchangeDataContent';

const Setting = {
    [BetLimitProduct.SB]: SBDataContent,
    [BetLimitProduct.VS]: SportDataContent,
    [BetLimitProduct.VS2]: SportDataContent,
    [BetLimitProduct.LT]: GameDataContent,
    [BetLimitProduct.NG]: NGDataContent,
    [BetLimitProduct.KN]: GameDataContent,
    [BetLimitProduct.AGC]: ACGDataContent,
    [BetLimitProduct.TG]: GameDataContent,
    [BetLimitProduct.I4D]: I4DDataContent,
    [BetLimitProduct.SLT]: GameDataContent,
    [BetLimitProduct.CG]: GameDataContent,
    [BetLimitProduct.AB]: GameDataContent,
    [BetLimitProduct.VB]: GameDataContent,
    [BetLimitProduct.CO]: ThirdPartyDataContent,
    [BetLimitProduct.BX]: ExchangeDataContent,
    [BetLimitProduct.PP]: GameDataContent,
    [BetLimitProduct.SXG]: GameDataContent,
    [BetLimitProduct.HB]: GameDataContent,
    [BetLimitProduct.JK]: GameDataContent,
    [BetLimitProduct.SAG]: GameDataContent,
    [BetLimitProduct.PGS]: GameDataContent,
    [BetLimitProduct.SBC]: GameDataContent,
    [BetLimitProduct.WM]: GameDataContent,
    [BetLimitProduct.SG]: GameDataContent,
    [BetLimitProduct.VRG]: GameDataContent,
    [BetLimitProduct.CC]: GameDataContent,
    [BetLimitProduct.CS]: GameDataContent,
    [BetLimitProduct.VGA]: GameDataContent,
    [BetLimitProduct.AP]: GameDataContent,
    [BetLimitProduct.APM]: GameDataContent,
    [BetLimitProduct.LG]: VS3DataContent,
    [BetLimitProduct.BTC]: GameDataContent,
    [BetLimitProduct.FGGL]: GameDataContent,
    [BetLimitProduct.MXG]: GameDataContent,
    [BetLimitProduct.JIG]: GameDataContent,
    [BetLimitProduct.YB]: GameDataContent,
    [BetLimitProduct.MGRNG]: GameDataContent,
    [BetLimitProduct.SEA]: GameDataContent,
    [BetLimitProduct.WE]: GameDataContent,
    [BetLimitProduct.L22]: GameDataContent,
    [BetLimitProduct.UUSL]: GameDataContent,
    [BetLimitProduct.FS]: GameDataContent
};

const DataContent = (props) => {
    const { ProductId } = props.requestParam;
    const Content = !!Setting[ProductId] ? Setting[ProductId] : GameDataContent;

    return <Content />
}

function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps)(DataContent)