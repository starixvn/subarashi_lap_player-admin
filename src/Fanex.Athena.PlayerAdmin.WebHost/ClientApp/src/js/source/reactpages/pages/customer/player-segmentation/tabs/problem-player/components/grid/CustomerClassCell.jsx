import React from 'react';
import LangHelper from '@jsroot/lib/langHelper';
import Popup from '@jsroot/components/popup/Popup';

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
        var url = "CustomerClass/CustomerClassInformation?time=" + (new Date().getTime());
        return (<>
            {this.props.cell.text}&nbsp;&nbsp;<div class="icon-info" onClick={this.showCustomerClassInformation}></div>
            <Popup width={1000} height={600} isShow={this.state.isShowCustomerClassInformation}
                title={LangHelper.getSingleResource('CustomerClassInformation')}
                onClose={this.closeCustomerClassInfomation}
                onClick={this.clickOnPopup} isHtml="false"
                iframeUrl={url}></Popup>
        </>);
    }
}
