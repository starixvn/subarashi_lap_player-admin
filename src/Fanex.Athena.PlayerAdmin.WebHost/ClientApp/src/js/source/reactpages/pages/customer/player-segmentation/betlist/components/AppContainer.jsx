import React from 'react';
import { connect } from 'react-redux';
import Loader from '@jsroot/components/Loader';
import AppLayout from '@jsroot/layouts/AppLayout';
import BetListLayout from '@jsroot/layouts/betlist/BetListLayout';
import Site from '@jsroot/lib/site';
import AppData from '@jsroot/lib/AppData';
import LangHelper from '@jsroot/lib/langHelper'
import FormFilter from './FormFilter';
import DataGrid from './DataGrid';

export const AppContainer = props => {
    const { isLoading } = props;
    const _contentTitle = React.useMemo(() => {
        return (<div className="content-title">
            <span>{`${AppData.externalId}(${AppData.systemId})`}</span>
            <span className="base-currency">{` - ${LangHelper.getSingleResource('Viewin').replace('{0}', AppData.baseCurrency)}`}</span>
        </div>);
    }, []);

    return (
        <AppLayout>
            <BetListLayout
                pageName='PlayerSegmentation'
                rootUrl={Site.getRootUrl()} >
                <Loader isLoading={isLoading} />
                {_contentTitle}
                <FormFilter />
                <DataGrid />
            </BetListLayout>
        </AppLayout>
    )
}

function mapStateToProps(state) {
    const { isLoading } = state;
    return Object.assign({ isLoading });
}

export default connect(mapStateToProps)(AppContainer);