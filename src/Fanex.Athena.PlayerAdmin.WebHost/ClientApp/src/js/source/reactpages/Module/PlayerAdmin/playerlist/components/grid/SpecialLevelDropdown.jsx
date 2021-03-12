import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { DropdownMenu, DropdownItem } from '@jsroot/components/dropdown';
import { specialLevelListWithColor } from '../../lib/constants';
import GridInteractionService from '../../lib/services/gridInteractionService';
import LangHelper from '@jsroot/lib/langHelper';
import { ActionCreators } from '../../actions';
import { getSpecialLevelText, getErrorMessageForUpdating } from '../../lib/helpers/utils';
import { generateDefaultConfirmOption, generateDefaultAlertOption, predefinedIcons } from '../../../../../lib/sweetAlert';

const SpecialLevelDropdown = (props) => {
    const { specialLevelDropdownPosition, setSpecialLevelDropdownPosition } = props;
    const isOpen = specialLevelDropdownPosition !== null;
    const style = specialLevelDropdownPosition && {
        top: specialLevelDropdownPosition.y,
        left: specialLevelDropdownPosition.x
    }

    const handleClose = () => {
        setSpecialLevelDropdownPosition(null);
    }

    const { currentActivePlayer, updateSpecialLevel, playerList, setPlayerList, toggleLoader, setSweetAlertOption } = props;
    const handleCloseAlert = () => {
        setSweetAlertOption(null);
    }

    const cancelProps = {
        onClick: handleCloseAlert
    }

    const handleChangeOddsGroup = (event, value) => {
        event.preventDefault();
        const resourceName = Number(value) === 0 ? 'AlertRemoveSpecialLevelForCustomerMsg' : 'AlertSetSpecialLevelForCustomerMsg';
        const confirmMessage = LangHelper.getSingleResource(resourceName);

        const confirmProps = {
            onClick: () => {
                if (currentActivePlayer && value !== currentActivePlayer.specialLevel) {
                    const { ibcCustId, licenseeCustId, specialLevel } = currentActivePlayer;
                    const param = {
                        custId: ibcCustId,
                        specialLevel: value,
                        oldValue: specialLevel,
                        licenseeCustId
                    }

                    toggleLoader(true);
                    updateSpecialLevel(param).then(response => {
                        const { Success, Message, ErrorMessage } = response.data;
                        if (Success) {
                            const newPlayerList = GridInteractionService.updatePlayerItem(playerList, ibcCustId, value, 'SpecialLevel');
                            setPlayerList(newPlayerList);
                        }

                        const resultMessage = Success ? LangHelper.getSingleResource('UpdateSuccess') : (Message || LangHelper.getSingleResource(ErrorMessage));
                        const displayIcon = Success ? predefinedIcons.success : predefinedIcons.error;
                        const alertOpt = generateDefaultAlertOption(resultMessage, handleCloseAlert, null, displayIcon);
                        setSweetAlertOption(alertOpt);

                        toggleLoader(false);
                    }).catch((error) => {
                        console.log(error.message);
                        const message = getErrorMessageForUpdating(error);
                        const alertOpt = generateDefaultAlertOption(message, handleCloseAlert, null, predefinedIcons.error);
                        setSweetAlertOption(alertOpt);
                        toggleLoader(false);
                    })
                }
                else {
                    handleCloseAlert();
                }

                handleClose();
            }
        }

        const confirmOption = generateDefaultConfirmOption(confirmMessage, handleCloseAlert, confirmProps, cancelProps);
        setSweetAlertOption(confirmOption);
    }

    return (
        <div className="dropdown special-level-menu" style={style}>
            <DropdownMenu isOpen={isOpen} onClose={handleClose}>
                {
                    Object.keys(specialLevelListWithColor).map(item => {
                        const color = specialLevelListWithColor[item];
                        return <DropdownItem key={item} className="special-level-item"
                            style={{ backgroundColor: color }} onClick={e => handleChangeOddsGroup(e, item)}>{getSpecialLevelText(item)}</DropdownItem>
                    })
                }
            </DropdownMenu>
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { setSpecialLevelDropdownPosition, updateSpecialLevel, setPlayerList, toggleLoader, setSweetAlertOption } = ActionCreators;
    return bindActionCreators({ setSpecialLevelDropdownPosition, updateSpecialLevel, setPlayerList, toggleLoader, setSweetAlertOption }, dispatch);
}

function mapStateToProps(state) {
    const { specialLevelDropdownPosition, currentActivePlayer, playerList } = state;
    return Object.assign({}, { specialLevelDropdownPosition, currentActivePlayer, playerList });
};

export default connect(mapStateToProps, mapDispatchToProps)(SpecialLevelDropdown);