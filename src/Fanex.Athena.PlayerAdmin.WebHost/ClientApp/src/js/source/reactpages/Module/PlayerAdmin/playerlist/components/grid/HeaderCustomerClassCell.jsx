import React from 'react';
import LangHelper from '@jsroot/lib/langHelper';
import Popup from '@jsroot/components/popup/Popup';
import FactoryCommonService from '@jsroot/Module/lib/factoryCommonService';

export default class CustomerClassCell extends React.Component {
    constructor(props) {
        super(props);

        this.state = { isShowCustomerClassInformation: false }
    };

    showCustomerClassInformation = (e) => {
        this.setState({ isShowCustomerClassInformation: true });
        e.stopPropagation();
        return false;
    };

    closeCustomerClassInfomation = () => {
        this.setState({ isShowCustomerClassInformation: false });
    };

    clickOnPopup = (e) => {
        e.stopPropagation();
        return false;
    };

    render() {
        const commonService = FactoryCommonService.createInstance();
        const lrfSystemId = 1;
        var url = "CustomerClass/CustomerClassInformation?time=" + (new Date().getTime());

        return (<>
            {this.props.text}&nbsp;&nbsp;<div class="icon-info" onClick={this.showCustomerClassInformation}></div>
            <Popup width={1000} height={600} isShow={this.state.isShowCustomerClassInformation}
                title={LangHelper.getSingleResource('CustomerClassInformation')}
                onClose={this.closeCustomerClassInfomation}
                onClick={this.clickOnPopup} isHtml="false"
                iframeUrl={commonService.getLink(lrfSystemId, url)}></Popup>
        </>);
    }
}
