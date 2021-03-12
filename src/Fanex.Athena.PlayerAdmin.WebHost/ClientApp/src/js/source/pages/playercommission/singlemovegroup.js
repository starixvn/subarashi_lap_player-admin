'use strict';
define(
    ['jquery', 'common/site', 'common/popup', './playercommissioncommon'],
    function ($, Site, Popup, PlayerCommissionComm) {
        const _me = {
            elements: {
                custIds: null,
                products: null,
                ddlGroup: null,
                commValues: null,
                tblComm: null,
                btnSave: null,
                groupCommId: null,
                divResult: null,
                currentGroupId: null
            },
            initialize: function () {
                Site.initialize();
                _me.storeElements();
                _me.elements.ddlGroup.change(_me.changeGroup);
                _me.elements.btnSave.off("click").on("click", _me.saveClick);
                $(".btn-update-comm").off("click").on("click", _me.updateClick);
                $(".btn-remove-comm").off("click").on("click", _me.removeClick);

                setInterval(() => {
                    var singlePlayerComissionResult = window["singlePlayerComissionResult"];

                    if (singlePlayerComissionResult) {
                        Popup.closePopup(singlePlayerComissionResult.popupId);
                        var isSuccess = singlePlayerComissionResult.isSuccess;
                        window["singlePlayerComissionResult"] = null;

                        if (isSuccess) {
                            location.reload(true);
                        }
                    }
                }, 200);
            },
            storeElements: function () {
                _me.elements.custIds = $("#CustIds");
                _me.elements.ddlGroup = $("#ddl-group");
                _me.elements.commValues = $(".comm-values");
                _me.elements.tblComm = $("#tbl-comm");
                _me.elements.products = $("#Products").val().split(",");
                _me.elements.btnSave = $(".btn-save-comm");
                _me.elements.groupCommId = $("#GroupCommId");
                _me.elements.divResult = $("#div-result");

                _me.elements.currentGroupId = _me.elements.groupCommId.val();
            },
            changeGroup: function () {
                var id = _me.elements.ddlGroup.val();
                for (var i = 0; i < _me.elements.products.length; i++) {
                    var p = _me.elements.products[i];
                    var value = _me.elements.commValues.find("#com-" + p + "-" + id).val();
                    _me.elements.tblComm.find(".comm-" + p).html(value);
                }

                _me.elements.groupCommId.val(id);
                _me.enableSubmit();
            },
            enableSubmit: function () {
                var selectedId = _me.elements.groupCommId.val();
                var isEnable = selectedId != 0 && selectedId != _me.elements.currentGroupId;
                _me.elements.btnSave.attr("disabled", isEnable ? null : "disabled");
            },
            saveClick: function () {
                var param = {
                    CustIds: _me.elements.custIds.val(),
                    GroupCommId: _me.elements.groupCommId.val()
                };

                $.ajax({
                    type: 'POST',
                    data: param,
                    url: Site.resolveClientUrl('PlayerCommission/PostMultipleMoveGroup'),
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

                        _me.showMsg(Site.getSingleResource('MoveSuccessfullyMsg'), result.IsSuccessful);
                        var newGroup = _me.elements.ddlGroup.find("option:selected").text();
                        PlayerCommissionComm.setTopWindowValue("playerComissionResult", {
                            newGroup,
                            isSuccess: true,
                            custIds: param.CustIds
                        });

                        setTimeout(() => {
                            location.reload(true);
                        }, 3000);
                    }
                });
            },
            updateClick: function () {
                var url = Site.resolveClientUrl('PlayerCommission/SingleUpdateGroup') + "?" + $.param({
                    CustId: _me.elements.custIds.val(),
                    CurrencyId: 0,
                    t: Date.now()
                });

                var opt = {
                    id: "popup-update-comm",
                    width: 800,
                    height: 950,
                    title: Site.getSingleResource('UpdateCommissionValue')
                };

                Popup.openPopupWithIframe(url, opt);
            },
            removeClick: function () {
                var url = Site.resolveClientUrl('PlayerCommission/SingleRemoveFromGroup') + "?" + $.param({
                    CustId: _me.elements.custIds.val(),
                    CurrencyId: 0,
                    t: Date.now()
                });

                var opt = {
                    id: "popup-remove-comm",
                    width: 600,
                    height: 300,
                    title: Site.getSingleResource('RemoveFromGroup')
                };

                Popup.openPopupWithIframe(url, opt);
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
