import React from 'react';
import AppContainer from './components/AppContainer.jsx';
import { Provider } from 'react-redux';
import Store from '@jsroot/lib/store';
import reducers from './reducers';
import { tabKeys } from '../../lib/constants';

const initialState = {
  ...window.initialState[tabKeys.problemPlayer],
  ...{
    userGuideContent: '',
    subsidiarySearch: false,
    pagingOption: {
      PageIndex: 1,
      PageSize: 500,
      PageList: [500, 1000, 1500, 2000],
      TotalItem: null
    }
  }
}


export default (props) => {
  const { selectedTabKey } = props;
  const defaultState = {
    ...initialState,
    ...{ selectedTabKey }
  }

  const store = Store.createStore(reducers, defaultState);
  
  return (
    <Provider store={store}>
      <AppContainer />
    </Provider>
  );
}