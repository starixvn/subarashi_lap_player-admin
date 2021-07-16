'use strict';
define(
    ['jquery', 'common/site', './batch-update-common', 'lib/jquery.multiselect.nlevel'],
    function ($, Site, BaseBatchUpdateController) {
        const _me = {
            commonService: null,
            elements: {
                btnSave: null,
                ddlOddsGroup: null,
                isSelectedOddsGroup: false,
                msgConfirm: null
            },
            initialize: function () {
                _me.commonService = BaseBatchUpdateController;

                _me.commonService.initialize({
                    enableSubmit: _me.enableSubmit
                });

                _me.elements.btnSave = $(".btn-submit");
                _me.elements.msgConfirm = $(".msg-confirm");
                _me.elements.ddlOddsGroup = $("#ddl-odds-group");
                _me.elements.ddlOddsGroup.change(_me.changeOddsGroup);

                _me.elements.btnSave.off("click").on("click", _me.saveClick);
                _me.enableSubmit(true);
            },
            enableSubmit: function (isEnable) {
                isEnable = isEnable && _me.elements.isSelectedOddsGroup &&
                    _me.commonService.elements.custIds.val();
                _me.elements.btnSave.attr("disabled", isEnable ? null : "disabled");
            },
            changeOddsGroup: function () {
                _me.elements.isSelectedOddsGroup = true;
                _me.enableSubmit(true);
                _me.elements.msgConfirm.removeClass("hide");
            },
            saveClick: function () {
                var param = {
                    CustIds: _me.commonService.elements.custIds.val(),
                    CustNames: _me.commonService.elements.custNames.val(),
                    OddsGroupId: _me.elements.ddlOddsGroup.val()
                };

                $.ajax({
                    type: 'POST',
                    data: param,
                    url: Site.resolveClientUrl('PlayerBatchUpdate/UpdateOddsGroup'),
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
                                    OddsGroup: param.OddsGroupId
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
