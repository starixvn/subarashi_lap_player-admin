import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import Store from '@jsroot/lib/store';
import reducer from '../reducers';
import AppContainer from './AppContainer.jsx';

const defaultState = Object.assign({}, window.initialState);
const store = Store.createStore(reducer, defaultState);

const App = () => {
    return (
        <Provider store={store}>
            <AppContainer />
        </Provider>
    )
};

render(<App />, document.getElementById("app-container"));
