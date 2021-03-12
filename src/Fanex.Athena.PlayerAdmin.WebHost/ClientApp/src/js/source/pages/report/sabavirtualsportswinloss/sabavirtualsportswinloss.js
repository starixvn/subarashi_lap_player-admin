define(
    ['jquery', 'common/querystring', 'common/site', '../report', './sabavirtualsportswinloss.option',
        'lib/jqgrid/jquery.jqgrid.additional', 'components/pluginhelper/daterangepicker', 'moment', 'common/popup', 'components/pluginhelper/multiselect',
        'helpers/daterange', 'helpers/downloader', 'components/toast', 'helpers/datahelper', 'lib/jqgrid/jquery.jqGrid.en',
        'lib/jqgrid/jquery.jqGrid.min', 'extensions/jquery.extension', 'lib/linq', 'extensions/ajaxBinaryTransporting'],
    function ($, QueryString, Site, Report, SabaVirtualSportsWinLossOption, jqGridAdditional, DatePicker, moment, Popup,
        MultiSelect, DateHelper, Downloader, Toast, DataHelper) {
        'use strict';
        const REPORT_BY_MONTHLY = 2;
        const VALID_MONTH_RANGE = 3;
        const EXCEL_FILENAME = 'SabaVirtualSportsWinLoss.xls';

        const cacheData = {};
        let ajaxRequest = null;
        let _sortColumnName = null;
        let _sortOrder = 'desc';
        let _level = 0;
        let _reportBy = null;
        let _submitData; // cache submit data of level 0.
        let drillDownData = {};
        let ajaxLeagues = null;
        let _dateRangeValue = '';

        const _me = {
            initialize: function () {
                Site.initialize();
                _me.initEvent();
                _me.initSetting();
                _me.registerGlobalFunction();

                SabaVirtualSportsWinLossOption.gridOption.onSortCol = function (index) {
                    Site.blockUI();
                    window.setTimeout(function () {
                        if (_sortColumnName === index) {
                            _sortOrder = _sortOrder === 'asc' ? 'desc' : 'asc';
                        } else {
                            _sortOrder = 'asc';
                            _sortColumnName = index;
                        }

                        _me.bindData(_level, _reportBy, cacheData[_level]);

                        Site.unBlockUI();
                    }, 100);
                    return 'stop';
                };

                Site.addUserGuide({
                    resourceName: 'SabaVirtualSportsWL',
                    title: Site.getSingleResource('SabaVirtualSportsWinLoss')
                });
                DatePicker.initDateRangePicker();
                DateHelper.setConstraintMonthRange(VALID_MONTH_RANGE, SabaVirtualSportsWinLossOption.urlExcelData);
            },

            registerGlobalFunction: function () {
                window.sabavirtualsportswinlossDrilldown = _me.drillDown;
            },

            sortData: function (arrayData) {
                const totalData = arrayData.length;

                if (!_sortColumnName || totalData <= 1) {
                    return arrayData;
                }

                const sortThenBy = arrayData[0].Id ? 'Id' : 'Index';

                const data = DataHelper.sort(
                    arrayData,
                    function (x) { return x[_sortColumnName]; },
                    _sortOrder,
                    function (x) { return x[sortThenBy]; },
                    'asc');

                if (sortThenBy === 'Id') {
                    for (let i = 0; i < totalData; i++) {
                        data[i].Index = i + 1;
                    }
                }
                else {
                    for (let j = 0; j < totalData; j++) {
                        arrayData[j].Id = j + 1;
                        data[j].Index = j + 1;
                    }
                }

                return data;
            },

            initSetting: function () {
                SabaVirtualSportsWinLossOption.hasCustomerComm = Site.asBool($('#IsAllowedCustomerCommission').val());
                SabaVirtualSportsWinLossOption.licenseeId = parseInt($('#drillDownLicenseeId').val(), 10);
            },

            initEvent: function () {
                $('body').on('click', '.bet-list-link', function () {
                    const dataDrillDown = $(this).attr('data-drill-down');
                    _me.openBetlistPopUp(dataDrillDown);
                });

                $('#SubmitButton').click(function () {
                    $('#IsExportExcel').val(false);
                    _level = 0;
                    _sortColumnName = null;
                    _sortOrder = 'desc';
                    _me.submit();
                });

                if ($("#IsExcludedTestData").is(':checked')) {
                    $("#IsExcludedTestData").attr('value', 'true');
                }

                $('#ExportToExcel').click(function () {
                    $('#IsExportExcel').val(true);

                    const data = $('#sabavirtualsportswinloss').serializeObject();
                    data.League = $('#League').val();
                    if (_sortColumnName) {
                        data.sortby = _sortColumnName;
                        data.sortOrder = _sortOrder;
                    }

                    _me.exportExcel(SabaVirtualSportsWinLossOption.urlExcelData, data, EXCEL_FILENAME);
                });

                $('#level-link-container').on('click', '.level-link', function () {
                    const level = Number($(this).attr('data-level'));
                    _level = level;
                    if (level < 2) {
                        _me.goBack(level);
                    }
                });

                $('#Sport, #CurrencyId').change(function () {
                    _me.loadLeagues();
                });

                $(' #DateRange').change(function () {
                    var value = $(this).val();
                    if (value !== _dateRangeValue) {
                        _dateRangeValue = value;
                        _me.loadLeagues();
                    }
                });

                $(window).on('resize', function () {
                    _me.resizeGrid();
                });

                $(window).on('unload', function () {
                    $(window).off('resize');
                });
            },

            exportExcel: function (url, data, fileName) {
                const option = {
                    type: 'POST',
                    data: data,
                    dataType: 'binary',
                    responseType: 'blob',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    cache: false,
                    url: Site.resolveClientUrl(url),
                    success: function (response) {
                        Downloader.download(response, fileName);
                    },
                    beforeSend: function () {
                        Toast.show();
                    }
                };

                $.ajax(option);

                return false;
            },

            //#region Layout
            setExcelUrl: function (level, currencyId, custId, fdate, tdate, currencyName) {
                let temp = 'api/SabaVirtualSportsWinLossReport/Index';
                if (level > 0) {
                    temp += '?level=' + level + '&currencyId='
                        + currencyId + '&custId=' + custId + '&fdate=' + fdate
                        + '&tdate=' + tdate + '&currencyName=' + currencyName;
                }
                SabaVirtualSportsWinLossOption.urlExcelData = temp;
            },

            setReportTitle: function (currencyId, fromdate, todate, currencyName) {
                const titlePattern = Site.getSingleResource('SabaVirtualSportsWinLoss') + ', {0} → {1} - {2}';
                const currencyNameOnTitle = currencyName ? Site.getSingleResource('Viewin').replace('{0}', currencyName.toUpperCase())
                    : Report.getCurrencyNameForReportTitle(currencyId);

                const title = $.format(titlePattern,
                    moment(fromdate, 'MM/DD/YYYY').format('MM/DD/YYYY'),
                    moment(todate, 'MM/DD/YYYY').format('MM/DD/YYYY'),
                    currencyNameOnTitle);
                $('.report-title').html(title);
                $('.panel-heading').removeClass('hide');
            },
            //#end

            //#Get Data
            ajaxComplete: function (level, response) {
                const levelNumber = Number(level);
                if (response !== null) {
                    const reportBy = $('input[name="ReportBy"]:checked').val();
                    _reportBy = reportBy;
                    cacheData[levelNumber] = response;

                    if (levelNumber === 0) {
                        _me.setReportTitle($('#CurrencyId').val(), $('#FromDate').val(), $('#ToDate').val(), $('#BaseCurrency').val());
                    }

                    Report.renderBreadCrumb(levelNumber, SabaVirtualSportsWinLossOption.licenseeId);
                    _me.bindData(levelNumber, reportBy, response);
                }
            },

            loadLeagues: function () {
                if (ajaxLeagues !== null) {
                    ajaxLeagues.abort();
                }

                const $submitButton = $('#SubmitButton');
                $submitButton.prop('disabled', true);

                Site.blockUI();
                const $league = $('#League');
                $league.selectList('disabled', true);
                $league.html('');

                const mainForm = $('#sabavirtualsportswinloss');
                const data = mainForm.serializeObject();

                ajaxLeagues = $.ajax({
                    type: 'POST',
                    url: Site.resolveClientUrl('SabaVirtualSportsWinLossReport/GetLeaguesBySport'),
                    data: {
                        currencyId: $('#CurrencyId').val(),
                        sportid: $('#Sport').val(),
                        fromDate: data.FromDate,
                        toDate: data.ToDate,
                    },
                    success: function (data) {
                        let str = '';
                        $.each(data, function () {
                            str += _me.renderHtmlOption(this.Key, this.Value);
                        });

                        if (data.length > 0) {
                            $league.append(str);
                            $submitButton.prop('disabled', false);
                            $league.selectList('disabled', false);
                            $league.selectList('update');
                        }

                        Site.unBlockUI();
                    }
                });
            },

            renderHtmlOption: function (key, value) {
                return '<option value="' + key + '">' + value + '</option>';
            },

            submit: function () {
                Report.setOriginalProduct();
                Site.updateOriginalLicenseeOption();

                const mainForm = $('#sabavirtualsportswinloss');
                const data = mainForm.serializeObject();
                data.League = $('#League').val();
                const option = {
                    type: 'POST',
                    data: data,
                    dataType: 'json',
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    cache: false,
                    url: Site.resolveClientUrl(SabaVirtualSportsWinLossOption.urlData),
                    success: function (response) {
                        _submitData = {
                            ProductOptions: response.ProductOptions,
                            DrawFilter: data.DrawFilter,
                            CurrencyId: data.CurrencyId,
                            ReportBy: Number(data.ReportBy),
                            FromDate: data.FromDate,
                            ToDate: data.ToDate,
                            CurrencyName: data.CurrencyName,
                            BaseCurrency: data.BaseCurrency,
                            IsExcludedTestData: data.IsExcludedTestData,
                            League: data.League,
                            Sport: data.Sport
                        };

                        _me.setExcelUrl(0, 0, 0, '', '', '');
                        _me.ajaxComplete(0, response);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                if (ajaxRequest !== null) {
                    ajaxRequest.abort();
                }
                ajaxRequest = $.ajax(option);

                return true;
            },

            lastDateOfMonth: function (year, month) {
                return new Date(year, month + 1, 0);
            },

            drillDown: function (level, currencyId, customerId, datetime, currencyName, sport, league) {
                const levelNumber = Number(level);

                if (_level !== levelNumber) {
                    _level = levelNumber;
                }
                const param =
                {
                    ProductOptions: $.parseJSON(_submitData.ProductOptions),
                    DrawFilter: _submitData.DrawFilter,
                    IsExcludedTestData: _submitData.IsExcludedTestData,
                    CurrencyId: _submitData.CurrencyId,
                    ReportBy: _submitData.ReportBy,
                    FromDate: _submitData.FromDate,
                    ToDate: _submitData.ToDate,
                    BaseCurrency: $('#BaseCurrency').attr('data-original'),
                    Sport: _submitData.Sport,
                    League: _submitData.League
                };
                const drilldownDate = new Date(datetime);
                const dateRange = Report.GetDrilldownDateRange(param.ReportBy, param.FromDate, param.ToDate, drilldownDate);
                const drillDownFDate = moment(dateRange.FromDate).format('MM/DD/YYYY');
                const drillDownTDate = moment(dateRange.ToDate).format('MM/DD/YYYY');
                const postData = JSON.stringify(param);
                const option = {
                    type: 'POST',
                    data: postData,
                    dataType: 'json',
                    contentType: 'application/json',
                    cache: false,

                    url: Site.resolveClientUrl(SabaVirtualSportsWinLossOption.urlData +
                        '?level=' + levelNumber + '&currencyId=' + currencyId + '&custId=' + customerId + '&fdate=' +
                        drillDownFDate + '&tdate=' + drillDownTDate),
                    success: function (response) {
                        _me.ajaxComplete(levelNumber, response);
                        if (response !== null && response.Data.length > 0) {
                            if (levelNumber > 0 && Number(param.CurrencyId) === 0) {
                                const viewInCurrency = param.BaseCurrency ? param.BaseCurrency.toUpperCase() : currencyName;
                                _me.setReportTitle(currencyId, dateRange.FromDate, dateRange.ToDate, viewInCurrency);
                            } else {
                                _me.setReportTitle(param.CurrencyId, dateRange.FromDate, dateRange.ToDate);
                            }
                        }

                        //update excel ULR
                        _me.setExcelUrl(levelNumber, currencyId, customerId, drillDownFDate, drillDownTDate, currencyName, _submitData.Sport, _submitData.League);
                        _me.updateDrilldownData(currencyId, currencyName, drillDownFDate, drillDownTDate, customerId);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                if (ajaxRequest !== null) {
                    ajaxRequest.abort();
                }
                ajaxRequest = $.ajax(option);
            },

            updateDrilldownData: function (currencyId, currencyName, fromDate, toDate, agentId) {
                drillDownData = {
                    CurrencyId: currencyId,
                    CurrencyName: currencyName,
                    FromDate: fromDate,
                    ToDate: toDate,
                    AgentId: agentId,
                    IsExcludedTestData: _submitData.IsExcludedTestData
                };
            },

            goBack: function (level) {
                const reportBy = _submitData.ReportBy;
                Report.renderBreadCrumb(level, SabaVirtualSportsWinLossOption.licenseeId);

                const data = cacheData[level];
                _me.bindData(level, reportBy, data);

                if (level === 0) {
                    _me.setExcelUrl(0, 0, 0, '', '', '');
                    _me.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                }
                else if (level === 1) {
                    _me.updateDrilldownData(drillDownData.CurrencyId, drillDownData.CurrencyName, drillDownData.FromDate, drillDownData.ToDate, 0);
                    SabaVirtualSportsWinLossOption.urlExcelData = SabaVirtualSportsWinLossOption.urlExcelData.replace('level=2', 'level=1');
                }
            },

            //#BindReport

            bindTotal: function (level, totalData, summaryTotal) {
                const productOptions = $.parseJSON(_submitData.ProductOptions);
                const newProductOptions = productOptions.reverse(); //make orders of product columns like excel file
                const columnModel = SabaVirtualSportsWinLossOption.getTotalColModel(level, newProductOptions);
                const colspan = SabaVirtualSportsWinLossOption.totalColspan[level];
                const summaryColspan = colspan + 1;

                jqGridAdditional.assignTotal(totalData, columnModel);
                const summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
                jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
            },

            bindData: function (level, reportBy, response) {
                jqGridAdditional.clearGrid('ReportGrid');
                const numOfRow = response.Data.length;
                if (numOfRow === 0) {
                    jqGridAdditional.setNoInfo();
                    return;
                }

                const levelNumber = Number(level);
                const reportGrid = $('#ReportGrid');
                const productOptions = $.parseJSON(_submitData.ProductOptions);

                const newProductOptions = productOptions.reverse(); //make orders of product columns like excel file

                newProductOptions.forEach(function (product, i) { if (product.Code === 'SB') newProductOptions[i].Name = Site.getSingleResource('ProductSBVS'); });

                const numOfTotalRow = response.Total.length;
                let onPaging = false;
                const data = _me.sortData(response.Data);

                const gridOption = SabaVirtualSportsWinLossOption.getGridOption(levelNumber, reportBy, newProductOptions);
                gridOption.rownumbers = true;
                gridOption.data = data;
                gridOption.userData = {};
                gridOption.rowNum = levelNumber === 2 ? SabaVirtualSportsWinLossOption.pageSize : SabaVirtualSportsWinLossOption.nonePagingSize;
                gridOption.rowList = SabaVirtualSportsWinLossOption.rowList;
                gridOption.rowTotal = numOfRow;
                gridOption.pager = '#ppdata';
                gridOption.onPaging = function (pgButton) {
                    if (levelNumber === 2 && numOfRow > SabaVirtualSportsWinLossOption.pageSize) {
                        if ($('#' + pgButton).hasClass('ui-state-disabled') === false) {
                            Site.blockUI();
                        }
                        onPaging = true;
                    }
                    else {
                        return 'stop';
                    }
                };
                gridOption.gridComplete = function () {
                    jqGridAdditional.gridComplete();
                    if (levelNumber === 2 && numOfRow > SabaVirtualSportsWinLossOption.pageSize) {
                        $('#pg_ppdata').show();
                    }
                    else {
                        $('#pg_ppdata').hide();
                    }

                    if (onPaging && levelNumber === 2) {
                        $('.ui-jqgrid-ftable').html($('#footer').html());
                    }
                    else {
                        _me.bindTotal(levelNumber, response.Total, response.SummaryTotal);
                        $('#footer').html($('.ui-jqgrid-ftable').html());
                    }

                    onPaging = false;
                    _me.resizeGrid();

                    /*group header*/
                    if (numOfTotalRow > 0) {
                        SabaVirtualSportsWinLossOption.addProductButton(reportGrid, 'jqgh_ReportGrid', 'ReportGrid', productOptions);
                    }

                    Site.unBlockUI();
                };

                // Show the grid
                reportGrid.jqGrid(gridOption);

                if (_sortColumnName) {
                    jqGridAdditional.addSortableHeader(reportGrid, _sortColumnName, _sortOrder);
                }

                SabaVirtualSportsWinLossOption.setGroupHeaderColumns(reportGrid, newProductOptions);

                SabaVirtualSportsWinLossOption.restoreExpanse();
            },

            //#end

            openBetlistPopUp: function (data) {
                const dataDrillDown = $.parseJSON(data);
                dataDrillDown.DrawFilter = _submitData.DrawFilter;
                dataDrillDown.IsExcludedTestData = _submitData.IsExcludedTestData;
                dataDrillDown.CurrencyId = _submitData.CurrencyId;
                dataDrillDown.CurrencyName = _submitData.CurrencyName;
                dataDrillDown.BaseCurrency = $('#BaseCurrency').attr('data-original');

                if (typeof dataDrillDown.DrillDownFromDate === 'string') {
                    dataDrillDown.DrillDownFromDate = new Date(dataDrillDown.DrillDownFromDate);
                    dataDrillDown.DrillDownToDate = new Date(dataDrillDown.DrillDownToDate);
                }

                if (_submitData.ReportBy === REPORT_BY_MONTHLY) {
                    let fday = 1;
                    const month = dataDrillDown.DrillDownFromDate.getMonth();
                    const year = dataDrillDown.DrillDownFromDate.getFullYear();

                    const orignalFDate = new Date(_submitData.FromDate);
                    const originalTDate = new Date(_submitData.ToDate);

                    if (month === orignalFDate.getMonth()) {
                        fday = orignalFDate.getDate();
                    }
                    dataDrillDown.DrillDownFromDate = new Date(year, month, fday, 0, 0, 0, 0);
                    if (month === originalTDate.getMonth()) {
                        const tday = originalTDate.getDate();
                        dataDrillDown.DrillDownToDate = new Date(year, month, tday, 0, 0, 0, 0);
                    }
                    else {
                        dataDrillDown.DrillDownToDate = _me.lastDateOfMonth(year, month);
                    }
                }

                let url = Site.resolveClientUrl('SabaVirtualSportsWinLossBetList/Index');
                url = QueryString.setParam(url, 'fromdate', moment(dataDrillDown.DrillDownFromDate).format('MM/DD/YYYY'));
                url = QueryString.setParam(url, 'todate', moment(dataDrillDown.DrillDownToDate).format('MM/DD/YYYY'));
                url = QueryString.setParam(url, 'excludetest', dataDrillDown.IsExcludedTestData);
                url = QueryString.setParam(url, 'CurrencyId', dataDrillDown.DrillDownCurrencyId);
                url = QueryString.setParam(url, 'CurrencyName', dataDrillDown.DrillDownCurrencyName);
                url = QueryString.setParam(url, 'drawfilter', dataDrillDown.DrawFilter);
                url = QueryString.setParam(url, 'custid', dataDrillDown.CustomerId);
                url = QueryString.setParam(url, 'systemId', dataDrillDown.SystemId);
                url = QueryString.setParam(url, 'externalId', dataDrillDown.ExternalId);
                url = QueryString.setParam(url, 'BaseCurrency', dataDrillDown.BaseCurrency);
                url = QueryString.setParam(url, 'SportType', _submitData.Sport);
                url = QueryString.setParam(url, 'LeagueId', _submitData.League);

                const popH = Math.min($(window).height() - 100, 650), popW = 920;
                const popupOption = { id: 'betlist-popup', width: popW + 50, height: popH + 50, title: Site.getSingleResource('CustomerBetList') };
                Popup.openPopupWithIframe(url, popupOption);
                window.logOpenBetlistEvent && window.logOpenBetlistEvent();
            },

            resizeGrid: function () {
                const reportGrid = $('#ReportGrid');
                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth(reportGrid);
            }
        };

        return _me;
    });
