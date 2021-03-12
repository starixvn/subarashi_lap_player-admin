define(['jquery', 'common/site', 'lib/jqgrid/jquery.jqgrid.additional'],
    function ($, Site, jqGridAdditional) {
        'use strict';
        const BA_PRODUCTCODE = 'BA';

        const betlistLink = function (cellvalue, options, rowObject) {
            const param =
            {
                DrillDownCurrencyId: rowObject.currencyId,
                DrillDownCurrencyName: rowObject.currencyName,
                CustomerId: rowObject.customerId,
                ExternalId: rowObject.externalId,
                SystemId: rowObject.systemId
            };
            return '<a class="bet-list-link" href="javascript:;" data-drill-down=\'' + JSON.stringify(param) + '\'>' + cellvalue + '</a>';
        };
        const drilldownLink = function (cellvalue, options, rowObject) {
            const parentClassName = rowObject.isLeaf ? '' : ' text-bold';
            const param =
            {
                PlatformType: rowObject.isLeaf ? Number(rowObject.parent) : rowObject.platformType,
                ProductCode: rowObject.isLeaf ? rowObject.productCode : undefined
            };

            return '<a class="drilldown' + parentClassName + '" href="javascript:;" data-drill-down=\'' + JSON.stringify(param) + '\'>' + cellvalue + '</a>';
        };

        const idColumn = {
            name: 'platformType',
            index: 'platformType',
            key: true,
            hidden: true,
            align: 'center'
        };
        const platformNameColumn = {
            name: 'name',
            index: 'name',
            width: 120,
            align: 'left',
            valign: 'middle',
            sortable: false,
            resizable: false,
            formatter: drilldownLink
        };
        const indexColumn = {
            name: 'index',
            index: 'index',
            width: 60,
            align: 'center',
            valign: 'middle',
            sortable: false,
            resizable: false
        };
        const externalIdColumn = {
            name: 'externalId',
            index: 'externalId',
            width: 85,
            align: 'left',
            sortable: true,
            resizable: false,
            formatter: betlistLink
        };
        const systemIdColumn = {
            name: 'systemId',
            index: 'systemId',
            width: 120,
            align: 'left',
            sortable: true,
            resizable: false,
            formatter: betlistLink
        };
        const currencyColumn = {
            name: 'currencyName',
            index: 'currencyName',
            width: 60,
            align: 'center',
            sortable: true,
            resizable: false
        };
        const turnoverColumn = {
            name: 'turnover',
            index: 'turnover',
            resizable: false,
            width: 85,
            align: 'right',
            sortable: true,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };
        const turnoverPercentColumn = {
            name: 'turnoverPercent',
            index: 'turnoverPercent',
            resizable: false,
            width: 85,
            align: 'right',
            sortable: true,
            formatter: 'currency',
            formatoptions: { decimalPlaces: 2, suffix: '%' }
        };
        const totalTransColumn = {
            name: 'totalTransactions',
            index: 'totalTransactions',
            width: 95,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'integer',
            frozen: true,
            formatoptions:
            {
                thousandsSeparator: ','
            }
        };
        const customerWlColumn = {
            name: 'customerWinLoss',
            index: 'customerWinLoss',
            width: 90,
            align: 'right',
            sortable: true,
            resizable: false,
            frozen: true,
            formatter: jqGridAdditional.formatDecimal
        };
        const playerCommColumn = {
            name: 'customerComm',
            index: 'customerComm',
            width: 50,
            align: 'right',
            sortable: true,
            resizable: false,
            frozen: true,
            formatter: jqGridAdditional.formatDecimal
        };
        const customerWlTotalColumn = {
            name: 'customerTotal',
            index: 'customerTotal',
            width: 90,
            align: 'right',
            sortable: true,
            resizable: false,
            frozen: true,
            formatter: jqGridAdditional.formatDecimal
        };
        const licenseeWlColumn = {
            name: 'licenseeWL',
            index: 'licenseeWL',
            width: 85,
            align: 'right',
            sortable: true,
            resizable: false,
            frozen: true,
            title: true,
            formatter: jqGridAdditional.formatLicenseeWl
        };
        const baCommissionColumn = {
            name: 'baCommission',
            index: 'baCommission',
            width: 70,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };
        const amtDueToBaColumn = {
            name: 'amtDueToBA',
            index: 'amtDueToBA',
            width: 60,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };
        const currencyIdColumn = {
            name: 'currencyId',
            index: 'currencyId',
            width: 0,
            align: 'center',
            sortable: false,
            resizable: false,
            hidden: true
        };
        const customerIdColumn = {
            name: 'customerId',
            index: 'customerId',
            width: 0,
            align: 'center',
            sortable: false,
            resizable: false,
            hidden: true
        };
        const columnConfigLevel0 = {
            colNames: [
                'Id',
                'Platform',
                'Turnover',
                'TurnoverPercent',
                'TotalTransaction',
                'LicenseeW_L',
                'CurrencyId',
                'CustomerId'],
            colModels: [
                idColumn,
                platformNameColumn,
                turnoverColumn,
                turnoverPercentColumn,
                totalTransColumn,
                licenseeWlColumn,
                currencyIdColumn,
                customerIdColumn]
        };
        const columnConfigLevel1 = {
            colNames: [
                '#',
                'ExternalId',
                'SystemId',
                'Currency',
                'Turnover',
                'TotalTransaction',
                'LicenseeW_L',
                'Id',
                'CurrencyId',
                'CustomerId'],
            colModels: [
                indexColumn,
                externalIdColumn,
                systemIdColumn,
                currencyColumn,
                turnoverColumn,
                totalTransColumn,
                licenseeWlColumn,
                idColumn,
                currencyIdColumn,
                customerIdColumn]
        };
        const columnConfigCustomerComm = {
            colsCustomerComm: {
                colNames: ['WinLoss', 'Comm', 'Total'],
                colModels: [customerWlColumn, playerCommColumn, customerWlTotalColumn]
            },
            colsWithoutComm: {
                colNames: ['Customer W/L'],
                colModels: [customerWlTotalColumn]
            }
        };
        const columnConfigBA = {
            colNames: ['BA<br/>@[Commission]', '@[AmtDueToBA]'],
            colModels: [baCommissionColumn, amtDueToBaColumn]
        };

        function getColConfigLevel0(cols, customerWL) {
            let customerWlIndex = 4;

            customerWL.forEach(function (item) {
                cols.insertAfter(item, customerWlIndex);
                ++customerWlIndex;
            });

            return cols;
        }

        function getColConfigLevel1(cols, customerWL, colsBA) {
            let customerWlIndex = 5;
            customerWL.forEach(function (item) {
                cols.insertAfter(item, customerWlIndex);
                ++customerWlIndex;
            });

            if (colsBA !== null) {
                let bAIndex = customerWlIndex + 1;
                colsBA.forEach(function (item) {
                    cols.insertAfter(item, bAIndex);
                    ++bAIndex;
                });
            }

            return cols;
        }

        function isProductChecked(productOptions, productCode) {
            const length = productOptions.length;

            for (let i = 0; i < length; i++) {
                if (productOptions[i].Code === productCode) {
                    return productOptions[i].Status;
                }
            }

            return false;
        }

        const _me = {
            hasCustomerComm: true,
            pageSize: 50,
            totalColspan: [0, 4],
            urlData: 'PlatformWinLossReport/Index',
            urlExcelData: 'PlatformWinLossReport/Index',
            gridOption: {
                datatype: 'jsonstring',
                colNames: [],
                colModel: [],
                data: [],
                viewrecords: true,
                height: '100%',
                autowidth: false,
                forceFit: false,
                shrinkToFit: true,
                loadtext: '',
                loadui: 'disable',
                loadonce: true,
                gridview: true,
                footerrow: true,
                cmTemplate: { title: true }, //enable tooltip
                treeGrid: true,
                treeGridModel: 'adjacency',
                treedatatype: 'local',
                ExpandColumn: 'name',
                treeReader: {
                    level_field: 'level',       // jshint ignore:line
                    parent_id_field: 'parent',  // jshint ignore:line
                    leaf_field: 'isLeaf'        // jshint ignore:line
                },
                treeIcons: { plus: 'ui-icon-circlesmall-plus icon-arrow', minus: 'ui-icon-circlesmall-minus icon-arrow', leaf: '' },
                pager: '#ppdata',
                rowNum: 50,
                rowList: [50, 100, 200, 300, 400, 500, 1000, 2000]
            },

            getColName: function (level, productOptions) {
                const customerWL = _me.hasCustomerComm ? columnConfigCustomerComm.colsCustomerComm.colNames : columnConfigCustomerComm.colsWithoutComm.colNames;

                if (level === 0) {
                    return Site.getResources(getColConfigLevel0(Site.clone(columnConfigLevel0.colNames), customerWL));
                }

                const allowedBa = isProductChecked(productOptions, BA_PRODUCTCODE);
                const colsBA = allowedBa ? columnConfigBA.colNames : null;

                return Site.getResources(getColConfigLevel1(Site.clone(columnConfigLevel1.colNames), customerWL, colsBA));
            },

            getColModel: function (level, productOptions) {
                const customerWL =  Site.clone(_me.hasCustomerComm ? 
                    columnConfigCustomerComm.colsCustomerComm.colModels : 
                    columnConfigCustomerComm.colsWithoutComm.colModels);

                if (level === 0) {
                    const colModels = getColConfigLevel0(Site.clone(columnConfigLevel0.colModels), customerWL);

                    colModels.forEach(function (item) {
                        item.sortable = false;
                    });

                    return colModels;
                }

                const allowedBa = isProductChecked(productOptions, BA_PRODUCTCODE);
                const colsBA = allowedBa ? columnConfigBA.colModels : null;

                return getColConfigLevel1(Site.clone(columnConfigLevel1.colModels), customerWL, colsBA);
            },

            getGridOption: function (level, productOptions) {
                _me.gridOption.colNames = _me.getColName(level, productOptions);
                _me.gridOption.colModel = _me.getColModel(level, productOptions);

                return _me.gridOption;
            },

            setCustomerWLGroupHeaderColumns: function (grid) {
                grid.jqGrid('setGroupHeaders',
                    {
                        useColSpanStyle: true,
                        groupHeaders: [{
                            startColumnName: 'customerWinLoss',
                            numberOfColumns: 3,
                            titleText: Site.getSingleResource('CustomerW_L')
                        }]
                    });
            }
        };

        return _me;
    }
);