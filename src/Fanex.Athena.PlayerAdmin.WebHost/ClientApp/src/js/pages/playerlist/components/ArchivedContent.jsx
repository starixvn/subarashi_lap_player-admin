import React, { useEffect, useState } from 'react';
import { Pager } from '@9softstudio/react-grid';
import { defaultPagingOption } from '../lib/constants';
import ArchivedGridCaption from './content-header/ArchivedGridCaption';
import ArchivedDataGrid from './ArchivedDataGrid';
import PlayerQueryService from '../lib/services/playerQueryService';
import { archivedSearchOptionMapping } from '../lib/constants';
import { buildDisplayedColumnObject } from '../lib/helpers/utils';
import LangHelper from '@jsroot/lib/langHelper';
import { buildArchivedColumnList,  } from '../lib/helpers/gridColumnBuilder';


const ArchivedContent = ({ archivedSearch, isLoading, isEnableSingleWallet, toggleLoader }) => {
    const [pagingOption, setPagingOption] = useState(defaultPagingOption);
    const [displayedColumnList, setDisplayedColumnList] = useState(buildArchivedColumnList(isEnableSingleWallet));
    const [archivedPlayerList, setArchivedPlayerList] = useState([]);

    const defaultParams = React.useMemo(() => ({
        ...archivedSearch,
        searchOption: archivedSearchOptionMapping[archivedSearch.searchOption],
        pageIndex: defaultPagingOption.PageIndex,
        pageSize: defaultPagingOption.PageSize
    }), [archivedSearch])

    useEffect(() => {
        _fetchPlayerList(defaultParams);
    }, [archivedSearch])

    const handlePaging = (pageIndex, pageSize) => {
        _fetchPlayerList({
            ...defaultParams,
            pageIndex,
            pageSize,
        })
    }

    const handleExportExcel = () => {
        if (!displayedColumnList.some(x => x.show)) {
            window.alert(LangHelper.getSingleResource("NoInfo"));
            return;
        }
        const displayedColumn = buildDisplayedColumnObject(displayedColumnList);
        const service = new PlayerQueryService();
        service.exportExcel(archivedPlayerList, displayedColumn, 'ArchivedPlayerList');
    }

    const _fetchPlayerList = async (params) => {
        const service = new PlayerQueryService();
        toggleLoader(true);
        const { playerList, totalItem } = await service.getArchivedPlayerList(params);
        setArchivedPlayerList(playerList);
        const newPagingOption = {
            ...pagingOption,
            PageIndex: params.pageIndex,
            PageSize: params.pageSize,
            TotalItem: totalItem
        }
        setPagingOption(newPagingOption);
        toggleLoader(false);
    }

    if (pagingOption.TotalItem === null) {
        return null;
    }

    return (
        <div className="result-content">
            <Pager pageOption={pagingOption} onPaging={handlePaging} />
            <ArchivedGridCaption displayedColumnList={displayedColumnList} setDisplayedColumnList={setDisplayedColumnList} handleExportExcel={handleExportExcel} playerCount={archivedPlayerList.length} />
            <ArchivedDataGrid playerList={archivedPlayerList} setPlayerList={setArchivedPlayerList} toggleLoader={toggleLoader} displayedColumnList={displayedColumnList} isLoading={isLoading} />
        </div>
    )
}

export default ArchivedContent
