import React from 'react';
import ReportTitle from '@jsroot/components/ReportTitle';
import LangHelper from '@jsroot/lib/langHelper';

export default (props) => {
    const { playerCount } = props;

    const _buildContent = () => {
            return (<span key="searchResult">{`${LangHelper.getSingleResource('SearchResult')}: ${playerCount} ${LangHelper.getSingleResource('Player')}(s)`}</span>);
     }

    return (<ReportTitle tag="div" content={_buildContent()} />)
}
