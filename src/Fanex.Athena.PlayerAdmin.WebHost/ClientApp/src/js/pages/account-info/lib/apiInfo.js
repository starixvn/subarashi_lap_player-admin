import { systemType } from '@jsroot/constants/config';

export const lapAccountProfile = { action: 'PlayerAccountInfo/GetAccountProfile', type: systemType.newAdmin };
export const lapBankInfo = { action: 'PlayerAccountInfo/GetBankInfo', type: systemType.newAdmin };
export const playerInfoSettings = { action: 'api/Setting/GetPlayerInfoSetting', type: systemType.playerAdmin };
export const custBettingStatus = { action: 'api/PlayerBettingStatistics/GetData', type: systemType.playerAdmin };
export const lrfActivityRecord = { action: 'api/PlayerActivityRecord/Index', type: systemType.playerAdmin };
export const lrfSubsidiaryViewType = { action: 'api/Setting/GetSubsidiarySetting', type: systemType.playerAdmin };