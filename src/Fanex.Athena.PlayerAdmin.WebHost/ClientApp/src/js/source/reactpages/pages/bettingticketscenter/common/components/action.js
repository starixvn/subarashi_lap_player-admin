import * as loaderAction from '@jsroot/actions/loaderAction'
import * as gridDataAction from '@jsroot/actions/gridDataAction'
import * as requestParamAction from '@jsroot/actions/requestParamAction'

export const ActionCreators = Object.assign({},
    loaderAction,
    gridDataAction,
    requestParamAction
);