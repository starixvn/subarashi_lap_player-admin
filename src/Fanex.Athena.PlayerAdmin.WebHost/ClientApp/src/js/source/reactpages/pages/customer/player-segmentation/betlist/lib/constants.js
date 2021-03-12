import LangHelper from '@jsroot/lib/langHelper';

export const HeaderText = {
    Index: "#",
    RefNo: LangHelper.getSingleResource("RefNo"),
    TransTime: LangHelper.getSingleResource("TransTime"),
    LicenseeTxId: LangHelper.getSingleResource("LicenseeTxId"),
    Choice: LangHelper.getSingleResource("Choice"),
    Odds: LangHelper.getSingleResource("Odds"),
    Stake: LangHelper.getSingleResource("Stake"),
    WinLoss: LangHelper.getSingleResource("WinLoss"),
    LicenseeWL: LangHelper.getSingleResource("LicenseeWL"),
    Comm: LangHelper.getSingleResource("Comm"),
    Status: LangHelper.getSingleResource("Status")
}

export const Columns = [
    'Index', 'RefNo', 'TransTime', 'Choice', 'Odds', 'Stake', 'WinLoss', 'LicenseeWL', 'Status'
]

export const ColumnWidths = [
    40, 100, 150, 250, 70, 80, 100, 100, 70
]

export const ColumnAlignments = [
    'center-align', 'center-align', 'center-align', 'right-align', 'right-align', 'right-align', 'right-align', 'right-align', 'center-align'
]

export const DefaultPagingOption = {
    PageIndex: 1,
    PageSize: 50,
    PageList: [50, 100, 200, 300, 400, 500, 1000, 2000],
    TotalItem: null
}

export const Url = {
    AbnormalBetList: 'Api/PlayerSegmentation/AbnormalBets'
}
