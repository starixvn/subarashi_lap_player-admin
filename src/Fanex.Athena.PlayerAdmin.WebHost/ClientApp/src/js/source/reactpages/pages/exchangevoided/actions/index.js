import * as requestParamAction from '@jsroot/actions/requestParamAction';
import * as dataAction from './dataAction';

export const ActionCreators = Object.assign({},
    requestParamAction,
    dataAction
);