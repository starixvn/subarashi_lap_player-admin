import { systemType } from '@jsroot/constants/config';

export const lapAccountProfile = { action: 'PlayerAccountInfo/GetAccountProfile', type: systemType.newAdmin };
export const lapBankInfo = { action: 'PlayerAccountInfo/GetBankInfo', type: systemType.newAdmin };
export const lapTaxInfo = { action: 'PlayerAccountInfo/GetTaxInfo', type: systemType.newAdmin };
export const playerInfoSettings = { action: 'api/Setting/GetPlayerInfoSetting', type: systemType.playerAdmin };
export const lapAccountKYC = { action: 'ExternalAPI/GetMemberImage', type: systemType.newAdmin };
export const lapDeleteKYCImage = { action: 'ExternalAPI/KycReject', type: systemType.newAdmin };
export const custBettingStatus = { action: 'api/PlayerBettingStatistics/GetData', type: systemType.playerAdmin };
export const lrfActivityRecord = { action: 'api/PlayerActivityRecord/Index', type: systemType.playerAdmin };
export const lrfSubsidiaryViewType = { action: 'api/Setting/GetSubsidiarySetting', type: systemType.playerAdmin };