'use strict';
define(
    ['jquery', 'common/site', './playercommissioncommon'],
    function ($, Site, PlayerCommissionComm) {
        const _me = {
            elements: {
                custIds: null,
                comms: null,
                btnCancel: null,
                btnSave: null,
                divResult: null,
                oldCustIds: null
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
                _me.elements.oldCustIds = _me.elements.custIds.val();

                _me.elements.comms = $("[id$='-comm-discount']");
                _me.elements.btnCancel = $(".btn-cancel");
                _me.elements.btnSave = $(".btn-submit");
                _me.elements.divResult = $("#div-result");
            },
            initCommSelection: function () {
                _me.elements.comms.each(function () {
                    const id = $(this).attr("id");
                    $('#' + id + 1).on('change', function () {
                        _me.commissionOnChange($(this).val(), id);
                        _me.enableSubmit();
                    }).change();

                    $('#' + id + 2).on('change', _me.enableSubmit);
                    $('#' + id + 3).on('change', _me.enableSubmit);
                });
            },
            commissionOnChange: function (value, idName) {
                const comm2 = '#' + idName + 2, comm3 = '#' + idName + 3;

                if (Number(value) < 15) {
                    $(comm2).selectList('disabled', null);
                    $(comm3).selectList('disabled', null);
                } else {
                    $(comm2).val(0);
                    $(comm2).selectList('update', {
                        isDisabled: true,
                        selectionId: 'select-list-' + idName + 2,
                        labelText: '0'
                    });
                    $(comm3).val(0);
                    $(comm3).selectList('update', {
                        isDisabled: true,
                        selectionId: 'select-list-' + idName + 3,
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
                _me.elements.comms.each(function () {
                    const id = $(this).attr("id");

                    const comm1 = parseInt($('#' + id + 1).val(), 10);
                    const comm2 = parseInt($('#' + id + 2).val(), 10);
                    const comm3 = parseInt($('#' + id + 3).val(), 10);
                    const comm = parseFloat((comm1 + comm2 / 10 + comm3 / 100) / 100).toFixed(4);

                    if (comm != parseFloat($('#' + id).val())) {
                        isEnable = true;
                    }

                    $('#' + id + 4).val(comm);
                });

                _me.elements.btnSave.attr("disabled", isEnable ? null : "disabled");
            },
            saveClick: function () {
                var comms = [];

                _me.elements.comms.each(function () {
                    const id = $(this).attr("id");

                    var commElement = $('#' + id + 4);
                    const comm = commElement.length ? parseFloat(commElement.val()).toFixed(4) : 0;

                    var commBettype = $("#" + id + "bettype").val();
                    var commSporttype = $("#" + id + "sporttype").val();

                    comms.push({
                        Sporttype: commSporttype,
                        Bettype: commBettype,
                        Discount: comm
                    });
                });

                var param = {
                    CustIds: _me.elements.custIds.val(),
                    CommValues: comms
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