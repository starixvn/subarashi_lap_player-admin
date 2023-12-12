import React from 'react';
import { connect } from 'react-redux';
import AppLayout from '@jsroot/layouts/AppLayout'
import FormFilter from './FormFilter';
import DataContent from './DataContent';
import Loader from '@jsroot/components/Loader';
import Alert from '@jsroot/components/sweet-alert';

export const AppContainer = (props) => {
    return (
        <AppLayout>
            <Loader isLoading={props.isLoading} />
            <FormFilter />
            <DataContent />
            <Alert
                width={500}
                isHtml={false}
                {...props.alertOption}
            />
        </AppLayout>
    );
}

function mapStateToProps(state) {
    const { isLoading, alertOption } = state;
    return Object.assign({}, { isLoading, alertOption });
}

export default connect(mapStateToProps)(React.memo(AppContainer));