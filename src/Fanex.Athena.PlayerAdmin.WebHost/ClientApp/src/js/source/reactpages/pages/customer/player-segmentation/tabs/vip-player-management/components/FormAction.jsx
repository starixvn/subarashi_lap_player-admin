import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import LangHelper from '@jsroot/lib/langHelper';
import Excel from '@jsroot/lib/excel';
import { decode } from '@jsroot/lib/htmlHelper';
import { generateDefaultConfirmOption } from '@jsroot/lib/sweetAlert';
import { ActionCreators } from '../actions';
import DropzoneUpload from '../../../components/DropzoneUpload';
import { Url, ExcelReaderColumn } from '../lib/constants';
import ExcelHelper from '../lib/helpers/excelHelper';
import { UpdateVipStatusAlertOption } from '../../../lib/helpers/sweetAlertHelper';

export const FormAction = props => {
    const { isActiveTaggingAction, playerList, updateVipStatus, setSweetAlertOption, selectedParams } = props;
    const [displayUploadDropzone, setDisplayUploadDropzone] = useState(false);
    const [resultMsgSubmitVipListFile, setResultMsgSubmitVipListFile] = useState(null);

    useEffect(() => { resultMsgSubmitVipListFile && setDisplayUploadDropzone(false) }, [selectedParams]);

    const _handleCloseAlert = () => {
        setSweetAlertOption(null);
    }

    const _cancelProps = {
        onClick: _handleCloseAlert
    }

    const _getParams = (isVip) => {
        const selectedPlayers = playerList.filter(_ => _.isSelected);
        const systemIds = selectedPlayers.map(_ => _.systemId);
        const params = {
            systemIds: systemIds.join(),
            isVip
        }
        return params;
    }

    const _buildAlertOptionUpdateVipStatus = (params, isUpdateVip, noAlertResult, handlerUpdateVipStatusSuccess) => {
        const confirmMessage = isUpdateVip ? LangHelper.getSingleResource("ConfirmActivateVIPPlayer") : LangHelper.getSingleResource("ConfirmDeactivateVIPPlayer");
        handlerUpdateVipStatusSuccess = handlerUpdateVipStatusSuccess || UpdateVipStatusAlertOption.createHandlerAfterUpdateVipStatusSuccess(props);
        const handlerAfterUpdateVipStatus = UpdateVipStatusAlertOption.createHandlerAfterUpdateVipStatus({ setSweetAlertOption, handlerUpdateVipStatusSuccess, isUpdateVip, noAlertResult });
        const confirmProps = UpdateVipStatusAlertOption.createUpdateVipStatusConfirmProps({ params, updateVipStatus, handlerAfterUpdateVipStatus });
        const confirmOptions = generateDefaultConfirmOption(confirmMessage, _handleCloseAlert, confirmProps, _cancelProps);
        return confirmOptions;
    }

    const _handleActivateVip = (event) => {
        event.preventDefault();
        const params = _getParams(true);
        const alertOption = _buildAlertOptionUpdateVipStatus(params, true, false);
        setSweetAlertOption(alertOption);
    }


    const _handleDeactivateVip = (event) => {
        event.preventDefault();
        const params = _getParams(false);
        const alertOption = _buildAlertOptionUpdateVipStatus(params, false, false);
        setSweetAlertOption(alertOption);
    }

    const _handleDisplayUploadDropzone = () => {
        setResultMsgSubmitVipListFile(null);
        setDisplayUploadDropzone(!displayUploadDropzone);
    }

    const _handleActiveVipStatusFromFile = (params) => {
        const handlerUpdateVipStatusSuccess = resultMessage => {
            setSweetAlertOption(null);
            setResultMsgSubmitVipListFile(resultMessage);
        }
        const alertOption = _buildAlertOptionUpdateVipStatus(params, true, true, handlerUpdateVipStatusSuccess);
        setSweetAlertOption(alertOption);
    }

    const _onSubmitVipListFile = (files) => {
        const file = files[0];
        Excel.process(() => {
            Excel.read(file, 0, result => {
                const { header, content } = result;
                if (!ExcelHelper.isValidHeader(header) || !ExcelHelper.isValidContent(content)) {
                    alert(decode(LangHelper.getSingleResource("UploadFileError")))
                    return;
                }

                const columnSystemId = ExcelHelper.getColumnName(header, ExcelReaderColumn.SystemId);
                const columnExternalId = ExcelHelper.getColumnName(header, ExcelReaderColumn.ExternalId);
                if (!columnSystemId && !columnExternalId) {
                    alert(decode(LangHelper.getSingleResource("UploadFileError")))
                    return;
                }

                const systemIds = content.map(_ => _[columnSystemId]).join();
                const externalIds = content.map(_ => _[columnExternalId]).join();
                const params = {
                    systemIds,
                    externalIds,
                    isVip: true
                }
                _handleActiveVipStatusFromFile(params);
            })
        })
    }

    const _renderNoteDropzoneUploadFile = () => {
        return (<>
            <div>
                <a href={Url.vipPlayerListTemplate}>*{LangHelper.getSingleResource('ClickHere')}</a>&nbsp;
                <span>{LangHelper.getSingleResource('ToDowloadTemplate')}</span>
            </div>
            <div>
                <span>{`*${LangHelper.getSingleResource('MaximumUploadFileSize')}: ${1} MB`}</span>
            </div>
        </>)
    }

    const _renderDropzoneUploadFile = () => {
        return <DropzoneUpload multiple={false} accept={'.xls,.xlsx'} maxSize={1}
            onClose={_handleDisplayUploadDropzone}
            onSubmit={_onSubmitVipListFile}
            isShowInnerZone={!resultMsgSubmitVipListFile}>
            <div>
                {resultMsgSubmitVipListFile ? resultMsgSubmitVipListFile : _renderNoteDropzoneUploadFile()}
            </div>
        </DropzoneUpload>
    }

    return (
        <div className="form-action">
            <>
                <div className="action">
                    <button type="button" className="btn" disabled={!isActiveTaggingAction} onClick={_handleActivateVip}>{LangHelper.getSingleResource("ActivateVip")}</button>
                </div>
                <div className="action">
                    <button type="button" className="btn" onClick={_handleDeactivateVip} disabled={!isActiveTaggingAction}>{LangHelper.getSingleResource("DeactivateVip")}</button>
                </div>
                <div className="action">
                    <button type="button" className="btn" onClick={_handleDisplayUploadDropzone}>{LangHelper.getSingleResource("UploadVipList")}</button>
                </div>
            </>
            {displayUploadDropzone && _renderDropzoneUploadFile()}
        </div>
    );
}

function mapDispatchToProps(dispatch) {
    const { searchPlayer, updateVipStatus, setSweetAlertOption, setPagingOption } = ActionCreators;
    return bindActionCreators({ searchPlayer, updateVipStatus, setSweetAlertOption, setPagingOption }, dispatch);
}
function mapStateToProps(state) {
    const { playerList, selectedParams, selectedTabKey, pagingOption } = state;
    const isActiveTaggingAction = playerList && playerList.length && playerList.some(_ => _.isSelected);
    return Object.assign({}, { isActiveTaggingAction, playerList, selectedParams, selectedTabKey, pagingOption });
};

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(FormAction));
