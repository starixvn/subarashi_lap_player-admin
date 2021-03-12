import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Table } from '@9softstudio/react-grid';
import AppData from '@jsroot/lib/AppData';
import { ActionCreators } from '../actions';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import GridHelper from '../../../lib/helpers/gridHelper';
import PlayerRow from './grid/PlayerRow';
import { getDataGridHeaders } from '../lib/helpers/gridHelper';

const MAX_WIDTH_PAGE = 1920;

export const DataGrid = (props) => {
    const MAX_WIDTH = Math.min(document.body.clientWidth, MAX_WIDTH_PAGE);
    const { playerList, pagingOption, selectedParams, selectedTabKey, setPagingOption, searchPlayer, setSelectedPlayer } = props;

    if (playerList == null) {
        return null;
    }
    if (!playerList.length) {
        return <NoDataTitle />;
    }

    const showSubsidiary = GridHelper.isShowSubsidiary(selectedParams.subsidiarySearch);
    
    const onSelectPlayer = React.useCallback(custInfo => {
        setSelectedPlayer(custInfo)
    });

    const _renderDataSection = dataList => dataList.map((dataItem, index) => {
        return <PlayerRow {...{ dataItem, index, hasVipManagement: AppData.HasVipManagement, showSubsidiary, onSelectPlayer }} />
    })

    const _handleSort = (orderCol, sortType) => {
        const sortInfo = GridHelper.getSortInfo(orderCol, sortType);
        const _pagingOption = {
            ...pagingOption,
            ...sortInfo
        }
        const params = {
            ...selectedParams,
            ..._pagingOption
        }
        searchPlayer(selectedTabKey, params, GridHelper.callBackToUpdatePaging({ pagingOption, setPagingOption }));
    }

    const _gridHeaders = getDataGridHeaders(AppData.HasVipManagement, showSubsidiary);

    return (
        <React.Fragment>
            <Table minWidth={650}
                autoWidth={true}
                maxWidth={MAX_WIDTH}
                autoHeight={true}
                header={GridHelper.renderHeader(_gridHeaders)}
                body={_renderDataSection(playerList)}
                containerPadding={30}
                shouldResetScrollPosition={false}
                onSort={_handleSort} />
        </React.Fragment>
    );
}

function mapDispatchToProps(dispatch) {
    const { searchPlayer, setPagingOption, setSelectedPlayer } = ActionCreators;
    return bindActionCreators({ setPagingOption, searchPlayer, setSelectedPlayer }, dispatch);
}

function mapStateToProps(state) {
    const { playerList, selectedParams, selectedTabKey, pagingOption } = state;
    return Object.assign({}, { playerList, pagingOption, selectedParams, selectedTabKey });
};

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(DataGrid));


