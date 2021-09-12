import * as loaderAction from '@jsroot/actions/loaderAction';
import * as filterHandlerAction from './filterHandlerAction';
import * as playerDetailHandlerAction from './playerDetailHandlerAction';
import * as commonAction from './commonAction';
import * as gridHandlerAction from './gridHandlerAction';
import * as gridBaStatusAction from './gridBaStatusAction';
import * as gridOddsGroupAction from './gridOddsGroupAction';
import * as gridSpecialLevelAction from './gridSpecialLevelAction';
import * as gridPlayerStatusAction from './gridPlayerStatusAction';
import * as gridUserNameAction from './gridUserNameAction';
import * as settingAction from './settingAction';

export const ActionCreators = Object.assign({},
    loaderAction,
    commonAction,
    filterHandlerAction,
    playerDetailHandlerAction,
    gridHandlerAction,
    gridBaStatusAction,
    gridOddsGroupAction,
    gridSpecialLevelAction,
    gridPlayerStatusAction,
    gridUserNameAction,
    settingAction
);
