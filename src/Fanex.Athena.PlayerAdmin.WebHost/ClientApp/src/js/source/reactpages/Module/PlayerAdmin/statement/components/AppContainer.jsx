import React from 'react';
import AppLayout from '@jsroot/layouts/AppLayout';
import FormFilter from './FormFilter';
import DataContent from './DataContent';

export const AppContainer = (props) => {
    return (
        <AppLayout>
            <FormFilter />
            <DataContent />
        </AppLayout>
    );
}

export default React.memo(AppContainer);