import * as loaderAction from '@jsroot/actions/loaderAction';
import * as filterAction from './filterAction';
import * as usernameAction from '@jsroot/actions/usernameAction';
import * as userGuideAction from '@jsroot/actions/userGuideAction';
import * as playerDataAction from '../../../actions/playerDataAction';
import * as gridHandlerAction  from '../../../actions/gridHandlerAction';

export const ActionCreators = Object.assign({},
    loaderAction,
    filterAction,
    usernameAction,
    playerDataAction,
    gridHandlerAction,
    userGuideAction
);