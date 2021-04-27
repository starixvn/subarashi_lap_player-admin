import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { specialLevelListWithColor } from '../../lib/constants';
import DropdownLabel from '@jsroot/components/DropdownLabel';

import { ActionCreators } from '../../actions';
import GridInteractionService from '../../lib/services/gridInteractionService';
import { getSpecialLevelText } from '../../lib/helpers/utils';
import { buildCurrentActivePlayer } from '../../lib/helpers/playerDataBuilder';

export const SpecialLevelCell = (props) => {
    const { dataItem, specialLevelDropdownPosition, setCurrentActivePlayer, setSpecialLevelDropdownPosition, hasSpecialLevelPermission } = props;
    const value = dataItem.SpecialLevel;
    const style = { backgroundColor: specialLevelListWithColor[value] };

    const handleToggleSpecialLevel = (event) => {
        if(!hasSpecialLevelPermission){
            return;
        }

        const currentActivePlayer = buildCurrentActivePlayer(dataItem);
        setCurrentActivePlayer(currentActivePlayer);
        const dropdownName = 'specialLevelDropdown';
        const newPosition = GridInteractionService.getNextDropdownPosition(event.target, specialLevelDropdownPosition, dropdownName);
        setSpecialLevelDropdownPosition(newPosition);
    }

    return <DropdownLabel className={hasSpecialLevelPermission ? '' : 'disabled'} text={getSpecialLevelText(value)} labelStyle={style} onClick={handleToggleSpecialLevel} />
}

function mapDispatchToProps(dispatch) {
    const { setCurrentActivePlayer, setSpecialLevelDropdownPosition } = ActionCreators;
    return bindActionCreators({ setCurrentActivePlayer, setSpecialLevelDropdownPosition }, dispatch);
}

function mapStateToProps(state) {
    const { specialLevelDropdownPosition, hasSpecialLevelPermission } = state;
    return Object.assign({}, { specialLevelDropdownPosition, hasSpecialLevelPermission });
};

export default connect(mapStateToProps, mapDispatchToProps)(SpecialLevelCell);