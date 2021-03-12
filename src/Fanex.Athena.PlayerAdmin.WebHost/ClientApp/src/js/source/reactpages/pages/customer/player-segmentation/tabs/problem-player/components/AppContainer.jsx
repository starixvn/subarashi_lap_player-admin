
import React from 'react';
import { connect } from 'react-redux';
import Loader from '@jsroot/components/Loader';
import SweetAlert from '@jsroot/components/sweet-alert';
import FormFilter from './FormFilter';
import Content from './Content';

export const AppContainer = props => {
    const { isLoading, sweetAlertOption } = props;

    return (
        <div className="problem-player">
            <Loader isLoading={isLoading} />
            <FormFilter />
            <Content />
            {sweetAlertOption && <SweetAlert {...sweetAlertOption} />}
        </div>
    )
}

function mapStateToProps(state) {
    const { isLoading, sweetAlertOption} = state;
    return Object.assign({ isLoading, sweetAlertOption });
}

export default connect(mapStateToProps)(AppContainer);