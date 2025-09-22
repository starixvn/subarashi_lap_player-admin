import ClientResourceManager from '@jsroot/lib/clientResourceManager';

const getText = (resourceKey) => ClientResourceManager.getText(resourceKey);

export const tabKeys = {
    playerProfile: 'playerProfile',
    playerKYC: 'playerKYC',
    bankInformation: 'bankInformation',
    taxInformation: 'taxInformation',
    activityRecord: 'activityRecord'
}
export const tabList = [
    { name: tabKeys.playerProfile, text: getText('PlayerProfile') },
    { name: tabKeys.playerKYC, text: getText('PlayerKYC') },
    { name: tabKeys.bankInformation, text: getText('BankInformation') },
    { name: tabKeys.taxInformation, text: getText('TaxInformation') },
    { name: tabKeys.activityRecord, text: getText('ActivityRecord') }
]

export const licenseeType = {
    api: 0,
    nonApi: 1
}