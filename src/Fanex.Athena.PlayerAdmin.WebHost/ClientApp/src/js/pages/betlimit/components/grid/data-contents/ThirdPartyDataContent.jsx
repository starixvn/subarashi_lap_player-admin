import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../../actions/index.js';
import { bindActionCreators } from 'redux';
import LangHelper from '@jsroot/lib/langHelper';
import Utility from '../../../lib/utility';
import DataTransfer from '../../../lib/dataTransfer';
import NoDataTitle from '@jsroot/components/NoDataTitle';

export class RNGCDataContent extends PureComponent {
    constructor(props) {
        super(props);

        this.state = { setting: null, groups: [], selectedGroup: '', disabledButton: false };
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevState.setting !== this.state.setting) {
            this.props.toggleLoader(false);
            this.props.setAlertOption({ isShow: false });
        }
    }

    componentDidMount() {
        this._getData(this.props.requestParam);
    }

    onUpdateStatus = () => {
        if (!this.state.showError) {
            const oldStatus = this.state.setting.StatusEnable;
            const newStatus = !this.state.setting.StatusEnable;

            this._showUpdateStatusConfirmation(() => this._updateStatus(oldStatus, newStatus));
        }
    }

    onCloseAlert = () => {
        this.setState({ disabledButton: false }, () => this.props.setAlertOption({ isShow: false }));
    }

    onRefresh = () => {
        this._getData(this.props.requestParam);
    }

    _isDisabledButton(data, selectedGroup) {
        if (!selectedGroup) {
            return true;
        }

        const gamesHaveGroup = data.filter(x => x.Groups.find(g => g.GroupType === selectedGroup));

        return !gamesHaveGroup || gamesHaveGroup.length !== data.length;
    }

    _updateState(data, setting) {
        if (setting) {
            this.setState({
                data: data,
                setting: setting
            });
        }
    }

    _getData(param) {
        this.props.toggleLoader(true);

        DataTransfer.getData(param, this._updateState.bind(this));
    }

    _updateStatus(OldStatus, Status) {
        this.props.toggleLoader(true);
        var productName = this.props.productLabel;

        const param = {...this.props.requestParam, Status, OldStatus, productName};

        DataTransfer.updateStatus(param, this._showSubmitResult.bind(this));
    }

    _showUpdateStatusConfirmation(onOK) {
        const buttons = [
            {
                text: LangHelper.getSingleResource('OK'),
                onClick: onOK,
                className: 'btn btn-apply'
            }, {
                text: LangHelper.getSingleResource('Cancel'),
                onClick: this.onCloseAlert,
                className: 'btn btn-cancel'
            }];

        const msg = (<p className="center-align">{Utility.getConfirmationUpdateStatusMsg(this.props.requestParam.ProductId)}</p>);

        this.setState({ disabledButton: !this.state.setting.StatusEnable }, () => this.props.setAlertOption({
            isShow: true,
            title: LangHelper.getSingleResource('Confirmation'),
            children: msg,
            buttons: buttons,
            icon: '',
            onClose: this.onCloseAlert,
            height: 130
        }));
    }

    _showSubmitResult(title, msg, isSuccess) {
        const content = (<p className="center-align">{msg}</p>);

        const buttons = [
            {
                text: LangHelper.getSingleResource('OK'),
                onClick: this.onRefresh,
                className: 'btn btn-apply btn-alone'
            }];

        this.setState({ disabledButton: true }, () => {
            this.props.setAlertOption({
                isShow: true,
                title: title,
                children: content,
                onClose: this.onRefresh,
                icon: isSuccess ? 'success' : 'error',
                height: 225,
                buttons: buttons
            });
            this.props.toggleLoader(false);
        });
    }

    render() {
        const { setting } = this.state;

        if (setting === null) {
            return <NoDataTitle />;
        }

        return (
            <div className="pa-betlimit">
                {
                     setting.IsStatusEnable ? <NoDataTitle title={LangHelper.getSingleResource('CannotUpdateBetLimitForProduct').replace("{0}", this.props.productLabel)} /> : <NoDataTitle/>
                }
                {
                    <div className="pa-betlimit-button">
                        {setting.IsStatusEnable && <button type='button' className='btn btn-apply' onClick={this.onUpdateStatus}>
                        {setting.StatusEnable ? LangHelper.getSingleResource('DisableProduct').replace("{0}", this.props.productLabel) :  LangHelper.getSingleResource('EnableProduct').replace("{0}", this.props.productLabel)}
                        </button>}                  
                    </div>
                }
            </div>
        );
    }
}

function mapDispatchToProps(dispatch) {
    const { toggleLoader, setAlertOption  } = ActionCreators;
    return bindActionCreators({ toggleLoader, setAlertOption  }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam, productLabel } = state;
    return Object.assign({}, { requestParam, productLabel });
};

export default connect(mapStateToProps, mapDispatchToProps)(RNGCDataContent);