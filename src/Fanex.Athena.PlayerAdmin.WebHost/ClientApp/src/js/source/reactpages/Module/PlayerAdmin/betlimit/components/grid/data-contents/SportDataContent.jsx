import React from 'react';
import DataHandling from '../../../lib/dataHandling';
import createSetting from '../base/SettingContent';
import SettingGrid from '../base/SettingGrid';

const Content = createSetting(SettingGrid, new DataHandling());

const SportDataContent = () => {
    return <Content />
}

export default SportDataContent;