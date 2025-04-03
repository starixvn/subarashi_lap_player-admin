import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { DropdownMenu, DropdownItem } from '@jsroot/components/dropdown';
import { oddsGroupListWithStyle } from '../../lib/constants';
import GridInteractionService from '../../lib/services/gridInteractionService';
import LangHelper from '@jsroot/lib/langHelper';
import { ActionCreators } from '../../actions';
import { getErrorMessageForUpdating } from '../../lib/helpers/utils';
import { generateDefaultConfirmOption, generateDefaultAlertOption, predefinedIcons } from '@jsroot/lib/sweetAlert';

export const OddsGroupDropdown = (props) => {
    const { oddsGroupDropdownPosition, setOddsGroupDropdownPosition } = props;
    const isOpen = oddsGroupDropdownPosition !== null;

    const style = oddsGroupDropdownPosition && {
        top: oddsGroupDropdownPosition.y,
        left: oddsGroupDropdownPosition.x
    }

    const handleClose = () => {
        setOddsGroupDropdownPosition(null);
    }

    const { currentActivePlayer, updateOddsGroup, playerList, setPlayerList, toggleLoader, setSweetAlertOption, oddsGroups } = props;
    const handleCloseAlert = () => {
        setSweetAlertOption(null);
    }

    const cancelProps = {
        onClick: handleCloseAlert
    }

    const handleChangeOddsGroup = (event, value) => {
        event.preventDefault();
        const confirmMessage = LangHelper.getSingleResource('AlertSetOddsGroupForCustomerMsg').replace('{0}', value);

        const confirmProps = {
            onClick: () => {
                if (currentActivePlayer && value !== currentActivePlayer.oddsGroup) {
                    const { ibcCustId, licenseeCustId, oddsGroup } = currentActivePlayer;
                    const param = {
                        custId: ibcCustId,
                        oddsGroup: value,
                        oldValue: oddsGroup,
                        licenseeCustId
                    }

                    toggleLoader(true);
                    updateOddsGroup(param).then(response => {
                        const { Success, Message, ErrorMessage } = response.data;
                        if (Success) {
                            const newPlayerList = GridInteractionService.updatePlayerItem(playerList, ibcCustId, value, 'OddsGroup');
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
        <div className="dropdown oddsgroup-menu" style={style}>
            <DropdownMenu isOpen={isOpen} onClose={handleClose} className='odds-group-select'>
                {
                    oddsGroups.map(item => {
                        const style = oddsGroupListWithStyle[item];
                        return <DropdownItem key={item} className="oddsgroup-item" style={style}
                            onClick={e => handleChangeOddsGroup(e, item)}>{item}</DropdownItem>
                    })
                }
            </DropdownMenu>
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { setOddsGroupDropdownPosition, updateOddsGroup, setPlayerList, toggleLoader, setSweetAlertOption } = ActionCreators;
    return bindActionCreators({ setOddsGroupDropdownPosition, updateOddsGroup, setPlayerList, toggleLoader, setSweetAlertOption }, dispatch);
}

function mapStateToProps(state) {
    const { oddsGroupDropdownPosition, currentActivePlayer, playerList, depositSetting } = state;
    return Object.assign({}, { oddsGroups: depositSetting.OddsGroups, oddsGroupDropdownPosition, currentActivePlayer, playerList });
}

export default connect(mapStateToProps, mapDispatchToProps)(OddsGroupDropdown);