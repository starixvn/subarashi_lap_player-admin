import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { oddsGroupListWithStyle } from '../../lib/constants';
import DropdownLabel from '@jsroot/components/DropdownLabel';
import { ActionCreators } from '../../actions';
import GridInteractionService from '../../lib/services/gridInteractionService';
import { buildCurrentActivePlayer } from '../../lib/helpers/playerDataBuilder';

export const OddsGroupCell = (props) => {
    const { dataItem, oddsGroupDropdownPosition, setCurrentActivePlayer, setOddsGroupDropdownPosition, hasOddsGroupPermission } = props;

    const value = dataItem.OddsGroup;
    const style = oddsGroupListWithStyle[value];
    const canUpdate = hasOddsGroupPermission && !dataItem.BaStatus;

    const handleToggleOddsGroup = (event) => {
        if(!canUpdate){
            return;
        }

        const currentActivePlayer = buildCurrentActivePlayer(dataItem);
        setCurrentActivePlayer(currentActivePlayer);
        const dropdownName = 'oddsGroupDropdown';
        const newPosition = GridInteractionService.getNextDropdownPosition(event.target, oddsGroupDropdownPosition, dropdownName);
        setOddsGroupDropdownPosition(newPosition);
    }

    return <DropdownLabel className={canUpdate ? '' : 'disabled'} text={value} labelStyle={style} onClick={handleToggleOddsGroup} />
}

function mapDispatchToProps(dispatch) {
    const { setCurrentActivePlayer, setOddsGroupDropdownPosition } = ActionCreators;
    return bindActionCreators({ setCurrentActivePlayer, setOddsGroupDropdownPosition }, dispatch);
}

function mapStateToProps(state) {
    const { oddsGroupDropdownPosition, hasOddsGroupPermission } = state;
    return Object.assign({}, { oddsGroupDropdownPosition, hasOddsGroupPermission });
};

export default connect(mapStateToProps, mapDispatchToProps)(OddsGroupCell);