import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators, compose } from 'redux';
import { ActionCreators } from '../../../actions/index';
import LangHelper from '@jsroot/lib/langHelper';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import DataTransfer from '../../../lib/dataTransfer';

import Utility from '../../../lib/utility';
import CheckBox from '@jsroot/components/CheckBox';
import Balloon from '@jsroot/components/Balloon';

const SettingContent = (Grid, DataHandling) => {
    return class extends PureComponent {
        constructor(props) {
            super(props);

            this.state = {
                data: null,
                setting: { Editable: false, EnableCopy: false, CopyId: 0 },
                disabledButton: false,
                isCopy: false,
                showError: false,
                errorBalloon: { msg: '', targetTop: 0, targetLeft: 0 }
            };

            this.ref = {};
            this.checkBoxRef = React.createRef();
            this.errorRef = React.createRef();
            this.submitButtonRef = React.createRef();
            this.getHeaderName = DataHandling.getHeaderName;
        }

        componentDidUpdate(prevProps, prevState) {
            if (prevState.timeStamp !== this.state.timeStamp) {
                this.props.toggleLoader(false);
                this.props.setAlertOption({ isShow: false });
            }

            if (prevProps.requestParam !== this.props.requestParam) {
                this._getData(this.props.requestParam);
            }
        }

        componentWillUnmount() {
            this._unRegisterEvents();
        }

        componentDidMount() {
            this._getData(this.props.requestParam);
            this._registerEvents();
        }

        _registerEvents() {
            document.addEventListener('click', this._handleClickOutSide);
        }

        _unRegisterEvents = () => {
            document.removeEventListener('click', this._handleClickOutSide);
        }

        _handleClickOutSide = (event) => {
            if (this.state.showError) {
                const isError = this.errorRef.current && this.errorRef.current.contains(event.target);
                const isCheckBox = this.checkBoxRef.current && this.checkBoxRef.current.contains(event.target);
                const isButton = this.submitButtonRef.current && this.submitButtonRef.current.contains(event.target);
                const clickOutside = !isError && !isCheckBox && !isButton;

                if (clickOutside) {
                    this._closeError();
                }
            }
        }

        setInputRef = (type, id, element) => {
            this.ref[type] = this.ref[type] || {};
            this.ref[type][id] = element;
        }

        _closeError(callback) {
            this.setState({ showError: false }, callback);
        }

        onReset = () => {
            if (!this.state.disabledButton) {
                this.setState({ isCopy: false }, () => this._getData(this.props.requestParam));
            }
        }

        onChange = (data) => {
            this.setState({ data: data });
        }

        onSubmit = () => {
            this._closeError(this.submitData);
        }

        submitData = () => {

            if (!this.state.showError) {
                const data = this.state.data;
                const submitData = DataHandling.getSubmitData(data);

                if (!submitData.IsValid) {
                    this._showError(submitData.Data);
                }
                else {
                    this._showConfirmation(() => this._submit(submitData.Data));
                }

            }
        }

        onUpdateStatus = () => {
            if (!this.state.showError) {
                const oldStatus = this.state.setting.StatusEnable;
                const newStatus = !this.state.setting.StatusEnable;

                this._showUpdateStatusConfirmation(() => this._updateStatus(oldStatus, newStatus));
            }
        }

        onCheck = (checked) => {
            if (checked) {
                const { setting, errorBalloon, showError } = this.state;

                if (showError && errorBalloon.Id === setting.CopyId) {
                    return false;
                }

                const { data } = this.state;
                const copyRow = data.find(e => e.Id === setting.CopyId);
                const copyValue = DataHandling.getSubmitRowData(copyRow);
                const invalidData = DataHandling.validate(copyValue, copyRow);

                if (invalidData !== null) {
                    this._showError(invalidData);
                }
                else {
                    const newData = data.map(e => {
                        if (e.Id === copyRow.id) {
                            return e;
                        }

                        return Object.assign({}, e, { MinBet: copyRow.MinBet, MaxBet: copyRow.MaxBet, MaxPerMatch: copyRow.MaxPerMatch });
                    });

                    this.setState({ isCopy: true, data: newData });
                }
            }
            else {
                this.setState({ isCopy: false });
            }
        }

        onCloseAlert = () => {
            this.setState({ disabledButton: false }, () => this.props.setAlertOption({ isShow: false }));
        }

        onRefresh = () => {
            this._getData(this.props.requestParam);
        }

        _submit(data) {
            this.props.toggleLoader(true);

            const param = Object.assign({}, this.props.requestParam, { Setting: data });

            DataTransfer.updateData(param, this._showSubmitResult.bind(this));
        }

        _updateStatus(OldStatus, Status) {
            this.props.toggleLoader(true);
            var productName = this.props.productLabel;

            const param = { ...this.props.requestParam, Status, OldStatus, productName };

            DataTransfer.updateStatus(param, this._showSubmitResult.bind(this));
        }

        _showConfirmation(onOK) {
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

            const msg = (<p className="center-align">{Utility.getConfirmationMsg(this.props.requestParam.ProductId)}</p>);

            this.setState({ disabledButton: true }, () => this.props.setAlertOption({
                isShow: true,
                title: LangHelper.getSingleResource('Confirmation'),
                children: msg,
                buttons: buttons,
                icon: '',
                onClose: this.onCloseAlert,
                height: 130
            }));
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

        _showError(invalidData) {
            const invalidInput = this.ref[invalidData.Type][invalidData.Id];

            if (invalidInput) {
                const pos = invalidInput.getBoundingClientRect();

                this.setState({
                    showError: true,
                    errorBalloon: { Id: invalidData.Id, msg: invalidData.Message, targetTop: pos.top + window.scrollY, targetLeft: pos.left + pos.width / 2 }
                }, () => invalidInput.focus());
            }
        }

        _refreshState(data, setting) {
            this.setState((state) => ({
                data: DataHandling.getViewData(data),
                setting: Object.assign({}, state.setting, setting),
                disabledButton: false,
                isCopy: false,
                showError: false,
                timeStamp: new Date().getTime()
            }));
        }

        _getData(param) {
            this.props.toggleLoader(true);

            DataTransfer.getData(param, this._refreshState.bind(this));
        }

        render() {
            const data = this.state.data;

            const { setting, disabledButton, showError, errorBalloon, isCopy } = this.state;

            const noSetting = !data || !data.length;

            if (!setting.IsStatusEnable && noSetting) {
                return <NoDataTitle />;
            }

            return (
                <div className="pa-betlimit">
                    {
                        setting.EnableCopy && setting.Editable && !noSetting &&
                        <CheckBox id="IsCopy"
                            checked={isCopy}
                            onChange={this.onCheck}
                            ref={this.checkBoxRef}
                            label={LangHelper.getSingleResource("CopyBetSettingFromSoccer")} />
                    }
                    {!noSetting ? <Grid
                        data={data}
                        setting={setting}
                        setInputRef={this.setInputRef}
                        onChange={this.onChange}
                        isCopy={isCopy}
                        getHeaderName={this.getHeaderName} /> : <NoDataTitle title={LangHelper.getSingleResource('CannotUpdateBetLimitForProduct').replace("{0}", this.props.productLabel)} />}
                    {
                        <div className="pa-betlimit-button">
                            {setting.Editable && !noSetting && <>
                                <button type='button' className='btn btn-cancel' disabled={disabledButton} onClick={this.onReset}>
                                    {LangHelper.getSingleResource('Reset')}
                                </button>
                                <button type='button' className='btn btn-apply' disabled={disabledButton} onClick={this.onSubmit} ref={this.submitButtonRef}>
                                    {LangHelper.getSingleResource('Submit')}
                                </button> </>}
                            {setting.IsStatusEnable && <button type='button' className='btn btn-apply' onClick={this.onUpdateStatus}>
                                {setting.StatusEnable ? LangHelper.getSingleResource('DisableProduct').replace("{0}", this.props.productLabel) : LangHelper.getSingleResource('EnableProduct').replace("{0}", this.props.productLabel)}
                            </button>}
                        </div>
                    }
                    {
                        setting.Editable && !noSetting &&
                        <Balloon
                            width={140}
                            isOpen={showError}
                            ref={this.errorRef}
                            {...errorBalloon}
                        />
                    }
                </div>
            );
        }
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

const createSetting = compose(connect(mapStateToProps, mapDispatchToProps), SettingContent);

export default createSetting;