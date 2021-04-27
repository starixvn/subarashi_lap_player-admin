import * as loaderAction from '@jsroot/actions/loaderAction';
import * as requestParamAction from '@jsroot/actions/requestParamAction';
import * as alertAction from './alertAction';

export const ActionCreators = Object.assign({},
    loaderAction,
    requestParamAction,
    alertAction
);