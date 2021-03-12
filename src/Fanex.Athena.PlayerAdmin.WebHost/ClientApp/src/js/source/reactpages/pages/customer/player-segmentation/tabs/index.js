import React from 'react';
import { render } from 'react-dom';
import AppContainer from '../components/AppContainer.jsx';
import { Provider } from 'react-redux';
import Store from '@jsroot/lib/store';
import reducers from '../reducers';
import { tabMenuList } from '../lib/constants';

const defaultState = {
  selectedTabKey: tabMenuList[0].key
}
const store = Store.createStore(reducers, defaultState);

const App = () => {
  return (
    <Provider store={store}>
      <AppContainer />
    </Provider>
  );
}

render(<App />, document.getElementById("app-container"));