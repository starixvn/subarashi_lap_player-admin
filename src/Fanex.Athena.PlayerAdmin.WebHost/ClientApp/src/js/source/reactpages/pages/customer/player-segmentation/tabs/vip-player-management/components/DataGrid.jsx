import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import { Table } from '@9softstudio/react-grid';
import CheckBox from '@jsroot/components/CheckBox'
import PlayerRow from './grid/PlayerRow';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import GridHelper from '../../../lib/helpers/gridHelper';
import { getDataGridHeaders } from '../lib/helpers/gridHelper';

const MAX_WIDTH_PAGE = 1920;

export const DataGrid = (props) => {
    const MAX_WIDTH = Math.min(document.body.clientWidth, MAX_WIDTH_PAGE);
    const { isCheckAll, playerList, pagingOption, selectedParams, selectedTabKey, setPlayerList, setPagingOption, searchPlayer } = props;

    if (playerList == null) {
        return null;
    }

    if (!playerList.length) {
        return <NoDataTitle />;
    }

    const showSubsidiary = GridHelper.isShowSubsidiary(selectedParams.subsidiarySearch)

    const _dataGridHeaders = [
        {
            key: 'CheckAll', text: null, colWidth: 30, childComponent: <CheckBox id="IsCheckAll"
                checked={isCheckAll}
                onChange={(checked) => {
                    const newPlayerList = playerList.map(_ => {
                        return {
                            ..._,
                            ...{ isSelected: checked && !(_.isBA) }
                        }
                    });

                    setPlayerList(newPlayerList);
                }} />
        },
        ...getDataGridHeaders(showSubsidiary)
    ]

    const _renderDataSection = dataList => dataList.map((dataItem, index) => {
        const { isSelected, ...rest } = dataItem;
        return <PlayerRow {...rest} index={index} showSubsidiary={showSubsidiary} />
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

    return (
        <React.Fragment>
            <Table minWidth={650}
                autoWidth={true}
                maxWidth={MAX_WIDTH}
                autoHeight={true}
                header={GridHelper.renderHeader(_dataGridHeaders)}
                body={_renderDataSection(playerList)}
                containerPadding={30}
                shouldResetScrollPosition={false}
                onSort={_handleSort} />
        </React.Fragment>
    );
}

function mapDispatchToProps(dispatch) {
    const { searchPlayer, setPlayerList, setPagingOption, setSelectedPlayer } = ActionCreators;
    return bindActionCreators({ setPlayerList, setPagingOption, searchPlayer, setSelectedPlayer }, dispatch);
}

function mapStateToProps(state) {
    const { playerList, selectedParams, selectedTabKey, pagingOption } = state;
    const isCheckAll = playerList && !playerList.some(_ => !(_.isSelected || _.isBA));
    return Object.assign({}, { playerList, isCheckAll, pagingOption, selectedParams, selectedTabKey });
};

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(DataGrid));