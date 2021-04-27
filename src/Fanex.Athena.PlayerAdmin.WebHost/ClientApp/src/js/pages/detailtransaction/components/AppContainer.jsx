import React from 'react';
import { connect } from 'react-redux';
import AppLayout from '@jsroot/layouts/AppLayout';
import Loader from '@jsroot/components/Loader';
import FormFilter from './FormFilter';
import DataGrid from './DataGrid';

export const AppContainer = (props) => {
    return (
        <AppLayout>
            <Loader isLoading={props.isLoading} />
            <FormFilter />
            <DataGrid />
        </AppLayout>
    );
};

function mapStateToProps(state) {
    const { requestParam, isLoading } = state;
    return Object.assign({}, { requestParam, isLoading });
};

export default connect(mapStateToProps)(React.memo(AppContainer));
