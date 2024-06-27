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
                oldCustIds: null
            },
            initialize: function () {
                Site.initialize();
                _me.storeElements();
                _me.initSelectedCustomers();
                _me.elements.btnCancel.off("click").on("click", _me.cancelClick);
                _me.elements.btnSave.off("click").on("click", _me.saveClick);
                _me.initCommSelection();

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
            initCommSelection: function () {
                _me.elements.products.forEach((e) => {
                    $('#comm1-' + e).on('change', function () {
                        _me.commissionOnChange($(this).val(), e);
                    });
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
                PlayerCommissionComm.setTopWindowValue("playerComissionResult", { isCancel: true });
            },
            enableSubmit: function () {
                const checkedItems = _me.elements.ddlCustomer.multiSelect('getCheckedItems');
                var isEnable = checkedItems.length > 0;
                _me.elements.btnSave.attr("disabled", isEnable ? null : "disabled");
            },
            saveClick: function () {
                var comms = {};
                _me.elements.products.forEach((id) => {
                    if ($("#comm1-" + id).length == 0) {
                        comms.push(0);
                        return;
                    }

                    const comm1 = parseInt($("#comm1-" + id).val(), 10);
                    const comm2 = parseInt($("#comm2-" + id).val(), 10);
                    const comm3 = parseInt($("#comm3-" + id).val(), 10);
                    const comm = parseFloat((comm1 + comm2 / 10 + comm3 / 100) / 100).toFixed(4);

                    var commBettype = $("#comm-bettype-" + id).val();
                    var commSporttype = $("#comm-sporttype-" + id).val();

                    comms[id] = {
                        Sporttype: commSporttype,
                        Bettype: commBettype,
                        Discount: comm
                    };
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