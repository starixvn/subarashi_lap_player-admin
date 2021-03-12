import LangHelper from '@jsroot/lib/langHelper';

export const fields = {
    index: 'index',
    refNo: 'refNo',
    eventType: 'eventType',
    event: 'event',
    status: 'status'
}

export const headerText = {
    [fields.refNo]: LangHelper.getSingleResource('RefNo'),
    [fields.eventType]: LangHelper.getSingleResource('EventType'),
    [fields.event]: LangHelper.getSingleResource('Event'),
    [fields.status]: LangHelper.getSingleResource('Status')
}

export const VOIDED_STATUS = "Void";