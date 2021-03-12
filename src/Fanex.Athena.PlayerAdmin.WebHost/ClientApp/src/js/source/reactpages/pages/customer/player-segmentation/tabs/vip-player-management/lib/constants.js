import LangHelper from '@jsroot/lib/langHelper';

export const dataGridHeaders = [
    { key: 'subsidiary', text: LangHelper.getSingleResource('Subsidiary'), colWidth: 100 },
    { key: 'externalId', text: LangHelper.getSingleResource('ExternalId'), sortBy: 'externalId', colWidth: 100 },
    { key: 'systemId', text: LangHelper.getSingleResource('SystemId'), sortBy: 'systemId', colWidth: 100 },
    { key: 'customerClass', text: LangHelper.getSingleResource('CustomerClass'), colWidth: 100, sortBy: 'customerClass' },
    { key: 'vip', text: LangHelper.getSingleResource('Vip'), colWidth: 100 }
]

export const Url = {
    vipPlayerListTemplate: 'PlayerSegmentation/DowloadVipListTemplate',
    exportVipPlayerList: 'Api/PlayerSegmentation/ExportVipPlayerList'
}

export const ExcelReaderColumn = {
    SystemId: 'systemid',
    ExternalId: 'externalid'
}
