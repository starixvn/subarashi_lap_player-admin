import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Pager } from '@9softstudio/react-grid';
import GridCaption from './content-header/GridCaption';
import DataGrid from './DataGrid';
import { ActionCreators } from '../actions';

export const Content = (props) => {
    const { pagingOption } = props;

    if (pagingOption.TotalItem === null) {
        return null;
    }

    const handlePaging = (pageIndex, pageSize) => {
        const { param, searchPlayer } = props;
        const newParam = {
            ...param,
            pageIndex,
            pageSize,
        }

        searchPlayer(newParam);
    }

    return (
        <div className="result-content">
            <Pager pageOption={pagingOption} onPaging={handlePaging} />
            <GridCaption />
            <DataGrid />
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { searchPlayer } = ActionCreators;
    return bindActionCreators({ searchPlayer }, dispatch);
}
function mapStateToProps(state) {
    const { pagingOption, param } = state;
    return Object.assign({}, { pagingOption, param });
};

export default connect(mapStateToProps, mapDispatchToProps)(Content);
