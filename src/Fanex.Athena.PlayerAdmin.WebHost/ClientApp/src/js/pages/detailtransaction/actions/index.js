import * as requestParamAction from '@jsroot/actions/requestParamAction';
import * as dateRangeAction from '@jsroot/actions/dateRangeAction';
import * as loaderAction from '@jsroot/actions/loaderAction';
import * as filterActions from './filter-actions';

export const ActionCreators = Object.assign({},
    requestParamAction,
    dateRangeAction,
    loaderAction,
    filterActions
);
