import LangHelper from '@jsroot/lib/langHelper';

export const HeaderText = {
    Index: "#",
    Platform: LangHelper.getSingleResource("Platform"),
    ExternalId: LangHelper.getSingleResource("ExternalId"),
    BetCount: LangHelper.getSingleResource("BetCount"),
    TurnOver: LangHelper.getSingleResource("Turnover"),
    NetTurnOver: LangHelper.getSingleResource("NetTurnover"),
    PlayerWL: LangHelper.getSingleResource("PlayerWL"),
    EffectiveBettingValue: LangHelper.getSingleResource("EffectiveBettingValue"),
    PlayerComm: LangHelper.getSingleResource("PlayerComm"),
    LicenseeComm: LangHelper.getSingleResource("LicenseeComm"),
    AmountDueToBA: LangHelper.getSingleResource("AmountDueToBA"),
    Margin: LangHelper.getSingleResource("Margin"),
    SystemId: LangHelper.getSingleResource("SystemId"),
    LicenseeWL: LangHelper.getSingleResource("LicenseeW_L"),
    GroupBy: LangHelper.getSingleResource("GroupBy")
}

export const OneTwoBetLicenseeName = '12Bet';

export const GroupByMode = {
    Date: 1,
    Month: 2,
    Product: 3,
    Sport: 4,
    BetType: 5,
    Currency: 6,
    Platform: 7,
    Player: 8,
    Agent: 9
}

export const BreakdownByMode = {
    Currency: 1,
    Product: 2,
    Player: 3
}

export const ALL_SELECTED_TEXT = 'ALL';

export const SortBy = {
    Default: 0,
    BetCount: 1,
    Turnover: 2,
    NetTurnover: 3,
    MemWinloss: 4,
    MemComm: 5,
    LicWinloss: 6,
    LicComm: 7,
    AmtDueToBA: 8,
    BetEffective: 9,
    Margin: 10
}

export const SortOrder = {
    ASC: 0,
    DESC: 1
}

export const SearchMode = {
    MatchPrecise: 1,
    MatchCase: 2,
    MatchPrefix: 3
}

export const ExportExcelMode = {
    All: 1,
    GroupByLevelOnly: 2
}