'use strict';
define(
    ['jquery', 'common/site', './batch-update-common', 'lib/jquery.multiselect.nlevel'],
    function ($, Site, BaseBatchUpdateController) {
        const _me = {
            commonService: null,
            enableTristateToggleAnimate: true,
            elements: {
                btnSave: null,
                tglOn: null,
                tglNone: null,
                tglOff: null,
                baValue: -1,
                msgOnBA: null,
                msgOffBA: null
            },
            initialize: function () {
                _me.commonService = BaseBatchUpdateController;

                _me.commonService.initialize({
                    enableSubmit: _me.enableSubmit
                });

                _me.elements.msgOnBA = $(".msg-on-ba-status");
                _me.elements.msgOffBA = $(".msg-off-ba-status");

                _me.elements.tglOn = $(".tgl-on");
                _me.elements.tglOn.off("click").on("click", _me.onBA);

                _me.elements.tglNone = $(".tgl-none");

                _me.elements.tglOff = $(".tgl-off");
                _me.elements.tglOff.off("click").on("click", _me.offBA);

                _me.elements.btnSave = $(".btn-submit");
                _me.elements.btnSave.off("click").on("click", _me.saveClick);
            },
            onBA: function () {
                _me.elements.tglOn.removeClass("on-to-none select-on");
                _me.elements.tglNone.removeClass("default-to-on default-to-off none-to-on none-to-off selected");
                _me.elements.tglOff.removeClass("off-to-none select-off selected");

                if (_me.enableTristateToggleAnimate) {
                    switch (_me.elements.baValue) {
                        case -1:
                            _me.elements.tglNone.addClass("default-to-on");
                            _me.elements.tglOn.addClass("select-on");
                            break;
                        default:
                            _me.elements.tglOff.addClass("off-to-none");
                            _me.elements.tglNone.addClass("none-to-on");
                            _me.elements.tglOn.addClass("on-from-none");
                            break;
                    }

                    setTimeout(() => { _me.elements.tglOn.addClass("selected"); }, 300);
                }
                else {
                    _me.elements.tglOn.addClass("selected");
                }

                _me.elements.baValue = 1;

                _me.elements.msgOffBA.addClass("hide");
                _me.elements.msgOnBA.removeClass("hide");
                _me.enableSubmit(true);
            },
            offBA: function () {
                _me.elements.tglOn.removeClass("on-to-none select-on selected");
                _me.elements.tglNone.removeClass("default-to-on default-to-off none-to-on none-to-off selected");
                _me.elements.tglOff.removeClass("off-to-none select-off");

                if (_me.enableTristateToggleAnimate) {
                    switch (_me.elements.baValue) {
                        case -1:
                            _me.elements.tglNone.addClass("default-to-off");
                            _me.elements.tglOff.addClass("select-off");
                            break;
                        default:
                            _me.elements.tglOn.addClass("on-to-none");
                            _me.elements.tglNone.addClass("none-to-off");
                            _me.elements.tglOff.addClass("off-from-none");
                            break;
                    }

                    setTimeout(() => { _me.elements.tglOff.addClass("selected"); }, 300);
                } else {
                    _me.elements.tglOff.addClass("selected");
                }

                _me.elements.baValue = 0;

                _me.elements.msgOnBA.addClass("hide");
                _me.elements.msgOffBA.removeClass("hide");
                _me.enableSubmit(true);
            },
            enableSubmit: function (isEnable) {
                _me.elements.btnSave.attr("disabled", isEnable ? null : "disabled");
            },
            saveClick: function () {
                var param = {
                    CustIds: _me.commonService.elements.custIds.val(),
                    CustNames: _me.commonService.elements.custNames.val(),
                    BAStatusId: _me.elements.baValue
                };

                $.ajax({
                    type: 'POST',
                    data: param,
                    url: Site.resolveClientUrl('PlayerBatchUpdate/UpdateBAStatus'),
                    dataType: 'json',
                    beforeSend: function () {
                        _me.commonService.ajaxStart();
                    },
                    complete: function () {
                        _me.commonService.ajaxStop();
                    },
                    success: function (result) {
                        if (!result.IsSuccessful) {
                            _me.commonService.showMsg(result.Message, result.IsSuccessful);
                            return;
                        }

                        _me.elements.btnSave.addClass("completed");
                        _me.commonService.showMsg(result.Message, result.Code == 0);

                        setTimeout(() => {
                            _me.commonService.setTopWindowValue("playerComissionResult", {
                                isCancel: true,
                                oldCustIds: _me.commonService.elements.oldCustIds,
                                custIds: result.ExtendedData || "",
                                reloadGrid: true,
                                newValue: {
                                    BaStatus: param.BAStatusId == 1
                                }
                            });
                        }, 3000);
                    }
                });
            }
        };

        $(document).ready(function () {
            _me.initialize();
        });

        return _me;
    });
