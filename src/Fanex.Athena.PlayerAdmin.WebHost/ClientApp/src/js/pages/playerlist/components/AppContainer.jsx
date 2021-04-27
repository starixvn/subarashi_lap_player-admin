import React from 'react';
import { connect } from 'react-redux';
import Content from './Content';
import FormFilter from './FormFilter';
import Loader from '@jsroot/components/Loader';
import AppLayout from '@jsroot/layouts/AppLayout';
import PlayerDetail from './PlayerDetail';

export const AppContainer = (props) => {
    return (
        <AppLayout>
            <Loader isLoading={props.isLoading} />
            <FormFilter hasCommPermissionRead={props.hasCommPermissionRead} />
            <Content />
            <PlayerDetail />
        </AppLayout>
    );
}

function mapStateToProps(state) {
    const { isLoading, hasCommPermissionRead } = state;
    return Object.assign({}, { isLoading, hasCommPermissionRead });
}

export default connect(mapStateToProps)(React.memo(AppContainer)); 
