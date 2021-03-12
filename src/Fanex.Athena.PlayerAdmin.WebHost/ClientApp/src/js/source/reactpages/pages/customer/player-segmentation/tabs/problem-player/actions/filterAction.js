import * as types from './types';
import * as dateRangeAction from '@jsroot/actions/dateRangeAction';
import * as currencyAction from '@jsroot/actions/currencyAction';
import * as baseCurrencyAction from '@jsroot/actions/baseCurrencyAction'
import * as usernameAction from '@jsroot/actions/usernameAction'
import * as paramHanlderAction from '../../../actions/paramHanlderAction';
import * as filterAction from '../../../actions/filterAction';

export const changeIsGetAll = isGetAll => {
    return {
        type: types.SET_IS_GET_ALL,
        data: isGetAll
    }
}

export const setReason = selectedReasons => {
    return {
        type: types.SET_SELECTED_REASON,
        data: selectedReasons
    }
}

export const setSelectedCustomerClass = selectedCustomerClass => {
    return {
        type: types.SET_SELECTED_CUSTOMERCLASS,
        data: selectedCustomerClass
    }
}

export const setDateRange = dateRangeAction.setDateRange;
export const setSelectedCurrency =  currencyAction.setSelectedCurrency;
export const setUsername =  usernameAction.setUsername;
export const setSelectedBaseCurrency = baseCurrencyAction.setSelectedBaseCurrency;
export const setSubsidiaryList = filterAction.setSubsidiaryList;
export const setSubsidiarySearch = filterAction.setSubsidiarySearch;
export const setSelectedParams =  paramHanlderAction.setSelectedParams;
