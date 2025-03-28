import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../../actions';
import GridHelper from '../../lib/helpers/gridHelper';
import { buildSelectedPlayer, buildCurrentActivePlayer } from '../../lib/helpers/playerDataBuilder';
import { getActiveTabList } from '../../lib/helpers/contextMenuHelper';
import { playerStatusByName } from '../../lib/constants';
import { apiInfo } from '../../lib/constants';
import FactoryCommonService from '../../../../lib/factoryCommonService';
import LangHelper from '@jsroot/lib/langHelper';

const itemContextMenuHeight = 28;

export const UserNameCell = (props) => {

    const commonService = FactoryCommonService.createInstance();

    const {
        dataItem,
        hasCommPermissionRead,
        setSelectedPlayer,
        setCurrentActivePlayer,
        setContextMenu,
        contextMenu,
        setSelectedTabName,
        setSelectedSubTabName,
        getTabList,
        type,
        isShowSabaIntelligence
    } = props;

    const handleSelectPlayer = (event) => {
        event.preventDefault();
        const newSelectedPlayer = buildSelectedPlayer(dataItem);
        setSelectedPlayer(newSelectedPlayer);
        setSelectedTabName(null);
        setSelectedSubTabName(null);
    }

    const handleViewSabaIntelligence = (event) => {
        event.preventDefault();
        const {action, type} = apiInfo.lapSabaIntelligence;

        const url = commonService.getLink(type, action + `?username=${dataItem.UserName}`);
        window.open(url, "_blank").focus();
    }

    const handleEnterContextMenu = (event) => {
        event.preventDefault();
        const { isOpen } = contextMenu;
        if (!isOpen) {
            const currentActivePlayer = buildCurrentActivePlayer(dataItem);
            const tabList = getTabList();
            const activeMenuList = getActiveTabList(currentActivePlayer, tabList, type, hasCommPermissionRead);

            const contextMenuHeight = itemContextMenuHeight * activeMenuList.length;
            const rect = event.target.getBoundingClientRect();
            const position = {
                x: rect.left - 10,
                y: GridHelper.calculateTopPosition(contextMenuHeight, rect.top)
            };

            setContextMenu({ position, isOpen: true });
            setCurrentActivePlayer(currentActivePlayer);
        }
    }

    const handleLeaveContextMenu = (event) => {
        event.preventDefault();
        if (contextMenu.isOpen) {
            setContextMenu({ ...contextMenu, isOpen: false })
        }
    }

    const { Status, IsDeposit } = dataItem;
    const isClosedOrArchived = Status === playerStatusByName.Closed || Status === playerStatusByName.Archived;

    return (
        <div className="username-cell">
            <button className="btn-default btn-link username-link"
                onClick={handleSelectPlayer}>
                {dataItem.UserName}
            </button>

            <div className='cell-menu'>
                { IsDeposit && !isClosedOrArchived && isShowSabaIntelligence && (
                    <button className="btn-default btn-link btn-icon"
                        onClick={handleViewSabaIntelligence}>
                        <i className="personal-card" title={LangHelper.getSingleResource('SabaIntelligenceTooltip')}></i>
                    </button>)
                }  
                { !isClosedOrArchived && (
                    <button className="btn-default btn-link btn-icon"
                        onMouseEnter={handleEnterContextMenu} onMouseLeave={handleLeaveContextMenu}>
                        <i className="material-icons">edit</i>
                    </button>)
                }  
            </div>
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { setSelectedPlayer, setCurrentActivePlayer, setContextMenu, setSelectedTabName, setSelectedSubTabName, getTabList } = ActionCreators;
    return bindActionCreators({ setSelectedPlayer, setCurrentActivePlayer, setContextMenu, setSelectedTabName, setSelectedSubTabName, getTabList }, dispatch);
}

function mapStateToProps(state) {
    const { contextMenu, type } = state;
    return Object.assign({}, { contextMenu, type });
}

export default connect(mapStateToProps, mapDispatchToProps)(UserNameCell);
