import LangHelper from '@jsroot/lib/langHelper';

export const HeaderText = {
    Index: "#",
    Platform: LangHelper.getSingleResource("Platform"),
    ExternalId: LangHelper.getSingleResource("ExternalId"),
    SystemId: LangHelper.getSingleResource("SystemId"),
    RefNo: LangHelper.getSingleResource("RefNo"),
    LicenseeTxId: LangHelper.getSingleResource("LicenseeTxId"),
    TransTime: LangHelper.getSingleResource("TransTime"),
    Choice: LangHelper.getSingleResource("Choice"),
    Odds: LangHelper.getSingleResource("Odds"),
    OddsType: LangHelper.getSingleResource("OddsType"),
    Stake: LangHelper.getSingleResource("Stake"),
    Status: LangHelper.getSingleResource("Status"),
    Live: LangHelper.getSingleResource("Live"),
    WinLoss: LangHelper.getSingleResource("WinLoss"),
    LicenseeWL: `${LangHelper.getSingleResource("LicenseeWL")}/${LangHelper.getSingleResource("Comm")}`,
    Comm: LangHelper.getSingleResource("Comm")
}