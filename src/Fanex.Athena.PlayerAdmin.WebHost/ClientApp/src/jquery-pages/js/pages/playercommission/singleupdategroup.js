'use strict';
define(
    ['jquery', 'common/site', './playercommissioncommon'],
    function ($, Site, PlayerCommissionComm) {
        const _me = {
            elements: {
                custIds: null,
                products: null,
                btnCancel: null,
                btnSave: null,
                divResult: null
            },
            initialize: function () {
                Site.initialize();
                _me.storeElements();
                _me.elements.btnCancel.off("click").on("click", _me.cancelClick);
                _me.elements.btnSave.off("click").on("click", _me.saveClick);
                _me.initCommSelection();
            },
            storeElements: function () {
                _me.elements.custIds = $("#CustIds");
                _me.elements.products = $("#Products").val().split(",");
                _me.elements.btnCancel = $(".btn-cancel");
                _me.elements.btnSave = $(".btn-submit");
                _me.elements.divResult = $("#div-result");
            },
            initCommSelection: function () {
                _me.elements.products.forEach((e) => {
                    $('#comm1-' + e).on('change', function () {
                        _me.commissionOnChange($(this).val(), e);
                        _me.enableSubmit();
                    }).change();

                    $('#comm2-' + e).on('change', _me.enableSubmit);
                    $('#comm3-' + e).on('change', _me.enableSubmit);
                });
            },
            commissionOnChange: function (value, idName) {
                const comm2 = '#comm2-' + idName, comm3 = '#comm3-' + idName;

                if (Number(value) < 15) {
                    $(comm2).selectList('disabled', null);
                    $(comm3).selectList('disabled', null);
                } else {

                    $(comm2).val(0);
                    $(comm2).selectList('update', {
                        isDisabled: true,
                        selectionId: 'select-list-comm2' + idName,
                        labelText: '0'
                    });
                    $(comm3).val(0);
                    $(comm3).selectList('update', {
                        isDisabled: true,
                        selectionId: 'select-list-comm3' + idName,
                        labelText: '0'
                    });
                }
            },
            cancelClick: function () {
                PlayerCommissionComm.setParentWindowValue("singlePlayerComissionResult", {
                    isCancel: true,
                    popupId: "popup-update-comm"
                });
            },
            enableSubmit: function () {
                var isEnable = false;
                _me.elements.products.forEach((id) => {
                    const comm1 = parseInt($("#comm1-" + id).val(), 10);
                    const comm2 = parseInt($("#comm2-" + id).val(), 10);
                    const comm3 = parseInt($("#comm3-" + id).val(), 10);
                    const comm = parseFloat((comm1 + comm2 / 10 + comm3 / 100) / 100).toFixed(4);
                    if (comm != parseFloat($("#comm0-" + id).val())) {
                        isEnable = true;
                    }

                    $("#comm4-" + id).val(comm);
                });

                _me.elements.btnSave.attr("disabled", isEnable ? null : "disabled");
            },
            saveClick: function () {
                var comms = [];
                _me.elements.products.forEach((id) => {
                    var commElement = $("#comm4-" + id);
                    const comm = commElement.length ? parseFloat(commElement.val()).toFixed(4) : 0;
                    comms.push(comm);
                });

                var param = {
                    CustIds: _me.elements.custIds.val(),
                    Products: _me.elements.products,
                    Comms: comms
                };

                $.ajax({
                    type: 'POST',
                    data: param,
                    url: Site.resolveClientUrl('PlayerCommission/PostMultipleUpdateGroup'),
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
                        var newValue = {
                            newGroup: Site.getSingleResource('None'),
                            isSuccess: true,
                            custIds: param.CustIds,
                            popupId: "popup-update-comm"
                        };

                        PlayerCommissionComm.setTopWindowValue("playerComissionResult", newValue);
                        setTimeout(() => {
                            PlayerCommissionComm.setParentWindowValue("singlePlayerComissionResult", newValue);
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
