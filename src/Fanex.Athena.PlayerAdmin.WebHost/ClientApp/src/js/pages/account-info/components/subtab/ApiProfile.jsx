import React from 'react';
import ClientResourceManager from '@jsroot/lib/clientResourceManager';
import { SubsidiaryViewType } from '@jsroot/constants/report'

const ApiProfile = ({ data, subsidiaryViewType }) => {
    const { MemberId, Whitelabel, FirstName, LastName, OperatorId, SiteCode, ProductCode,
        CustomInfo1, CustomInfo2, CustomInfo3, CustomInfo4, CustomInfo5, ExchangeUserId } = data || {};

    return (
        <tbody>
            <tr>
                <td className="first-cell">{ClientResourceManager.getText('MemberId')}</td>
                <td>{MemberId}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('FirstName')}</td>
                <td>{FirstName}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('LastName')}</td>
                <td>{LastName}</td>
            </tr>
            {subsidiaryViewType !== SubsidiaryViewType.NOT_MANAGEMENT && <tr>
                <td>{ClientResourceManager.getText('OperatorId')}</td>
                <td>{OperatorId}</td>
            </tr>}
            {subsidiaryViewType === SubsidiaryViewType.MASTER_MANAGEMENT && <tr>
                <td>{ClientResourceManager.getText('Whitelabel')}</td>
                <td>{Whitelabel}</td>
            </tr>}
            <tr>
                <td colSpan="2"></td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('SiteCode')}</td>
                <td>{SiteCode}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('ProductCode')}</td>
                <td>{ProductCode}</td>
            </tr>
            <tr>
                <td colSpan="2"></td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('CustomInfo1')}</td>
                <td>{CustomInfo1}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('CustomInfo2')}</td>
                <td>{CustomInfo2}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('CustomInfo3')}</td>
                <td>{CustomInfo3}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('CustomInfo4')}</td>
                <td>{CustomInfo4}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('CustomInfo5')}</td>
                <td>{CustomInfo5}</td>
            </tr>
            <tr>
                <td>{ClientResourceManager.getText('ExchangeUserId')}</td>
                <td>{ExchangeUserId}</td>
            </tr>
        </tbody>
    );
}

export default ApiProfile;