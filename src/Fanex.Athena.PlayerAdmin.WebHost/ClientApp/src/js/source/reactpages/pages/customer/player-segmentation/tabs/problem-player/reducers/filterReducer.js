import * as types from '../actions/types';
import createReducer from '@jsroot/lib/createReducer.js';
import * as dateRangeReducer from '@jsroot/reducers/dateRangeReducer';
import * as currencyReducer from '@jsroot/reducers/currencyReducer';
import * as baseCurrencyReducer from '@jsroot/reducers/baseCurrencyReducer';
import * as usernameReducer from '@jsroot/reducers/usernameReducer';
import * as paramHandlerReducer from '../../../reducers/paramHandlerReducer';
import * as filterReducer from '../../../reducers/filterReducer';

export const isGetAll = createReducer({}, {
    [types.SET_IS_GET_ALL](state, action) {
        return action.data
    }
});

export const selectedReason = createReducer({}, {
    [types.SET_SELECTED_REASON](state, action) {
        return action.data
    }
});

export const selectedCustomerClass = createReducer({}, {
    [types.SET_SELECTED_CUSTOMERCLASS](state, action) {
        return action.data
    }
});

export const dateRange = dateRangeReducer.dateRange;
export const selectedCurrency = currencyReducer.selectedCurrency;
export const username = usernameReducer.username;
export const selectedBaseCurrency = baseCurrencyReducer.selectedBaseCurrency;
export const selectedSubsidiary = filterReducer.selectedSubsidiary;
export const subsidiarySearch = filterReducer.subsidiarySearch;
export const selectedParams = paramHandlerReducer.selectedParams;
