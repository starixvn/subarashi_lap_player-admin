'use strict';
define(['jquery', 'common/site', 'lib/jquery.multiselect.nlevel'], function ($, Site) {
    const _me = {
        elements: {
            custIds: null,
            custNames: null,
            ddlCustomer: null,
            btnCancel: null,
            divResult: null,
            oldCustIds: null
        },
        options: {
            enableSubmit: null
        },
        setTopWindowValue: function (key, value) {
            try {
                top.window.postMessage({ isSetData: true, key, value }, "*");
            }
            catch (err) {
                console.log(err)
            }
        },
        setParentWindowValue: function (key, value) {
            try {
                parent.window[key] = value;
            }
            catch (err) {
            }
        },
        bindTopClickEvent: function () {
            $(top.window).bind("click", () => {
                closeDropdown();
            });

            top.window.$("#mainFrame > iframe").contents().click(() => {
                closeDropdown();
            });
        },
        closeDropdown: function () {
            $("#ddl-customers").multiSelect("hideMe");
        },
        initialize: function (options) {
            _me.options = Object.assign(_me.options, options);

            Site.initialize();
            _me.storeElements();
            _me.initSelectedCustomers();

            _me.elements.btnCancel.off("click").on("click", _me.cancelClick);
        },
        storeElements: function () {
            _me.elements.custIds = $("#CustIds");
            _me.elements.oldCustIds = _me.elements.custIds.val();

            _me.elements.custNames = $("#CustNames");
            _me.elements.ddlCustomer = $("#ddl-customers");
            _me.elements.btnCancel = $(".btn-cancel");
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

                    (_me.options.enableSubmit || _me.enableSubmit)(_me.elements.custIds.val() !== "");
                }
            });

            _me.elements.ddlCustomer = $("#ddl-customers");
        },
        cancelClick: function () {
            _me.setTopWindowValue("playerComissionResult", { isCancel: true });
        },
        enableSubmit: function () {
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

    return _me;
});
