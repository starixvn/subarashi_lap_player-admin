import React, { Component } from 'react';
import Popup from '@jsroot/components/popup';
import PropTypes from 'prop-types';
class BetListLayout extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isShowPopup: false,
            popupUrl: null,
            popupTitle: null
        };
        
        window.openPopup = this._openPopup.bind(this);
        window.getResource = this._getResource.bind(this);
        window.formatDateString = this._formatDateString.bind(this);
        window.beforeShow = this._beforeShow.bind(this);
        window.afterShow = this._afterShow.bind(this);
        window.getCommonParams = this._getCommonParams.bind(this);
    }

    componentDidMount() {
        window.onload = () => {
            window.logOpenBetlistEvent && window.logOpenBetlistEvent(this.props.pageName);
        };
    }

    _getResource(key) {
        return this.props.getResourceFunc ? this.props.getResourceFunc(key) : key;
    }

    _formatDateString(dateString) {
        return this.props.formatDateStringFunc ? this.props.formatDateStringFunc(dateString) : dateString;
    }

    _beforeShow() {
        this.props.beforeShowFunc && this.props.beforeShowFunc();
    }

    _afterShow() {
        this.props.afterShowFunc && this.props.afterShowFunc();
    }

    _openPopup(url, title) {
        this.setState({ isShowPopup: true, popupUrl: url, popupTitle: title });
    }

    _closePopup() {
        this.setState({ isShowPopup: false });
    }

    _getCommonParams() {
        return { baseCurrency: this.props.baseCurrency };
    }

    render() {
        return (
            <div className="betlist-layout">
                {this.props.children}
                {this.state.isShowPopup &&
                    <Popup isShow={true}
                        iframeUrl={this.state.popupUrl}
                        title={this.state.popupTitle}
                        width={900} height={600}
                        onClose={() => this._closePopup()}
                    >
                    </Popup>}
            </div>
        );
    }
}

BetListLayout.propTypes = {
    pageName: PropTypes.string.isRequired,
    rootUrl: PropTypes.string.isRequired,
    getResourceFunc: PropTypes.func,
    baseCurrency: PropTypes.string,
    beforeShowFunc: PropTypes.func,
    afterShowFunc: PropTypes.func,
    formatDateStringFunc: PropTypes.func
}

BetListLayout.defaultProps = {
    rootUrl: '/',
    baseCurrency: ''
}

export default BetListLayout
