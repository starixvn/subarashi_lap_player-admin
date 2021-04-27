import React from 'react';
import langHelper from '../lib/langHelper';

const NoDataTitle = ({ title }) => {
    return (
        <p className="no-info-row center-align" dangerouslySetInnerHTML={{ __html: title ? title : langHelper.getSingleResource("NoInfo") }}></p>
    )
}

export default NoDataTitle;