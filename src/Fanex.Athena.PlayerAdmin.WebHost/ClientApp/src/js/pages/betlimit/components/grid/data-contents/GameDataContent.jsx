import React from 'react';
import GameSettingDataHandling from '../../../lib/gameSettingDataHandling';
import createSetting from '../base/SettingContent';
import SettingGrid from '../base/SettingGrid';

const Content = createSetting(SettingGrid, new GameSettingDataHandling());

const GameDataContent = () => {
    return <Content />
}

export default GameDataContent;