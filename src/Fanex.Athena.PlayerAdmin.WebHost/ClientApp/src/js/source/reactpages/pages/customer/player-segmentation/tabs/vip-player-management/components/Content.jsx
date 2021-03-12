import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Pager } from '@9softstudio/react-grid';
import GridCaption from '../../../components/GridCaption';
import DataGrid from './DataGrid';
import { ActionCreators } from '../actions';
import GridHelper from "../../../lib/helpers/gridHelper";

export const Content = props => {
    const { playerCount, selectedParams, selectedTabKey, selectedPlayer, searchPlayer, pagingOption, setPagingOption, setSelectedPlayer } = props;

    if (pagingOption.TotalItem == null) {
        return null;
    }

    const _handlePaging = (pageIndex, pageSize) => {
        const sortInfo = GridHelper.getSortInfoFromPagingOption(pagingOption);
        const params = {
            ...selectedParams,
            ...sortInfo,
            ...{
                pageIndex,
                pageSize
            },
        }
        searchPlayer(selectedTabKey, params, GridHelper.callBackToUpdatePaging({ pagingOption, setPagingOption }));
    }

    return (
        <div className="result-content">
            <GridCaption playerCount={playerCount} />
            <DataGrid />
            <Pager pageOption={pagingOption} onPaging={_handlePaging} />
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { searchPlayer, setPagingOption, setSelectedPlayer } = ActionCreators;
    return bindActionCreators({ searchPlayer, setPagingOption, setSelectedPlayer }, dispatch);
}
function mapStateToProps(state) {
    const { pagingOption, selectedParams, playerList, selectedTabKey, selectedPlayer } = state;
    const playerCount = pagingOption.TotalItem || (playerList && playerList.length) || 0;
    return Object.assign({}, { pagingOption, selectedParams, selectedTabKey, playerCount, selectedPlayer });
};

export default connect(mapStateToProps, mapDispatchToProps)(Content);