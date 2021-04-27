import React from 'react';
import NGDataHandling from '../../../lib/ngDataHandling';
import createSetting from '../base/SettingContent';
import NGSettingGrid from '../sport-settings/NGSettingGrid';

const Content = createSetting(NGSettingGrid, new NGDataHandling());

const NGDataContent = () => {
    return <Content />
}

export default NGDataContent;