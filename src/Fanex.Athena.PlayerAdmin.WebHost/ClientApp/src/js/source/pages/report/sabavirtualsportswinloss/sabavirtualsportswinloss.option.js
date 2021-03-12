define(
    ['jquery', 'common/querystring', 'common/site', 'lib/jqgrid/jquery.jqgrid.additional', 'lib/linq'],
    function ($, QueryString, Site, jqGridAdditional) {
        'use strict';
        const IGNORED_LICENSEEWL_PRODUCTS = ['BA'];
        const IGNORED_TURNOVER_PRODUCTS = ['BT', 'CO'];
        const _me =
        {
            isDisableBetList: false,
            licenseeId: 0,
            currencyIndex: 4,
            turnoverIndex: 5,
            licenseeWlIndex: 10,
            customerWlTotalIndex: 9,
            customerWlIndex: 7,
            hasCustomerComm: true,
            pageSize: 50,
            nonePagingSize: 2000,
            rowNum: 50,
            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000],
            totalColspan: [2, 2, 3],
            //level 0,1,2
            urlData: 'api/SabaVirtualSportsWinLossReport/Index',
            urlExcelData: 'api/SabaVirtualSportsWinLossReport/Index',
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
                userDataOnFooter: true,
                userData: [],
                grouping: true,
                viewrecords: true,
                gridview: true,
                rownumbers: true
            },
            restoreExpanse: function () {
                for (let property in _expanseTypes) {
                    if (_expanseTypes.hasOwnProperty(property)) {
                        //_me.toggle(_expanseTypes[property], property);
                    }
                }
            }
        };

        // store state of toggle() function
        const _expanseTypes = {};

        const setDrillDownLink = function (level, currencyId, custId, datetime, currencyName, cellvalue) {
            return '<a class="drilldown" href="javascript:;" onclick="return sabavirtualsportswinlossDrilldown(' + level + ',' +
                currencyId + ',' + custId + ',\'' + datetime + '\',\'' + currencyName + '\');">' + cellvalue + '</a>';
        };

        const currencyFormatter = function (cellvalue, options, rowObject) {
            const level = Number(_me.licenseeId) === 1 ? 1 : 2;
            return setDrillDownLink(level, rowObject.CurrencyId, 0, rowObject.Date, rowObject.CurrencyName, cellvalue);
        };

        const systemIdFormatter = function (cellvalue, options, rowObject) {
            return setDrillDownLink(2, rowObject.CurrencyId, rowObject.CustomerId, rowObject.Date, rowObject.CurrencyName, cellvalue);
        };

        const betlistLink = function (cellvalue, options, rowObject) {
            const param =
            {
                DrillDownFromDate: rowObject.Date,
                DrillDownToDate: rowObject.Date,
                DrillDownCurrencyId: rowObject.CurrencyId,
                DrillDownCurrencyName: rowObject.CurrencyName,
                CustomerId: rowObject.CustomerId,
                ExternalId: rowObject.ExternalId,
                SystemId: rowObject.SystemId
            };

            return '<a class="bet-list-link" href="javascript:;" data-drill-down=\'' + JSON.stringify(param) + '\'>' + cellvalue + '</a>';
        };

        //#region Columns

        const indexColumn =
        {
            name: 'Index',
            index: 'Index',
            resizable: false,
            width: 45,
            align: 'center',
            valign: 'middle',
            sortable: false
        };

        const dateColumn =
        {
            name: 'Date',
            index: 'Date',
            resizable: false,
            width: 85,
            align: 'center',
            valign: 'middle',
            sortable: true,
            formatter: 'date',
            formatoptions:
            {
                newformat: 'm/d/Y'
            }
        };

        const externalIdColumn =
        {
            name: 'ExternalId',
            index: 'ExternalId',
            resizable: false,
            width: 120,
            align: 'left',
            sortable: true
        };

        const systemIdColumn =
        {
            name: 'SystemId',
            index: 'SystemId',
            resizable: false,
            width: 150,
            align: 'left',
            sortable: true
        };

        const currencyColumn =
        {
            name: 'CurrencyName',
            index: 'CurrencyName',
            width: 65,
            align: 'center',
            sortable: true,
            resizable: false,
            formatter: currencyFormatter
        };

        const turnoverColumn =
        {
            name: 'TotalTurnOverSingle',
            index: 'TotalTurnOverSingle',
            width: 120,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            hidden: false,
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

        const totalTransColumn =
        {
            name: 'TotalTransaction',
            index: 'TotalTransaction',
            width: 80,
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

        const customerWlColumn =
        {
            name: 'CustomerWinLoss',
            index: 'CustomerWinLoss',
            width: 120,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        const playerCommColumn =
        {
            name: 'CustomerComm',
            index: 'CustomerComm',
            width: 70,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        const customerWlTotalColumn =
        {
            name: 'CustomerTotal',
            index: 'CustomerTotal',
            width: 120,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: jqGridAdditional.formatDecimal
        };

        const licenseeWlColumn =
        {
            name: 'TotalLicenseeWLSingle',
            index: 'TotalLicenseeWLSingle',
            width: 120,
            align: 'right',
            sortable: true,
            resizable: false,
            hidden: false,
            title: true,
            formatter: jqGridAdditional.formatLicenseeWl
        };

        const cashOutCommissionColumn =
        {
            name: 'CashOutCommission',
            index: 'CashOutCommission',
            width: 85,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

        const baCommissionColumn =
        {
            name: 'BACommission',
            index: 'BACommission',
            width: 85,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

        const amtDueToBaColumn =
        {
            name: 'AmtDueToBA',
            index: 'AmtDueToBA',
            width: 85,
            align: 'right',
            sortable: true,
            resizable: false,
            formatter: 'currency',
            formatoptions: jqGridAdditional.DECIMAL_FORMAT_OPTIONS
        };

        const currencyIdColumn =
        {
            name: 'CurrencyId',
            index: 'CurrencyId',
            width: 2,
            align: 'center',
            sortable: true,
            resizable: false,
            hidden: true
        };

        const customerIdColumn =
        {
            name: 'CustomerId',
            index: 'CustomerId',
            width: 2,
            align: 'center',
            sortable: true,
            resizable: false,
            hidden: true
        };

        const productColumn =
        {
            name: 'productName',
            index: 'productName',
            width: 120,
            sortable: true,
            resizable: false,
            align: 'right',
            hidden: true,
            title: true,
            formatter: jqGridAdditional.formatLicenseeWl
        };

        //#endregion

        const colNameList = [
            '#',
            'Date',
            'ExternalId',
            'SystemId',
            'Currency',
            'Turnover',
            'TotalTransaction',
            'WinLoss', 'Comm',
            'Total', 'LicenseeW_L',
            '@[ProductCO]<br/>@[Commission]',
            'BA<br/>@[Commission]',
            '@[AmtDueToBA]',
            'CurrencyId',
            'CustomerId'];

        const colModelList = [indexColumn, dateColumn, externalIdColumn, systemIdColumn, currencyColumn,
            turnoverColumn, totalTransColumn, customerWlColumn, playerCommColumn,
            customerWlTotalColumn, licenseeWlColumn, cashOutCommissionColumn, baCommissionColumn,
            amtDueToBaColumn, currencyIdColumn, customerIdColumn];

        function isProductChecked(productOptions, productCode) {
            const length = productOptions.length;

            for (let i = 0; i < length; i++) {
                if (productOptions[i].Code === productCode) {
                    return productOptions[i].Status;
                }
            }

            return null;
        }

        function isIgnoredTurnoverProduct(productCode) {
            return $.inArray(productCode, IGNORED_TURNOVER_PRODUCTS) >= 0;
        }

        function isIgnoredLicenseeWLProduct(productCode) {
            return $.inArray(productCode, IGNORED_LICENSEEWL_PRODUCTS) >= 0;
        }

        function turnoverProductsFilter(item) {
            return item.Status === true && isIgnoredTurnoverProduct(item.Code) === false;
        }

        function licenseeWLProductsFilter(item) {
            return item.Status === true && isIgnoredLicenseeWLProduct(item.Code) === false;
        }

        _me.setGroupHeaderColumns = function (grid, productOptions) {
            const numOfTurnover = Enumerable.From(productOptions).Count(turnoverProductsFilter) + 1;
            const lastOfTurnover = numOfTurnover > 1 ? Enumerable.From(productOptions).Last(turnoverProductsFilter) : null;

            const numOfLicenseeWL = Enumerable.From(productOptions).Count(licenseeWLProductsFilter) + 1;
            const lastOfLicenseeWL = numOfLicenseeWL > 1 ? Enumerable.From(productOptions).Last(licenseeWLProductsFilter) : null;

            const groupCustomerWinloss = _me.hasCustomerComm ? {
                startColumnName: 'CustomerWinLoss',
                numberOfColumns: 3,
                titleText: Site.getSingleResource('CustomerW_L')
            } : {};

            const groupTurnoverWL = lastOfTurnover !== null ? {
                startColumnName: 'TurnOver' + lastOfTurnover.Code,
                numberOfColumns: numOfTurnover,
                titleText: Site.getSingleResource('Turnover')
            } : {};

            const groupLicenseeWL = lastOfLicenseeWL !== null ? {
                startColumnName: 'LicenseeWL' + lastOfLicenseeWL.Code,
                numberOfColumns: numOfLicenseeWL,
                titleText: Site.getSingleResource('LicenseeW_L')
            } : {};

            grid.jqGrid('setGroupHeaders',
                {
                    useColSpanStyle: true,
                    groupHeaders: [groupTurnoverWL, groupLicenseeWL, groupCustomerWinloss]
                });
        };

        _me.getColName = function (productOptions) {
            // Column Customer Wl
            const temp = Site.clone(colNameList);
            let turnOverIndex = _me.turnoverIndex;
            let licenseeIndex = _me.licenseeWlIndex;
            let customerWLTotalIndex = _me.customerWlTotalIndex;
            temp[_me.customerWlIndex] = _me.hasCustomerComm ? 'WinLoss' : 'CustomerW_L';
            $.each(productOptions, function (index, item) {
                //insert turn over column
                if (item.Status === true) {
                    if (isIgnoredTurnoverProduct(item.Code) === false) {
                        temp.insertAfter(item.Name, _me.currencyIndex);
                        turnOverIndex++;
                        licenseeIndex++;
                        customerWLTotalIndex++;
                    }
                    if (isIgnoredLicenseeWLProduct(item.Code) === false) {
                        temp.insertAfter(item.Name, customerWLTotalIndex);
                        licenseeIndex++;
                    }
                }
            });
            //add total column
            temp.insertBefore('Total', turnOverIndex);
            licenseeIndex++;
            temp.insertBefore('Total', licenseeIndex);

            return Site.getResources(temp);
        };

        _me.getColModel = function (level, reportBy, productOptions) {
            // Column hidden by customer comm
            customerWlTotalColumn.hidden = !_me.hasCustomerComm;
            playerCommColumn.hidden = !_me.hasCustomerComm;
            const levelNumber = Number(level);
            //set width of licenseeWL column
            if (!_me.hasCustomerComm) { licenseeWlColumn.width = 90; }
            // Column hidden by level
            currencyColumn.formatter = levelNumber === 0 ? currencyFormatter : null;
            systemIdColumn.formatter = levelNumber === 1 ? systemIdFormatter : null;

            dateColumn.hidden = levelNumber > 0;
            externalIdColumn.hidden = levelNumber < 2;
            systemIdColumn.hidden = levelNumber === 0;

            // Column hidden by product
            const allowedBa = isProductChecked(productOptions, 'BA');
            const allowedCashOut = isProductChecked(productOptions, 'CO');
            baCommissionColumn.hidden = !allowedBa;
            amtDueToBaColumn.hidden = !allowedBa;
            cashOutCommissionColumn.hidden = !allowedCashOut;

            if (levelNumber === 2) {
                externalIdColumn.formatter = betlistLink;
                systemIdColumn.formatter = betlistLink;
            }
            dateColumn.formatoptions = { newformat: Number(reportBy) === 2 ? 'm/Y' : 'm/d/Y' };

            const temp = Site.clone(colModelList);

            let turnOverIndex = _me.turnoverIndex;

            let licenseeIndex = _me.licenseeWlIndex;

            let customerWLTotalIndex = _me.customerWlTotalIndex;
            $.each(productOptions, function (index, item) {
                //insert turn over column
                if (item.Status === true) {
                    let productCol = Object.assign({}, productColumn);;
                    if (item.Code === 'BA' || item.Code === 'CO') {
                        productCol.width = 80;
                    }

                    // Exclude bettrade
                    if (isIgnoredTurnoverProduct(item.Code) === false) {
                        const turnOver = Site.clone(productCol);
                        turnOver.name = 'TurnOver' + item.Code;
                        turnOver.index = 'TurnOver' + item.Code;
                        turnOver.sortable = true;

                        temp.insertAfter(turnOver, _me.currencyIndex);
                        turnOverIndex++;
                        licenseeIndex++;
                        customerWLTotalIndex++;
                    }

                    if (isIgnoredLicenseeWLProduct(item.Code) === false) {
                        const licenseeWL = Site.clone(productCol);
                        licenseeWL.name = 'LicenseeWL' + item.Code;
                        licenseeWL.index = 'LicenseeWL' + item.Code;
                        licenseeWL.sortable = true;

                        temp.insertAfter(licenseeWL, customerWLTotalIndex);
                        licenseeIndex++;
                    }
                }
            });

            //add total

            const turnOverTotal = Site.clone(productColumn);
            turnOverTotal.name = 'TotalTurnOver';
            turnOverTotal.index = 'TotalTurnOver';
            temp.insertBefore(turnOverTotal, turnOverIndex);
            licenseeIndex++;
            const licenseeWLTotal = Site.clone(productColumn);
            licenseeWLTotal.name = 'TotalLicenseeWL';
            licenseeWLTotal.index = 'TotalLicenseeWL';
            temp.insertBefore(licenseeWLTotal, licenseeIndex);

            return temp;
        };

        _me.getGridOption = function (level, reportBy, productOptions) {
            _me.gridOption.colNames = _me.getColName(productOptions);
            _me.gridOption.colModel = _me.getColModel(level, reportBy, productOptions);
            return Site.clone(_me.gridOption);
        };

        _me.getTotalColModel = function (level, productOptions) {
            const colModel = _me.getColModel(level, -1, productOptions);
            // Not show drill down link on total link
            const curCol = Enumerable.From(colModel).First(function (item) {
                return item.name === 'CurrencyName';
            });

            curCol.formatter = null;
            return colModel;
        };

        _me.addProductButton = function (grid, gridId, gridName, productOptions) {
            /// <summary>
            /// add collapse/expand button to header product columns
            /// </summary>
            /// <param name='grid'>grid instance</param>

            const numOfActiveLicPro = Enumerable.From(productOptions).Count(licenseeWLProductsFilter);
            const numOfActiveTurnPro = Enumerable.From(productOptions).Count(turnoverProductsFilter);

            //reset licenseWL column
            const curWidth = $('#ReportGrid_TotalLicenseeWLSingle div').width();

            if (curWidth < 100) {
                $('#ReportGrid_TotalLicenseeWLSingle div').css('width', '100px');
            }

            //#region Toggle
            // column params: TurnOver, LicenseeWL
            const toggle = function (isShow, column) {
                if ((column === 'LicenseeWL' && numOfActiveLicPro <= 1) ||
                    (column === 'TurnOver' && numOfActiveTurnPro <= 1)) {
                    return;
                }
                _expanseTypes[column] = isShow;

                const param = isShow ? 'showCol' : 'hideCol';
                let fColumns = [];
                let productColumnNames = [];

                $.each(productOptions, function (index, value) {
                    if (value.Status === true) {
                        const columnName = column + value.Code;
                        productColumnNames.push(columnName);
                        fColumns.push(gridName + '_' + columnName);
                    }
                });
                const totalColumn = 'Total' + column;
                productColumnNames.push(totalColumn);

                grid.jqGrid(param, productColumnNames);

                const fTotalColumn = gridName + '_' + totalColumn;
                fColumns.push(fTotalColumn);

                /*show /hide single column*/
                const singleColumn = totalColumn + 'Single';
                const singleParam = isShow ? 'hideCol' : 'showCol';
                grid.jqGrid(singleParam, singleColumn);

                /*show/hide footer column*/
                jqGridAdditional.setFooterColumns(param, fColumns);

                const singleFColumn = gridName + '_' + singleColumn;
                jqGridAdditional.setFooterColumns(singleParam, [singleFColumn]);

                //update footer row
                $('#footer').html($('.ui-jqgrid-ftable').html());
                $('.report-content').css('cursor', 'default');

                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth(grid);
                return false;
            };

            _me.toggle = toggle;
            //#end

            toggle(true, 'LicenseeWL');
            toggle(true, 'TurnOver');
        };

        return _me;
    });