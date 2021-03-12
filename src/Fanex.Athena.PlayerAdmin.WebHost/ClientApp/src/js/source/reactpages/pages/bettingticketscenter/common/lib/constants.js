import LangHelper from '@jsroot/lib/langHelper';
import Site from '@jsroot/lib/site';

export const ApiUrl = {
    CUSTOMER_SEARCH_URL: Site.resolveClientUrl("SearchCustomerAPI/SearchCustomer"),
    RUNNING_DATA_URL: Site.resolveClientUrl('api/BettingTicketCenter/GetByRunning'),
    REFNO_DATA_URL: Site.resolveClientUrl('api/BettingTicketCenter/GetByRefNo'),
    SINGLECUSTOMER_DATA_URL: Site.resolveClientUrl('api/BettingTicketCenter/GetBySingleCustomer'),
    ADVANCEDSEARCH_DATA_URL: Site.resolveClientUrl('api/BettingTicketCenterAdvancedSearch/GetData')
}

export const HeaderText = {
    Index: "#",
    Platform: LangHelper.getSingleResource("Platform"),
    ExternalId: LangHelper.getSingleResource("ExternalId"),
    SystemId: LangHelper.getSingleResource("SystemId"),
    RefNo: LangHelper.getSingleResource("RefNo"),
    TransTime: LangHelper.getSingleResource("TransTime"),
    LicenseeTxId: LangHelper.getSingleResource("LicenseeTxId"),
    Choice: LangHelper.getSingleResource("Choice"),
    Odds: LangHelper.getSingleResource("Odds"),
    OddsType: LangHelper.getSingleResource("OddsType"),
    Stake: LangHelper.getSingleResource("Stake"),
    WinLoss: LangHelper.getSingleResource("WinLoss"),
    LicenseeWL: LangHelper.getSingleResource("LicenseeWL"),
    Comm: LangHelper.getSingleResource("Comm"),
    Status: LangHelper.getSingleResource("Status"),
    Live: LangHelper.getSingleResource("Live")
}

export const DefaultPageSize = 50;

export const tabKeys = {
    advancedSearch: 'BTC_MultiCustomer',
    singleCustomer: 'BTC_SingleCustomer',
    refNo: 'BTC_RefNo',
    running: 'BTC_Running'
}

export const tabList = [
    {
        key: tabKeys.singleCustomer,
        label: LangHelper.getSingleResource("SingleCustomer"),
        url: Site.resolveClientUrl("BettingTicketCenter/SingleCustomer"),
        displayOrder: 0
    },
    {
        key: tabKeys.refNo,
        label: LangHelper.getSingleResource("RefNo"),
        url: Site.resolveClientUrl("BettingTicketCenter/RefNo"),
        displayOrder: 1
    },
    {
        key: tabKeys.running,
        label: LangHelper.getSingleResource("Running"),
        url: Site.resolveClientUrl("BettingTicketCenter/Running"),
        displayOrder: 2
    },
    {
        key: tabKeys.advancedSearch,
        label: LangHelper.getSingleResource("AdvancedSearch"),
        url: Site.resolveClientUrl("BettingTicketCenter/AdvancedSearch"),
        displayOrder: 3
    }
]