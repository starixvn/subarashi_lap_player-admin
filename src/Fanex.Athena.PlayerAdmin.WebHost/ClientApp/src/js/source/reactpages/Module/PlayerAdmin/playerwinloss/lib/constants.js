import LangHelper from '@jsroot/lib/langHelper';

export const headerText = {
    Index: '#',
    Turnover: LangHelper.getSingleResource('Turnover'),
    TotalTransaction: LangHelper.getSingleResource('TotalTransaction'),
    WinLoss: LangHelper.getSingleResource('CustomerW_L'),
    Comm: LangHelper.getSingleResource('Comm'),
    Total: LangHelper.getSingleResource('Total'),
    BetEffective: LangHelper.getSingleResource('EffectiveBettingValue'),
    LicenseeWL: LangHelper.getSingleResource('LicenseeW_L'),
    BAComm: LangHelper.getSingleResource('BACommission'),
    AmtDueToBA: LangHelper.getSingleResource('AmtDueToBA'),
    CustomerWinLoss: LangHelper.getSingleResource('CustomerW_L')
}

export const winLossText = `${LangHelper.getSingleResource('Win')}/${LangHelper.getSingleResource('Loss')}`;