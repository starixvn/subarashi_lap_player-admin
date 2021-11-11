import React from 'react';
import { connect } from 'react-redux';
import { BetLimitProduct } from '../lib/constants';
import SportDataContent from './grid/data-contents/SportDataContent';
import GameDataContent from './grid/data-contents/GameDataContent';
import NGDataContent from './grid/data-contents/NGDataContent';
import ACGDataContent from './grid/data-contents/AGCDataContent';
import I4DDataContent from './grid/data-contents/I4DDataContent';
import RNGCDataContent from './grid/data-contents/RNGCDataContent';
import ThirdPartyDataContent from './grid/data-contents/ThirdPartyDataContent';

const Setting = {
    [BetLimitProduct.SB]: SportDataContent,
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
    [BetLimitProduct.BX]: GameDataContent,
    [BetLimitProduct.PP]: GameDataContent,
    [BetLimitProduct.SXG]: GameDataContent,
    [BetLimitProduct.GDG]: GameDataContent,
    [BetLimitProduct.HB]: GameDataContent,
    [BetLimitProduct.JK]: GameDataContent,
    [BetLimitProduct.SAG]: GameDataContent,
    [BetLimitProduct.PGS]: GameDataContent,
    [BetLimitProduct.SBC]: GameDataContent,
    [BetLimitProduct.WM]: GameDataContent,
    [BetLimitProduct.SG]: GameDataContent,
    [BetLimitProduct.VRG]: GameDataContent,
    [BetLimitProduct.CC]: GameDataContent,
    [BetLimitProduct.CS]: RNGCDataContent,
    [BetLimitProduct.CB]: ThirdPartyDataContent,
    [BetLimitProduct.VGA]: GameDataContent,
    [BetLimitProduct.AP]: GameDataContent,
    [BetLimitProduct.APM]: GameDataContent
};

const DataContent = (props) => {
    const { ProductId } = props.requestParam;
    const Content = Setting[ProductId];

    return <Content />
}

function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps)(DataContent)