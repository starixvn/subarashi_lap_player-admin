import { systemType } from '@jsroot/constants/config';
import LangHelper from '@jsroot/lib/langHelper';

export const specialLevelListWithColor = {
    0: '#fcfcfc',
    1: '#00ff00',
    2: '#008000',
    3: '#8b4513',
    4: '#daa520',
    5: '#006699',
    6: '#666633',
    7: '#a52a2a',
    8: '#0099ff',
    9: '#cc6600',
    10: '#ff0000'
}

export const oddsGroupListWithStyle = {
    A: { backgroundColor: '#006400', color: '#fff' },
    B: { backgroundColor: '#0099ff', color: '#fff' },
    C: null,
    D: null,
    E: null,
    F: null
}

export const licenseeType = {
    api: 0,
    nonApi: 1
}

export const fields = {
    Index: 'Index',
    Status: 'Status',
    UserName: 'UserName',
    SystemId: 'SystemId',
    VendorMemberId: 'VendorMemberId',
    RealName: 'RealName',
    Currency: 'Currency',
    CurrentBalance: 'CurrentBalance',
    OutstandingBalance: 'OutstandingBalance',
    OddsGroup: 'OddsGroup',
    BaStatus: 'BaStatus',
    SpecialLevel: 'SpecialLevel',
    LastTransaction: 'LastTransaction',
    Email: 'Email',
    CommissionGroup: 'CommissionGroup',
    CustomerClass: 'CustomerClass'
}

export const columnList = [
    { key: fields.Index, value: '#', show: true },
    { key: fields.Status, value: LangHelper.getSingleResource('Status'), show: true },
    { key: fields.UserName, value: LangHelper.getSingleResource('Username'), show: true },
    { key: fields.SystemId, value: LangHelper.getSingleResource('SystemId'), show: true },
    { key: fields.VendorMemberId, value: LangHelper.getSingleResource('VendorMemberID'), show: true },
    { key: fields.RealName, value: LangHelper.getSingleResource('RealName'), show: true },
    { key: fields.Currency, value: LangHelper.getSingleResource('Currency'), show: true },
    { key: fields.CurrentBalance, value: LangHelper.getSingleResource('CurrentBalance'), show: true },
    { key: fields.OutstandingBalance, value: LangHelper.getSingleResource('OutstandingBalance'), show: true },
    { key: fields.OddsGroup, value: LangHelper.getSingleResource('OddsGroup'), show: true },
    { key: fields.BaStatus, value: LangHelper.getSingleResource('BAStatus'), show: true },
    { key: fields.SpecialLevel, value: LangHelper.getSingleResource('SpecialLevel'), show: true },
    { key: fields.LastTransaction, value: LangHelper.getSingleResource('LastTransfer'), show: true },
    { key: fields.Email, value: LangHelper.getSingleResource('Email'), show: true },
    { key: fields.CommissionGroup, value: LangHelper.getSingleResource('CommissionGroup'), show: true },
    { key: fields.CustomerClass, value: LangHelper.getSingleResource('CustomerClass'), show: true }
];

export const contextMenuKeys = {
    bankInformation: 'bankInformation',
    note: 'note',
    password: 'password',
    fundtransferLimit: 'fundtransferLimit',
    betSetting: 'betSetting',
    statement: 'statement',
    personalMessage: 'personalMessage',
    commissionGroup: 'commissionGroup'
}

export const tabNames = {
    playerInfo: 'lblPlayerAdminPlayerInfo',
    statement: 'lblPlayerAdminPlayerStatement',
    betlist: 'lblPlayerAdminBetList',
    betSetting: 'lblPlayerAdminBetSetting',
    setting: 'lblPlayerAdminSetting',
    fundTransferLimit: 'FundTransferLimit',
    password: 'Password',
    bankInfo: 'BankInfo',
    personalMessage: 'PersonalMessage',
    detailTransaction: 'lblPlayerDetailTransaction',
    commissionGroup: 'lblCommissionGroup'
}

export const excludedTabNameWithNonDepositAndArchived =
    [tabNames.statement, tabNames.betlist, tabNames.betSetting, tabNames.detailTransaction, tabNames.commissionGroup];

export const excludedTabNameWithClosed = [tabNames.commissionGroup];

export const contextMenuList = [
    { key: contextMenuKeys.note, name: LangHelper.getSingleResource('Note'), tabName: 'lblPlayerAdminNote' },
    { key: contextMenuKeys.password, name: LangHelper.getSingleResource('Password'), tabName: tabNames.setting, subTabName: tabNames.password },
    { key: contextMenuKeys.bankInformation, name: LangHelper.getSingleResource('BankInformation'), tabName: tabNames.setting, subTabName: tabNames.bankInfo },
    { key: contextMenuKeys.fundtransferLimit, name: LangHelper.getSingleResource('FundtransferLimit'), tabName: tabNames.setting, subTabName: tabNames.fundTransferLimit },
    { key: contextMenuKeys.betSetting, name: LangHelper.getSingleResource('BetSetting'), tabName: tabNames.betSetting },
    { key: contextMenuKeys.statement, name: LangHelper.getSingleResource('Statement'), tabName: tabNames.statement },
    { key: contextMenuKeys.personalMessage, name: LangHelper.getSingleResource('PersonalMessage'), tabName: 'lblPlayerAdminMessage', subTabName: tabNames.personalMessage },
    { key: contextMenuKeys.commissionGroup, name: LangHelper.getSingleResource('CommissionGroup'), tabName: 'lblCommissionGroup' }
]

export const heightSlicing = {
    default: 300,
    withAdvancedSearch: 357,
    whenCollapsing: 235
}

export const searchOptions = {
    matchPrefix: '1',
    matchCase: '2',
    matchWholeWord: '3',
    matchPrecise: '4'
}

export const defaultPagingOption = {
    PageIndex: 1,
    PageSize: 500,
    PageList: [500],
    TotalItem: null
}

export const playerStatus = {
    '0': LangHelper.getSingleResource('Active'),
    '1': LangHelper.getSingleResource('Locked'),
    '2': LangHelper.getSingleResource('Closed'),
    '3': LangHelper.getSingleResource('Archived')
}

export const playerStatusByName = {
    'Active': 0,
    'Locked': 1,
    'Closed': 2,
    'Archived': 3
}

export const displayedPlayerStatusList = {
    [playerStatusByName.Active]: [playerStatusByName.Closed, playerStatusByName.Locked],
    [playerStatusByName.Locked]: [playerStatusByName.Active, playerStatusByName.Closed]
}

export const messageUpdateState = {
    [playerStatusByName.Locked]: [LangHelper.getSingleResource('UpdateToLockedConfirmMsg'), LangHelper.getSingleResource('WarningToLockedMsg')],
    [playerStatusByName.Closed]: [LangHelper.getSingleResource('UpdateToClosedConfirmMsg'), LangHelper.getSingleResource('WarningToClosedMsg')],
    [playerStatusByName.Active]: [LangHelper.getSingleResource('UpdateToActiveConfirmMsg'), LangHelper.getSingleResource('WarningToActiveMsg')]
}

export const baStatusList = {
    'All': 0,
    'On': 1,
    'Off': 2
};

export const headerText = {
    Index: '',
    Status: LangHelper.getSingleResource('Status'),
    UserName: LangHelper.getSingleResource('Username'),
    SystemId: LangHelper.getSingleResource('SystemId'),
    VendorMemberId: LangHelper.getSingleResource('VendorMemberID'),
    RealName: LangHelper.getSingleResource('RealName'),
    Currency: LangHelper.getSingleResource('Currency'),
    CurrentBalance: LangHelper.getSingleResource('CurrentBalance'),
    OutstandingBalance: LangHelper.getSingleResource('OutstandingBalance'),
    OddsGroup: LangHelper.getSingleResource('OddsGroup'),
    BaStatus: LangHelper.getSingleResource('BAStatus'),
    SpecialLevel: LangHelper.getSingleResource('SpecialLevel'),
    LastTransaction: LangHelper.getSingleResource('LastTransfer'),
    Email: LangHelper.getSingleResource('Email'),
    CommissionGroup: LangHelper.getSingleResource('CommissionGroup'),
    CustomerClass: LangHelper.getSingleResource('CustomerClass')
}

export const gridItemDropdownHeight = {
    'specialLevelDropdown': 231,
    'oddsGroupDropdown': 126,
    'statusDropdown': 56
}

export const stringTypeFields = [fields.UserName, fields.RealName, fields.Currency, fields.OddsGroup];

export const apiInfo = {
    lrfSearch: { action: 'api/PlayerList/Search', type: systemType.playerAdmin },
    lrfDataById: { action: 'api/PlayerList/GetPlayerListById', type: systemType.playerAdmin },
    lapSearch: { action: 'PlayerAdminContainer/Search', type: systemType.newAdmin },
    lapDataById: { action: 'PlayerAdminContainer/GetPlayerListById', type: systemType.newAdmin },
    lapGetPlayerByLicenseeId: { action: 'PlayerAdminContainer/GetPlayerById', type: systemType.newAdmin }
}

export const apiUpdateInfo = {
    updateBaStatus: { action: 'api/PlayerBaStatus/Update', type: systemType.playerAdmin },
    updateOddsGroup: { action: 'api/PlayerOddsGroup/Update', type: systemType.playerAdmin },
    updateSpecialLevel: { action: 'api/PlayerSpecialLevel/Update', type: systemType.playerAdmin },
    updatePlayerStatus: { action: 'PlayerAdminContainer/UpdatePlayerStatus', type: systemType.newAdmin }
}

export const apiSiteSetting = { type: systemType.playerAdmin, action: 'api/SiteSetting/GetSetting' };

export const apiSuggestion = { action: 'PlayerAdminContainer/GetSuggestionList', type: systemType.newAdmin };

export const excludedContextMenuWithNonDeposit =
    [contextMenuKeys.betSetting, contextMenuKeys.statement, contextMenuKeys.commissionGroup];
export const excludedContextMenuWithClosed = [contextMenuKeys.password, contextMenuKeys.commissionGroup];
export const excludedContextMenuWithArchived =
    [contextMenuKeys.password, contextMenuKeys.betSetting, contextMenuKeys.statement, contextMenuKeys.commissionGroup];
export const excludedContextMenuWithApiLicensee =
    [contextMenuKeys.password, contextMenuKeys.bankInformation, contextMenuKeys.personalMessage];

export const isDepositedList = {
    'All': 2,
    'Yes': 1,
    'No': 0
};

export const emailPattern = '^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$';

export const maxLapPageSize = 250;
export const maxLapSearchItems = 10000;