import LangHelper from '@jsroot/lib/langHelper';
import $ from 'jquery';
import FactoryCommonService from '@jsroot/Module/lib/factoryCommonService';
import { clearInterval } from 'timers';

export const batchUpdateManager = {
    editCommMenu: [
        { value: 1, label: LangHelper.getSingleResource('MoveCommissionGroup') },
        { value: 2, label: LangHelper.getSingleResource('UpdateCommissionValue') },
        { value: 3, label: LangHelper.getSingleResource('RemoveFromGroup') }
    ],
    popupBatchUpdateOption_1: {
        url: 'PlayerCommission/MultipleMoveGroup',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('MoveCommissionGroup'),
        width: 800,
        height: 600
    },
    popupBatchUpdateOption_2: {
        url: 'PlayerCommission/MultipleUpdateGroup',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('UpdateCommissionValue'),
        width: 800,
        height: 600
    },
    popupBatchUpdateOption_3: {
        url: 'PlayerCommission/MultipleRemoveFromGroup',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('RemoveFromGroup'),
        width: 600,
        height: 400
    },
    updateTableInterval: null,
    getLink: function (action) {
        const lrfSystemId = 1;
        const commonService = FactoryCommonService.createInstance();
        return commonService.getLink(lrfSystemId, action);
    },
    bindMenuEvent: function (handler) {
        var searchInput = $(".ddl-batch-update .Select-input");
        searchInput.find(" > input").attr("readonly", "readonly");
        $(".ddl-batch-update .Select-value").off("click").on("click", () => {
            searchInput.focus();
        });

        if (batchUpdateManager.updateTableInterval) {
            clearInterval(batchUpdateManager.updateTableInterval);
        }

        batchUpdateManager.updateTableInterval = setInterval(() => {
            var result = window.top["playerComissionResult"];
            if (result && result.isSuccess) {
                var newGroup = result.newGroup;
                result.custIds.split(",").forEach((e) => {
                    var col = $(".td-comm-" + e);
                    col.html(newGroup);
                    col.attr("title", newGroup);
                });
            }

            if (result && result.isCancel) {
                handler.setState({
                    batchUpdateSelectedOption: 0,
                    popupBatchUpdateShow: false
                });
            }

            window.top["playerComissionResult"] = null;
        }, 200);
    },
    bindGridEvents: function () {
        var checkAll = $("#chk-cust-0");
        var checkCusts = $(".chk-cust");
        checkAll.prop("checked", false);
        checkCusts.prop("checked", false);

        checkAll.off("click").on("click", function () {
            var isChecked = $(this).is(":checked");
            checkCusts.prop("checked", isChecked);

            var selectedLength = $(".chk-cust:checked").length;
            batchUpdateManager.showHideBatchUpdateMenu(selectedLength > 1);
        });

        checkCusts.each((i, e) => {
            $(e).off("click").on("click", () => {
                var selectedLength = $(".chk-cust:checked").length;
                var isChecked = checkCusts.length == $(".chk-cust:checked").length;
                checkAll.prop("checked", isChecked);
                batchUpdateManager.showHideBatchUpdateMenu(selectedLength > 1);
            });
        });

        batchUpdateManager.showHideBatchUpdateMenu(false);
    },
    showHideBatchUpdateMenu: function (isShow) {
        isShow ? $(".ddl-batch-update").removeClass("hide") : $(".ddl-batch-update").addClass("hide");
    },
    getSelectCusts: function () {
        var checkCusts = $(".chk-cust:checked");
        var custIds = [];
        var custNames = [];

        checkCusts.each((i, e) => {
            var $this = $(e);
            custIds.push($this.attr("data-custid"));
            custNames.push($this.attr("data-username"));
        });

        return { custIds, custNames };
    },
    handleMenuSelected: function (handler, selectedOption) {
        handler.setState({
            batchUpdateSelectedOption: selectedOption.value,
            popupBatchUpdateShow: true
        });
    },
    closePopup: function (handler, selectedOption) {
        handler.setState({
            batchUpdateSelectedOption: 0,
            popupBatchUpdateShow: false
        });
    },
    getPopupOption: function (selectedOption) {
        var timestamps = Date.now();
        var { custIds, custNames } = batchUpdateManager.getSelectCusts();

        var query = "?" + $.param({
            t: timestamps,
            custIds: custIds.join(","),
            custNames: custNames.join(",")
        });

        var opt = $.extend({}, batchUpdateManager["popupBatchUpdateOption_" + selectedOption]);
        opt.url = batchUpdateManager.getLink(opt.url + query);
        return opt;
    }
};
