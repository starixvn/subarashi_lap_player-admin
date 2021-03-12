import React, { Component } from 'react';
import { connect } from 'react-redux';
import DataGrid from './DataGrid'
import FormFilter from './FormFilter'
import PageLayout from '../../common/components/PageLayout'
import Loader from '@jsroot/components/Loader';

export class AppContainer extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <PageLayout selectedTabIndex="2">
                <Loader isLoading={this.props.isLoading} />
                <FormFilter />
                <DataGrid />
            </PageLayout>
        );
    }
}

function mapStateToProps(state) {
    return Object.assign({}, { isLoading: state.isLoading });
}

export default connect(mapStateToProps)(AppContainer);