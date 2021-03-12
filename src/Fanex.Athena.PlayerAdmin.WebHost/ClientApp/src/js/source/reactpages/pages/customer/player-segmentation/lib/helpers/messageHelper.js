import React from 'react';
import LangHelper from '@jsroot/lib/langHelper';
import { UpdateVIPStatusCode } from '../constants';

export default class MessageHelper {
    static MsgColors = {
        [UpdateVIPStatusCode.Success]: {
            content: 'msg-success',
            icon: 'icon-tick'
        },
        [UpdateVIPStatusCode.InvalidSystemId]: {
            content: 'msg-error',
            icon: 'icon-warning'
        },
        [UpdateVIPStatusCode.NoChangeBySystemId]: {
            content: 'msg-success',
            icon: 'icon-tick'
        },
        [UpdateVIPStatusCode.NoChangeByBAStatus]: {
            content: 'msg-success',
            icon: 'icon-tick'
        },
        [UpdateVIPStatusCode.Avoid]: {
            content: 'msg-error',
            icon: 'icon-warning'
        },
        [UpdateVIPStatusCode.InvalidExternalId]: {
            content: 'msg-error',
            icon: 'icon-warning'
        }
    }

    static GetMsgUpdateVipStatus = {
        singleUpdate: {
            [UpdateVIPStatusCode.Success]: _ => LangHelper.getSingleResource('SuccessUpdateVipStatusSingle'),

            [UpdateVIPStatusCode.InvalidSystemId]: data => LangHelper.getSingleResource('InvalidPlayerUpdateVipStatusSingle')
                .replace('{0}', data.ids),

            [UpdateVIPStatusCode.NoChangeBySystemId]: (_, isUpdateVip) => (isUpdateVip ? LangHelper.getSingleResource('AlreadyUpdateVipStatusSingle') : LangHelper.getSingleResource('AlreadyUpdateNotVipStatusSingle')),

            [UpdateVIPStatusCode.NoChangeByBAStatus]: () => LangHelper.getSingleResource('BAPlayerNoUpdateVIPSingle'),

            [UpdateVIPStatusCode.Avoid]: data => LangHelper.getSingleResource('RetractedPlayerUpdateVipStatusSingle')
                .replace('{0}', data.ids)
        },
        multipleUpdate: {
            [UpdateVIPStatusCode.Success]: data => LangHelper.getSingleResource('SuccessUpdateVipStatus')
                .replace('{0}', data.numberOfPlayer),

            [UpdateVIPStatusCode.InvalidSystemId]: data => LangHelper.getSingleResource('InvalidPlayerUpdateVipStatusBySystemId')
                .replace('{0}', data.numberOfPlayer)
                .replace('{1}', data.ids),

            [UpdateVIPStatusCode.NoChangeBySystemId]: (data, isUpdateVip) => (isUpdateVip ? LangHelper.getSingleResource('AlreadyUpdateVipStatus') : LangHelper.getSingleResource('AlreadyUpdateNotVipStatus'))
                .replace('{0}', data.numberOfPlayer),

            [UpdateVIPStatusCode.NoChangeByBAStatus]: data => LangHelper.getSingleResource('BAPlayerNoUpdateVIP')
                .replace('{0}', data.numberOfPlayer)
                .replace('{1}', data.ids),

            [UpdateVIPStatusCode.Avoid]: data => LangHelper.getSingleResource('RetractedPlayerUpdateVipStatus')
                .replace('{0}', data.numberOfPlayer)
                .replace('{1}', data.ids),

            [UpdateVIPStatusCode.InvalidExternalId]: data => LangHelper.getSingleResource('InvalidPlayerUpdateVipStatusByExternalId')
                .replace('{0}', data.numberOfPlayer)
                .replace('{1}', data.ids)
        }
    }

    static GetListMsgUpdateVipStatus = (msgResults, currentVipStatus, isSingleUpdate = false) => {
        const fieldName = isSingleUpdate ? 'singleUpdate' : 'multipleUpdate';
        const combinedResult = this.combineMsgResult(msgResults);
        return combinedResult.map(_ => {
            const code = _.code;
            const msgColor = MessageHelper.MsgColors[code];
            return (<p className="msg-result">
                <i className={`icon ${msgColor.icon}`}></i>
                <span className={msgColor.content}>{MessageHelper.GetMsgUpdateVipStatus[fieldName][code](_, currentVipStatus)}</span>
            </p>)
        })
    }

    static combineMsgResult = (msgResults) => {
        const msgResultExternalIdIndex = msgResults.findIndex(_ => _.code === UpdateVIPStatusCode.NoChangeByExternalId); // no change VIP status - by externalId
        if(msgResultExternalIdIndex >= 0) {
            msgResults = this.combineMsgResultWithStatusNoChange(msgResults, msgResultExternalIdIndex);
        }
        return msgResults;
    }

    static combineMsgResultWithStatusNoChange = (msgResults, msgResultExternalIdIndex) => {
        const msgResultSystemId = msgResults.find(_ => _.code === UpdateVIPStatusCode.NoChangeBySystemId); // no change VIP status - by systemId
        if(msgResultSystemId) {
            msgResultSystemId.numberOfPlayer += msgResults[msgResultExternalIdIndex].numberOfPlayer;
            msgResults.splice(msgResultExternalIdIndex, 1);
        } else {
            msgResults[msgResultExternalIdIndex].code = UpdateVIPStatusCode.NoChangeBySystemId;
        }
        return msgResults;
    }
}

