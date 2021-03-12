import React from 'react';
import { Tabs, Tab } from '@9softstudio/react-simple-tab';
import LangHelper from '@jsroot/lib/langHelper';
import Site from '@jsroot/lib/site';
import ExchangeVoided from './AppContainer';
import { tabList } from '../../bettingticketscenter/common/lib/constants';
import AppData from '@jsroot/lib/AppData';

export const BTCContainer = (props) => {
    const activedTabList = tabList.filter(x => AppData.tabList.includes(x.key));

    return (
        <div>
            <Tabs>
                {
                    activedTabList.map(item => <Tab label={item.label} url={item.url} />)
                }
                <Tab isActive={true} label={LangHelper.getSingleResource("Exchange")}
                    url={Site.resolveClientUrl("ExchangeVoided")}>
                </Tab>
            </Tabs>
            <div>
                <ExchangeVoided />
            </div>
        </div>
    )
}

export default BTCContainer;