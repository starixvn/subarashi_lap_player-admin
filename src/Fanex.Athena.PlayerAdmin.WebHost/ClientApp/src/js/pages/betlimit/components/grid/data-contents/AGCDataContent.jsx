import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../../../actions/index.js';
import { bindActionCreators } from 'redux';
import LangHelper from '@jsroot/lib/langHelper';
import Utility from '../../../lib/utility';
import DataTransfer from '../../../lib/dataTransfer';
import Select from 'react-select';
import AGCSettingGrid from '../sport-settings/AGCSettingGrid';
import NoDataTitle from '@jsroot/components/NoDataTitle';

export class AGCDataContent extends PureComponent {
    constructor(props) {
        super(props);

        this.state = { data: null, setting: {}, groups: [], selectedGroup: '', disabledButton: false };
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevState.data !== this.state.data) {
            this.props.toggleLoader(false);
            this.props.setAlertOption({ isShow: false });
        }
    }

    componentDidMount() {
        this._getData(this.props.requestParam);
    }

    onGroupChange = (item) => {
        if (this.state.selectedGroup !== item.value) {
            const { data } = this.state;
            const disabledButton = this._isDisabledButton(data, item.value);

            this.setState({ selectedGroup: item.value, disabledButton: disabledButton });
        }
    }

    onReset = () => {
        this._getData(this.props.requestParam);
    }

    onSubmit = () => {
        const { selectedGroup, data, setting } = this.state;
        const invalid = this._isDisabledButton(data, selectedGroup);

        if (invalid) {
            this.setState({ disabledButton: true });
        }
        else {
            this._showConfirmation(() => this._submit(selectedGroup, setting.BetSetting.AgGroup));
        }
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
        if (setting.BetSetting) {
            const groups = setting.BetSetting ? setting.BetSetting.CheckedGroups.map(x => { return { value: x, label: x }; }) : [];
            const disabledButton = this._isDisabledButton(data, setting.BetSetting.AgGroup);

            this.setState({
                data: data,
                setting: setting,
                groups: groups,
                selectedGroup: setting.BetSetting.AgGroup,
                disabledButton: disabledButton
            });
        }
    }

    _getData(param) {
        this.props.toggleLoader(true);

        DataTransfer.getData(param, this._updateState.bind(this));
    }

    _submit(group, oldGroup) {
        this.props.toggleLoader(true);

        const param = Object.assign({}, this.props.requestParam, { Setting: { group, oldGroup } });

        DataTransfer.updateData(param, this._showSubmitResult.bind(this));
    }

    _updateStatus(OldStatus, Status) {
        this.props.toggleLoader(true);
        var productName = this.props.productLabel;

        const param = { ...this.props.requestParam, Status, OldStatus, productName };

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

    _showConfirmation(onOK) {
        const onClose = () => this.onCloseAlert(false);

        const buttons = [
            {
                text: LangHelper.getSingleResource('OK'),
                onClick: onOK,
                className: 'btn btn-apply'
            }, {
                text: LangHelper.getSingleResource('Cancel'),
                onClick: onClose,
                className: 'btn btn-cancel'
            }];

        const msg = (<p className="center-align">{Utility.getConfirmationMsg(this.props.requestParam.ProductId)}</p>);

        this.setState({ disabledButton: true }, () => this.props.setAlertOption({
            isShow: true,
            title: LangHelper.getSingleResource('Confirmation'),
            children: msg,
            buttons: buttons,
            icon: '',
            onClose: onClose,
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
        const { data, setting } = this.state;

        if (setting.BetSetting && !setting.BetSetting.IsLineEnabled) {
            return <NoDataTitle title={LangHelper.getSingleResource('AGCasinoNotOfferCurrency')} />;
        }

        if (data === null) {
            return <NoDataTitle />;
        }

        const { groups, selectedGroup, disabledButton } = this.state;
        const hasData = data.length > 0;

        return (
            <div className="pa-betlimit">
                <div className="pa-betlimit-group row">
                    <div className="col-12 col-md-4 col-sm-12 col-lg-3 col-xl-2">
                        {
                            setting.BetSetting.IsEnabled && hasData
                                ? <div className="form-group">
                                    <label className="label-text">{LangHelper.getSingleResource('BetSettingGroups')}</label>
                                    <Select instanceId="ddlGroup" value={selectedGroup} options={groups} onChange={this.onGroupChange} />
                                </div>
                                : <div>{`${LangHelper.getSingleResource('BetSettingGroups')} :`} <span className="disabled-text">{setting.BetSetting.AgGroup}</span></div>
                        }
                    </div>
                </div>
                {
                    hasData
                        ? <AGCSettingGrid data={data} isEnabled={setting.BetSetting.IsEnabled} group={selectedGroup} />
                        : (setting.IsStatusEnable ? <NoDataTitle title={LangHelper.getSingleResource('CannotUpdateBetLimitForProduct').replace("{0}", LangHelper.getSingleResource('AGCasino'))} />
                            : <NoDataTitle title={LangHelper.getSingleResource('ProblemDisplayBetSetting')} />)
                }
                {
                    <div className="pa-betlimit-button">
                        {hasData && setting.Editable && <>
                            <button type='button' className='btn btn-cancel' disabled={disabledButton} onClick={this.onReset}>
                                {LangHelper.getSingleResource('Reset')}
                            </button>
                            <button type='button' className='btn btn-apply' disabled={disabledButton} onClick={this.onSubmit}>
                                {LangHelper.getSingleResource('Submit')}
                            </button> </>
                        }

                        {setting.IsStatusEnable && <button type='button' className='btn btn-apply' onClick={this.onUpdateStatus}>
                            {setting.StatusEnable ? LangHelper.getSingleResource('DisableAgCasino') : LangHelper.getSingleResource('EnableAgCasino')}
                        </button>}
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
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(AGCDataContent);