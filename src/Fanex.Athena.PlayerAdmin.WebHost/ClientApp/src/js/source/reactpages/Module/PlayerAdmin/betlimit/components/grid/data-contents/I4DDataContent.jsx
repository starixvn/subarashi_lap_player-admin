import React from 'react';
import I4DDataHandling from '../../../lib/i4DDataHandling';
import createSetting from '../base/SettingContent';
import I4DSettingGrid from '../sport-settings/I4DSettingGrid';

const Content = createSetting(I4DSettingGrid, new I4DDataHandling());

const I4DDataContent = () => {
    return <Content />
}

export default I4DDataContent;