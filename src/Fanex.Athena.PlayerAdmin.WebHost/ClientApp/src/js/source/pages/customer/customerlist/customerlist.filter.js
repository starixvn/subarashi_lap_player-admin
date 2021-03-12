'use strict';
define(['jquery', 'common/site', './customerlist.state', './customerlist.service', './customerlist.service.dormant',
    'components/pluginhelper/autocomplete', 'lib/jquery.multiselect.nlevel'],
    function ($, Site, CustomerListState, CustomerListService, CustomerListServiceDormant, AutoComplete) {
        let _state;
        let _dormantState;

        const _me = {
            initialize: function () {
                _state = CustomerListState.getState();
                _dormantState = CustomerListState.getDormantState();
                Site.addUserGuide({
                    resourceName: 'CustomerList',
                    title: Site.getSingleResource('CustomerList')
                });
                AutoComplete.initialize({
                    url: Site.resolveClientUrl('SearchCustomerAPI/SearchActiveAndDormantCustomer'),
                    target: '#Username'
                });
                _me.initEvent();
            },
            submit: function () {
                _state.pagingOption.pageindex = 1;
                _dormantState.pagingOption.pageindex = 1;
                CustomerListService.submit();
                CustomerListServiceDormant.submit();
                $('.page-results').show();
            },
            initEvent: function () {
                $('#SubmitButton').click(function () {
                    _me.submit();
                });

                $('#Username').on('keyup', function (e) {
                    if (e.keyCode === 13) {
                        _state.pagingOption.pageindex = 1;
                        _dormantState.pagingOption.pageindex = 1;
                        CustomerListService.submit();
                        CustomerListServiceDormant.submit();
                    }
                });

                _me.initGridColumnOption();
                _me.initOddGroupOption();
                _me.initSubsidiaryOption();
            },

            initGridColumnOption: function () {
                const data = $.parseJSON($('#AllowedGridColumns').val());
                const totalColumns = data.length;
                const dataUI = [];
                let checkedCount = 0;
                for (let i = 0; i < totalColumns; i++) {
                    const isChecked = Site.asBool(data[i].Checked);
                    if (isChecked) {
                        checkedCount++;
                    }
                    dataUI.push({
                        Id: data[i].Name,
                        Value: data[i].Name,
                        Text: Site.getSingleResource(data[i].Name),
                        Checked: isChecked,
                        Enable: true,
                        Css: '',
                        CssClass: ''
                    });
                }
                const ischeckAll = checkedCount === totalColumns;
                $('#grid-columns-multiselect').multiSelect({
                    minWidth: 160,
                    maxHeight: 400,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: ischeckAll,
                    showFooter: false,
                    checkAllText: Site.getSingleResource('All'),
                    noneSelectedText: Site.getSingleResource('SelectOptions'),
                    tooltipNoItem: Site.getSingleResource('ThereIsNoSelectedItem'),
                    selectedText: Site.getSingleResource('SelectedItemCount'),
                    listData: dataUI,
                    onCheck: function () {
                        const checkedItems = $('#grid-columns-multiselect').multiSelect('getCheckedItems');
                        if (!checkedItems.length) {
                            $('#SubmitButton').prop('disabled', true);
                        }
                        else {
                            const validOddItems = !$('#odds-group-multiselect').length
                                || $('#odds-group-multiselect').multiSelect('getCheckedItems').length;
                            const validSubItems = !$('#subsidiary-multiselect').length
                                || !$('#IsViewOnSubsidiary').is(':checked')
                                || $('#subsidiary-multiselect').multiSelect('getCheckedItems').length;

                            $('#SubmitButton').prop('disabled', !(validOddItems && validSubItems));
                        }
                    }
                });
            },

            initOddGroupOption: function () {
                let data = $('#OddsGroups').val();
                if (!data) {
                    return;
                }

                data = $.parseJSON(data);
                const totalColumns = data.length;
                const dataUI = [];
                let checkedCount = 0;
                for (let i = 0; i < totalColumns; i++) {
                    const isChecked = Site.asBool(data[i].Checked);
                    if (isChecked) {
                        checkedCount++;
                    }

                    dataUI.push({
                        Id: data[i].Name,
                        Value: data[i].Name,
                        Text: Site.getSingleResource(data[i].Name),
                        Checked: isChecked,
                        Enable: true,
                        Css: '',
                        CssClass: ''
                    });
                }

                const ischeckAll = checkedCount === totalColumns;
                $('#odds-group-multiselect').multiSelect({
                    minWidth: 100,
                    maxHeight: 400,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: ischeckAll,
                    showFooter: false,
                    checkAllText: Site.getSingleResource('All'),
                    noneSelectedText: Site.getSingleResource('SelectOptions'),
                    tooltipNoItem: Site.getSingleResource('ThereIsNoSelectedItem'),
                    selectedText: Site.getSingleResource('SelectedItemCount'),
                    listData: dataUI,
                    onCheck: function () {
                        const checkedItems = $('#odds-group-multiselect').multiSelect('getCheckedItems');
                        if (!checkedItems.length) {
                            $('#SubmitButton').prop('disabled', true);
                        }
                        else {
                            const validColumnsItems = $('#grid-columns-multiselect').multiSelect('getCheckedItems').length;
                            const validSubItems = !$('#subsidiary-multiselect').length
                                || !$('#IsViewOnSubsidiary').is(':checked')
                                || $('#subsidiary-multiselect').multiSelect('getCheckedItems').length;

                            $('#SubmitButton').prop('disabled', !(validColumnsItems && validSubItems));
                        }
                    }
                });
            },

            initSubsidiaryOption: function () {
                const data = $('#SubsidiaryList').val();
                if (!data) {
                    return;
                }

                const enableSubmitButton = function (isViewOnSubsidiary) {
                    const checkedItems = $('#subsidiary-multiselect').multiSelect('getCheckedItems');
                    if (isViewOnSubsidiary && !checkedItems.length) {
                        $('#SubmitButton').prop('disabled', true);
                    }
                    else {
                        const validColumnsItems = $('#grid-columns-multiselect').multiSelect('getCheckedItems').length;
                        const validOddItems = !$('#odds-group-multiselect').length
                            || $('#odds-group-multiselect').multiSelect('getCheckedItems').length;

                        $('#SubmitButton').prop('disabled', !(validColumnsItems && validOddItems));
                    }
                }

                $('#IsViewOnSubsidiary').on('change', function (e) {
                    const checked = e.target.checked;
                    $('#subsidiary-view').toggleClass('hide', !checked);
                    enableSubmitButton(checked);
                });

                const groupData = CustomerListService.getSubsidiaryTree($.parseJSON(data));

                $('#subsidiary-multiselect').multiSelect(
                    {
                        minWidth: 100,
                        maxHeight: 400,
                        shadow: true,
                        showHeader: true,
                        isCheckAll: true,
                        showFooter: false,
                        checkAllText: Site.getSingleResource('All'),
                        selectedText: Site.getSingleResource('SelectedItemCount'),
                        noneSelectedText: Site.getSingleResource('SelectOptions'),
                        tooltipNoItem: Site.getSingleResource('ThereIsNoSelectedItem'),
                        listData: groupData,
                        onCheck: function () {
                            enableSubmitButton(true);
                        }
                    });
            }
        };

        return _me;
    });
