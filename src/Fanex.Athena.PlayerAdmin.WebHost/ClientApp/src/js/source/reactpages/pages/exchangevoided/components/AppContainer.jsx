import React, { Component } from 'react';
import { connect } from 'react-redux';
import DataGrid from './DataGrid'
import FormFilter from './FormFilter'
import Loader from '@jsroot/components/Loader'
import AppLayout from '@jsroot/layouts/AppLayout'

export const AppContainer = (props) => {
  return (
    <AppLayout>
      <Loader isLoading={props.isLoading} />
      <FormFilter />
      <DataGrid />
    </AppLayout>
  );
}

function mapStateToProps(state) {
    return Object.assign({}, { isLoading: state.isLoading });
}

export default connect(mapStateToProps)(AppContainer);