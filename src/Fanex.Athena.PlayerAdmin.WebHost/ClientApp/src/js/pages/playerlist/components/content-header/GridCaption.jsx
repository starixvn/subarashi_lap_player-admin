import React from 'react';
import Select from 'react-select';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { ActionCreators } from '../../actions';
import ReportTitle from '@jsroot/components/ReportTitle';
import MultipleSelect from '@9softstudio/react-multiselect';
import LangHelper from '@jsroot/lib/langHelper';
import { buildDisplayedColumnObject } from '../../lib/helpers/utils';
import { batchUpdateManager, batchUpdatePermission } from '../../batch-update/batch-update-manager';
import Popup from '@jsroot/components/popup';

export class GridCaption extends React.Component {
    constructor(props) {
        super(props);
        this.initBatchUpdate();
    }

    initBatchUpdate() {
        this.batchUpdateMenuRef = React.createRef();
        var batchUpdateOptions = {
            permission: {
                [batchUpdatePermission.playerCommGroup]: this.props.hasCommPermissionEdit,
                [batchUpdatePermission.accountStatus]: false,
                [batchUpdatePermission.betSetting]: false,
                [batchUpdatePermission.specialLevel]: this.props.hasSpecialLevelPermission,
                [batchUpdatePermission.oddsGroup]: this.props.hasOddsGroupPermission,
                [batchUpdatePermission.baStatus]: this.props.siteSetting.isAllowedBA && this.props.hasBaPermission
            }
        };

        this.state = {
            batchUpdateSelectedOption: 0,
            popupBatchUpdateShow: false,
            batchUpdateMenu: batchUpdateManager.getMenu(batchUpdateOptions)
        };
    }

    componentDidMount() {
        batchUpdateManager.bindMenuEvent(this);
    }

    handleChangeDisplayedColumn = (currentItem, selectedItemsKey) => {
        const { displayedColumnList, setDisplayedColumnList } = this.props;
        const selectedKeyList = selectedItemsKey.split(',');
        const newColumnList = displayedColumnList.map(item => {
            const isShow = selectedKeyList.indexOf(item.key) !== -1;

            return item.show === isShow ? item : { ...item, show: isShow };
        })

        setDisplayedColumnList(newColumnList);
    }

    handleChangeBatchUpdateMenu = (selectedOption) => {
        this.setState({ batchUpdateSelectedOption: selectedOption });
        batchUpdateManager.handleMenuSelected(this, selectedOption);
    }

    _buildDisplayedColumnList() {
        return (
            <MultipleSelect id="columnList" className="selection-column"
                language={LangHelper.currentLanguage}
                dataSource={this.props.displayedColumnList}
                keyField={"key"}
                valueField={"value"}
                statusField={"show"}
                onChange={this.handleChangeDisplayedColumn} />
        )
    }

    _buildBatchUpdateMenu() {
        if (this.state.batchUpdateMenu.length === 0) {
            return (<></>);
        }

        return (
            <Select instanceId="ddl-batch-update"
                ref={this.batchUpdateMenuRef}
                className="ddl-batch-update"
                options={this.state.batchUpdateMenu}
                value={this.state.batchUpdateSelectedOption}
                onChange={(selectedOption) => this.handleChangeBatchUpdateMenu(selectedOption)}
                placeholder={LangHelper.getSingleResource('BatchUpdate')}
            />
        )
    }

    _buildLeftPart() {
        const { selectedPlayer, toggleLoader } = this.props;
        if (selectedPlayer) {
            const { userName, currency } = selectedPlayer;
            return (
                <>
                    <span className="link" onClick={() => {
                        toggleLoader(true)
                        setTimeout(() => {
                            this.props.setSelectedPlayer(null);
                            toggleLoader(false)
                        }, 10)
                    }}>{LangHelper.getSingleResource('PreviousSearchResult')}</span>
                    <span><i className="chevron right icon-separator"></i> {LangHelper.getSingleResource('Player')}: {userName} {currency && `(${currency})`}</span>
                </>
            )
        }
        else {
            const { playerCount } = this.props;
            return (<span key="searchResult">{`${LangHelper.getSingleResource('SearchResult')}: ${playerCount} ${LangHelper.getSingleResource('Player')}(s)`}</span>);
        }
    }

    _buildRightPart() {
        return (
            <span className="caret-block">
                {this._buildBatchUpdateMenu()}
                {this._buildDisplayedColumnList()}
                <span className="icon-excel-wrapper" onClick={this.handleExportExcel}>
                    <i className="icon-excel" title={LangHelper.getSingleResource('ExportToExcel')}></i>
                </span>
            </span>
        )
    }

    handleExportExcel = () => {
        const { playerList, param, displayedColumnList, exportExcel, selectedPlayer, hasCommPermissionRead } = this.props;
        if (displayedColumnList.filter(x => x.show).length === 0) {
            window.alert(LangHelper.getSingleResource("NoInfo"));
            return;
        }

        const displayedColumn = buildDisplayedColumnObject(displayedColumnList);
        const dataList = selectedPlayer ? [playerList.find(item => item.LicenseeCustId === selectedPlayer.licenseeCustId)] : playerList;

        exportExcel(dataList, param, displayedColumn, hasCommPermissionRead);
    }

    render() {
        const Content = <>{this._buildLeftPart()}{this._buildRightPart()}</>;
        if (this.state.batchUpdateSelectedOption == 0) {
            return (<ReportTitle tag="div" content={Content} />);
        }

        const opt = batchUpdateManager.getPopupOption(this.state.batchUpdateSelectedOption);
        return (
            <>
                <Popup id="popup-comm-group"
                    isShow={this.state.popupBatchUpdateShow}
                    iframeUrl={opt.url} title={opt.title} width={opt.width} height={opt.height}
                    onClose={() => batchUpdateManager.closePopup(this, this.state.batchUpdateSelectedOption)} />
                <ReportTitle tag="div" content={Content} />
            </>
        )
    }
}

function mapDispatchToProps(dispatch) {
    const { setSelectedPlayer, setDisplayedColumnList, toggleLoader, exportExcel, setPlayerList } = ActionCreators;
    return bindActionCreators({ setSelectedPlayer, setDisplayedColumnList, toggleLoader, exportExcel, setPlayerList }, dispatch);
}

function mapStateToProps(state) {
    const { pagingOption, selectedPlayer, displayedColumnList, param, playerList, hasCommPermissionEdit, hasCommPermissionRead,
    hasBaPermission, hasOddsGroupPermission, hasSpecialLevelPermission, siteSetting } = state;
    const playerCount = pagingOption.TotalItem || (playerList && playerList.length) || 0;
    return Object.assign({}, { playerList, playerCount, selectedPlayer, displayedColumnList, param, hasCommPermissionEdit, hasCommPermissionRead,
        hasBaPermission, hasOddsGroupPermission, hasSpecialLevelPermission, siteSetting });
};

export default connect(mapStateToProps, mapDispatchToProps)(GridCaption);

