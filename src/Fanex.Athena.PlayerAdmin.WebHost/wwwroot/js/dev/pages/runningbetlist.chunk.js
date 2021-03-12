/******/ (function(modules) { // webpackBootstrap
/******/ 	// install a JSONP callback for chunk loading
/******/ 	function webpackJsonpCallback(data) {
/******/ 		var chunkIds = data[0];
/******/ 		var moreModules = data[1];
/******/ 		var executeModules = data[2];
/******/
/******/ 		// add "moreModules" to the modules object,
/******/ 		// then flag all "chunkIds" as loaded and fire callback
/******/ 		var moduleId, chunkId, i = 0, resolves = [];
/******/ 		for(;i < chunkIds.length; i++) {
/******/ 			chunkId = chunkIds[i];
/******/ 			if(Object.prototype.hasOwnProperty.call(installedChunks, chunkId) && installedChunks[chunkId]) {
/******/ 				resolves.push(installedChunks[chunkId][0]);
/******/ 			}
/******/ 			installedChunks[chunkId] = 0;
/******/ 		}
/******/ 		for(moduleId in moreModules) {
/******/ 			if(Object.prototype.hasOwnProperty.call(moreModules, moduleId)) {
/******/ 				modules[moduleId] = moreModules[moduleId];
/******/ 			}
/******/ 		}
/******/ 		if(parentJsonpFunction) parentJsonpFunction(data);
/******/
/******/ 		while(resolves.length) {
/******/ 			resolves.shift()();
/******/ 		}
/******/
/******/ 		// add entry modules from loaded chunk to deferred list
/******/ 		deferredModules.push.apply(deferredModules, executeModules || []);
/******/
/******/ 		// run deferred modules when all chunks ready
/******/ 		return checkDeferredModules();
/******/ 	};
/******/ 	function checkDeferredModules() {
/******/ 		var result;
/******/ 		for(var i = 0; i < deferredModules.length; i++) {
/******/ 			var deferredModule = deferredModules[i];
/******/ 			var fulfilled = true;
/******/ 			for(var j = 1; j < deferredModule.length; j++) {
/******/ 				var depId = deferredModule[j];
/******/ 				if(installedChunks[depId] !== 0) fulfilled = false;
/******/ 			}
/******/ 			if(fulfilled) {
/******/ 				deferredModules.splice(i--, 1);
/******/ 				result = __webpack_require__(__webpack_require__.s = deferredModule[0]);
/******/ 			}
/******/ 		}
/******/
/******/ 		return result;
/******/ 	}
/******/
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// object to store loaded and loading chunks
/******/ 	// undefined = chunk not loaded, null = chunk preloaded/prefetched
/******/ 	// Promise = chunk loading, 0 = chunk loaded
/******/ 	var installedChunks = {
/******/ 		"www/js/build/dev/reactpages/playeradmin/runningbetlist": 0
/******/ 	};
/******/
/******/ 	var deferredModules = [];
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	var jsonpArray = window["webpackJsonp"] = window["webpackJsonp"] || [];
/******/ 	var oldJsonpFunction = jsonpArray.push.bind(jsonpArray);
/******/ 	jsonpArray.push = webpackJsonpCallback;
/******/ 	jsonpArray = jsonpArray.slice();
/******/ 	for(var i = 0; i < jsonpArray.length; i++) webpackJsonpCallback(jsonpArray[i]);
/******/ 	var parentJsonpFunction = oldJsonpFunction;
/******/
/******/
/******/ 	// add entry module to deferred list
/******/ 	deferredModules.push(["./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/index.jsx","lib/common"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/actions/index.js":
/*!**************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/actions/index.js ***!
  \**************************************************************************************/
/*! exports provided: ActionCreators */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ActionCreators", function() { return ActionCreators; });
/* harmony import */ var _jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");
/* harmony import */ var _jsroot_actions_requestParamAction__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/actions/requestParamAction */ "./www/js/source/reactpages/actions/requestParamAction.js");


const ActionCreators = Object.assign({}, _jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_0__, _jsroot_actions_requestParamAction__WEBPACK_IMPORTED_MODULE_1__);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/AppContainer.jsx":
/*!*************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/AppContainer.jsx ***!
  \*************************************************************************************************/
/*! exports provided: AppContainer, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppContainer", function() { return AppContainer; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions_index__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../actions/index */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/actions/index.js");
/* harmony import */ var _jsroot_layouts_betlist_BetListLayout__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/layouts/betlist/BetListLayout */ "./www/js/source/reactpages/layouts/betlist/BetListLayout.jsx");
/* harmony import */ var _FormFilter__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./FormFilter */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/FormFilter.jsx");
/* harmony import */ var _DataGrid__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./DataGrid */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/DataGrid.jsx");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");
/* harmony import */ var _lib_service__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/lib/service.js");
/* harmony import */ var _jsroot_components_Loader__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @jsroot/components/Loader */ "./www/js/source/reactpages/components/Loader.jsx");
/* harmony import */ var _jsroot_layouts_AppLayout__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @jsroot/layouts/AppLayout */ "./www/js/source/reactpages/layouts/AppLayout.jsx");
/* harmony import */ var _jsroot_components_ReportTitle__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @jsroot/components/ReportTitle */ "./www/js/source/reactpages/components/ReportTitle.jsx");
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }














class AppContainer extends react__WEBPACK_IMPORTED_MODULE_0__["Component"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "getResource", key => {
      return _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource(key);
    });

    _defineProperty(this, "formatDateString", dateString => {
      const date = new Date(dateString);
      const month = date.getMonth() + 1;
      const day = date.getDate();
      return `${month < 10 ? '0' + month : month}/${day < 10 ? '0' + day : day}/${date.getFullYear()}`;
    });

    _defineProperty(this, "beforeShow", () => {
      this.props.toggleLoader(true);
    });

    _defineProperty(this, "afterShow", () => {
      this.props.toggleLoader(false);
    });

    this.title = _lib_service__WEBPACK_IMPORTED_MODULE_9__["default"].getReportTitle(props.requestParam.Currency);
  }

  render() {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_layouts_AppLayout__WEBPACK_IMPORTED_MODULE_11__["default"], null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_layouts_betlist_BetListLayout__WEBPACK_IMPORTED_MODULE_4__["default"], {
      pageName: "PlayerAdmin",
      rootUrl: _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_8__["default"].getRootUrl(),
      getResourceFunc: this.getResource,
      formatDateStringFunc: this.formatDateString,
      baseCurrency: "",
      beforeShowFunc: this.beforeShow,
      afterShowFunc: this.afterShow
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_Loader__WEBPACK_IMPORTED_MODULE_10__["default"], {
      isLoading: this.props.isLoading
    }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_FormFilter__WEBPACK_IMPORTED_MODULE_5__["default"], null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "betlist-container result-content"
    }, this.title && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_ReportTitle__WEBPACK_IMPORTED_MODULE_12__["default"], {
      content: this.title
    }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_DataGrid__WEBPACK_IMPORTED_MODULE_6__["default"], null))));
  }

}

function mapDispatchToProps(dispatch) {
  const {
    toggleLoader
  } = _actions_index__WEBPACK_IMPORTED_MODULE_3__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    toggleLoader
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    isLoading,
    requestParam
  } = state;
  return Object.assign({}, {
    isLoading,
    requestParam
  });
}

/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(AppContainer));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/DataGrid.jsx":
/*!*********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/DataGrid.jsx ***!
  \*********************************************************************************************/
/*! exports provided: DataGrid, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DataGrid", function() { return DataGrid; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/actions/index.js");
/* harmony import */ var _jsroot_components_betlist_BetListDataGrid__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/components/betlist/BetListDataGrid */ "./www/js/source/reactpages/components/betlist/BetListDataGrid.jsx");
/* harmony import */ var _jsroot_lib_AppData__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/lib/AppData */ "./www/js/source/reactpages/lib/AppData.js");
/* harmony import */ var _lib_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/lib/service.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }









class DataGrid extends react__WEBPACK_IMPORTED_MODULE_0__["Component"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "getHeaderName", column => {
      return this.headerText[column];
    });

    _defineProperty(this, "getResource", key => {
      return _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource(key);
    });

    _defineProperty(this, "afterLoad", () => {
      this.props.toggleLoader(false);
    });

    _defineProperty(this, "onSort", (sortBy, sortOrder) => {
      const param = Object.assign({}, this.props.requestParam, {
        SortBy: sortBy,
        SortOrder: sortOrder
      });

      this._getData(param);

      this.props.setRequestParam(param);
    });

    _defineProperty(this, "onPaging", (index, size) => {
      const param = Object.assign({}, this.props.requestParam, {
        PageIndex: index,
        PageSize: size
      });

      this._getData(param);

      this.props.setRequestParam(param);
    });

    this.headerText = _lib_service__WEBPACK_IMPORTED_MODULE_6__["default"].getHeaderText();
    this.totalText = _lib_service__WEBPACK_IMPORTED_MODULE_6__["default"].getTotalText();
    this.state = {
      data: null,
      total: null,
      totalRows: 0
    };
  }

  componentDidUpdate(prevProps) {
    if (prevProps.requestParam.TimeStamp !== this.props.requestParam.TimeStamp) {
      this._getData(this.props.requestParam);
    }
  }

  componentDidMount() {
    this._getData(this.props.requestParam);
  }

  _updateState(data, total, totalRows) {
    this.setState({
      data: data,
      total: total,
      totalRows: totalRows
    });
    this.props.toggleLoader(false);
  }

  _getData(param) {
    this.props.toggleLoader(true);
    _lib_service__WEBPACK_IMPORTED_MODULE_6__["default"].getData(param, this._updateState.bind(this));
  }

  render() {
    const {
      data,
      total,
      totalRows
    } = this.state;
    const requestParam = this.props.requestParam;
    const pageOption = {
      PageIndex: requestParam.PageIndex,
      PageSize: requestParam.PageSize,
      TotalItem: totalRows,
      PageList: [50, 100, 200, 300, 400, 500, 1000, 2000]
    };
    let Columns = ['Index', 'RefNo', 'TransTime', 'Choice', 'Odds', 'OddsType', 'Stake', 'Status', 'Platform', 'Live'];
    let ColumnWidths = [40, 110, 150, 400, 70, 70, 100, 80, 80, 80];
    let ColumnAlignments = ['center-align', 'center-align', 'center-align', 'right-align', 'right-align', 'right-align', 'right-align', 'center-align', 'center-align', 'center-align'];

    if (_jsroot_lib_AppData__WEBPACK_IMPORTED_MODULE_5__["default"].isAllowTxIDColumn && !Columns.includes('LicenseeTxId')) {
      Columns.splice(2, 0, 'LicenseeTxId');
      ColumnWidths.splice(2, 0, 110);
      ColumnAlignments.splice(2, 0, 'center-align');
    }

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_betlist_BetListDataGrid__WEBPACK_IMPORTED_MODULE_4__["default"], {
      isPaging: true,
      pageOption: pageOption,
      onPaging: this.onPaging,
      getHeaderNameFunc: this.getHeaderName,
      columns: Columns,
      columnWidths: ColumnWidths,
      sortableColumns: ['Stake'],
      totalColumns: ['Stake'],
      columnAlignments: ColumnAlignments,
      data: data,
      total: total,
      totalText: this.totalText,
      afterLoad: this.afterLoad,
      onSort: this.onSort,
      adjustedHeight: -20,
      getResourceFunc: this.getResource
    });
  }

}

function mapDispatchToProps(dispatch) {
  const {
    toggleLoader,
    setRequestParam
  } = _actions__WEBPACK_IMPORTED_MODULE_3__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    toggleLoader,
    setRequestParam
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    requestParam
  } = state;
  return Object.assign({}, {
    requestParam
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(DataGrid));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/FormFilter.jsx":
/*!***********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/FormFilter.jsx ***!
  \***********************************************************************************************/
/*! exports provided: FormFilter, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FormFilter", function() { return FormFilter; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _actions_index_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/actions/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _lib_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../lib/service */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/lib/service.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }







class FormFilter extends react__WEBPACK_IMPORTED_MODULE_0__["Component"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "submit", () => {
      let param = _objectSpread({}, this.props.requestParam);

      param.TimeStamp = new Date().getTime();
      this.props.setRequestParam(param);
    });

    _defineProperty(this, "exportExcel", event => {
      event.preventDefault();
      const requestParam = this.props.requestParam;
      this.props.toggleLoader(true);
      _lib_service__WEBPACK_IMPORTED_MODULE_5__["default"].export(requestParam, () => {
        this.props.toggleLoader(false);
      });
    });
  }

  render() {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "pa-running-betlist-filter"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", {
      type: "button",
      className: "btn btn-submit",
      onClick: this.submit
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Refresh')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "pa-running-betlist-export"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("i", {
      className: "icon-excel",
      onClick: this.exportExcel,
      title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('ExportToExcel')
    })));
  }

}

function mapDispatchToProps(dispatch) {
  const {
    setRequestParam,
    toggleLoader
  } = _actions_index_js__WEBPACK_IMPORTED_MODULE_2__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_3__["bindActionCreators"])({
    setRequestParam,
    toggleLoader
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    requestParam
  } = state;
  return Object.assign({}, {
    requestParam
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(FormFilter));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/index.jsx":
/*!******************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/index.jsx ***!
  \******************************************************************************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _AppContainer_jsx__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./AppContainer.jsx */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/AppContainer.jsx");
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _reducers_index_js__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../reducers/index.js */ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/reducers/index.js");
/* harmony import */ var _jsroot_lib_store__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/lib/store */ "./www/js/source/reactpages/lib/store.js");






const defaultState = Object.assign({}, window.initialState, {
  isLoading: false
});
const store = _jsroot_lib_store__WEBPACK_IMPORTED_MODULE_5__["default"].createStore(_reducers_index_js__WEBPACK_IMPORTED_MODULE_4__["default"], defaultState);
const App = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(() => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_redux__WEBPACK_IMPORTED_MODULE_3__["Provider"], {
    store: store
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_AppContainer_jsx__WEBPACK_IMPORTED_MODULE_2__["default"], null));
});
react_dom__WEBPACK_IMPORTED_MODULE_1___default.a.render( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(App, null), document.getElementById("app-container"));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/lib/service.js":
/*!************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/lib/service.js ***!
  \************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return Service; });
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");
/* harmony import */ var _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/lib/DataService */ "./www/js/source/reactpages/lib/DataService.js");
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(axios__WEBPACK_IMPORTED_MODULE_3__);




const HeaderText = {
  Index: "#",
  Platform: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("Platform"),
  ExternalId: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("ExternalId"),
  SystemId: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("SystemId"),
  RefNo: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("RefNo"),
  LicenseeTxId: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("LicenseeTxId"),
  TransTime: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("TransTime"),
  Choice: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("Choice"),
  Odds: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("Odds"),
  OddsType: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("OddsType"),
  Stake: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("Stake"),
  Status: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("Status"),
  Live: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("Live")
};
const DataApi = "PlayerBetList/GetRunningBetList";
class Service {
  static getHeaderText() {
    return HeaderText;
  }

  static getReportTitle(currency) {
    return currency ? `${_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('Currency')}: ${currency.toUpperCase()}` : '';
  }

  static getTotalText() {
    return _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('Total');
  }

  static export(param, callback) {
    const url = _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_1__["default"].resolveClientUrl(DataApi);
    const exportParam = Object.assign({}, param, {
      IsExport: true
    });
    _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_2__["default"].getFile(url, exportParam).then(response => {
      const NoContentStatusCode = 204;

      if (response.status === NoContentStatusCode) {
        alert(_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("NoInfo"));
      } else {
        _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_1__["default"].downloadFile(response.data, 'PlayerAdministratorBetList.xls');
      }

      callback();
    }).catch(error => {
      console.log(error.message);

      if (error.response) {
        alert(_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource("ErrorProcessingMsg"));
        callback();
      }
    });
  }

  static getData(param, callBack) {
    const url = _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_1__["default"].resolveClientUrl(DataApi);
    _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_2__["default"].postData(url, param).then(response => {
      const result = response.data;
      const data = result.Data;

      if (data && data.length > 0) {
        callBack && callBack(data, result.Total, result.TotalRows);
      } else {
        callBack && callBack([], {}, 0);
      }
    }).catch(error => {
      console.log(error.message);
      callBack && callBack([], {}, 0);
    });
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/reducers/index.js":
/*!***************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/reducers/index.js ***!
  \***************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _jsroot_reducers_loaderReducer__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/reducers/loaderReducer */ "./www/js/source/reactpages/reducers/loaderReducer.js");
/* harmony import */ var _jsroot_reducers_requestParamReducer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/reducers/requestParamReducer */ "./www/js/source/reactpages/reducers/requestParamReducer.js");



/* harmony default export */ __webpack_exports__["default"] = (Object(redux__WEBPACK_IMPORTED_MODULE_0__["combineReducers"])(Object.assign({}, _jsroot_reducers_loaderReducer__WEBPACK_IMPORTED_MODULE_1__, _jsroot_reducers_requestParamReducer__WEBPACK_IMPORTED_MODULE_2__)));

/***/ }),

/***/ "./www/js/source/reactpages/actions/loaderAction.js":
/*!**********************************************************!*\
  !*** ./www/js/source/reactpages/actions/loaderAction.js ***!
  \**********************************************************/
/*! exports provided: toggleLoader */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "toggleLoader", function() { return toggleLoader; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

const toggleLoader = show => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["TOGGLE_LOADER"],
    data: show
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/actions/requestParamAction.js":
/*!****************************************************************!*\
  !*** ./www/js/source/reactpages/actions/requestParamAction.js ***!
  \****************************************************************/
/*! exports provided: setRequestParam */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setRequestParam", function() { return setRequestParam; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/actions/types.js");

const setRequestParam = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_REQUEST_PARAM"],
    data: data
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/actions/types.js":
/*!***************************************************!*\
  !*** ./www/js/source/reactpages/actions/types.js ***!
  \***************************************************/
/*! exports provided: FETCH_DATA, SET_GRID_DATA, SET_SELECTED_PRODUCT, SET_SELECTED_BASECURRENCY, SET_BASECURRENCY_LIST, SET_SELECTED_CURRENCY, TOGGLE_LOADER, CHANGE_IS_EXCLUDE_TEST, USER_GUIDE_CONTENT, SET_DATE_RANGE, GET_DATE_CONSTRAINT, CHANGE_IS_CONVERT_ALL, SET_SELECTED_STATUS, SET_SELECTED_PLATFORM, SET_SELECTED_LIVESTATUS, SET_USERNAME, SET_REQUEST_PARAM, SET_REPORT_TITLE, SET_PAGING_OPTION */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FETCH_DATA", function() { return FETCH_DATA; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_GRID_DATA", function() { return SET_GRID_DATA; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_PRODUCT", function() { return SET_SELECTED_PRODUCT; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_BASECURRENCY", function() { return SET_SELECTED_BASECURRENCY; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_BASECURRENCY_LIST", function() { return SET_BASECURRENCY_LIST; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_CURRENCY", function() { return SET_SELECTED_CURRENCY; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TOGGLE_LOADER", function() { return TOGGLE_LOADER; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CHANGE_IS_EXCLUDE_TEST", function() { return CHANGE_IS_EXCLUDE_TEST; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "USER_GUIDE_CONTENT", function() { return USER_GUIDE_CONTENT; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_DATE_RANGE", function() { return SET_DATE_RANGE; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "GET_DATE_CONSTRAINT", function() { return GET_DATE_CONSTRAINT; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "CHANGE_IS_CONVERT_ALL", function() { return CHANGE_IS_CONVERT_ALL; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_STATUS", function() { return SET_SELECTED_STATUS; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_PLATFORM", function() { return SET_SELECTED_PLATFORM; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_LIVESTATUS", function() { return SET_SELECTED_LIVESTATUS; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_USERNAME", function() { return SET_USERNAME; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_REQUEST_PARAM", function() { return SET_REQUEST_PARAM; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_REPORT_TITLE", function() { return SET_REPORT_TITLE; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_PAGING_OPTION", function() { return SET_PAGING_OPTION; });
const FETCH_DATA = 'FETCH_DATA';
const SET_GRID_DATA = "SET_GRID_DATA";
/* Product */

const SET_SELECTED_PRODUCT = "SET_SELECTED_PRODUCT";
/* Base Currency */

const SET_SELECTED_BASECURRENCY = "SET_SELECTED_BASECURRENCY";
const SET_BASECURRENCY_LIST = "SET_BASECURRENCY_LIST";
/* Currency */

const SET_SELECTED_CURRENCY = "SET_SELECTED_CURRENCY";
/* Loader */

const TOGGLE_LOADER = "TOGGLE_LOADER";
/* Exclude Test Data */

const CHANGE_IS_EXCLUDE_TEST = "CHANGE_IS_EXCLUDE_TEST";
/* User mini guide */

const USER_GUIDE_CONTENT = "USER_GUIDE_CONTENT;";
/* Date range */

const SET_DATE_RANGE = 'SET_DATE_RANGE';
const GET_DATE_CONSTRAINT = 'GET_DATE_CONSTRAINT';
/* Convert all figures to base currency */

const CHANGE_IS_CONVERT_ALL = "CHANGE_IS_CONVERT_ALL";
/* Status */

const SET_SELECTED_STATUS = "SET_SELECTED_STATUS";
/* Platform  */

const SET_SELECTED_PLATFORM = "SET_SELECTED_PLATFORM";
/* Live Status  */

const SET_SELECTED_LIVESTATUS = "SET_SELECTED_LIVESTATUS";
const SET_USERNAME = "SET_USERNAME";
const SET_REQUEST_PARAM = "SET_REQUEST_PARAM";
const SET_REPORT_TITLE = "SET_REPORT_TITLE";
/* Live Status  */

const SET_PAGING_OPTION = 'SET_PAGING_OPTION';

/***/ }),

/***/ "./www/js/source/reactpages/components/Loader.jsx":
/*!********************************************************!*\
  !*** ./www/js/source/reactpages/components/Loader.jsx ***!
  \********************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);


const Loader = ({
  isLoading
}) => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "loading-process default",
    style: {
      display: `${isLoading ? 'block' : 'none'}`
    }
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "icon-loading"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "loader-inner ball-beat"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null))));
};

/* harmony default export */ __webpack_exports__["default"] = (Loader);

/***/ }),

/***/ "./www/js/source/reactpages/components/NoDataTitle.jsx":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/components/NoDataTitle.jsx ***!
  \*************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _lib_langHelper__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");



const NoDataTitle = ({
  title
}) => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
    className: "no-info-row center-align",
    dangerouslySetInnerHTML: {
      __html: title ? title : _lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource("NoInfo")
    }
  });
};

/* harmony default export */ __webpack_exports__["default"] = (NoDataTitle);

/***/ }),

/***/ "./www/js/source/reactpages/components/ReportTitle.jsx":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/components/ReportTitle.jsx ***!
  \*************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




const ReportTitle = (_ref) => {
  let {
    content,
    tag
  } = _ref,
      attrs = _objectWithoutProperties(_ref, ["content", "tag"]);

  const Tag = tag;
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "report-title-container"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Tag, _extends({
    className: "report-title"
  }, attrs), content));
};

const propTypes = {
  tag: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  content: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.any
};
const defaultProps = {
  tag: 'p'
};
ReportTitle.propTypes = propTypes;
ReportTitle.defaultProps = defaultProps;
/* harmony default export */ __webpack_exports__["default"] = (ReportTitle);

/***/ }),

/***/ "./www/js/source/reactpages/components/betlist/BetListDataGrid.jsx":
/*!*************************************************************************!*\
  !*** ./www/js/source/reactpages/components/betlist/BetListDataGrid.jsx ***!
  \*************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @9softstudio/react-grid */ "./node_modules/@9softstudio/react-grid/dist/index.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _jsroot_components_popup__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/components/popup */ "./www/js/source/reactpages/components/popup/index.jsx");
/* harmony import */ var _jsroot_components_NoDataTitle__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @jsroot/components/NoDataTitle */ "./www/js/source/reactpages/components/NoDataTitle.jsx");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_4__);
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }







class BetListDataGrid extends react__WEBPACK_IMPORTED_MODULE_0__["Component"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "onSort", (sortBy, sortOrder) => {
      this.props.onSort && this.props.onSort(sortBy, sortOrder);
    });

    _defineProperty(this, "onPaging", (index, size) => {
      this.props.onPaging && this.props.onPaging(index, size);
    });
  }

  componentDidUpdate(prevProps) {
    if (this.props.data !== prevProps.data) {
      this.props.afterLoad && this.props.afterLoad();
    }
  }

  _getResource(key) {
    return this.props.getResourceFunc ? this.props.getResourceFunc(key) : key;
  }

  _getTotalCellClassName(column, value) {
    if (this.props.getTotalCellClassNameFunc) {
      return this.props.getTotalCellClassNameFunc(column, value);
    }

    return column === 'LicenseeWL' && value.substring(0, 1) === '-' ? 'negative' : '';
  }

  _getHeaderName(column) {
    return this.props.getHeaderNameFunc ? this.props.getHeaderNameFunc(column) : column;
  }

  _mapData(data) {
    return this.props.mappingDataFunc ? this.props.mappingDataFunc(data) : data;
  }

  _buildHeader() {
    let rows = [];
    const {
      columns,
      columnWidths,
      sortableColumns
    } = this.props;

    for (let i = 0; i < columns.length; i++) {
      const column = columns[i];
      const isSortable = sortableColumns.indexOf(column) >= 0;

      const headerName = this._getHeaderName(column);

      rows.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
        key: column,
        colWidth: columnWidths[i],
        sortBy: isSortable ? column : null
      }, headerName));
    }

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderRow"], {
      style: {
        height: 32
      }
    }, rows);
  }

  _buildBody() {
    const {
      columns,
      columnAlignments,
      data
    } = this.props;
    let rows = [];

    for (let i = 0; i < data.length; i++) {
      const item = this._mapData(data[i]);

      let cells = [];

      for (let j = 0; j < columns.length; j++) {
        const column = columns[j];
        cells.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["Cell"], {
          key: `${column}-${i}`,
          className: columnAlignments[j],
          dangerouslySetInnerHTML: {
            __html: item[column]
          }
        }));
      }

      rows.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["Row"], {
        key: i
      }, cells));
    }

    return rows;
  }

  _buildFooter() {
    if (!this.props.totalColumns || !this.props.totalColumns.length) {
      return false;
    }

    const {
      columns,
      totalColumns,
      total,
      totalText,
      columnAlignments
    } = this.props;
    const startTotalIndex = columns.indexOf(totalColumns[0]);
    let rows = [];
    let cells = [];
    cells.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["Cell"], {
      key: "total",
      className: "right-align",
      colSpan: startTotalIndex
    }, totalText));

    const item = this._mapData(total);

    for (let i = 0; i < totalColumns.length; i++) {
      const column = totalColumns[i];
      const columnIndex = columns.indexOf(column);

      const className = this._getTotalCellClassName(column, item[column]);

      cells.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["Cell"], {
        key: `Sum${column}`,
        className: `${columnAlignments[columnIndex]} ${className}`,
        dangerouslySetInnerHTML: {
          __html: item[column]
        }
      }));
    }

    const blankRows = columns.length - totalColumns.length - startTotalIndex;

    if (blankRows) {
      cells.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["Cell"], {
        key: "blank",
        colSpan: blankRows
      }));
    }

    rows.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["Row"], {
      className: "fixed-table-grant-total",
      key: "totalRow"
    }, cells));
    return rows;
  }

  _renderNodata() {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "result-content"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
      className: "no-info-row center-align"
    }, this._getResource('NoInfo')));
  }

  render() {
    if (!this.props.data) {
      return null;
    }

    const {
      data
    } = this.props;

    if (!data.length) {
      return this._renderNodata();
    }

    const {
      isPaging,
      pageOption,
      adjustedHeight
    } = this.props;
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "betlist-container result-content"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["Table"], {
      onSort: this.onSort,
      minWidth: 980,
      autoWidth: true,
      bodyHeight: 300,
      header: this._buildHeader(),
      body: this._buildBody(),
      footer: this._buildFooter(),
      isPaging: isPaging,
      pageOption: pageOption,
      onPaging: this.onPaging,
      adjustedHeight: adjustedHeight
    }));
  }

}

BetListDataGrid.propTypes = {
  isPaging: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.bool,
  pageOption: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.shape({
    PageIndex: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.number,
    PageSize: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.number,
    TotalItem: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.number,
    PageList: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.arrayOf(prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.number)
  }),
  columns: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.arrayOf(prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.string),
  columnWidths: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.arrayOf(prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.number),
  mappingDataFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  sortableColumns: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.arrayOf(prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.string),
  totalColumns: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.arrayOf(prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.string),
  getHeaderNameFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  columnAlignments: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.arrayOf(prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.string),
  data: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.arrayOf(prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.object),
  total: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.object,
  getTotalCellClassNameFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  totalText: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.string,
  onSort: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  onPaging: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  afterLoad: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  adjustedHeight: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.number,
  getResourceFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func
};
BetListDataGrid.defaultProps = {
  isPaging: false,
  pageOption: {
    PageIndex: 1,
    PageSize: 50,
    TotalItem: 0,
    PageList: [50, 100, 200, 300, 400, 500, 1000, 2000]
  },
  columns: ['Index', 'ExternalId', 'SystemId', 'RefNo', 'TransTime', 'Choice', 'Odds', 'OddsType', 'Stake', 'WinLoss', 'LicenseeWL', 'Status', 'Platform', 'Live'],
  columnWidths: [40, 120, 120, 110, 150, 400, 70, 70, 100, 100, 100, 80, 80, 80],
  sortableColumns: ['Stake', 'WinLoss', 'LicenseeWL'],
  totalColumns: ['Stake', 'WinLoss', 'LicenseeWL'],
  columnAlignments: ['center-align', '', '', 'center-align', 'center-align', 'right-align', 'right-align', 'right-align', 'right-align', 'right-align', 'right-align', 'center-align', 'center-align', 'center-align'],
  data: null,
  total: null,
  totalText: 'Total',
  adjustedHeight: 0
};
/* harmony default export */ __webpack_exports__["default"] = (BetListDataGrid);

/***/ }),

/***/ "./www/js/source/reactpages/components/popup/Popup.jsx":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/components/popup/Popup.jsx ***!
  \*************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/components/popup/constants.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_2__);




const Popup = props => {
  const {
    title,
    isShow,
    width,
    height,
    isHtml,
    children,
    iframeUrl
  } = props;
  let content;

  if (iframeUrl) {
    content = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("iframe", {
      frameBorder: "0",
      src: iframeUrl,
      width: "100%",
      height: "100%"
    });
  } else {
    content = isHtml ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "popup-content",
      dangerouslySetInnerHTML: {
        __html: children
      }
    }) : /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "popup-content"
    }, children);
  }

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: `popup-container ${isShow ? 'show' : 'hide'}`,
    onClick: props.onClick
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "popup-block-body"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "flat-popup",
    style: {
      height: height,
      width: width
    }
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "popup-title"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "txt-title"
  }, title), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("a", {
    className: "popup-close",
    onClick: () => {
      props.onClose && props.onClose();
    }
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("i", {
    className: "icon-close"
  }))), content));
};

Popup.propTypes = {
  width: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.oneOfType([prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.number, prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string]),
  height: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.oneOfType([prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.number, prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string]),
  title: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string,
  isShow: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.bool,
  onClose: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.func,
  isHtml: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.bool,
  iframeUrl: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string,
  onClick: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.func
};
Popup.defaultProps = {
  width: _constants__WEBPACK_IMPORTED_MODULE_1__["DEFAULT_WIDTH"],
  height: _constants__WEBPACK_IMPORTED_MODULE_1__["DEFAULT_HEIGHT"],
  isShow: false,
  isHtml: false,
  iframeUrl: null
};
/* harmony default export */ __webpack_exports__["default"] = (Popup);

/***/ }),

/***/ "./www/js/source/reactpages/components/popup/constants.js":
/*!****************************************************************!*\
  !*** ./www/js/source/reactpages/components/popup/constants.js ***!
  \****************************************************************/
/*! exports provided: DEFAULT_WIDTH, DEFAULT_HEIGHT */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DEFAULT_WIDTH", function() { return DEFAULT_WIDTH; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DEFAULT_HEIGHT", function() { return DEFAULT_HEIGHT; });
const DEFAULT_WIDTH = '55%';
const DEFAULT_HEIGHT = '55%';

/***/ }),

/***/ "./www/js/source/reactpages/components/popup/index.jsx":
/*!*************************************************************!*\
  !*** ./www/js/source/reactpages/components/popup/index.jsx ***!
  \*************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _Popup__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./Popup */ "./www/js/source/reactpages/components/popup/Popup.jsx");

/* harmony default export */ __webpack_exports__["default"] = (_Popup__WEBPACK_IMPORTED_MODULE_0__["default"]);

/***/ }),

/***/ "./www/js/source/reactpages/constants/config.js":
/*!******************************************************!*\
  !*** ./www/js/source/reactpages/constants/config.js ***!
  \******************************************************/
/*! exports provided: debuggingConfig */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "debuggingConfig", function() { return debuggingConfig; });
const debuggingConfig = {
  name: "__lrfConfig__",
  env: "local"
};

/***/ }),

/***/ "./www/js/source/reactpages/layouts/AppLayout.jsx":
/*!********************************************************!*\
  !*** ./www/js/source/reactpages/layouts/AppLayout.jsx ***!
  \********************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return AppLayout; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);

class AppLayout extends react__WEBPACK_IMPORTED_MODULE_0__["Component"] {
  constructor(props) {
    super(props);
  }

  render() {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      id: "container",
      className: "container-fluid"
    }, this.props.children);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/layouts/betlist/BetListLayout.jsx":
/*!********************************************************************!*\
  !*** ./www/js/source/reactpages/layouts/betlist/BetListLayout.jsx ***!
  \********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/DataService */ "./www/js/source/reactpages/lib/DataService.js");
/* harmony import */ var _lib_BetList__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./lib/BetList */ "./www/js/source/reactpages/layouts/betlist/lib/BetList.js");
/* harmony import */ var _jsroot_components_popup__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @jsroot/components/popup */ "./www/js/source/reactpages/components/popup/index.jsx");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./lib/constants */ "./www/js/source/reactpages/layouts/betlist/lib/constants.js");
/* harmony import */ var _babel_types__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @babel/types */ "./node_modules/@babel/types/lib/index.js");
/* harmony import */ var _babel_types__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_babel_types__WEBPACK_IMPORTED_MODULE_6__);








class BetListLayout extends react__WEBPACK_IMPORTED_MODULE_0__["Component"] {
  constructor(props) {
    super(props);
    this.state = {
      isShowPopup: false,
      popupUrl: null,
      popupTitle: null
    };
  }

  componentDidMount() {
    window.onload = () => {
      window.logOpenBetlistEvent && window.logOpenBetlistEvent(this.props.pageName);
    };

    window.ViewResult = (matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid, transdecs) => {
      const title = this._getResource('Result');

      const defaultPlatform = 'd';
      const rootUrl = this.props.rootUrl;

      if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].RNG_GAME || sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].RNG_MINIGAME || sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].RNG_MOBILE) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getRngCasinoResultUrl(rootUrl);

        this._showRngCasinoResult(refno, url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].VOIDBRIDGE) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getVoidbridgeResultUrl(rootUrl, custid, extension2);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].PP) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getPPResultUrl(rootUrl, custid, extension2);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].CARDCLUB) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getCardClubResultUrl(rootUrl, custid, bettype, extension2);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].SBC || sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].SBC_FreeBets) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSabaClubResultUrl(rootUrl, custid, bettype, extension2, transdecs, sporttype);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].SG) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSGResultUrl(rootUrl, custid, extension2);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].VRG) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getVRGResultUrl(rootUrl, refno, defaultPlatform);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].ALLBET) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getAllBetResultUrl(rootUrl, transdecs, extension2, bettype);

        this._openPopup(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].SAG) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSAGamingResultUrl(rootUrl, transdecs, extension2, bettype);

        this._openPopup(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].GLOBALGAMING) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getGlobalGamingResultUrl(rootUrl, custid, extension2);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].JK) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getJokerResultUrl(rootUrl, custid, transdecs, extension2);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].SXG) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSexyGamingResultUrl(rootUrl, transdecs, extension2, bettype);

        this._openPopup(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].PGS) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getPGSoftResultUrl(rootUrl, custid, transdecs, extension2);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].Happy5LinkedGames) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getHappy5LinkedGamesResultUrl(rootUrl, extension1, refno, defaultPlatform);

        this._showThirdPartyResult(url, title);
      } else if (sporttype === _lib_constants__WEBPACK_IMPORTED_MODULE_5__["SportType"].GDG) {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getGDGResultUrl(rootUrl, custid, transdecs, extension2, league);

        this._showThirdPartyResult(url, title);
      } else {
        const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getResultUrl(rootUrl, matchid, raceNo, bettype, sporttype, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid);

        this._openPopup(url, title);
      }
    };

    window.showMP = transId => {
      let divEvent = document.querySelector('#divEvent_' + transId);
      divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket

      this._showMixParlayDetail(transId, divEvent);
    };

    window.showCombMP = (transId, index) => {
      let divEvent = document.querySelector('#divEvent_' + transId + '' + index);
      divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket

      this._showMixParlayDetail(transId, divEvent);
    }; // expand choice content


    window.showNewCombMP = (...args) => {
      this._showNewParlayDetail.apply(this, [this.props.rootUrl, ...args]);
    }; // expand choice content


    window.showSingleMatchParlayDetail = (...args) => {
      this._showSingleMatchParlayDetail.apply(this, [this.props.rootUrl, ...args]);
    }; // show detail popup (in choice column)


    window.showCombMPDetail = (transId, refNo, betId, custId, betTeam) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getParlayDetailUrl(rootUrl, transId, refNo, betId, custId, betTeam, baseCurrency);

      this._openPopup(url, betTeam);
    };

    window.ViewCashOutHistory = (transId, refNo, betType, betdaqId) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const {
        url,
        isCashOut
      } = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getCashOutHistoryPopupSetting(rootUrl, baseCurrency, transId, refNo, betType, betdaqId);

      const title = this._getResource(isCashOut ? 'CashOutDetails' : 'BoughtDetails');

      this._openPopup(url, title);
    };

    window.ShowBonusDetails = (refCode, transId) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getBonusDetailUrl(rootUrl, refCode, transId, baseCurrency);

      const title = this._getResource('TransactionDetail');

      this._openPopup(url, title);
    };

    window.showAGCasinoDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getAGCasinoDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('AGCasino') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showRnGCasinoDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getRnGCasinoDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductCS') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showVoidBridgeDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getVoidBridgeDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductVB') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showTodayRunningBetsDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getAllBetDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductAB') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showColossusBetsDetails = (refNo, winlostDate, transId, custId, betCheck) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getColossusBetsDetailUrl(rootUrl, baseCurrency, refNo, winlostDate, transId, custId, betCheck);

      const title = this._getResource('TicketDetailOf').replace('{0}', refNo);

      this._openPopup(url, title);
    };

    window.showGlobalGaming = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getGlobalGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductFW') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showPPDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getPPDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductPP') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showPPJackpotWonDetails = (transId, custId, betTypeId) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getPPJackpotWonDetailUrl(rootUrl, baseCurrency, transId, custId, betTypeId);

      const title = this._getResource('ProductPP') + ' - ' + this._getResource('JackpotWonDetails');

      this._openPopup(url, title);
    };

    window.showSabaClubJackpotWonDetails = (transId, custId, betTypeId) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSabaClubJackpotWonDetailUrl(rootUrl, baseCurrency, transId, custId, betTypeId);

      const title = this._getResource('ProductSBC') + ' - ' + this._getResource('JackpotWonDetails');

      this._openPopup(url, title);
    };

    window.showVideoPokerGameDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getVideoPokerGameDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductCC') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showSGDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSGDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductSG') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showJokerBetDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getJokerDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductJK') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showSexyGamingBetDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSexyGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductSXG') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showSAGamingDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSAGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductSAG') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showSabaClubDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSBCDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductSBC') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showPGSoftDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getPGSDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductPGS') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };

    window.showGDGBetDetails = (transId, winlostDate, custId, transType) => {
      const {
        rootUrl,
        baseCurrency
      } = this.props;
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getGDGDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType);

      const title = this._getResource('ProductGDG') + ' - ' + this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

      this._openPopup(url, title);
    };
  }

  componentWillUnmount() {
    window.showMP = undefined;
    window.showCombMP = undefined;
    window.ViewResult = undefined;
    window.showNewCombMP = undefined;
    window.showCombMPDetail = undefined;
    window.ViewCashOutHistory = undefined;
    window.ShowBonusDetails = undefined;
    window.showAGCasinoDetails = undefined;
    window.showRnGCasinoDetails = undefined;
    window.showVoidBridgeDetails = undefined;
    window.showTodayRunningBetsDetails = undefined;
    window.showColossusBetsDetails = undefined;
    window.showGlobalGaming = undefined;
    window.showPPDetails = undefined;
    window.showPPJackpotWonDetails = undefined;
    window.showSabaClubJackpotWonDetails = undefined;
    window.showSingleMatchParlayDetail = undefined;
    window.showSGDetails = undefined;
    window.showJokerBetDetails = undefined;
    window.showSexyGamingBetDetails = undefined;
    window.showSAGamingDetails = undefined;
    window.showSabaClubDetails = undefined;
    window.showPGSoftDetails = undefined;
    window.showGDGBetDetails = undefined;
  }

  _getResource(key) {
    return this.props.getResourceFunc ? this.props.getResourceFunc(key) : key;
  }

  _formatDateString(dateString) {
    return this.props.formatDateStringFunc ? this.props.formatDateStringFunc(dateString) : dateString;
  }

  _beforeShow() {
    this.props.beforeShowFunc && this.props.beforeShowFunc();
  }

  _afterShow() {
    this.props.afterShowFunc && this.props.afterShowFunc();
  }

  _resolveClientUrl(relativeUrl) {
    return this.props.rootUrl + relativeUrl;
  }

  _showMixParlayDetail(transId, divEvent) {
    if (divEvent.style.display === 'none') {
      const url = this._resolveClientUrl('api/MixParlayBetList/Detail?transId=' + transId);

      this._beforeShow(); // this.props.toggleLoader(true);


      _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_1__["default"].getData(url).then(result => {
        let choice = '';
        const length = result.data.length;

        for (let i = 0; i < length; i++) {
          choice += result.data[i];

          if (i < length - 1) {
            choice += '<div class="line"></div>';
          }
        }

        divEvent.innerHTML = choice;
        divEvent.style.display = 'block';

        this._afterShow();
      }).catch(error => {
        this._afterShow();

        console.log(error);
      });
    } else {
      divEvent.style.display = 'none';
      divEvent.innerHTML = '';
    }
  }

  _showNewParlayDetail(rootUrl, transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam, custId) {
    let divEvent = document.querySelector('#divEvent_' + transId + '' + transIndex);
    let nameDiv = document.querySelector('#combinationOfferName_' + transId + transIndex);
    divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket

    if (divEvent.style.display == 'none') {
      let url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getChoiceParlayDetailUrl(rootUrl, transId, refNo, handicap1, handicap2, betcheck, betteam, custId);

      this._beforeShow(); //this.props.toggleLoader(true);


      _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_1__["default"].getData(url).then(result => {
        let choice = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].buildChoiceParlayDetail(result.data);
        divEvent.innerHTML = choice;
        divEvent.style.display = 'block';
        nameDiv.style.display = 'none';

        this._afterShow();
      }).catch(error => {
        this._afterShow();

        console.log(error);
      });
    } else {
      divEvent.style.display = 'none';
      divEvent.innerHTML = '';
      nameDiv.style.display = 'block';
    }
  }

  _showThirdPartyResult(url, title) {
    this._beforeShow(); //this.props.toggleLoader(true);


    _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_1__["default"].getData(url).then(result => {
      if (result.data !== '') {
        this._openPopup(result.data, title);
      }

      this._afterShow();
    }).catch(error => {
      this._afterShow();

      console.log(error);
    });
  }

  _showRngCasinoResult(refno, url, title) {
    this._beforeShow();

    _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_1__["default"].postData(url, {
      refno
    }).then(result => {
      if (result.data.Success) {
        if (result.data.Result === '') {
          return;
        }

        this._openPopup(result.data.Result, title);
      } else {
        alert(this._getResource("ErrorProcessingMsg"));
      }

      this._afterShow();
    }).catch(error => {
      this._afterShow();

      console.log(error);
    });
  }

  _showSingleMatchParlayDetail(rootUrl, transId, custId, status, transIndex) {
    const divEvent = document.querySelector('#divEvent_' + transId + '' + transIndex);

    if (divEvent.style.display == 'none') {
      const url = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleMatchParlayDetailUrl(rootUrl, transId, custId, status);

      this._beforeShow();

      _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_1__["default"].getData(url).then(result => {
        const selection = _lib_BetList__WEBPACK_IMPORTED_MODULE_2__["default"].buildSingleMatchParlayDetail(result.data);
        divEvent.innerHTML = selection;
        divEvent.style.display = 'block';

        this._afterShow();
      }).catch(function (error) {
        console.log(error);
      });
    } else {
      divEvent.style.display = 'none';
      divEvent.innerHTML = '';
    }
  }

  _openPopup(url, title) {
    this.setState({
      isShowPopup: true,
      popupUrl: url,
      popupTitle: title
    });
  }

  _closePopup() {
    this.setState({
      isShowPopup: false
    });
  }

  render() {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "betlist-layout"
    }, this.props.children, this.state.isShowPopup && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_popup__WEBPACK_IMPORTED_MODULE_3__["default"], {
      isShow: true,
      iframeUrl: this.state.popupUrl,
      title: this.state.popupTitle,
      width: 900,
      height: 600,
      onClose: () => this._closePopup()
    }));
  }

}

BetListLayout.propTypes = {
  pageName: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.string.isRequired,
  rootUrl: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.string.isRequired,
  getResourceFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  baseCurrency: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.string,
  beforeShowFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  afterShowFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  beforeShowFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  afterShowFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func,
  formatDateStringFunc: prop_types__WEBPACK_IMPORTED_MODULE_4___default.a.func
};
BetListLayout.defaultProps = {
  rootUrl: '/',
  baseCurrency: ''
};
/* harmony default export */ __webpack_exports__["default"] = (BetListLayout);

/***/ }),

/***/ "./www/js/source/reactpages/layouts/betlist/lib/BetList.js":
/*!*****************************************************************!*\
  !*** ./www/js/source/reactpages/layouts/betlist/lib/BetList.js ***!
  \*****************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/QueryString */ "./www/js/source/reactpages/lib/QueryString.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/layouts/betlist/lib/constants.js");



class BetList {
  _isFastMarket(bettype) {
    return bettype >= _constants__WEBPACK_IMPORTED_MODULE_1__["BetType"].MIN_FASTMARKET && bettype <= _constants__WEBPACK_IMPORTED_MODULE_1__["BetType"].MAX_FASTMARKET;
  }

  _getMixParlayResultUrl(rootUrl, refno, winlostdate, bettype) {
    let url = rootUrl + 'ResultReport/ResultPopup';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'refno', refno);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'winlossdate', winlostdate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'bettype', bettype);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'matchid', 1);
    return url;
  }

  _getSportBookResultUrl(rootUrl, matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2) {
    let url = rootUrl + 'ResultReport/ResultPopup';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'matchid', matchid);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'bettype', bettype);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'raceno', raceNo);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'sporttype', sporttype);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'isoutright', isOutright);

    if (this._isFastMarket(bettype)) {
      url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'handicap1', extension1);
      url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'handicap2', extension2);
    } else {
      url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'betid', extension1);
    }

    return url;
  }

  getResultUrl(rootUrl, matchid, raceNo, bettype, sporttype, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid) {
    bettype = Number(bettype);
    let isParlay = bettype === _constants__WEBPACK_IMPORTED_MODULE_1__["BetType"].MIXPARLAY || bettype === _constants__WEBPACK_IMPORTED_MODULE_1__["BetType"].SYSTEMPARLAY || bettype === _constants__WEBPACK_IMPORTED_MODULE_1__["BetType"].MIXPARLAY_BETRADAR;
    let url = isParlay ? this._getMixParlayResultUrl(rootUrl, refnoMixParlay, winlostdate, bettype) : this._getSportBookResultUrl(rootUrl, matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid);
    return url;
  }

  getChoiceParlayDetailUrl(rootUrl, transId, refNo, handicap1, handicap2, betcheck, betteam, custId) {
    var url = rootUrl + 'api/MixParlayBetList/NewParlayDetail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'refNo', refNo);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'handicap1', handicap1);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'handicap2', handicap2);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'betcheck', betcheck);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'betteam', betteam);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    return url;
  }

  buildChoiceParlayDetail(response) {
    let choice = '';
    let length = response.Data === null ? 0 : response.Data.length;

    if (length > 0) {
      for (let i = 0; i < response.Texts.length; i++) {
        choice += response.Texts[i];
      }

      choice += '<div class="line"></div>';

      for (let i = 0; i < response.Data.length; i++) {
        let item = response.Data[i];
        choice += item;

        if (i < length - 1) {
          choice += '<div class="line"></div>';
        }
      }
    }

    return choice;
  }

  buildSingleMatchParlayDetail(data) {
    let selection = '';
    let length = data === null ? 0 : data.length;

    if (length > 0) {
      selection = '<div class="single-match-parlay-detail">';

      for (let i = 0; i < data.length; i++) {
        selection += data[i];
      }

      selection += '</div>';
    }

    return selection;
  }

  getParlayDetailUrl(rootUrl, transId, refNo, betId, custId, betTeam, baseCurrency) {
    if (Number(transId) === 0) {
      return;
    }

    var url = rootUrl + 'SystemParlayBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'refno', refNo);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'betteam', betTeam);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'basecurrency', baseCurrency);
    return url;
  }

  getSingleMatchParlayDetailUrl(rootUrl, transId, custId, status) {
    if (Number(transId) === 0) {
      return;
    }

    var url = rootUrl + 'api/SingleMatchParlayBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, "status", status);
    return url;
  }

  getCashOutHistoryPopupSetting(rootUrl, baseCurrency, transId, refNo, betType, betdaqId) {
    var isCashOut = betdaqId < 1 || betType === _constants__WEBPACK_IMPORTED_MODULE_1__["BetType"].CASHOUT;
    var url = rootUrl + 'CashOutBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'refNo', refNo);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'betType', betType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'betdaqId', betdaqId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'isCashout', isCashOut);
    return {
      url: url,
      isCashOut: isCashOut
    };
  }

  getBonusDetailUrl(rootUrl, refCode, transId, baseCurrency) {
    let url = rootUrl + 'BonusBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'RefCode', refCode);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'TransId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getAGCasinoDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'AGCasinoBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transid', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getRnGCasinoDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'RnGCasinoBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getVoidBridgeDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'VoidbridgeBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getAllBetDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'AllBetBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transid', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getColossusBetsDetailUrl(rootUrl, baseCurrency, refNo, winlostDate, transId, custId, betCheck) {
    let url = rootUrl + 'ColossusBetBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'refNo', refNo);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transid', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'betCheck', betCheck);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getGlobalGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'GlobalGamingBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getPPDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'PragmaticPlayBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getPPJackpotWonDetailUrl(rootUrl, baseCurrency, transId, custId, betTypeId) {
    let url = rootUrl + 'JackpotWonBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'bettype', betTypeId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getSabaClubJackpotWonDetailUrl(rootUrl, baseCurrency, transId, custId, betTypeId) {
    let url = rootUrl + 'SabaClubJackpotWonBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'bettype', betTypeId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getVideoPokerGameDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'CardClubBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getSGDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'SGBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getJokerDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'JokerBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getSexyGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'SexyGamingBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getSAGamingDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'SAGamingBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getSBCDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'SabaClubBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getPGSDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'PGSoftBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getPGSDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'PGSoftBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getGDGDetailUrl(rootUrl, baseCurrency, transId, winlostDate, custId, transType) {
    let url = rootUrl + 'GDGBetList/Detail';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId', transId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'date', winlostDate);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transType', transType);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'BaseCurrency', baseCurrency);
    return url;
  }

  getRngCasinoResultUrl(rootUrl) {
    return rootUrl + 'ResultReport/ShowRngCasinoSingleWalletResult';
  }

  getVoidbridgeResultUrl(rootUrl, custid, gamePlayID) {
    let url = rootUrl + 'ResultReport/VoidbridgeResult';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custid);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gamePlayID', gamePlayID);
    return url;
  }

  getPPResultUrl(rootUrl, custid, gamePlayId) {
    let url = rootUrl + 'PPResultPopup/Index';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custid);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gamePlayId', gamePlayId);
    return url;
  }

  getCardClubResultUrl(rootUrl, custid, gameTypeId, transId3rd) {
    let url = rootUrl + 'CardClubResultPopup/Index';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custid);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gameTypeId', gameTypeId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId3rd', transId3rd);
    return url;
  }

  getSabaClubResultUrl(rootUrl, custid, gameId, siteId, gameRoundId, sportType) {
    let url = rootUrl + 'SabaClubResultPopup/Index';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custid);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gameId', gameId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'siteId', siteId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gameRoundId', gameRoundId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'sportType', sportType);
    return url;
  }

  getSGResultUrl(rootUrl, custid, gamePlayId) {
    let url = rootUrl + 'SGResultPopup/Index';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custid);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gamePlayId', gamePlayId);
    return url;
  }

  getVRGResultUrl(rootUrl, gamePlayId, platform) {
    let url = rootUrl + 'VirtualGamesResultPopup/Index';
    console.log(gamePlayId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gamePlayId', gamePlayId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'platform', platform);
    return url;
  }

  getAllBetResultUrl(rootUrl, transdesc, betcheck, bettype) {
    let url = rootUrl + 'ResultReport/AllBetResult';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'betcheck', betcheck);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'bettype', bettype);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transdesc', encodeURIComponent(transdesc));
    return url;
  }

  getSAGamingResultUrl(rootUrl, transdesc, matchCode, bettype) {
    let url = rootUrl + 'ResultReport/SAGaming';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'matchCode', matchCode);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'bettype', bettype);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transdesc', encodeURIComponent(transdesc));
    return url;
  }

  getJokerResultUrl(rootUrl, custId, gamePlayId, gameCode) {
    let url = rootUrl + 'ResultReport/Joker';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gamePlayId', gamePlayId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gameCode', gameCode);
    return url;
  }

  getSexyGamingResultUrl(rootUrl, transdesc, matchCode, bettype) {
    let url = rootUrl + 'ResultReport/SexyGaming';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'matchCode', matchCode);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'bettype', bettype);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transdesc', encodeURIComponent(transdesc));
    return url;
  }

  getGlobalGamingResultUrl(rootUrl, custid, gameId) {
    let url = rootUrl + 'ResultReport/GlobalGamingResult';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custid);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gameId', gameId);
    return url;
  }

  getPGSoftResultUrl(rootUrl, custid, gamePlayId, gameId3rd) {
    let url = rootUrl + 'PGSoftResultPopup/Index';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custid', custid);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gamePlayId', gamePlayId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gameId3rd', gameId3rd);
    return url;
  }

  getHappy5LinkedGamesResultUrl(rootUrl, custId, transId3rd, platform) {
    let url = rootUrl + 'Happy5LinkedGamesResultPopup/Index';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId3rd', transId3rd);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'platform', platform);
    return url;
  }

  getGDGResultUrl(rootUrl, custId, transId3rd, gameId3rd, siteId) {
    let url = rootUrl + 'ResultReport/GDG';
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'custId', custId);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'transId3rd', transId3rd);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'gameId3rd', gameId3rd);
    url = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_0__["default"].setParam(url, 'siteId', siteId);
    return url;
  }

}

/* harmony default export */ __webpack_exports__["default"] = (new BetList());

/***/ }),

/***/ "./www/js/source/reactpages/layouts/betlist/lib/constants.js":
/*!*******************************************************************!*\
  !*** ./www/js/source/reactpages/layouts/betlist/lib/constants.js ***!
  \*******************************************************************/
/*! exports provided: BetType, SportType */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "BetType", function() { return BetType; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SportType", function() { return SportType; });
const BetType = {
  CASHOUT: 911,
  MIXPARLAY: 9,
  SYSTEMPARLAY: 29,
  MIXPARLAY_BETRADAR: 2799,
  MIN_FASTMARKET: 221,
  MAX_FASTMARKET: 227
};
const SportType = {
  RNG_GAME: 208,
  RNG_MINIGAME: 209,
  RNG_MOBILE: 210,
  ALLBET: 211,
  VOIDBRIDGE: 212,
  PP: 248,
  GLOBALGAMING: 219,
  CARDCLUB: 165,
  SG: 247,
  VRG: 245,
  JK: 234,
  SXG: 243,
  SBC: 200,
  SBC_FreeBets: 150,
  SAG: 244,
  PGS: 232,
  Happy5LinkedGames: 168,
  GDG: 236
};

/***/ }),

/***/ "./www/js/source/reactpages/lib/AppData.js":
/*!*************************************************!*\
  !*** ./www/js/source/reactpages/lib/AppData.js ***!
  \*************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = (window._AppData || {});

/***/ }),

/***/ "./www/js/source/reactpages/lib/DataService.js":
/*!*****************************************************!*\
  !*** ./www/js/source/reactpages/lib/DataService.js ***!
  \*****************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return DataService; });
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(axios__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");


class DataService {
  static getData(url, params) {
    return axios__WEBPACK_IMPORTED_MODULE_0___default.a.get(url, {
      params: params
    });
  }

  static postData(url, params) {
    return axios__WEBPACK_IMPORTED_MODULE_0___default.a.post(url, params, {
      headers: {
        'X-Requested-With': 'XMLHttpRequest',
        '__RequestVerificationToken': _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_1__["default"].getXsrfToken()
      }
    });
  }

  static getFile(url, params) {
    return axios__WEBPACK_IMPORTED_MODULE_0___default.a.post(url, params, {
      responseType: 'blob',
      headers: {
        'X-Requested-With': 'XMLHttpRequest',
        '__RequestVerificationToken': _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_1__["default"].getXsrfToken()
      }
    });
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/lib/QueryString.js":
/*!*****************************************************!*\
  !*** ./www/js/source/reactpages/lib/QueryString.js ***!
  \*****************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return QueryString; });
class QueryString {
  static _parseQueryString(queryString) {
    let args = queryString.split('&');
    let argsList = {};

    for (let i = 0; i < args.length; i++) {
      let arg = unescape(args[i]);

      if (arg.length > 0) {
        if (arg.indexOf('=') === -1) {
          argsList[arg.toLowerCase().trim()] = null;
        } else {
          let keyValuePair = arg.split('=');
          argsList[keyValuePair[0].toLowerCase().trim()] = keyValuePair[1].trim();
        }
      }
    }

    return argsList;
  }

  static _getQueryString(url) {
    var index = url.indexOf("?");
    return index < 0 ? "" : url.substring(index + 1, url.length);
  }

  static _getRawUrl(url) {
    var index = url.indexOf("?");
    return index < 0 ? url : url.substring(0, index);
  }

  static setParam(url, name, value) {
    var rawUrl = QueryString._getRawUrl(url);

    var queryString = QueryString._getQueryString(url);

    var args = QueryString._parseQueryString(queryString);

    if (!name) {
      return url;
    } // Update value


    var found = false;

    for (var key in args) {
      if (key && key.toLowerCase() === name.toLowerCase()) {
        // Does not update data if value = undefined
        if (value !== undefined) {
          args[key] = value;
        }

        found = true;
      }
    }

    if (!found) {
      args[name.toLowerCase()] = value;
    }

    var queryArray = []; // Rebuild query string, ignore null value

    for (var key1 in args) {
      if (args[key1] != null && args[key1] !== undefined) {
        queryArray.push(key1.toLowerCase() + "=" + args[key1]);
      }
    }

    return rawUrl + "?" + queryArray.join("&");
  }

  static setParams(url, parameters) {
    for (var property in parameters) {
      if (parameters.hasOwnProperty(property)) {
        url = QueryString.setParam(url, property, parameters[property]);
      }
    }

    return url;
  }

  static getParam(url, name) {
    var queryString = QueryString._getQueryString(url);

    var args = QueryString._parseQueryString(queryString);

    for (var key in args) {
      if (key != null && key.toLowerCase() === name.toLowerCase()) {
        return args[key];
      }
    }

    return null;
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/lib/createReducer.js":
/*!*******************************************************!*\
  !*** ./www/js/source/reactpages/lib/createReducer.js ***!
  \*******************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return createReducer; });
function createReducer(initialState, handlers) {
  return function reducer(state = initialState, action) {
    if (handlers.hasOwnProperty(action.type)) {
      return handlers[action.type](state, action);
    } else {
      return state;
    }
  };
}

/***/ }),

/***/ "./www/js/source/reactpages/lib/langHelper.js":
/*!****************************************************!*\
  !*** ./www/js/source/reactpages/lib/langHelper.js ***!
  \****************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return LangHelper; });
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

const DEFAULT_RESOURCE_NAME = "_language_en_US";
const langCode = window.PageLanguage || "en-US";
const resourceName = `_language_${langCode.replace("-", "_")}`;
class LangHelper {
  static get currentLanguage() {
    return langCode;
  }

  static getSingleResource(key) {
    var langHandler = window[resourceName];
    return langHandler && langHandler[key] || window[DEFAULT_RESOURCE_NAME] && window[DEFAULT_RESOURCE_NAME][key] || key;
  }

  static getResources(keys) {
    var langHandler = window[resourceName];
    return keys.map(key => langHandler && langHandler[key] || window[DEFAULT_RESOURCE_NAME] && window[DEFAULT_RESOURCE_NAME][key] || key);
  }

}

_defineProperty(LangHelper, "languageKeys", {
  eng: "en-US",
  zhCN: "zh-CN"
});

/***/ }),

/***/ "./www/js/source/reactpages/lib/logger.js":
/*!************************************************!*\
  !*** ./www/js/source/reactpages/lib/logger.js ***!
  \************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return Logger; });
/* harmony import */ var redux_logger__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! redux-logger */ "./node_modules/redux-logger/dist/redux-logger.js");
/* harmony import */ var redux_logger__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux_logger__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _constants_config__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../constants/config */ "./www/js/source/reactpages/constants/config.js");


const isDev = "development" === "development";
const configName = _constants_config__WEBPACK_IMPORTED_MODULE_1__["debuggingConfig"].name; // middleware that logs actions

const loggerMiddleware = Object(redux_logger__WEBPACK_IMPORTED_MODULE_0__["createLogger"])({
  predicate: (getState, action) => isDev || window[configName] && window[configName].env === _constants_config__WEBPACK_IMPORTED_MODULE_1__["debuggingConfig"].env && (!window[configName].actionFilter || window[configName].actionFilter === action.type)
});
class Logger {
  static getLoggerMiddleware() {
    return loggerMiddleware;
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/lib/site.js":
/*!**********************************************!*\
  !*** ./www/js/source/reactpages/lib/site.js ***!
  \**********************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return Site; });
const _rootUrl = window.rootUrl || "/";

let _xsrfToken = null;
class Site {
  static getXsrfToken() {
    if (!_xsrfToken) {
      _xsrfToken = document.querySelector("input[name=__RequestVerificationToken]").value;
    }

    return _xsrfToken;
  }

  static resolveClientUrl(relativeUrl) {
    return _rootUrl + relativeUrl;
  }

  static convertDateToString(date) {
    return typeof date === "string" ? date : date.format("MM/DD/YYYY");
  }

  static asNull(text) {
    if (text != null) {
      text = text.trim();

      if (text === '') {
        return null;
      }
    }

    return text;
  }

  static downloadFile(data, fileName) {
    const url = window.URL.createObjectURL(new Blob([data]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', fileName);
    document.body.appendChild(link);
    link.click();
    link.parentNode.removeChild(link);
    window.URL.revokeObjectURL(url);
  }

  static readAsJson(blob, callback) {
    const reader = new FileReader();

    reader.onload = function () {
      callback && callback(JSON.parse(reader.result));
    };

    reader.readAsText(blob);
  }

  static getRootUrl() {
    return _rootUrl;
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/lib/store.js":
/*!***********************************************!*\
  !*** ./www/js/source/reactpages/lib/store.js ***!
  \***********************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return Store; });
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! redux-thunk */ "./node_modules/redux-thunk/es/index.js");
/* harmony import */ var _logger__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./logger */ "./www/js/source/reactpages/lib/logger.js");



class Store {
  static createStore(reducer, initialState) {
    const loggerMiddleware = _logger__WEBPACK_IMPORTED_MODULE_2__["default"].getLoggerMiddleware();
    const enhancer = Object(redux__WEBPACK_IMPORTED_MODULE_0__["compose"])(Object(redux__WEBPACK_IMPORTED_MODULE_0__["applyMiddleware"])(redux_thunk__WEBPACK_IMPORTED_MODULE_1__["default"], // lets us dispatch() functions
    loggerMiddleware));
    return Object(redux__WEBPACK_IMPORTED_MODULE_0__["createStore"])(reducer, initialState, enhancer);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/reducers/loaderReducer.js":
/*!************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/loaderReducer.js ***!
  \************************************************************/
/*! exports provided: isLoading */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isLoading", function() { return isLoading; });
/* harmony import */ var _actions_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");
/* harmony import */ var _lib_createReducer_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");


const isLoading = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_1__["default"])({}, {
  [_actions_types__WEBPACK_IMPORTED_MODULE_0__["TOGGLE_LOADER"]](state, action) {
    return action.data;
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/reducers/requestParamReducer.js":
/*!******************************************************************!*\
  !*** ./www/js/source/reactpages/reducers/requestParamReducer.js ***!
  \******************************************************************/
/*! exports provided: requestParam */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "requestParam", function() { return requestParam; });
/* harmony import */ var _actions_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/actions/types.js");
/* harmony import */ var _lib_createReducer_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const requestParam = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_1__["default"])({}, {
  [_actions_types__WEBPACK_IMPORTED_MODULE_0__["SET_REQUEST_PARAM"]](state, action) {
    return _objectSpread({}, action.data);
  }

});

/***/ })

/******/ });
//# sourceMappingURL=runningbetlist.chunk.js.map