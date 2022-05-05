import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../../actions/index.js';
import { bindActionCreators } from 'redux';
import Select from 'react-select';
import LangHelper from '@jsroot/lib/langHelper';
import Utility from '../../../lib/utility';
import DataTransfer from '../../../lib/dataTransfer';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import { BetLimitProduct } from '../../../lib/constants';

export class ExchangeDataContent extends PureComponent {
    constructor(props) {
        super(props);

        this.state = { setting: null, commList: [], selectedCommission: null };
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
        const oldStatus = this.state.setting.StatusEnable;
        const newStatus = !this.state.setting.StatusEnable;

        this._showUpdateStatusConfirmation(() => this._updateStatus(oldStatus, newStatus), Utility.getConfirmationUpdateStatusMsg(BetLimitProduct.BX));
    }

    onUpdateChargeComm = () => {
        this._showUpdateStatusConfirmation(
            () => this._updateChargeComm(this.state.setting.Commission, this.state.selectedCommission), Utility.getConfirmationUpdateChargeCommMsg(BetLimitProduct.BX));
    }

    onCloseAlert = () => {
        this.props.setAlertOption({ isShow: false });
    }

    onRefresh = () => {
        this._getData(this.props.requestParam);
    }

    _updateState(data, setting) {
        if (setting) {
            this.setState({
                data,
                setting,
                selectedCommission: setting.Commission,
                commList: setting.ValidComms.map(_ => ({ value: _, label: _ + '%' }))
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

        const param = { ...this.props.requestParam, Status, OldStatus, productName };

        DataTransfer.updateStatus(param, this._showSubmitResult.bind(this));
    }

    _updateChargeComm(oldComm, comm) {
        this.props.toggleLoader(true);
        var productName = this.props.productLabel;

        const param = { ...this.props.requestParam, comm, oldComm, productName };

        DataTransfer.updateExchangeChargeComm(param, this._showSubmitResult.bind(this));
    }

    _showUpdateStatusConfirmation(onOK, confirmMsg) {
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

        const msg = (<p className="center-align">{confirmMsg}</p>);

        this.props.setAlertOption({
            isShow: true,
            title: LangHelper.getSingleResource('Confirmation'),
            children: msg,
            buttons: buttons,
            icon: '',
            onClose: this.onCloseAlert,
            height: 130
        });
    }

    _showSubmitResult(title, msg, isSuccess) {
        const content = (<p className="center-align">{msg}</p>);

        const buttons = [
            {
                text: LangHelper.getSingleResource('OK'),
                onClick: this.onRefresh,
                className: 'btn btn-apply btn-alone'
            }];

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
    }

    _renderCommList() {
        const { setting, commList, selectedCommission } = this.state;

        return <div className="col-12 col-md-4 col-sm-12 col-lg-3 col-xl-2">
            <div className="form-group">
                <label className="label-text">{LangHelper.getSingleResource('ChargeCommission')}</label>
                {setting.Editable && setting.StatusEnable ? <Select instanceId="ddlChargeCommission"
                    value={selectedCommission}
                    options={commList}
                    onChange={item => this.setState({ selectedCommission: item.value })} />
                    : <div style={{ width: '100%', borderBottom: '1px solid #ddd', color: '#aaa' }}>{selectedCommission + '%'}</div>
                }
            </div>
        </div>
    }

    render() {
        const { setting } = this.state;

        if (setting === null) {
            return <NoDataTitle />;
        }

        return (
            <div className="pa-betlimit">
                {
                    !!setting.Commission ? this._renderCommList() : <NoDataTitle title={LangHelper.getSingleResource('CannotUpdateBetLimitForProduct').replace("{0}", this.props.productLabel)} />
                }
                {setting.Editable &&
                    <div className="pa-betlimit-button">
                        {setting.StatusEnable && !!setting.Commission &&
                            <>
                                <button type='button' className='btn btn-cancel' onClick={this.onRefresh}>
                                    {LangHelper.getSingleResource('Reset')}
                                </button>
                                <button type='button' className='btn btn-apply' onClick={this.onUpdateChargeComm} ref={this.submitButtonRef}>
                                    {LangHelper.getSingleResource('Submit')}
                                </button>
                            </>}
                        <button type='button' className='btn btn-apply' onClick={this.onUpdateStatus}>
                            {setting.StatusEnable ? LangHelper.getSingleResource('DisableProduct').replace("{0}", this.props.productLabel) : LangHelper.getSingleResource('EnableProduct').replace("{0}", this.props.productLabel)}
                        </button>
                    </div>
                }
            </div>
        );
    }
}

function mapDispatchToProps(dispatch) {
    const { toggleLoader, setAlertOption } = ActionCreators;
    return bindActionCreators({ toggleLoader, setAlertOption }, dispatch);
}
function mapStateToProps(state) {
    const { requestParam, productLabel } = state;
    return Object.assign({}, { requestParam, productLabel });
};

export default connect(mapStateToProps, mapDispatchToProps)(ExchangeDataContent);