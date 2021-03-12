define(['jquery', 'common/site', 'common/popup', './commgroup', 'lib/numberformat',
    'lib/jquery.multiselect.nlevel', 'lib/bootstrap-table/bootstrap-table', 'lib/bootstrap-table/bootstrap-table-fixed-columns'],
    function ($, Site, Popup, CustomerCommGroup, NumberFormatter) {
        'use strict';

        const deleteGroupData = {
            groupId: 0,
            isRemovePlayerComm: false,
            isResetDefault: false
        };

        function initialize() {
            Site.initialize();
            Site.addUserGuide({
                resourceName: 'CommissionGroupList',
                title: Site.getSingleResource('CommissionGroupList')
            });

            _me.initEvent();
        }

        function initEvent() {
            $('#ExportToExcel').click(function () {
                $('#CommissionGroupForm').submit();
            });
            initShowHideProducts();
            initTableComm();
        }

        function confirmDeleteCommGroup(groupId) {
            $.ajax({
                url: Site.resolveClientUrl('CustomerComGroup/GetDefaultGroup'),
                type: 'GET',
                dataType: 'JSon',
                cache: false,
                success: function (response) {
                    deleteGroupData.groupId = groupId;
                    if (response.id.toString() === groupId.toString()) {
                        deleteGroupData.isResetDefault = true;
                        _me.showConfirmDeleteDefaulCommGroupPopup();
                    }
                    else {
                        deleteGroupData.isResetDefault = false;
                        _me.showOptionDeleteCommGroupPopup();
                    }
                }
            });
        }

        function deleteCommGroup() {
            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl('CustomerComGroup/Delete'),
                data: deleteGroupData,
                success: function (result) {
                    if (result.Code > 0) {
                        _me.showResultPopup('Error', result.Message);
                    } else {
                        _me.showResultPopup('Success', Site.getSingleResource('MessageSuccess'));

                        setTimeout(() => {
                            Site.redirect(Site.resolveClientUrl('CustomerComGroup/Index'));
                        }, 3000);
                    }

                    Site.unBlockUI();
                },
                beforeSend: Site.blockUI
            });
        }

        function showConfirmDeleteDefaulCommGroupPopup() {
            const options = {
                modal: true,
                title: Site.getSingleResource('Confirm'),
                height: 'auto',
                id: 'del-default-comm-group',
                width: 380
            };

            const buttons = [
                {
                    text: Site.getSingleResource('OK'),
                    isClosedOnClick: true,
                    click: _me.showOptionDeleteCommGroupPopup,
                    classes: 'btn btn-submit'
                }, {
                    text: Site.getSingleResource('Cancel'),
                    isClosedOnClick: true,
                    classes: 'btn btn-cancel'
                }];

            Popup.openPopupWithButton('<div><p>' + Site.getSingleResource('DefaultGroupMsg') + '</p></div>', options, buttons);
        }

        function showOptionDeleteCommGroupPopup() {
            let popup = $('#option-del-comm-group');

            if (popup.length === 0) {
                popup = $('#confirm-delete-comm-group');
                popup.remove();
                popup.find('input[name="IsRemovePlayerComm"]').on('click', function () {
                    $(this).prop('checked', true);
                });
            }

            const options = {
                modal: true,
                title: Site.getSingleResource('DeleteCommissionGroup'),
                height: 'auto',
                id: 'option-del-comm-group',
                width: 380
            };

            const buttons = [
                {
                    text: Site.getSingleResource('Delete'),
                    isClosedOnClick: true,
                    click: function () {
                        deleteGroupData.isRemovePlayerComm = $('input[name="IsRemovePlayerComm"]:checked').val();
                        _me.showConfirmDeleteCommGroupPopup();
                    },
                    classes: 'btn btn-submit'
                }, {
                    text: Site.getSingleResource('Cancel'),
                    isClosedOnClick: true,
                    click: function () {
                        $('#IsRemovePlayerComm').prop('checked', true);
                    },
                    classes: 'btn btn-cancel',
                    id: 'option-del-comm-group-cancel'
                }];

            Popup.openPopupWithButton(popup, options, buttons);
        }

        function showConfirmDeleteCommGroupPopup() {
            const options = {
                modal: true,
                title: Site.getSingleResource('Confirm'),
                height: 'auto',
                id: 'confirm-del-comm-group',
                width: 380
            };

            const buttons = [
                {
                    text: Site.getSingleResource('OK'),
                    isClosedOnClick: true,
                    click: _me.deleteCommGroup,
                    classes: 'btn btn-submit'
                }, {
                    text: Site.getSingleResource('Cancel'),
                    isClosedOnClick: true,
                    click: _me.showOptionDeleteCommGroupPopup,
                    classes: 'btn btn-cancel',
                    id: 'confirm-del-comm-group-cancel'
                }];

            Popup.openPopupWithButton('<div><p>' + Site.getSingleResource('AreYouSure') + '</p></div>', options, buttons);
        }

        function showResultPopup(title, message) {
            const options = {
                modal: true,
                title: title,
                height: 'auto',
                id: 'result-del-comm-group',
                width: 380
            };

            if ($('#result-del-comm-group').length > 0) {
                $('#result-msg').html(message);
            }

            const buttons = [
                {
                    text: Site.getSingleResource('OK'),
                    isClosedOnClick: true,
                    classes: 'btn btn-submit',
                    id: 'result-OK'
                }];

            Popup.openPopupWithButton('<div><p id="result-msg">' + message + '</p></div>', options, buttons);
        }

        function initShowHideProducts() {
            let products = $('#ProductList').val().split(",");
            var dataUI = [];
            for (let i = 0; i < products.length; i++) {
                var product = products[i];
                dataUI.push({
                    Id: product,
                    Value: product,
                    Text: Site.getSingleResource("Product" + product),
                    Checked: true,
                    Enable: true,
                    Css: '',
                    CssClass: ''
                });
            }

            $("#mult-products").multiSelect({
                minWidth: 200,
                maxHeight: 400,
                shadow: true,
                showHeader: true,
                isCheckAll: true,
                showFooter: false,
                checkAllText: Site.getSingleResource('All'),
                noneSelectedText: Site.getSingleResource('SelectOptions'),
                tooltipNoItem: Site.getSingleResource('ThereIsNoSelectedItem'),
                selectedText: Site.getSingleResource('SelectedItemCount'),
                listData: dataUI,
                onCheck: function () {
                    var l = $(".fixed-table-container > .fixed-table-body").scrollLeft();
                    freezeTableCommColumns();
                    if (l > 0) {
                        $(".fixed-table-container > .fixed-table-body").scrollLeft(l);
                    }

                    $('#comm-group-list .delete-comm-group').on('click', function (e) {
                        _me.confirmDeleteCommGroup($(e.target).data('id'));
                    });
                }
            });
        }

        function initTableComm() {
            freezeTableCommColumns();

            $('#comm-group-list .delete-comm-group').on('click', function (e) {
                _me.confirmDeleteCommGroup($(e.target).data('id'));
            });
        }

        function freezeTableCommColumns() {
            let $tableComm = $('#comm-group-list');

            const formatter = new NumberFormatter();

            let columns = [
                { field: 'RowNum', valign: 'middle', align: 'center', title: '#', width: 80, class: 'col-frozen' },
                { field: 'GroupCommId', valign: 'middle', align: 'center', title: Site.getSingleResource('GroupId'), width: 100, class: 'col-frozen' },
                {
                    field: 'GroupName',
                    valign: 'middle',
                    halign: 'center',
                    align: 'left',
                    title: Site.getSingleResource('GroupName'),
                    width: 200,
                    class: 'col-frozen',
                    formatter: function (val, row) {
                        var url = Site.resolveClientUrl('CustomerComGroup/Update?groupId=' + row.GroupCommId);
                        return `<a href="${url}" title="${Site.getSingleResource('EditGroup')}" class="truncate">${val}</a>`;
                    }
                },
                {
                    field: 'IsDefaultGroup',
                    valign: 'middle',
                    align: 'center',
                    title: Site.getSingleResource('DefaultGroup'),
                    width: 100,
                    class: 'col-frozen',
                    formatter: function (val) {
                        var s = val ? "icon-checkmark" : "";
                        return `<div class="${s}"></div>`;
                    }
                },
                {
                    field: 'NumberOfCustomer',
                    valign: 'middle',
                    align: 'center',
                    title: Site.getSingleResource('NoOfCustmer'),
                    width: 120,
                    class: 'col-frozen',
                    formatter: function (val) {
                        var n = formatter.formatNumber(val, { format: '#,##0' });
                        return `<a>${n}</a>`;
                    }
                }];

            let products = $('#ProductList').val().split(",");
            let selectedProducts = $("#mult-products").multiSelect("getCheckedItems");
            for (var i = 0; i < products.length; i++) {
                var key = products[i];
                columns.push({
                    field: 'Col-' + key,
                    valign: 'middle',
                    align: 'center',
                    title: Site.getSingleResource('Product' + key),
                    class: `col-${key} ${selectedProducts.indexOf(key) != -1 ? "" : "hide"}`,
                    width: 115,
                    formatter: function (val) {
                        var n = formatter.formatNumber(val * 100, { format: '#,##0.00' });
                        return `${n}%`;
                    }
                });
            }

            columns.push({
                field: 'GroupColor',
                valign: 'middle',
                align: 'center',
                title: Site.getSingleResource('GroupColor'),
                width: 100,
                class: 'col-frozen',
                formatter: function (val) {
                    return `<div class="group-color-box" style="background-color:${val || '#ffffff'};"></div>`;
                }
            });

            columns.push({
                field: 'EditGroup',
                valign: 'middle',
                align: 'center',
                title: Site.getSingleResource('EditGroup'),
                width: 100,
                class: 'col-frozen',
                formatter: function (_, row) {
                    let editUrl = Site.resolveClientUrl('CustomerComGroup/Update?groupId=' + row.GroupCommId);
                    return `<a class="icon-pencil" title="${Site.getSingleResource('EditGroup')}" href="${editUrl}"></a>\
                            <a class="icon-bin delete-comm-group" title="${Site.getSingleResource('DeleteGroup')}" data-id="${row.GroupCommId}"></a>`;
                }
            });

            let rows = [];
            let data = $.parseJSON($("#CommGroupsData").val());
            for (var i = 0; i < data.length; i++) {
                let item = data[i];
                let row = {
                    RowNum: i + 1,
                    ...item
                };

                let commValues = item.CommValues;
                for (var j = 0; j < products.length; j++) {
                    var key = products[j];
                    row['Col-' + key] = commValues[key];
                }

                rows.push(row);
            }

            let gridOptions = {
                columns: columns,
                data: rows,
                fixedColumns: true,
                fixedNumber: 5,
                fixedRightNumber: 2,
                showColumns: false
            };

            $tableComm.bootstrapTable('destroy').bootstrapTable(gridOptions);
        }

        const _me = {
            initialize: initialize,
            initEvent: initEvent,
            confirmDeleteCommGroup: confirmDeleteCommGroup,
            showConfirmDeleteDefaulCommGroupPopup: showConfirmDeleteDefaulCommGroupPopup,
            showOptionDeleteCommGroupPopup: showOptionDeleteCommGroupPopup,
            showConfirmDeleteCommGroupPopup: showConfirmDeleteCommGroupPopup,
            showResultPopup: showResultPopup,
            deleteCommGroup: deleteCommGroup
        };

        return _me;
    }
);
