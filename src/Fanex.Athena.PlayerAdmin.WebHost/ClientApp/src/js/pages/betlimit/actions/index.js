import * as loaderAction from '@jsroot/actions/loaderAction';
import * as requestParamAction from '@jsroot/actions/requestParamAction';
import * as alertAction from './alertAction';
import * as productAction from './productAction';

export const ActionCreators = Object.assign({},
    loaderAction,
    requestParamAction,
    alertAction,
    productAction
);