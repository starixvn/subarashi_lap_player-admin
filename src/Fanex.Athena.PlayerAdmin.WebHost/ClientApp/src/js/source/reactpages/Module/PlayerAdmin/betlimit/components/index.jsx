import React from 'react';
import ReactDOM from 'react-dom';
import AppContainer from './AppContainer.jsx';
import { Provider } from 'react-redux';
import reducer from '../reducers/index.js';
import Store from '@jsroot/lib/store';

const defaultState = Object.assign({}, window.initialState, { isLoading: false });
const store = Store.createStore(reducer, defaultState);

const App = React.memo(() => {
    return (
        <Provider store={store}>
          <AppContainer />
        </Provider>
      );
})

ReactDOM.render(<App />, document.getElementById("app-container"));