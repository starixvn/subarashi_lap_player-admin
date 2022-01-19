'use strict';
define(
    ['jquery', 'common/site', './batch-update-common', 'lib/jquery.multiselect.nlevel'],
    function ($, Site, BaseBatchUpdateController) {
        const _me = {
            commonService: null,
            elements: {
                btnSave: null,
                ddlSpecialLevel: null,
                isSelectedSpecialLevel: false,
                msgConfirm: null
            },
            initialize: function () {
                _me.commonService = BaseBatchUpdateController;

                _me.commonService.initialize({
                    enableSubmit: _me.enableSubmit
                });

                _me.elements.btnSave = $(".btn-submit");

                _me.elements.msgConfirm = $(".msg-confirm");
                _me.elements.ddlSpecialLevel = $("#ddl-special-level");
                _me.elements.ddlSpecialLevel.change(_me.changeSpecialLevel);

                _me.elements.btnSave.off("click").on("click", _me.saveClick);
            },
            enableSubmit: function (isEnable) {
                isEnable = isEnable && _me.elements.isSelectedSpecialLevel &&
                    _me.commonService.elements.custIds.val();

                _me.elements.btnSave.attr("disabled", isEnable ? null : "disabled");
            },
            changeSpecialLevel: function () {
                _me.elements.isSelectedSpecialLevel = true;
                _me.enableSubmit(true);
                _me.elements.msgConfirm.removeClass("hide");
            },
            saveClick: function () {
                var param = {
                    CustIds: _me.commonService.elements.custIds.val(),
                    CustNames: _me.commonService.elements.custNames.val(),
                    SpecialLevelId: _me.elements.ddlSpecialLevel.val()
                };

                $.ajax({
                    type: 'POST',
                    data: param,
                    url: Site.resolveClientUrl('PlayerBatchUpdate/UpdateSpecialLevel'),
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
                        _me.commonService.showMsg(result.Message, result.IsSuccessful);

                        setTimeout(() => {
                            _me.commonService.setTopWindowValue("playerComissionResult", {
                                isCancel: true,
                                oldCustIds: _me.commonService.elements.oldCustIds,
                                custIds: result.ExtendedData || "",
                                reloadGrid: true,
                                newValue: {
                                    SpecialLevel: param.SpecialLevelId
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
