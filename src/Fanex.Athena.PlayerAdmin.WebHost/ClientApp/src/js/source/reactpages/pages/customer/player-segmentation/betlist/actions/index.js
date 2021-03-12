import * as loaderAction from '@jsroot/actions/loaderAction';
import * as gridDataAction from '@jsroot/actions/gridDataAction'
import { setSelectedParams } from '../../actions/paramHanlderAction';
import * as filterAction from './filterAction';

export const ActionCreators = Object.assign({},
    gridDataAction,
    filterAction,
    loaderAction,
    { setSelectedParams }
);