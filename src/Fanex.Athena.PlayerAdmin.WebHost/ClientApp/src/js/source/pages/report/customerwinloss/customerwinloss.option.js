define([
    'jquery',
    'common/site',
    'lib/jqgrid/jquery.jqgrid.additional'],
    function ($, Site, jqGridAdditional) {
        'use strict';

        const indexColumn =
        {
            name: 'index',
            index: 'index',
            resizable: false,
            width: 40,
            align: 'center',
            valign: 'middle',
            sortable: false
        };

        const externalIdColumn =
        {
            name: 'externalId',
            index: 'externalId',
            resizable: false,
            width: 120,
            align: 'left',
            valign: 'middle',
            sortable: true,
            cellattr: function () {
                return ' style="text-overflow: ellipsis"';
            },
            formatter: function (cellvalue, options, rowObject) {
                const param =
                {
                    custId: rowObject.custId,
                    externalId: rowObject.externalId,
                    systemId: rowObject.systemId
                };
                return '<a class="bet-list-link" href="javascript:;" data-drill-down=\'' + JSON.stringify(param) + '\'>' + cellvalue + '</a>';
            }
        };

        const systemIdColumn =
        {
            name: 'systemId',
            index: 'systemId',
            resizable: false,
            width: 120,
            align: 'left',
            valign: 'middle',
            sortable: true
        };

        const turnoverColumn =
        {
            name: 'turnover',
            index: 'turnover',
            width: 110,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

        const totalTransactionColumn =
        {
            name: 'totalTransaction',
            index: 'totalTransaction',
            width: 90,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'integer',
            formatoptions:
            {
                thousandsSeparator: ','
            }
        };

        const customerWlColumn =
        {
            name: 'customerWL',
            index: 'customerWL',
            width: 90,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        const playerCommColumn =
        {
            name: 'playerComm',
            index: 'playerComm',
            width: 50,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        const customerWlTotalColumn =
        {
            name: 'customerTotalWL',
            index: 'customerTotalWL',
            width: 90,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        const betEffectiveColumn =
        {
            name: 'betEffective',
            index: 'betEffective',
            width: 100,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

        const licenseeWLColumn =
        {
            name: 'licenseeWL',
            index: 'licenseeWL',
            width: 100,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: jqGridAdditional.formatLicenseeWl
        };

        const bacommColumn =
        {
            name: 'baComm',
            index: 'baComm',
            width: 100,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };
        const amtDueToBAColumn =
        {
            name: 'amtDueToBA',
            index: 'amtDueToBA',
            width: 100,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

        const colNameList = [
            '#',
            'ExternalId',
            'SystemId',
            'Turnover',
            'TotalTransaction',
            'WinLoss',
            'Comm',
            'Total',
            'EffectiveBettingValue',
            'LicenseeW_L',
            'BACommission',
            'AmtDueToBA'
        ];

        const colModelList = [
            indexColumn,
            externalIdColumn,
            systemIdColumn,
            turnoverColumn,
            totalTransactionColumn,
            customerWlColumn,
            playerCommColumn,
            customerWlTotalColumn,
            betEffectiveColumn,
            licenseeWLColumn,
            bacommColumn,
            amtDueToBAColumn
        ];

        const _me =
        {
            urlData: 'CustomerWinLossReport/Index',
            urlExcelData: 'CustomerWinLossReport/Index',
            urlPromotionPrizeExcelData: 'PromotionPrizeReport/ExportExcel',
            hasCustomerComm: true,
            isAllowBA: true,
            customerCommIndex: 11,
            productBAIndex: 13,
            customerWlIndex: 5,
            totalColspan: 3,
            totalColspanFakeLicensee: 2,
            gridOption:
            {
                datatype: 'local',
                colNames: [],
                colModel: [],
                data: [],
                height: '100%',
                autowidth: false,
                forceFit: false,
                shrinkToFit: true,
                loadtext: '',
                loadui: 'disable',
                loadonce: true,
                footerrow: true,
                userDataOnFooter: false,
                gridview: true,
                rownumbers: false,
                sortable: false
            },

            getGridOption: function (totalRows) {
                _me.gridOption.colNames = _me.getColName();
                _me.gridOption.colModel = _me.getColModel(totalRows);
                return Site.clone(_me.gridOption);
            },

            setGroupHeaderColumns: function (grid) {
                const groupCustomerWinloss = _me.hasCustomerComm ?
                    {
                        startColumnName: 'customerWL',
                        numberOfColumns: 3,
                        titleText: Site.getSingleResource('CustomerW_L')
                    } : {};

                grid.jqGrid('setGroupHeaders',
                    {
                        useColSpanStyle: true,
                        groupHeaders: [groupCustomerWinloss]
                    });
            },

            getColName: function () {
                const colNames = Site.clone(colNameList);

                colNames[_me.customerWlIndex] = _me.hasCustomerComm ? 'WinLoss' : 'CustomerW_L';

                return Site.getResources(colNames);
            },

            getColModel: function (totalRows) {
                customerWlTotalColumn.hidden = !_me.hasCustomerComm;
                playerCommColumn.hidden = !_me.hasCustomerComm;
                bacommColumn.hidden = !_me.isAllowBA;
                amtDueToBAColumn.hidden = !_me.isAllowBA;
                systemIdColumn.hidden = Site.isFakeLicensee();

                const colModels = Site.clone(colModelList);

                if (totalRows <= 1) {
                    $.each(colModels, function (index, item) {
                        item.sortable = false;
                    });
                }

                return colModels;
            }
        };

        return _me;
    });