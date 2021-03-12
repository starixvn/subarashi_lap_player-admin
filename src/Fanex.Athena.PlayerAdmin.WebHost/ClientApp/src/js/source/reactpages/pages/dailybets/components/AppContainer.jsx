import React from 'react';
import { connect } from 'react-redux';
import DataGrid from './DataGrid';
import FormFilter from './FormFilter';
import GridCaption from './GridCaption';
import Loader from '@jsroot/components/Loader';
import AppLayout from '@jsroot/layouts/AppLayout';

export const AppContainer = (props) => {
    return (
        <AppLayout>
            <Loader isLoading={props.isLoading} />
            <FormFilter />
            <GridCaption />
            <DataGrid />
        </AppLayout>
    );
}

function mapStateToProps(state) {
    const { isLoading } = state;
    return Object.assign({}, { isLoading });
}

export default connect(mapStateToProps)(React.memo(AppContainer));