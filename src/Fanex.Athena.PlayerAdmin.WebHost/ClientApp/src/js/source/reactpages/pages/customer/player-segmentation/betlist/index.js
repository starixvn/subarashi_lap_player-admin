import React from 'react';
import { render } from 'react-dom';
import AppContainer from './components/AppContainer';
import { Provider } from 'react-redux';
import Store from '@jsroot/lib/store';
import reducers from './reducers';

const defaultState = {
    ...window.initialState
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