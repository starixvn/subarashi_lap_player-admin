import { systemType } from '@jsroot/constants/config';

export const custBettingStatus = { action: 'api/PlayerBettingStatistics/GetData', type: systemType.playerAdmin };

export const lapAccountProfile = { action: 'PlayerAccountInfo/GetAccountProfile', type: systemType.newAdmin };

export const lapBankInfo = { action: 'PlayerAccountInfo/GetBankInfo', type: systemType.newAdmin };

export const lrfActivityRecord = { action: 'api/PlayerActivityRecord/Index', type: systemType.playerAdmin };