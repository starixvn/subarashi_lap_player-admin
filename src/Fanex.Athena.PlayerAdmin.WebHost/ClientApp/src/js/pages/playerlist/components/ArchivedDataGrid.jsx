import React from 'react';
import { Table } from '@9softstudio/react-grid';
import GridHelper from '../lib/helpers/gridHelper';
import GridInteractionService from '../lib/services/gridInteractionService';
import ArchivedPlayerRow from './grid/ArchivedPlayerRow';
import { buildDisplayedColumnObject } from '../lib/helpers/utils';
import NoDataTitle from '@jsroot/components/NoDataTitle';

const MAX_WIDTH_PAGE = 1920;

const ArchivedDataGrid = props => {

    const maxWidth = Math.min(document.body.clientWidth, MAX_WIDTH_PAGE);

    const _renderDataSection = (dataList, displayedColumn) => {
        const dataRows = [];
        for (let i = 0; i < dataList.length; i++) {
            const dataItem = dataList[i];
            const rowProps = {
                dataItem,
                displayedColumn
            };
            dataRows.push(<ArchivedPlayerRow key={`'achived-player-'${dataItem.IbcCustId}`} {...rowProps} />);
        }

        return dataRows;
    }

    const handleSort = (sortBy, sortOrder) => {
        const { playerList, setPlayerList, toggleLoader } = props;
        toggleLoader(true);
        setTimeout(() => {
            const sortedPlayerList = GridInteractionService.sortPlayerList(playerList, sortBy, sortOrder);
            setPlayerList(sortedPlayerList);
            toggleLoader(false);
        }, 0);
    }

    const { playerList, displayedColumnList } = props;

    if (!playerList) {
        return null;
    }

    if (!playerList.length) {
        return <NoDataTitle />;
    }

    const displayedColumn = buildDisplayedColumnObject(displayedColumnList);

    return (
        <Table
            autoWidth={true}
            maxWidth={maxWidth}
            minWidth={1300}
            autoHeight={true}
            header={GridHelper.renderHeaderForArchived(displayedColumn)}
            body={_renderDataSection(playerList, displayedColumn)}
            containerPadding={30}
            shouldResetScrollPosition={props.isLoading}
            onSort={handleSort} />
    );
}

export default ArchivedDataGrid;