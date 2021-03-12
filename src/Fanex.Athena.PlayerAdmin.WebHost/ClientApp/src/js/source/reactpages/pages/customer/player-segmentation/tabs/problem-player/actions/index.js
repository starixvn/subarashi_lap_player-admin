import * as loaderAction from '@jsroot/actions/loaderAction';
import * as filterAction from './filterAction';
import * as userGuideAction from '@jsroot/actions/userGuideAction';
import * as playerDataAction from '../../../actions/playerDataAction';
import * as gridHandlerAction  from '../../../actions/gridHandlerAction';
import * as playerAction from './playerAction';

export const ActionCreators = Object.assign({},
    loaderAction,
    filterAction,
    userGuideAction,
    playerDataAction,
    gridHandlerAction,
    playerAction
);