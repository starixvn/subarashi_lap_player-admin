import React, { useState } from 'react';
import LangHelper from '@jsroot/lib/langHelper';
import { Cell } from '@9softstudio/react-grid';
import Popup from '@jsroot/components/popup/Popup';
import FactoryCommonService from '@jsroot/lib/factoryCommonService';

export const RiskLevelCell = (props) => {
    const riskLevel = props.riskLevel;
    const riskLevelKey = props.riskLevelKey;
    const customerClass = !props.customerClass ? "" : props.customerClass;
    const currencyId = props.currencyId;
    const [isShowRiskLevelInformation, setIsShowRiskLevelInformation] = useState(false);
    const commonService = FactoryCommonService.createInstance();
    const lrfSystemId = 1;
    var url = "RiskLevel/Information" + `?customerClass=${customerClass}` + `&currencyId=${currencyId}` +"&time=" + (new Date().getTime());

    const clickOnPopup = (e) => {
        e.stopPropagation();
        return false;
    };

    return (<><Cell>
                <button type="button" className={`risk-level ${riskLevelKey}`} onClick={(e) => {setIsShowRiskLevelInformation(true)}}>
                    <div className="text">{riskLevel}</div>
                    <div className={`icon icon-moreinfo ${riskLevelKey}`}></div>
                </button>
                {isShowRiskLevelInformation
                ? (<Popup width={1000} height={600} isShow="true"
                    title={LangHelper.getSingleResource('RiskLevelInformation')}
                    onClose={(e) => {setIsShowRiskLevelInformation(false)}}
                    onClick={clickOnPopup} isHtml="false"
                    iframeUrl={commonService.getLink(lrfSystemId, url)}></Popup>)
                : ""}
            </Cell>
            </>)
}

export default RiskLevelCell;