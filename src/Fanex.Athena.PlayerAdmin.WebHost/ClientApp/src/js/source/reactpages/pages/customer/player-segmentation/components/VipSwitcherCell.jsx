import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import ActionCreators from '../actions';
import { Cell } from '@9softstudio/react-grid';
import Switcher from '@jsroot/components/Switcher';
import LangHelper from '@jsroot/lib/langHelper';
import { generateDefaultConfirmOption, predefinedIcons } from '@jsroot/lib/sweetAlert';
import { UpdateVipStatusAlertOption } from '../lib/helpers/sweetAlertHelper'

const VipSwitcherCell = (props) => {
    const { index, updateVipStatus, setSweetAlertOption, playerList } = props;
    const { systemId, isVip, isBA, customerClass } = playerList[index];
    const canEditVip = !(isBA);

    const _handleCloseAlert = () => {
        setSweetAlertOption(null);
    }

    const _cancelProps = {
        onClick: _handleCloseAlert
    }

    const _buildAlertOptionUpdateVipStatus = (params, isUpdateVip) => {
        const confirmMessage = isUpdateVip ? LangHelper.getSingleResource("ConfirmActivateVIPPlayer") : LangHelper.getSingleResource("ConfirmDeactivateVIPPlayer");
        const handlerUpdateVipStatusSuccess = UpdateVipStatusAlertOption.createHandlerAfterUpdateVipStatusSuccess(props);
        const handlerAfterUpdateVipStatus = UpdateVipStatusAlertOption.createHandlerAfterUpdateVipStatus({ setSweetAlertOption, handlerUpdateVipStatusSuccess, isUpdateVip, isSingleUpdate: true });
        const confirmProps = UpdateVipStatusAlertOption.createUpdateVipStatusConfirmProps({ params, updateVipStatus, handlerAfterUpdateVipStatus });
        const confirmOptions = generateDefaultConfirmOption(confirmMessage, _handleCloseAlert, confirmProps, _cancelProps, predefinedIcons.warning, { height: 210 });
        return confirmOptions;
    }

    const _handleChangeVipStatus = (event) => {
        event.preventDefault();
        const checked = event.target.checked;
        const params = {
            systemIds: systemId,
            isVip: !isVip
        }
        const alertOption = _buildAlertOptionUpdateVipStatus(params, checked);
        setSweetAlertOption(alertOption);
    }

    const _getTextDisplay = isVip => {
        return isVip ? 'YES' : 'NO';
    }

    return (
        <Cell className={"center-align"}>
            {canEditVip ? <Switcher checked={isVip} onChange={_handleChangeVipStatus} /> : _getTextDisplay(isVip)}
        </Cell>
    )
}

function mapDispatchToProps(dispatch) {
    const { setPlayerList, searchPlayer, updateVipStatus, setSweetAlertOption, setPagingOption } = ActionCreators;
    return bindActionCreators({ setPlayerList, updateVipStatus, setSweetAlertOption, searchPlayer, setPagingOption }, dispatch);
}

function mapStateToProps(state) {
    const { playerList, pagingOption, selectedTabKey, selectedParams } = state;
    return Object.assign({}, { playerList, pagingOption, selectedTabKey, selectedParams });
};

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(VipSwitcherCell));