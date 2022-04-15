import React from 'react';
import SBDataHandling from '../../../lib/sbDataHandling';
import createSetting from '../base/SettingContent';
import SBSettingGrid from '../sport-settings/SBSettingGrid';

const Content = createSetting(SBSettingGrid, new SBDataHandling());

const SBDataContent = () => {
    return <Content />
}

export default SBDataContent;