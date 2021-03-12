import * as requestParamAction from '@jsroot/actions/requestParamAction';
import * as reportTitleAction from '@jsroot/actions/reportTitleAction';
import * as dataAction from './dataAction';

export const ActionCreators = Object.assign({},
    requestParamAction,
    reportTitleAction,
    dataAction
);