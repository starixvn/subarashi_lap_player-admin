import React from 'react';
import ReportTitle from '@jsroot/components/ReportTitle';
import MultipleSelect from '@9softstudio/react-multiselect';
import LangHelper from '@jsroot/lib/langHelper';

const ArchivedGridCaption = ({ displayedColumnList, setDisplayedColumnList, handleExportExcel, playerCount }) => {

    const handleChangeDisplayedColumn = (currentItem, selectedItemsKey) => {
        const selectedKeyList = selectedItemsKey.split(',');
        const newColumnList = displayedColumnList.map(item => {
            const isShow = selectedKeyList.indexOf(item.key) !== -1;
            return item.show === isShow ? item : { ...item, show: isShow };
        })

        setDisplayedColumnList(newColumnList);
    }

    const _buildDisplayedColumnList = () => {
        return (
            <MultipleSelect id="columnList" className="selection-column"
                language={LangHelper.currentLanguage}
                dataSource={displayedColumnList}
                keyField={"key"}
                valueField={"value"}
                statusField={"show"}
                onChange={handleChangeDisplayedColumn} />
        )
    }

    const _buildLeftPart = () => {
        return (<span key="searchResult">{`${LangHelper.getSingleResource('SearchResult')}: ${playerCount} ${LangHelper.getSingleResource('Player')}(s)`}</span>);
    }

    const _buildRightPart = () => {
        return (
            <span className="caret-block">
                {_buildDisplayedColumnList()}
                <span className="icon-excel-wrapper" onClick={handleExportExcel}>
                    <i className="icon-excel" title={LangHelper.getSingleResource('ExportToExcel')}></i>
                </span>
            </span>
        )
    }

    return (
        <ReportTitle tag="div" content={<>{_buildLeftPart()}{_buildRightPart()}</>} />
    )
}

export default ArchivedGridCaption

