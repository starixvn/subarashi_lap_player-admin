'use strict';
define(
    ['jquery', 'common/site', './playercommissioncommon'],
    function ($, Site, PlayerCommissionComm) {
        const _me = {
            elements: {
                custIds: null,
                btnCancel: null,
                btnSave: null,
                divResult: null,
                divContent: null,
                oldCustIds: null
            },
            initialize: function () {
                Site.initialize();
                _me.storeElements();
                _me.elements.btnCancel.off("click").on("click", _me.cancelClick);
                _me.elements.btnSave.off("click").on("click", _me.saveClick);
            },
            storeElements: function () {
                _me.elements.custIds = $("#CustIds");
                _me.elements.oldCustIds = _me.elements.custIds.val();

                _me.elements.btnCancel = $(".btn-cancel");
                _me.elements.btnSave = $(".btn-submit");
                _me.elements.divResult = $("#div-result");
                _me.elements.divContent = $(".div-content");
            },
            cancelClick: function () {
                PlayerCommissionComm.setParentWindowValue("singlePlayerComissionResult", {
                    isCancel: true,
                    popupId: "popup-remove-comm"
                });
            },
            saveClick: function () {
                var param = {
                    CustIds: _me.elements.custIds.val(),
                    IsRemovePlayerComm: $('input[name="rad-comm"]:checked').val() == "1"
                };

                $.ajax({
                    type: 'POST',
                    data: param,
                    url: Site.resolveClientUrl('PlayerCommission/PostMultipleRemoveFromGroup'),
                    dataType: 'json',
                    beforeSend: function () {
                        _me.ajaxStart();
                    },
                    complete: function () {
                        _me.ajaxStop();
                    },
                    success: function (result) {
                        if (!result.IsSuccessful) {
                            _me.showMsg(result.Message, result.IsSuccessful);
                            return;
                        }

                        _me.elements.btnSave.addClass("completed");
                        _me.showMsg(Site.getSingleResource('MessageSuccess'), result.IsSuccessful);

                        let message = {
                            isSuccess: true,
                            oldCustIds: _me.elements.oldCustIds,
                            custIds: param.CustIds,
                            reloadGrid: true,
                            newValue: {
                                CommissionGroup: Site.getSingleResource('None')
                            }
                        }

                        PlayerCommissionComm.setTopWindowValue("playerComissionResult", message);

                        setTimeout(() => {
                            PlayerCommissionComm.setParentWindowValue("singlePlayerComissionResult", message);
                        }, 3000);
                    }
                });
            },
            ajaxStart: function () {
                _me.elements.divResult.addClass("hide");
                Site.blockUI();
            },
            ajaxStop: function () {
                Site.unBlockUI();
            },
            showMsg: function (msg, isSuccess) {
                _me.elements.divResult.html(msg);
                if (isSuccess) {
                    _me.elements.divResult.addClass("success-message");
                    _me.elements.divResult.removeClass("error-message");
                }
                else {
                    _me.elements.divResult.addClass("error-message");
                    _me.elements.divResult.removeClass("success-message");
                }

                _me.elements.divResult.removeClass("hide");
            }
        };

        $(document).ready(function () {
            _me.initialize();
        });

        return _me;
    });