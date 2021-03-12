'use strict';
define(['jquery', 'common/site',
    'lib/jqgrid/jquery.jqgrid.additional', 'common/popup',
    'lib/jqgrid/jquery.jqGrid.en', 'lib/jqgrid/jquery.jqGrid.min'],
    function ($, Site, jqGridAdditional, Popup) {
        const GRID_ID = 'ReportGrid';
        let _ajaxRequest = null;

        const _me = {
            initialize: function () {
                Site.initialize();
                Site.addUserGuide({
                    resourceName: 'CustomerLog',
                    title: Site.getSingleResource('CustomerLog')
                });

                $(window).on('resize', function () {
                    _me.resizeGrid();
                });

                $(window).on('unload', function () {
                    $(window).off('resize');
                });
            },

            loadGrid: function (url, gridOption, hasCheckbox) {
                const option = {
                    type: 'GET',
                    url: url,
                    dataType: 'json',
                    contentType: 'application/json',
                    cache: false,
                    success: function (response) {
                        jqGridAdditional.clearGrid(GRID_ID);

                        const data = response.rows;
                        if (data.length === 0) {
                            jqGridAdditional.setNoInfo();
                            return;
                        }

                        _me.bindData(data, gridOption, hasCheckbox);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };

                _me.stopAjaxRequest();
                _ajaxRequest = $.ajax(option);
            },

            bindData: function (data, gridOption, hasCheckbox) {
                const grid = $('#ReportGrid');

                gridOption.data = data;
                gridOption.gridComplete = function () {
                    if (hasCheckbox) {
                        _me.checkShowAll();
                    }

                    const colModels = grid[0].p.colModel;
                    $.each(grid[0].grid.headers, function (index, value) {
                        if (!colModels[index].sortable) {
                            $('.ui-jqgrid-sortable', value.el).css({ cursor: 'default' });
                        }
                    });

                    jqGridAdditional.gridComplete();
                    _me.resizeGrid();
                };

                gridOption.onSortCol = function (index, columIndex) {
                    $('.ui-jqgrid-sortable', 'th').css({ 'text-decoration': 'none' });
                    $('.ui-jqgrid-sortable', grid[0].grid.headers[columIndex].el).css({ 'text-decoration': 'underline' });
                };

                grid.jqGrid(gridOption);
            },

            resizeGrid: function () {
                const $grid = $('#' + GRID_ID);
                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth($grid);
            },

            checkShowAll: function () {
                const checked = $('#showall').is(':checked');
                $('.old-value').toggleClass('hide', !checked);
            },

            stopAjaxRequest: function () {
                if (_ajaxRequest !== null) {
                    _ajaxRequest.abort();
                }
            },
            informRemoveFunction: function () {
                const buttons = [
                    {
                        text: Site.getSingleResource('OK'),
                        isClosedOnClick: true,
                        classes: 'btn btn-submit'
                    }];

                Popup.openPopupWithButton(
                    '<div><p>' + Site.getSingleResource('Message_RemoveCustomerLog') + '</p></div>',
                    { width: 700, height: 150 },
                    buttons);
            }
        };

        return _me;
    });
