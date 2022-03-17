import React from 'react';
import vs3DataHandling from '../../../lib/vs3DataHandling';
import createSetting from '../base/SettingContent';
import VS3SettingGrid from '../sport-settings/VS3SettingGrid';

const Content = createSetting(VS3SettingGrid, new vs3DataHandling());

const VS3DataContent = () => {
    return <Content />
}

export default VS3DataContent;