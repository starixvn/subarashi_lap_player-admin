import React from 'react';
import { Provider } from 'react-redux'
import reducer from '../reducers/index.js'
import Store from '../../../../lib/store'
import { render } from 'react-dom';
import AppContainer from './AppContainer';

const defaultState = { ...window.initialState };
const store = Store.createStore(reducer, defaultState);

const App = () => {
    return (
        <Provider store={store}>
            <AppContainer />
        </Provider>
    )
}

render(<App />, document.getElementById("app-container"));