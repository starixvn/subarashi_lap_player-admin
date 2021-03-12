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
                ddlGroup: null,
                commValues: null,
                tblComm: null,
                btnCancel: null,
                btnSave: null,
                groupCommId: null,
                divResult: null
            },
            initialize: function () {
                Site.initialize();
                _me.storeElements();
                _me.initSelectedCustomers();

                $("#select-list-ddl-group .selection-label").html(Site.getSingleResource('PleaseSelectOne'));
                _me.elements.ddlGroup.change(_me.changeGroup);

                _me.elements.btnCancel.off("click").on("click", _me.cancelClick);
                _me.elements.btnSave.off("click").on("click", _me.saveClick);
            },
            storeElements: function () {
                _me.elements.custIds = $("#CustIds");
                _me.elements.custNames = $("#CustNames");
                _me.elements.ddlCustomer = $("#ddl-customers");
                _me.elements.ddlGroup = $("#ddl-group");
                _me.elements.commValues = $(".comm-values");
                _me.elements.tblComm = $("#tbl-comm");
                _me.elements.products = $("#Products").val().split(",");
                _me.elements.btnCancel = $(".btn-cancel");
                _me.elements.btnSave = $(".btn-submit");
                _me.elements.groupCommId = $("#GroupCommId");
                _me.elements.divResult = $("#div-result");
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
            cancelClick: function () {
                PlayerCommissionComm.setTopWindowValue("playerComissionResult", { isCancel: true });
            },
            enableSubmit: function () {
                const checkedItems = _me.elements.ddlCustomer.multiSelect('getCheckedItems');
                var isEnable = _me.elements.groupCommId.val() != "0" && checkedItems.length > 0;
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

                        _me.elements.btnSave.addClass("completed");
                        _me.showMsg(Site.getSingleResource('MoveSuccessfullyMsg'), result.IsSuccessful);
                        var newGroup = _me.elements.ddlGroup.find("option:selected").text();
                        PlayerCommissionComm.setTopWindowValue("playerComissionResult", {
                            newGroup,
                            isSuccess: true,
                            custIds: param.CustIds
                        });

                        setTimeout(() => {
                            PlayerCommissionComm.setTopWindowValue("playerComissionResult", { isCancel: true });
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
