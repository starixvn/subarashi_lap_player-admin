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

export const AppContainer = ({ isLoading, isArchivedPlayersSearch, archivedSearch, isEnableSingleWallet, toggleLoader, fetchDataTime, setFetchDataTime }) => {
    const _renderContent = () => {
        return !isArchivedPlayersSearch ?
            <>
                <Content />
                <PlayerDetail />
            </> : <ArchivedContent 
                    archivedSearch={archivedSearch} 
                    isEnableSingleWallet={isEnableSingleWallet} 
                    isLoading={isLoading} 
                    toggleLoader={toggleLoader}
                    fetchDataTime={fetchDataTime}
                    setFetchDataTime={setFetchDataTime}/>
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
    const { toggleLoader, setFetchDataTime } = ActionCreators;
    return bindActionCreators({
        toggleLoader, setFetchDataTime
    }, dispatch);
}

function mapStateToProps(state) {
    const { isLoading, param, siteSetting, fetchDataTime } = state;
    return Object.assign({}, { 
        isLoading,
        isArchivedPlayersSearch: !!param.isArchivedPlayersSearch,
        isEnableSingleWallet: siteSetting.isEnableSingleWallet,
        archivedSearch: !!param.isArchivedPlayersSearch ? param: {},
        fetchDataTime
    });
}

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer); 
