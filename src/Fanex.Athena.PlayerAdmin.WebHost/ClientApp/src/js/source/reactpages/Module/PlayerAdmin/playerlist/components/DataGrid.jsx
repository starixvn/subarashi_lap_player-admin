import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';

import { Table } from '@9softstudio/react-grid';
import ContextMenu from './grid/ContextMenu';
import SpecialLevelDropdown from './grid/SpecialLevelDropdown';
import GridHelper from '../lib/helpers/gridHelper';
import GridInteractionService from '../lib/services/gridInteractionService';
import OddsGroupDropdown from './grid/OddsGroupDropdown';
import PlayerRow from './grid/PlayerRow';
import StatusDropdown from './grid/StatusDropdown';
import { buildDisplayedColumnObject } from '../lib/helpers/utils';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import SweetAlert from '../../../../components/sweet-alert';
import { batchUpdateManager } from '../batch-update/batch-update-manager';

const MAX_WIDTH_PAGE = 1920;

export class DataGrid extends PureComponent {
    constructor(props) {
        super(props);

        this.maxWidth = Math.min(document.body.clientWidth, MAX_WIDTH_PAGE);
    }

    componentDidMount() {
        batchUpdateManager.bindGridEvents();
    }

    componentDidUpdate() {
        batchUpdateManager.bindGridEvents();
    }

    _renderDataSection(dataList, displayedColumn, canSelectPlayers, hasCommPermissionRead) {
        const dataRows = [];
        for (let i = 0; i < dataList.length; i++) {
            const dataItem = dataList[i];
            const rowProps = {
                dataItem,
                displayedColumn,
                canSelectPlayers,
                hasCommPermissionRead
            };

            dataRows.push(<PlayerRow key={dataItem.LicenseeCustId} {...rowProps} />);
        }

        return dataRows;
    }

    handleSort = (sortBy, sortOrder) => {
        const { playerList, setPlayerList, toggleLoader } = this.props;
        toggleLoader(true);
        setTimeout(() => {
            const sortedPlayerList = GridInteractionService.sortPlayerList(playerList, sortBy, sortOrder);
            setPlayerList(sortedPlayerList);
            toggleLoader(false);
        }, 0);
    }

    render() {
        const { playerList, selectedPlayer, isFetchingData,
            displayedColumnList, sweetAlertOption, hasCommPermissionEdit, hasCommPermissionRead } = this.props;
        if (!playerList) {
            return null;
        }

        if (!playerList.length) {
            return <NoDataTitle />;
        }

        const dataList = selectedPlayer ? [playerList.find(item => item.LicenseeCustId === selectedPlayer.licenseeCustId)] : playerList;
        const displayedColumn = buildDisplayedColumnObject(displayedColumnList);
        const canSelectPlayers = hasCommPermissionEdit;

        return (
            <>
                <Table minWidth={650}
                    autoWidth={true}
                    maxWidth={this.maxWidth}
                    minWidth={1300}
                    autoHeight={true}
                    header={GridHelper.renderHeader(displayedColumn, canSelectPlayers, hasCommPermissionRead)}
                    body={this._renderDataSection(dataList, displayedColumn, canSelectPlayers, hasCommPermissionRead)}
                    containerPadding={30}
                    shouldResetScrollPosition={isFetchingData}
                    onSort={this.handleSort} />
                <ContextMenu hasCommPermissionRead={hasCommPermissionRead} />
                <SpecialLevelDropdown />
                <OddsGroupDropdown />
                <StatusDropdown />
                {sweetAlertOption && <SweetAlert {...sweetAlertOption} />}
            </>
        );
    }
}

function mapDispatchToProps(dispatch) {
    const { setPlayerList, toggleLoader } = ActionCreators;
    return bindActionCreators({ setPlayerList, toggleLoader }, dispatch);
}

function mapStateToProps(state) {
    const { playerList, selectedPlayer, displayedColumnList, isFetchingData, sweetAlertOption, hasCommPermissionEdit, hasCommPermissionRead } = state;
    return Object.assign({}, { playerList, selectedPlayer, displayedColumnList, isFetchingData, sweetAlertOption, hasCommPermissionEdit, hasCommPermissionRead });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);
