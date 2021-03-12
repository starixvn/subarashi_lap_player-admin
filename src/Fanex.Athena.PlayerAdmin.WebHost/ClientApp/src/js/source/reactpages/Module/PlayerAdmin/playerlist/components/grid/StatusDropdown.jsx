import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { DropdownMenu, DropdownItem } from '@jsroot/components/dropdown';
import { playerStatus, displayedPlayerStatusList, messageUpdateState } from '../../lib/constants';
import GridInteractionService from '../../lib/services/gridInteractionService';
import { ActionCreators } from '../../actions';
import LangHelper from '@jsroot/lib/langHelper';
import { getErrorMessageForUpdating } from '../../lib/helpers/utils';
import { generateDefaultConfirmOption, generateDefaultAlertOption, predefinedIcons } from '../../../../../lib/sweetAlert';

const StatusDropdown = (props) => {
    const { statusDropdownPosition, setStatusDropdownPosition } = props;
    const isOpen = statusDropdownPosition !== null;
    const style = statusDropdownPosition && {
        top: statusDropdownPosition.y,
        left: statusDropdownPosition.x
    }

    const handleClose = () => {
        setStatusDropdownPosition(null);
    }

    const { currentActivePlayer, playerList, setPlayerList, toggleLoader, updatePlayerStatus, setSweetAlertOption } = props;
    const handleCloseAlert = () => {
        setSweetAlertOption(null);
    }

    const cancelProps = {
        onClick: handleCloseAlert
    }

    const handleChangeOddsGroup = (event, value) => {
        event.preventDefault();
        const messageList = messageUpdateState[value] || [];
        const confirmMessage = messageList.length ? (<p>{messageList[0]}<br /><span className="warning">{messageList[1]}</span></p>) : null;

        const confirmProps = {
            onClick: () => {
                if (currentActivePlayer) {
                    const { licenseeCustId } = currentActivePlayer;
                    const param = {
                        custId: licenseeCustId,
                        status: value
                    }

                    toggleLoader(true);
                    updatePlayerStatus(param).then(response => {
                        const { IsSuccessful } = response.data;
                        if (IsSuccessful) {
                            const { ibcCustId } = currentActivePlayer;
                            const playerId = ibcCustId || licenseeCustId;
                            const newPlayerList = GridInteractionService.updatePlayerItem(playerList, playerId, value, 'Status');
                            setPlayerList(newPlayerList);
                        }

                        const resultMessage = IsSuccessful ? LangHelper.getSingleResource('UpdateSuccess') : LangHelper.getSingleResource('PlayerAdminUpdateFailedMsg');
                        const displayIcon = IsSuccessful ? predefinedIcons.success : predefinedIcons.error;
                        const alertOpt = generateDefaultAlertOption(resultMessage, handleCloseAlert, null, displayIcon);
                        setSweetAlertOption(alertOpt);

                        toggleLoader(false);
                    }).catch(error => {
                        console.log(error.message);
                        const message = getErrorMessageForUpdating(error);;
                        const alertOpt = generateDefaultAlertOption(message, handleCloseAlert, null, predefinedIcons.error);
                        setSweetAlertOption(alertOpt);
                        toggleLoader(false);
                    })
                }

                handleClose();
            }
        }

        const customAlertOption = { width: 480, height: 220 };
        const confirmOption = generateDefaultConfirmOption(confirmMessage, handleCloseAlert, confirmProps, cancelProps, predefinedIcons.warning, customAlertOption);
        setSweetAlertOption(confirmOption);
    }

    const displayedStatusList = currentActivePlayer && displayedPlayerStatusList[currentActivePlayer.status];

    if (!displayedStatusList) {
        return null;
    }

    return (
        <div className="dropdown status-menu" style={style}>
            <DropdownMenu isOpen={isOpen} onClose={handleClose}>
                {
                    displayedStatusList.map(item => {
                        const statusName = playerStatus[item];
                        const classname = 'status-item ' + statusName;
                        return <DropdownItem key={item} className={classname} onClick={e => handleChangeOddsGroup(e, item)}>{statusName}</DropdownItem>
                    })
                }
            </DropdownMenu>
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { setStatusDropdownPosition, setPlayerList, toggleLoader, updatePlayerStatus, setSweetAlertOption } = ActionCreators;
    return bindActionCreators({ setStatusDropdownPosition, setPlayerList, toggleLoader, updatePlayerStatus, setSweetAlertOption }, dispatch);
}

function mapStateToProps(state) {
    const { statusDropdownPosition, currentActivePlayer, playerList } = state;
    return Object.assign({}, { statusDropdownPosition, currentActivePlayer, playerList });
};

export default connect(mapStateToProps, mapDispatchToProps)(StatusDropdown);