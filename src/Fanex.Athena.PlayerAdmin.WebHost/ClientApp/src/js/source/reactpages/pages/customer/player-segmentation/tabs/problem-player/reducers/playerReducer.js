import createReducer from '@jsroot/lib/createReducer.js'
import { SET_SELECTED_PLAYER } from '../actions/types';

export const selectedPlayer = createReducer(null, {
    [SET_SELECTED_PLAYER](prevState, action) {
        return action.data;
    }
})
