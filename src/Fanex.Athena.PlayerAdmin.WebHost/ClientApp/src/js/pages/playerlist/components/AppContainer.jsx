import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions/index.js';
import Content from './Content';
import FormFilter from './FormFilter';
import Loader from '@jsroot/components/Loader';
import AppLayout from '@jsroot/layouts/AppLayout';
import PlayerDetail from './PlayerDetail';
import ArchivedContent from './ArchivedContent';

export const AppContainer = ({ isLoading, isArchivedPlayersSearch, archivedSearch, isEnableSingleWallet, toggleLoader }) => {
    const _renderContent = () => {
        return !isArchivedPlayersSearch ?
            <>
                <Content />
                <PlayerDetail />
            </> : <ArchivedContent archivedSearch={archivedSearch} isEnableSingleWallet={isEnableSingleWallet} isLoading={isLoading} toggleLoader={toggleLoader} />
    }
    return (
        <AppLayout>
            <Loader isLoading={isLoading} />
            <FormFilter />
            {_renderContent()}
        </AppLayout>
    );
}

function mapDispatchToProps(dispatch) {
    const { toggleLoader } = ActionCreators;
    return bindActionCreators({
        toggleLoader
    }, dispatch);
}

function mapStateToProps(state) {
    const { isLoading, param, siteSetting } = state;
    return Object.assign({}, { 
        isLoading,
        isArchivedPlayersSearch: !!param.isArchivedPlayersSearch,
        isEnableSingleWallet: siteSetting.isEnableSingleWallet,
        archivedSearch: !!param.isArchivedPlayersSearch ? param: {}
    });
}

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer); 
