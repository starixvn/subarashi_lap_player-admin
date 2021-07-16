'use strict';
define(
    ['jquery', 'common/site', './playercommissioncommon', 'lib/jquery.multiselect.nlevel'],
    function ($, Site, PlayerCommissionComm) {
        const _me = {
            elements: {
                custIds: null,
                custNames: null,
                ddlCustomer: null,
                products: null,
                btnCancel: null,
                btnSave: null,
                divResult: null,
                removeConfirmMsg: null,
                divContent: null,
                oldCustIds: null
            },
            initialize: function () {
                Site.initialize();
                _me.storeElements();
                _me.initSelectedCustomers();
                _me.elements.btnCancel.off("click").on("click", _me.cancelClick);
                _me.elements.btnSave.off("click").on("click", _me.saveClick);
                _me.initConfirmMsg();

                PlayerCommissionComm.bindTopClickEvent();
            },
            storeElements: function () {
                _me.elements.custIds = $("#CustIds");
                _me.elements.oldCustIds = _me.elements.custIds.val();

                _me.elements.custNames = $("#CustNames");
                _me.elements.ddlCustomer = $("#ddl-customers");
                _me.elements.products = $("#Products").val().split(",");
                _me.elements.btnCancel = $(".btn-cancel");
                _me.elements.btnSave = $(".btn-submit");
                _me.elements.divResult = $("#div-result");
                _me.elements.removeConfirmMsg = $("#remove-confirm-msg");
                _me.elements.divContent = $(".div-content");
            },
            initSelectedCustomers: function () {
                const ids = _me.elements.custIds.val().split(",");
                const names = _me.elements.custNames.val().split(",");
                const multiSelectData = [];

                for (var i = 0; i < ids.length; i++) {
                    multiSelectData.push({
                        Id: ids[i],
                        Value: ids[i],
                        Text: names[i],
                        Checked: true,
                        Enable: true,
                        Css: '',
                        CssClass: ''
                    });
                }

                _me.elements.ddlCustomer.multiSelect({
                    minWidth: 160,
                    maxHeight: 400,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: true,
                    showFooter: false,
                    checkAllText: Site.getSingleResource('All'),
                    noneSelectedText: Site.getSingleResource('SelectOptions'),
                    tooltipNoItem: Site.getSingleResource('ThereIsNoSelectedItem'),
                    selectedText: Site.getSingleResource('SelectedItemCount'),
                    listData: multiSelectData,
                    onCheck: function () {
                        const checkedItems = _me.elements.ddlCustomer.multiSelect('getCheckedItems');
                        const checkedItemsByText = _me.elements.ddlCustomer.multiSelect('getCheckedItemsByText');

                        _me.elements.custIds.val(checkedItems.join(","));
                        _me.elements.custNames.val(checkedItemsByText.join(","));

                        _me.enableSubmit();
                    }
                });

                _me.elements.ddlCustomer = $("#ddl-customers");
            },
            initConfirmMsg: function () {
                const length = _me.elements.ddlCustomer.multiSelect('getCheckedItems').length;
                const msg = length > 1
                    ? Site.getSingleResource('RemoveTheseCustomersFromGroupMsg')
                    : Site.getSingleResource('RemoveThisCustomerFromGroupMsg');

                _me.elements.removeConfirmMsg.html(msg);
            },
            cancelClick: function () {
                PlayerCommissionComm.setTopWindowValue("playerComissionResult", { isCancel: true });
            },
            enableSubmit: function () {
                const length = _me.elements.ddlCustomer.multiSelect('getCheckedItems').length;
                if (length > 0) {
                    _me.initConfirmMsg();
                    _me.elements.divContent.removeClass("hide");
                } else {
                    _me.elements.divContent.addClass("hide");
                }
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

                        _me.showMsg(Site.getSingleResource('MessageSuccess'), result.IsSuccessful);
                        setTimeout(() => {
                            PlayerCommissionComm.setTopWindowValue("playerComissionResult", {
                                isCancel: true,
                                oldCustIds: _me.elements.oldCustIds,
                                custIds: param.CustIds,
                                reloadGrid: true,
                                newValue: {
                                    CommissionGroup: Site.getSingleResource('None')
                                }
                            });
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
