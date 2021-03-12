import React, { useState, useEffect, useRef } from 'react';
import { connect } from 'react-redux';

import ReportTitle from '@jsroot/components/ReportTitle';
import { getFormattedDate } from '../lib/utils';
import AppData from '@jsroot/lib/AppData';
import LangHelper from '@jsroot/lib/langHelper';
import NumberFormat from '@jsroot/lib/numberFormat';

const buildCurrencytext = (selectedCurrency) => {
    const { currencyList } = AppData;
    const selectedCurrencyList = selectedCurrency.split(',');
    const isSelectAllCurrency = currencyList.map(x => x.Key).length === selectedCurrencyList.length;

    if (!isSelectAllCurrency) {
        const selectedCurrencyText = currencyList.reduce((result, currentItem) => {
            if (selectedCurrencyList.findIndex(x => Number(x) === currentItem.Key) !== -1) {
                result.push(currentItem.Value);
            }

            return result;
        }, []).join(', ');

        const resourceKey = selectedCurrencyList.length > 1 ? 'Currencies' : 'Currency';

        return `${LangHelper.getSingleResource(resourceKey)}: ` + selectedCurrencyText;
    }

    return LangHelper.getSingleResource('AllCurrencies');
}

export const GridCaption = (props) => {
    const isFirstRun = useRef(true);
    const [content, setContent] = useState('');

    useEffect(() => {
        if (isFirstRun.current) {
            isFirstRun.current = false;
            return;
        }

        const { requestParam, pagingOption } = props;
        const { date, currencyIdList } = requestParam;

        const dateString = getFormattedDate(date);
        const currencyText = buildCurrencytext(currencyIdList);
        const totalTicketText = `${LangHelper.getSingleResource('TotalTickets')}: ${NumberFormat.format(pagingOption.TotalItem)}`;

        const content = `${LangHelper.getSingleResource('DailyBets')}, GMT+8, ${dateString} - ${currencyText} - ${totalTicketText}`;
        setContent(content);
    }, [props.requestParam, props.pagingOption]);

    if(!content){
        return null;
    }

    return (<ReportTitle content={content} />)
}

function mapStateToProps(state) {
    const { requestParam, pagingOption } = state;
    return Object.assign({}, { requestParam, pagingOption });
};

export default connect(mapStateToProps)(GridCaption);