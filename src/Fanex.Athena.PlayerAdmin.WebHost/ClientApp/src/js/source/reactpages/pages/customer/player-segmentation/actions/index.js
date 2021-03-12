import * as tabHandlerActions from './tabHandlerAction';
import * as gridHandlerActions from './gridHandlerAction';
import * as playerDataAction from './playerDataAction';

export default {
    ...tabHandlerActions, 
    ...gridHandlerActions,
    ...playerDataAction
}