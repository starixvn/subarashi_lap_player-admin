import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { playerStatus, playerStatusByName, licenseeType } from '../../lib/constants';
import { ActionCreators } from '../../actions';
import GridInteractionService from '../../lib/services/gridInteractionService';
import { buildCurrentActivePlayer } from '../../lib/helpers/playerDataBuilder';
import LangHelper from '@jsroot/lib/langHelper';

export const StatusCell = (props) => {
    const { dataItem, statusDropdownPosition, setCurrentActivePlayer, setStatusDropdownPosition, type } = props;
    const { Status, IsOnline, IsDeposit, IsArchived } = dataItem;

    if (Status === undefined) {
        return null;
    }

    const statusName = playerStatus[Status];
    const classname = 'status-text status-' + Status;
    const canUpdate = type === licenseeType.nonApi &&  !IsArchived && Status !== playerStatusByName.Closed;

    const handleToggleStatus = (event) => {
        const currentActivePlayer = buildCurrentActivePlayer(dataItem);
        setCurrentActivePlayer(currentActivePlayer);
        const dropdownName = 'statusDropdown';
        const newPosition = GridInteractionService.getNextDropdownPosition(event.target, statusDropdownPosition, dropdownName);
        setStatusDropdownPosition(newPosition);
    }

    const shouldShowOnlineStatus = IsDeposit && Status === playerStatusByName.Active;
    const onlineText = LangHelper.getSingleResource('Online');
    const offlineText = LangHelper.getSingleResource('Offline');

    return (
        <div className="status-cell">
            <p className={classname}>{statusName || LangHelper.getSingleResource('Unknown')}</p>
            {shouldShowOnlineStatus && <p className="status-text">{IsOnline ? `(${onlineText})` : `(${offlineText})`}</p>}
            {statusName && canUpdate && <button type="button" className="btn-outline btn-status-menu" onClick={handleToggleStatus}><i className="site-caret"></i></button>}
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { setCurrentActivePlayer, setStatusDropdownPosition } = ActionCreators;
    return bindActionCreators({ setCurrentActivePlayer, setStatusDropdownPosition }, dispatch);
}

function mapStateToProps(state) {
    const { statusDropdownPosition, type } = state;
    return Object.assign({}, { statusDropdownPosition, type });
};

export default connect(mapStateToProps, mapDispatchToProps)(StatusCell);