import LangHelper from '@jsroot/lib/langHelper';
import $ from 'jquery';
import FactoryCommonService from '@jsroot/lib/factoryCommonService';
import { clearInterval } from 'timers';
import { systemType } from '@jsroot/constants/config';

export const batchUpdatePermission = {
    playerCommGroup: "PlayerCommGroup",
    accountStatus: "accountStatus",
    betLimit: "betLimit",
    specialLevel: "specialLevel",
    oddsGroup: "oddsGroup",
    baStatus: "baStatus"
};

export const batchUpdatePopups = {
    popupOption_1: {
        url: 'PlayerCommission/MultipleMoveGroup',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('MoveCommissionGroup'),
        width: 800,
        height: 600
    },
    popupOption_2: {
        url: 'PlayerCommission/MultipleUpdateGroup',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('UpdateCommissionValue'),
        width: 800,
        height: 600
    },
    popupOption_3: {
        url: 'PlayerCommission/MultipleRemoveFromGroup',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('RemoveFromGroup'),
        width: 600,
        height: 400
    },
    popupOption_4: {
        url: 'PlayerBatchUpdate/UpdateAccountStatus',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('UpdateAccountStatus'),
        width: 600,
        height: 400
    },
    popupOption_5: {
        url: 'SportsbookBetSetting/Index',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('UpdateBetLimit'),
        width: 800,
        height: 600
    },
    popupOption_6: {
        url: 'PlayerBatchUpdate/UpdateSpecialLevel',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('UpdateSpecialLevel'),
        width: 600,
        height: 400
    },
    popupOption_7: {
        url: 'PlayerBatchUpdate/UpdateOddsGroup',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('UpdateOddsGroup'),
        width: 600,
        height: 400
    },
    popupOption_8: {
        url: 'PlayerBatchUpdate/UpdateBAStatus',
        title: LangHelper.getSingleResource('BatchUpdate') + " - " + LangHelper.getSingleResource('UpdateBAStatus'),
        width: 600,
        height: 400
    }
};

export const batchUpdateManager = {
    updateMenu: [
        { value: 1, label: LangHelper.getSingleResource('MoveCommissionGroup'), permission: batchUpdatePermission.playerCommGroup },
        { value: 2, label: LangHelper.getSingleResource('UpdateCommissionValue'), permission: batchUpdatePermission.playerCommGroup },
        { value: 3, label: LangHelper.getSingleResource('RemoveFromGroup'), permission: batchUpdatePermission.playerCommGroup },
        { value: 4, label: LangHelper.getSingleResource('UpdateAccountStatus'), permission: batchUpdatePermission.accountStatus },
        { value: 5, label: LangHelper.getSingleResource('UpdateBetLimit'), permission: batchUpdatePermission.betLimit },
        { value: 6, label: LangHelper.getSingleResource('UpdateSpecialLevel'), permission: batchUpdatePermission.specialLevel },
        { value: 7, label: LangHelper.getSingleResource('UpdateOddsGroup'), permission: batchUpdatePermission.oddsGroup },
        { value: 8, label: LangHelper.getSingleResource('UpdateBAStatus'), permission: batchUpdatePermission.baStatus }
    ],
    updateTableInterval: null,
    isReloadingGrid: null,
    getLink: function (action) {
        const commonService = FactoryCommonService.createInstance();
        return commonService.getLink(systemType.playerAdmin, action);
    },
    getMenu: function (option) {
        var result = batchUpdateManager.updateMenu.filter(p => !p.permission || option.permission[p.permission]);
        return result;
    },
    bindMenuEvent: function (handler) {
        var searchInput = $(".ddl-batch-update .Select-input");
        searchInput.find(" > input").attr("readonly", "readonly");
        $(".ddl-batch-update .Select-value").off("click").on("click", () => {
            searchInput.focus();
        });

        window.top.addEventListener("message", (event) => {
            const message = event.data;
            if (message && message.key === 'playerComissionResult') {
                batchUpdateManager.updateTable(handler, message);
            }
         }, false);

        //  window.top.removeEventListener("message", () => {
        //      console.log('removeEventListener message')
        //  });
    },
    updateTable: function (handler, message) {
        var result = message.value;
        if (result && result.isCancel) {
            handler.setState({
                batchUpdateSelectedOption: 0,
                popupBatchUpdateShow: false
            });
        }

        if (result && result.reloadGrid) {
            batchUpdateManager.isReloadingGrid = true;
            var oldPlayerList = handler.props.playerList;
            var newPlayerList = batchUpdateManager.updatePlayerList(oldPlayerList, result);
            handler.props.setPlayerList(newPlayerList);
            batchUpdateManager.isReloadingGrid = false;
        }
    },
    bindGridEvents: function () {
        var checkAll = $("#chk-cust-0");
        var checkCusts = $(".chk-cust");

        checkAll.off("click").on("click", function () {
            var isChecked = $(this).is(":checked");
            checkCusts.prop("checked", isChecked);

            var selectedLength = $(".chk-cust:checked").length;
            batchUpdateManager.showHideBatchUpdateMenu(selectedLength > 1);
        });

        checkCusts.each((_, e) => {
            $(e).off("click").on("click", () => {
                var selectedLength = $(".chk-cust:checked").length;
                var isChecked = checkCusts.length == $(".chk-cust:checked").length;
                checkAll.prop("checked", isChecked);
                batchUpdateManager.showHideBatchUpdateMenu(selectedLength > 1);
            });
        });

        if (!batchUpdateManager.isReloadingGrid) {
            checkAll.prop("checked", false);
            checkCusts.prop("checked", false);
            batchUpdateManager.showHideBatchUpdateMenu(false);
        }
    },
    showHideBatchUpdateMenu: function (isShow) {
        isShow ? $(".ddl-batch-update").removeClass("hide") : $(".ddl-batch-update").addClass("hide");
    },
    getSelectCusts: function () {
        var checkCusts = $(".chk-cust:checked");
        var custIds = [];
        var custNames = [];

        checkCusts.each((_, e) => {
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

        var opt = Object.assign({}, batchUpdatePopups["popupOption_" + selectedOption]);
        opt.url = batchUpdateManager.getLink(opt.url + query);
        return opt;
    },
    updatePlayerList: function (oldPlayerList, result) {
        var custIds = result.custIds.split(",");
        var oldCustIds = result.oldCustIds.split(",");
        var newList = [];
        oldPlayerList.forEach((e) => {
            var newItem = Object.assign({}, e);
            newItem.Selected = oldCustIds.indexOf(e.IbcCustId.toString()) !== -1;

            if (custIds.indexOf(e.IbcCustId.toString()) !== -1) {
                newItem = Object.assign(newItem, result.newValue);
            }

            newList.push(newItem);
        });

        return newList;
    }
};
