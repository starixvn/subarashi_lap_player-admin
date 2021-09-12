import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../actions/index.js';
import { bindActionCreators } from 'redux';
import ClientResourceManager from '@jsroot/lib/clientResourceManager';

export const ActivityRecord = ({ ibcCustId, createdDate, getActivityRecord }) => {
    const [data, setData] = useState(null);

    useEffect(() => {
        if (data === null) {
            const param = { custId: ibcCustId };
            getActivityRecord(param).then(result => setData(result));
        }
    }, [ibcCustId]);

    const { LastTxn, LastUpdateBAString, BAUpdatedBy, LastUpdateSpecialLevelString, SpecialLevelUpdateBy } = data || {};

    return (
        <div className="left-side-content activity-record-container">
            <table className="table">
                <tbody>
                    <tr>
                        <td className="first-cell">{ClientResourceManager.getText('CreatedDate')}</td>
                        <td>{createdDate}</td>
                    </tr>
                    <tr>
                        <td>{ClientResourceManager.getText('LastTransaction')}</td>
                        <td>{LastTxn}</td>
                    </tr>
                    <tr>
                        <td colSpan="2"></td>
                    </tr>
                    <tr>
                        <td>{ClientResourceManager.getText('BAUpdatedBy')}</td>
                        <td>{BAUpdatedBy}</td>
                    </tr>
                    <tr>
                        <td>{ClientResourceManager.getText('LastUpdatedBA')}</td>
                        <td>{LastUpdateBAString}</td>
                    </tr>
                    <tr>
                        <td colSpan="2"></td>
                    </tr>
                    <tr>
                        <td>{ClientResourceManager.getText('SpecialLevelUpdatedBy')}</td>
                        <td>{SpecialLevelUpdateBy}</td>
                    </tr>
                    <tr>
                        <td>{ClientResourceManager.getText('LastUpdatedSpecialLevel')}</td>
                        <td>{LastUpdateSpecialLevelString}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { getActivityRecord } = ActionCreators;
    return bindActionCreators({ getActivityRecord }, dispatch);
}

function mapStateToProps(state) {
    const { ibcCustId, createdDate } = state;
    return Object.assign({}, { ibcCustId, createdDate });
};

export default connect(mapStateToProps, mapDispatchToProps)(ActivityRecord);