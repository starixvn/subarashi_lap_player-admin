import React from 'react'
import ReactDOM from 'react-dom';
import AppContainer from './AppContainer.jsx'
import { Provider } from 'react-redux'
import reducer from '../reducers/index.js'
import Store from '../../../../lib/store'
import { DefaultPageSize } from '../../common/lib/constants'

const defaultState = Object.assign({}, window.initialState, { userGuideContent: '', requestParam: { PageSize: DefaultPageSize } });
const store = Store.createStore(reducer, defaultState);

export default class App extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
          <Provider store={store}>
            <AppContainer />
          </Provider>
    );
}
}

ReactDOM.render(<App />, document.getElementById('app-container'));