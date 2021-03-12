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
/******/ 		"www/js/build/dev/reactpages/playeradmin/betlimit": 0
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
/******/ 	deferredModules.push(["./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/index.jsx","lib/common"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/alertAction.js":
/*!*************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/alertAction.js ***!
  \*************************************************************************************/
/*! exports provided: setAlertOption */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setAlertOption", function() { return setAlertOption; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/types.js");

const setAlertOption = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_ALERT_OPTION"],
    data: data
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/index.js":
/*!*******************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/index.js ***!
  \*******************************************************************************/
/*! exports provided: ActionCreators */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ActionCreators", function() { return ActionCreators; });
/* harmony import */ var _jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");
/* harmony import */ var _jsroot_actions_requestParamAction__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/actions/requestParamAction */ "./www/js/source/reactpages/actions/requestParamAction.js");
/* harmony import */ var _alertAction__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./alertAction */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/alertAction.js");



const ActionCreators = Object.assign({}, _jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_0__, _jsroot_actions_requestParamAction__WEBPACK_IMPORTED_MODULE_1__, _alertAction__WEBPACK_IMPORTED_MODULE_2__);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/types.js":
/*!*******************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/types.js ***!
  \*******************************************************************************/
/*! exports provided: SET_ALERT_OPTION */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_ALERT_OPTION", function() { return SET_ALERT_OPTION; });
const SET_ALERT_OPTION = "SET_ALERT_OPTION";

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/AppContainer.jsx":
/*!******************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/AppContainer.jsx ***!
  \******************************************************************************************/
/*! exports provided: AppContainer, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppContainer", function() { return AppContainer; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _jsroot_layouts_AppLayout__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/layouts/AppLayout */ "./www/js/source/reactpages/layouts/AppLayout.jsx");
/* harmony import */ var _FormFilter__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./FormFilter */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/FormFilter.jsx");
/* harmony import */ var _DataContent__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./DataContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/DataContent.jsx");
/* harmony import */ var _jsroot_components_Loader__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/components/Loader */ "./www/js/source/reactpages/components/Loader.jsx");
/* harmony import */ var _jsroot_components_sweet_alert__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @jsroot/components/sweet-alert */ "./www/js/source/reactpages/components/sweet-alert/index.jsx");
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }








const AppContainer = props => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_layouts_AppLayout__WEBPACK_IMPORTED_MODULE_2__["default"], null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_Loader__WEBPACK_IMPORTED_MODULE_5__["default"], {
    isLoading: props.isLoading
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_FormFilter__WEBPACK_IMPORTED_MODULE_3__["default"], null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_DataContent__WEBPACK_IMPORTED_MODULE_4__["default"], null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_sweet_alert__WEBPACK_IMPORTED_MODULE_6__["default"], _extends({
    width: 450,
    isHtml: false
  }, props.alertOption)));
};

function mapStateToProps(state) {
  const {
    isLoading,
    alertOption
  } = state;
  return Object.assign({}, {
    isLoading,
    alertOption
  });
}

/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps)( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(AppContainer)));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/DataContent.jsx":
/*!*****************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/DataContent.jsx ***!
  \*****************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/constants.js");
/* harmony import */ var _grid_data_contents_SportDataContent__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./grid/data-contents/SportDataContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/SportDataContent.jsx");
/* harmony import */ var _grid_data_contents_GameDataContent__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./grid/data-contents/GameDataContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/GameDataContent.jsx");
/* harmony import */ var _grid_data_contents_NGDataContent__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./grid/data-contents/NGDataContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/NGDataContent.jsx");
/* harmony import */ var _grid_data_contents_AGCDataContent__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./grid/data-contents/AGCDataContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/AGCDataContent.jsx");
/* harmony import */ var _grid_data_contents_I4DDataContent__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./grid/data-contents/I4DDataContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/I4DDataContent.jsx");








const Setting = {
  [_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].SB]: _grid_data_contents_SportDataContent__WEBPACK_IMPORTED_MODULE_3__["default"],
  [_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].VS]: _grid_data_contents_SportDataContent__WEBPACK_IMPORTED_MODULE_3__["default"],
  [_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].VS2]: _grid_data_contents_SportDataContent__WEBPACK_IMPORTED_MODULE_3__["default"],
  [_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].LT]: _grid_data_contents_GameDataContent__WEBPACK_IMPORTED_MODULE_4__["default"],
  [_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].NG]: _grid_data_contents_NGDataContent__WEBPACK_IMPORTED_MODULE_5__["default"],
  [_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].KN]: _grid_data_contents_GameDataContent__WEBPACK_IMPORTED_MODULE_4__["default"],
  [_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].AGC]: _grid_data_contents_AGCDataContent__WEBPACK_IMPORTED_MODULE_6__["default"],
  [_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].TG]: _grid_data_contents_GameDataContent__WEBPACK_IMPORTED_MODULE_4__["default"],
  [_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].I4D]: _grid_data_contents_I4DDataContent__WEBPACK_IMPORTED_MODULE_7__["default"]
};

const DataContent = props => {
  const {
    ProductId
  } = props.requestParam;
  const Content = Setting[ProductId];
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Content, null);
};

function mapStateToProps(state) {
  const {
    requestParam
  } = state;
  return Object.assign({}, {
    requestParam
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps)(DataContent));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/FormFilter.jsx":
/*!****************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/FormFilter.jsx ***!
  \****************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _actions_index_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _jsroot_lib_AppData__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/lib/AppData */ "./www/js/source/reactpages/lib/AppData.js");
/* harmony import */ var react_select__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! react-select */ "./node_modules/react-select/dist/react-select.es.js");








const FormFilter = props => {
  const [productId, setProductId] = Object(react__WEBPACK_IMPORTED_MODULE_0__["useState"])(props.requestParam.ProductId);
  const productList = _jsroot_lib_AppData__WEBPACK_IMPORTED_MODULE_5__["default"].productList;

  const onProductChange = item => {
    setProductId(item.value);

    if (item.value !== props.requestParam.ProductId) {
      const param = Object.assign({}, props.requestParam, {
        ProductId: item.value
      });
      props.setRequestParam(param);
    }
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "page-filter"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "row"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "col-12 col-md-4 col-sm-12 col-lg-3 col-xl-2"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "form-group"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
    className: "label-text"
  }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Product')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_select__WEBPACK_IMPORTED_MODULE_6__["default"], {
    instanceId: "ddlProduct",
    value: productId,
    options: productList,
    onChange: onProductChange
  })))));
};

function mapDispatchToProps(dispatch) {
  const {
    setRequestParam
  } = _actions_index_js__WEBPACK_IMPORTED_MODULE_2__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_3__["bindActionCreators"])({
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
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(FormFilter)));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingContent.jsx":
/*!******************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingContent.jsx ***!
  \******************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions_index__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../../actions/index */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/index.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _jsroot_components_NoDataTitle__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/components/NoDataTitle */ "./www/js/source/reactpages/components/NoDataTitle.jsx");
/* harmony import */ var _lib_dataTransfer__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../../lib/dataTransfer */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataTransfer.js");
/* harmony import */ var _lib_utility__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../../lib/utility */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js");
/* harmony import */ var _jsroot_components_CheckBox__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @jsroot/components/CheckBox */ "./www/js/source/reactpages/components/CheckBox.jsx");
/* harmony import */ var _jsroot_components_Balloon__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @jsroot/components/Balloon */ "./www/js/source/reactpages/components/Balloon.jsx");
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }












const SettingContent = (Grid, DataHandling) => {
  var _temp;

  return _temp = class extends react__WEBPACK_IMPORTED_MODULE_0__["PureComponent"] {
    constructor(props) {
      super(props);

      _defineProperty(this, "_unRegisterEvents", () => {
        document.removeEventListener('click', this._handleClickOutSide);
      });

      _defineProperty(this, "_handleClickOutSide", event => {
        if (this.state.showError) {
          const isError = this.errorRef.current && this.errorRef.current.contains(event.target);
          const isCheckBox = this.checkBoxRef.current && this.checkBoxRef.current.contains(event.target);
          const isButton = this.submitButtonRef.current && this.submitButtonRef.current.contains(event.target);
          const clickOutside = !isError && !isCheckBox && !isButton;

          if (clickOutside) {
            this._closeError();
          }
        }
      });

      _defineProperty(this, "setInputRef", (type, id, element) => {
        this.ref[type] = this.ref[type] || {};
        this.ref[type][id] = element;
      });

      _defineProperty(this, "onReset", () => {
        if (!this.state.disabledButton) {
          this.setState({
            isCopy: false
          }, () => this._getData(this.props.requestParam));
        }
      });

      _defineProperty(this, "onChange", data => {
        this.setState({
          data: data
        });
      });

      _defineProperty(this, "onSubmit", () => {
        if (!this.state.showError) {
          const data = this.state.data;
          const submitData = DataHandling.getSubmitData(data);

          if (!submitData.IsValid) {
            this._showError(submitData.Data);
          } else {
            this._showConfirmation(() => this._submit(submitData.Data));
          }
        }
      });

      _defineProperty(this, "onCheck", checked => {
        if (checked) {
          const {
            setting,
            errorBalloon,
            showError
          } = this.state;

          if (showError && errorBalloon.Id === setting.CopyId) {
            return false;
          }

          const {
            data
          } = this.state;
          const copyRow = data.find(e => e.Id === setting.CopyId);
          const copyValue = DataHandling.getSubmitRowData(copyRow);
          const invalidData = DataHandling.validate(copyValue, copyRow);

          if (invalidData !== null) {
            this._showError(invalidData);
          } else {
            const newData = data.map(e => {
              if (e.Id === copyRow.id) {
                return e;
              }

              return Object.assign({}, e, {
                MinBet: copyRow.MinBet,
                MaxBet: copyRow.MaxBet,
                MaxPerMatch: copyRow.MaxPerMatch
              });
            });
            this.setState({
              isCopy: true,
              data: newData
            });
          }
        } else {
          this.setState({
            isCopy: false
          });
        }
      });

      _defineProperty(this, "onCloseAlert", () => {
        this.setState({
          disabledButton: false
        }, () => this.props.setAlertOption({
          isShow: false
        }));
      });

      _defineProperty(this, "onRefresh", () => {
        this._getData(this.props.requestParam);
      });

      this.state = {
        data: null,
        setting: {
          Editable: false,
          EnableCopy: false,
          CopyId: 0
        },
        disabledButton: false,
        isCopy: false,
        showError: false,
        errorBalloon: {
          msg: '',
          targetTop: 0,
          targetLeft: 0
        }
      };
      this.ref = {};
      this.checkBoxRef = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createRef();
      this.errorRef = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createRef();
      this.submitButtonRef = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createRef();
      this.getHeaderName = DataHandling.getHeaderName;
    }

    componentDidUpdate(prevProps, prevState) {
      if (prevState.timeStamp !== this.state.timeStamp) {
        this.props.toggleLoader(false);
        this.props.setAlertOption({
          isShow: false
        });
      }

      if (prevProps.requestParam !== this.props.requestParam) {
        this._getData(this.props.requestParam);
      }
    }

    componentWillUnmount() {
      this._unRegisterEvents();
    }

    componentDidMount() {
      this._getData(this.props.requestParam);

      this._registerEvents();
    }

    _registerEvents() {
      document.addEventListener('click', this._handleClickOutSide);
    }

    _closeError() {
      this.setState({
        showError: false
      });
    }

    _submit(data) {
      this.props.toggleLoader(true);
      const param = Object.assign({}, this.props.requestParam, {
        Setting: data
      });
      _lib_dataTransfer__WEBPACK_IMPORTED_MODULE_6__["default"].updateData(param, this._showSubmitResult.bind(this));
    }

    _showConfirmation(onOK) {
      const buttons = [{
        text: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('OK'),
        onClick: onOK,
        className: 'btn btn-apply'
      }, {
        text: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Cancel'),
        onClick: this.onCloseAlert,
        className: 'btn btn-cancel'
      }];
      const msg = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
        className: "center-align"
      }, _lib_utility__WEBPACK_IMPORTED_MODULE_7__["default"].getConfirmationMsg(this.props.requestParam.ProductId));
      this.setState({
        disabledButton: true
      }, () => this.props.setAlertOption({
        isShow: true,
        title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Confirmation'),
        children: msg,
        buttons: buttons,
        icon: '',
        onClose: this.onCloseAlert,
        height: 130
      }));
    }

    _showSubmitResult(title, msg, isSuccess) {
      const content = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
        className: "center-align"
      }, msg);
      const buttons = [{
        text: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('OK'),
        onClick: this.onRefresh,
        className: 'btn btn-apply btn-alone'
      }];
      this.setState({
        disabledButton: true
      }, () => {
        this.props.setAlertOption({
          isShow: true,
          title: title,
          children: content,
          onClose: this.onRefresh,
          icon: isSuccess ? 'success' : 'error',
          height: 225,
          buttons: buttons
        });
        this.props.toggleLoader(false);
      });
    }

    _showError(invalidData) {
      const invalidInput = this.ref[invalidData.Type][invalidData.Id];

      if (invalidInput) {
        const pos = invalidInput.getBoundingClientRect();
        this.setState({
          showError: true,
          errorBalloon: {
            Id: invalidData.Id,
            msg: invalidData.Message,
            targetTop: pos.top,
            targetLeft: pos.left + pos.width / 2
          }
        }, () => invalidInput.focus());
      }
    }

    _refreshState(data, setting) {
      this.setState(state => ({
        data: DataHandling.getViewData(data),
        setting: Object.assign({}, state.setting, setting),
        disabledButton: false,
        isCopy: false,
        showError: false,
        timeStamp: new Date().getTime()
      }));
    }

    _getData(param) {
      this.props.toggleLoader(true);
      _lib_dataTransfer__WEBPACK_IMPORTED_MODULE_6__["default"].getData(param, this._refreshState.bind(this));
    }

    render() {
      const data = this.state.data;

      if (!data || !data.length) {
        return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_NoDataTitle__WEBPACK_IMPORTED_MODULE_5__["default"], null);
      }

      const {
        setting,
        disabledButton,
        showError,
        errorBalloon,
        isCopy
      } = this.state;
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "pa-betlimit"
      }, setting.EnableCopy && setting.Editable && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_CheckBox__WEBPACK_IMPORTED_MODULE_8__["default"], {
        id: "IsCopy",
        checked: isCopy,
        onChange: this.onCheck,
        ref: this.checkBoxRef,
        label: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource("CopyBetSettingFromSoccer")
      }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Grid, {
        data: data,
        setting: setting,
        setInputRef: this.setInputRef,
        onChange: this.onChange,
        isCopy: isCopy,
        getHeaderName: this.getHeaderName
      }), setting.Editable && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "pa-betlimit-button"
      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", {
        type: "button",
        className: "btn btn-cancel",
        disabled: disabledButton,
        onClick: this.onReset
      }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Reset')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", {
        type: "button",
        className: "btn btn-apply",
        disabled: disabledButton,
        onClick: this.onSubmit,
        ref: this.submitButtonRef
      }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Submit'))), setting.Editable && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_Balloon__WEBPACK_IMPORTED_MODULE_9__["default"], _extends({
        width: 140,
        isOpen: showError,
        ref: this.errorRef
      }, errorBalloon)));
    }

  }, _temp;
};

function mapDispatchToProps(dispatch) {
  const {
    toggleLoader,
    setAlertOption
  } = _actions_index__WEBPACK_IMPORTED_MODULE_3__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    toggleLoader,
    setAlertOption
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
const createSetting = Object(redux__WEBPACK_IMPORTED_MODULE_2__["compose"])(Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps), SettingContent);
/* harmony default export */ __webpack_exports__["default"] = (createSetting);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingGrid.jsx":
/*!***************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingGrid.jsx ***!
  \***************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _lib_utility__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../../lib/utility */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/constants.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }





const SettingGrid = props => {
  const {
    setting,
    data,
    onChange,
    getHeaderName
  } = props;
  const setInputRef = props.setInputRef;

  const onInputChange = (type, id, value) => {
    const newData = data.map(e => {
      if (e.Id === id) {
        const newItem = _objectSpread({}, e);

        newItem[type] = value;
        return newItem;
      }

      return e;
    });
    onChange(newData);
  };

  const onInputBlur = (type, id, value) => {
    const newValue = _lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].getValueAsNumber(value));

    if (newValue !== value) {
      const newData = data.map(e => {
        if (e.Id === id) {
          const newItem = _objectSpread({}, e);

          newItem[type] = newValue;
          return newItem;
        }

        return e;
      });
      onChange(newData);
    }
  };

  const buildCell = (type, value, disabled) => {
    const isChanged = value[type] !== value['Original' + type];
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
      className: isChanged ? _lib_constants__WEBPACK_IMPORTED_MODULE_2__["InputClassName"].OnChangedClassName : _lib_constants__WEBPACK_IMPORTED_MODULE_2__["InputClassName"].ClassName,
      type: "text",
      ref: e => setInputRef(type, value.Id, e),
      autoComplete: "off",
      maxLength: "14",
      value: value[type],
      onChange: e => onInputChange(type, value.Id, e.target.value),
      onBlur: e => onInputBlur(type, value.Id, e.target.value),
      disabled: disabled
    });
  };

  const buildBody = (editable, data) => {
    let rows = [];

    for (let i = 0; i < data.length; i++) {
      const item = data[i];
      const disabled = !editable;
      rows.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tr", {
        key: item.Id
      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", null, item.BetSettingType), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: "pa-betlimit-cell"
      }, buildCell(_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitType"].MinBet, item, disabled), item.BetValueMin > 0 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, ` >= ${_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(item.BetValueMin)}`)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: "pa-betlimit-cell"
      }, buildCell(_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitType"].MaxBet, item, disabled), item.BetValueMax > 0 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, ` <= ${_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(item.BetValueMax)}`)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: "pa-betlimit-cell"
      }, buildCell(_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitType"].MaxPerMatch, item, disabled), item.MaxPerMatchMax > 0 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, ` <= ${_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(item.MaxPerMatchMax)}`))));
    }

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tbody", null, rows);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "pa-betlimit-grid"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("table", {
    className: "table"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("thead", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tr", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, getHeaderName('Sport')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, getHeaderName('MinBet')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, getHeaderName('MaxBet')), data[0].Id == _lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitSport"].TG ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, getHeaderName('MaxBetPerMatch')) : /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, getHeaderName('MaxPerMatch')))), buildBody(setting.Editable, data)));
};

/* harmony default export */ __webpack_exports__["default"] = (/*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(SettingGrid));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/AGCDataContent.jsx":
/*!***************************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/AGCDataContent.jsx ***!
  \***************************************************************************************************************/
/*! exports provided: AGCDataContent, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AGCDataContent", function() { return AGCDataContent; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _actions_index_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../../actions/index.js */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _lib_utility__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../../lib/utility */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js");
/* harmony import */ var _lib_dataTransfer__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../../lib/dataTransfer */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataTransfer.js");
/* harmony import */ var react_select__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! react-select */ "./node_modules/react-select/dist/react-select.es.js");
/* harmony import */ var _sport_settings_AGCSettingGrid__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../sport-settings/AGCSettingGrid */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/sport-settings/AGCSettingGrid.jsx");
/* harmony import */ var _jsroot_components_NoDataTitle__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @jsroot/components/NoDataTitle */ "./www/js/source/reactpages/components/NoDataTitle.jsx");
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }











class AGCDataContent extends react__WEBPACK_IMPORTED_MODULE_0__["PureComponent"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "onGroupChange", item => {
      if (this.state.selectedGroup !== item.value) {
        const {
          data
        } = this.state;

        const disabledButton = this._isDisabledButton(data, item.value);

        this.setState({
          selectedGroup: item.value,
          disabledButton: disabledButton
        });
      }
    });

    _defineProperty(this, "onReset", () => {
      this._getData(this.props.requestParam);
    });

    _defineProperty(this, "onSubmit", () => {
      const {
        selectedGroup,
        data,
        setting
      } = this.state;

      const invalid = this._isDisabledButton(data, selectedGroup);

      if (invalid) {
        this.setState({
          disabledButton: true
        });
      } else {
        this._showConfirmation(() => this._submit(selectedGroup, setting.BetSetting.AgGroup));
      }
    });

    _defineProperty(this, "onCloseAlert", () => {
      this.setState({
        disabledButton: false
      }, () => this.props.setAlertOption({
        isShow: false
      }));
    });

    _defineProperty(this, "onRefresh", () => {
      this._getData(this.props.requestParam);
    });

    this.state = {
      data: null,
      setting: {},
      groups: [],
      selectedGroup: '',
      disabledButton: false
    };
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevState.data !== this.state.data) {
      this.props.toggleLoader(false);
      this.props.setAlertOption({
        isShow: false
      });
    }
  }

  componentDidMount() {
    this._getData(this.props.requestParam);
  }

  _isDisabledButton(data, selectedGroup) {
    if (!selectedGroup) {
      return true;
    }

    const gamesHaveGroup = data.filter(x => x.Groups.find(g => g.GroupType === selectedGroup));
    return !gamesHaveGroup || gamesHaveGroup.length !== data.length;
  }

  _updateState(data, setting) {
    if (setting.BetSetting) {
      const groups = setting.BetSetting ? setting.BetSetting.CheckedGroups.map(x => {
        return {
          value: x,
          label: x
        };
      }) : [];

      const disabledButton = this._isDisabledButton(data, setting.BetSetting.AgGroup);

      this.setState({
        data: data,
        setting: setting,
        groups: groups,
        selectedGroup: setting.BetSetting.AgGroup,
        disabledButton: disabledButton
      });
    }
  }

  _getData(param) {
    this.props.toggleLoader(true);
    _lib_dataTransfer__WEBPACK_IMPORTED_MODULE_6__["default"].getData(param, this._updateState.bind(this));
  }

  _submit(group, oldGroup) {
    this.props.toggleLoader(true);
    const param = Object.assign({}, this.props.requestParam, {
      Setting: {
        group,
        oldGroup
      }
    });
    _lib_dataTransfer__WEBPACK_IMPORTED_MODULE_6__["default"].updateData(param, this._showSubmitResult.bind(this));
  }

  _showConfirmation(onOK) {
    const onClose = () => this.onCloseAlert(false);

    const buttons = [{
      text: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('OK'),
      onClick: onOK,
      className: 'btn btn-apply'
    }, {
      text: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Cancel'),
      onClick: onClose,
      className: 'btn btn-cancel'
    }];
    const msg = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
      className: "center-align"
    }, _lib_utility__WEBPACK_IMPORTED_MODULE_5__["default"].getConfirmationMsg(this.props.requestParam.ProductId));
    this.setState({
      disabledButton: true
    }, () => this.props.setAlertOption({
      isShow: true,
      title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Confirmation'),
      children: msg,
      buttons: buttons,
      icon: '',
      onClose: onClose,
      height: 130
    }));
  }

  _showSubmitResult(title, msg, isSuccess) {
    const content = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
      className: "center-align"
    }, msg);
    const buttons = [{
      text: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('OK'),
      onClick: this.onRefresh,
      className: 'btn btn-apply btn-alone'
    }];
    this.setState({
      disabledButton: true
    }, () => {
      this.props.setAlertOption({
        isShow: true,
        title: title,
        children: content,
        onClose: this.onRefresh,
        icon: isSuccess ? 'success' : 'error',
        height: 225,
        buttons: buttons
      });
      this.props.toggleLoader(false);
    });
  }

  render() {
    const {
      data,
      setting
    } = this.state;

    if (setting.BetSetting && !setting.BetSetting.IsLineEnabled) {
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_NoDataTitle__WEBPACK_IMPORTED_MODULE_9__["default"], {
        title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('AGCasinoNotOfferCurrency')
      });
    }

    if (data === null) {
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_NoDataTitle__WEBPACK_IMPORTED_MODULE_9__["default"], null);
    }

    const {
      groups,
      selectedGroup,
      disabledButton
    } = this.state;
    const hasData = data.length > 0;
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "pa-betlimit"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "pa-betlimit-group row"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "col-12 col-md-4 col-sm-12 col-lg-3 col-xl-2"
    }, setting.BetSetting.IsEnabled && hasData ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "form-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
      className: "label-text"
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('BetSettingGroups')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_select__WEBPACK_IMPORTED_MODULE_7__["default"], {
      instanceId: "ddlGroup",
      value: selectedGroup,
      options: groups,
      onChange: this.onGroupChange
    })) : /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, `${_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('BetSettingGroups')} :`, " ", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
      className: "disabled-text"
    }, setting.BetSetting.AgGroup)))), hasData ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_sport_settings_AGCSettingGrid__WEBPACK_IMPORTED_MODULE_8__["default"], {
      data: data,
      isEnabled: setting.BetSetting.IsEnabled,
      group: selectedGroup
    }) : /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_NoDataTitle__WEBPACK_IMPORTED_MODULE_9__["default"], {
      title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('ProblemDisplayBetSetting')
    }), setting.Editable && hasData && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "pa-betlimit-button"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", {
      type: "button",
      className: "btn btn-cancel",
      disabled: disabledButton,
      onClick: this.onReset
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Reset')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", {
      type: "button",
      className: "btn btn-apply",
      disabled: disabledButton,
      onClick: this.onSubmit
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_4__["default"].getSingleResource('Submit'))));
  }

}

function mapDispatchToProps(dispatch) {
  const {
    toggleLoader,
    setAlertOption
  } = _actions_index_js__WEBPACK_IMPORTED_MODULE_2__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_3__["bindActionCreators"])({
    toggleLoader,
    setAlertOption
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
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(AGCDataContent));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/GameDataContent.jsx":
/*!****************************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/GameDataContent.jsx ***!
  \****************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _lib_gameSettingDataHandling__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../../lib/gameSettingDataHandling */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/gameSettingDataHandling.js");
/* harmony import */ var _base_SettingContent__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../base/SettingContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingContent.jsx");
/* harmony import */ var _base_SettingGrid__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../base/SettingGrid */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingGrid.jsx");




const Content = Object(_base_SettingContent__WEBPACK_IMPORTED_MODULE_2__["default"])(_base_SettingGrid__WEBPACK_IMPORTED_MODULE_3__["default"], new _lib_gameSettingDataHandling__WEBPACK_IMPORTED_MODULE_1__["default"]());

const GameDataContent = () => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Content, null);
};

/* harmony default export */ __webpack_exports__["default"] = (GameDataContent);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/I4DDataContent.jsx":
/*!***************************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/I4DDataContent.jsx ***!
  \***************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _lib_i4DDataHandling__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../../lib/i4DDataHandling */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/i4DDataHandling.js");
/* harmony import */ var _base_SettingContent__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../base/SettingContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingContent.jsx");
/* harmony import */ var _sport_settings_I4DSettingGrid__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../sport-settings/I4DSettingGrid */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/sport-settings/I4DSettingGrid.jsx");




const Content = Object(_base_SettingContent__WEBPACK_IMPORTED_MODULE_2__["default"])(_sport_settings_I4DSettingGrid__WEBPACK_IMPORTED_MODULE_3__["default"], new _lib_i4DDataHandling__WEBPACK_IMPORTED_MODULE_1__["default"]());

const I4DDataContent = () => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Content, null);
};

/* harmony default export */ __webpack_exports__["default"] = (I4DDataContent);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/NGDataContent.jsx":
/*!**************************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/NGDataContent.jsx ***!
  \**************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _lib_ngDataHandling__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../../lib/ngDataHandling */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/ngDataHandling.js");
/* harmony import */ var _base_SettingContent__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../base/SettingContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingContent.jsx");
/* harmony import */ var _sport_settings_NGSettingGrid__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../sport-settings/NGSettingGrid */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/sport-settings/NGSettingGrid.jsx");




const Content = Object(_base_SettingContent__WEBPACK_IMPORTED_MODULE_2__["default"])(_sport_settings_NGSettingGrid__WEBPACK_IMPORTED_MODULE_3__["default"], new _lib_ngDataHandling__WEBPACK_IMPORTED_MODULE_1__["default"]());

const NGDataContent = () => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Content, null);
};

/* harmony default export */ __webpack_exports__["default"] = (NGDataContent);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/SportDataContent.jsx":
/*!*****************************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/data-contents/SportDataContent.jsx ***!
  \*****************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _lib_dataHandling__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../../lib/dataHandling */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataHandling.js");
/* harmony import */ var _base_SettingContent__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../base/SettingContent */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingContent.jsx");
/* harmony import */ var _base_SettingGrid__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../base/SettingGrid */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/base/SettingGrid.jsx");




const Content = Object(_base_SettingContent__WEBPACK_IMPORTED_MODULE_2__["default"])(_base_SettingGrid__WEBPACK_IMPORTED_MODULE_3__["default"], new _lib_dataHandling__WEBPACK_IMPORTED_MODULE_1__["default"]());

const SportDataContent = () => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Content, null);
};

/* harmony default export */ __webpack_exports__["default"] = (SportDataContent);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/sport-settings/AGCSettingGrid.jsx":
/*!****************************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/sport-settings/AGCSettingGrid.jsx ***!
  \****************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _lib_utility__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../../lib/utility */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");




const AGCSettingGrid = props => {
  const {
    data,
    isEnabled,
    group
  } = props;

  const buildBody = () => {
    let rows = [];

    for (let i = 0; i < data.length; i++) {
      const game = data[i];
      const gameGroup = game.Groups.find(x => x.GroupType === group);
      rows.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tr", {
        key: game.GameType
      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", null, game.Name), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: isEnabled ? '' : 'disabled-text'
      }, gameGroup ? _lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(gameGroup.Min) : ''), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: isEnabled ? '' : 'disabled-text'
      }, gameGroup ? _lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(gameGroup.Max) : '')));
    }

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tbody", null, rows);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "pa-betlimit-grid"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("table", {
    className: "table"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("thead", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tr", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('GameType')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('MinBet')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('MaxBet')))), buildBody()));
};

/* harmony default export */ __webpack_exports__["default"] = (/*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(AGCSettingGrid));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/sport-settings/I4DSettingGrid.jsx":
/*!****************************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/sport-settings/I4DSettingGrid.jsx ***!
  \****************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _lib_utility__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../../lib/utility */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/constants.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }





const I4DSettingGrid = props => {
  const {
    setting,
    data,
    onChange,
    getHeaderName
  } = props;
  const setInputRef = props.setInputRef;

  const onInputChange = (type, id, value) => {
    const newData = data.map(e => {
      if (e.BetType === id) {
        const newItem = _objectSpread({}, e);

        newItem[type] = value;
        return newItem;
      }

      return e;
    });
    onChange(newData);
  };

  const onInputBlur = (type, id, value) => {
    const newValue = _lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].getValueAsNumber(value));

    if (newValue !== value) {
      const newData = data.map(e => {
        if (e.BetType === id) {
          const newItem = _objectSpread({}, e);

          newItem[type] = newValue;
          return newItem;
        }

        return e;
      });
      onChange(newData);
    }
  };

  const buildCell = (type, value, disabled) => {
    const isChanged = value[type] !== value['Original' + type];
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
      className: isChanged ? _lib_constants__WEBPACK_IMPORTED_MODULE_2__["InputClassName"].OnChangedClassName : _lib_constants__WEBPACK_IMPORTED_MODULE_2__["InputClassName"].ClassName,
      type: "text",
      ref: e => setInputRef(type, value.BetType, e),
      autoComplete: "off",
      maxLength: "14",
      value: value[type],
      onChange: e => onInputChange(type, value.BetType, e.target.value),
      onBlur: e => onInputBlur(type, value.BetType, e.target.value),
      disabled: disabled
    });
  };

  const buildBody = (editable, data) => {
    let rows = [];

    for (let i = 0; i < data.length; i++) {
      const item = data[i];
      const disabled = !editable;
      rows.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tr", {
        key: item.BetType
      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", null, item.BetSettingType), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: "pa-betlimit-cell"
      }, buildCell(_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitType"].MinBet, item, disabled), item.BetValueMin > 0 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, ` >= ${_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(item.BetValueMin)}`)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: "pa-betlimit-cell"
      }, buildCell(_lib_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitType"].MaxBet, item, disabled), item.BetValueMax > 0 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, ` <= ${_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(item.BetValueMax)}`))));
    }

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tbody", null, rows);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "pa-betlimit-grid"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("table", {
    className: "table"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("thead", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tr", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, getHeaderName('Sport')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, getHeaderName('MinBet')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, getHeaderName('MaxBet')))), buildBody(setting.Editable, data)));
};

/* harmony default export */ __webpack_exports__["default"] = (/*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(I4DSettingGrid));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/sport-settings/NGSettingGrid.jsx":
/*!***************************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/grid/sport-settings/NGSettingGrid.jsx ***!
  \***************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _lib_utility__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../../lib/utility */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/constants.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }






const NGSettingGrid = props => {
  const {
    setting,
    data,
    onChange
  } = props;
  const setInputRef = props.setInputRef;

  const onInputChange = (type, id, value) => {
    const newData = data.map(e => {
      if (e.Id === id) {
        const newItem = _objectSpread({}, e);

        newItem[type] = value;
        return newItem;
      }

      return e;
    });
    onChange(newData);
  };

  const onInputBlur = (type, id, value) => {
    const newValue = _lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].getValueAsNumber(value));

    if (newValue !== value) {
      const newData = data.map(e => {
        if (e.Id === id) {
          const newItem = _objectSpread({}, e);

          newItem[type] = newValue;
          return newItem;
        }

        return e;
      });
      onChange(newData);
    }
  };

  const buildCell = (type, value, disabled) => {
    const isChanged = value[type] !== value['Original' + type];
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
      className: isChanged ? _lib_constants__WEBPACK_IMPORTED_MODULE_3__["InputClassName"].OnChangedClassName : _lib_constants__WEBPACK_IMPORTED_MODULE_3__["InputClassName"].ClassName,
      type: "text",
      ref: e => setInputRef(type, value.Id, e),
      autoComplete: "off",
      maxLength: "14",
      value: value[type],
      onChange: e => onInputChange(type, value.Id, e.target.value),
      onBlur: e => onInputBlur(type, value.Id, e.target.value),
      disabled: disabled
    });
  };

  const buildBody = (editable, data) => {
    let rows = [];

    for (let i = 0; i < data.length; i++) {
      const item = data[i];
      const disabled = !editable;
      rows.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tr", {
        key: item.Id
      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", null, item.BetSettingType), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: "pa-betlimit-cell"
      }, buildCell(_lib_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitType"].MinBet, item, disabled), item.BetValueMin > 0 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, ` >= ${_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(item.BetValueMin)}`)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: "pa-betlimit-cell"
      }, buildCell(_lib_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitType"].MaxBet, item, disabled), item.BetValueMax > 0 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, ` <= ${_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(item.BetValueMax)}`)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: "pa-betlimit-cell"
      }, buildCell(_lib_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitType"].MaxPerMatch, item, disabled), item.MaxPerMatchMax > 0 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, ` <= ${_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(item.MaxPerMatchMax)}`)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("td", {
        className: "pa-betlimit-cell"
      }, buildCell(_lib_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitType"].MaxPerBall, item, disabled), item.MaxPerBallMax > 0 && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, ` <= ${_lib_utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(item.MaxPerBallMax)}`))));
    }

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tbody", null, rows);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "pa-betlimit-grid"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("table", {
    className: "table"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("thead", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("tr", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('Sport')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('MinBet')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('MaxBet')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('MaxPerMatch')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("th", null, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('MaxPerBall')))), buildBody(setting.Editable, data)));
};

/* harmony default export */ __webpack_exports__["default"] = (/*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(NGSettingGrid));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/index.jsx":
/*!***********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/index.jsx ***!
  \***********************************************************************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _AppContainer_jsx__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./AppContainer.jsx */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/AppContainer.jsx");
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _reducers_index_js__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../reducers/index.js */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/reducers/index.js");
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

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/constants.js":
/*!*******************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/constants.js ***!
  \*******************************************************************************/
/*! exports provided: BetLimitProduct, BetLimitSport, BetLimitType, InputClassName */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "BetLimitProduct", function() { return BetLimitProduct; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "BetLimitSport", function() { return BetLimitSport; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "BetLimitType", function() { return BetLimitType; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "InputClassName", function() { return InputClassName; });
const BetLimitProduct = {
  SB: 0,
  VS: 5,
  VS2: 27,
  LT: 28,
  NG: 2,
  KN: 8,
  AGC: 10,
  TG: 40,
  I4D: 43
};
const BetLimitSport = {
  I4D: 223,
  TG: 222
};
const BetLimitType = {
  MinBet: 'MinBet',
  MaxBet: 'MaxBet',
  MaxPerMatch: 'MaxPerMatch',
  MaxPerBall: 'MaxPerBall'
};
const InputClassName = {
  ClassName: 'input-text right-align',
  OnChangedClassName: 'input-text right-align input-changed'
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataHandling.js":
/*!**********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataHandling.js ***!
  \**********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return DataHandling; });
/* harmony import */ var _validatation__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./validatation */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/validatation.js");
/* harmony import */ var _utility__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./utility */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");



class DataHandling {
  getViewData(rawData) {
    return rawData.map(rawRow => {
      return this.getViewRowData(rawRow);
    });
  }

  getViewRowData(rawData) {
    let row = {
      Id: rawData.Id,
      BetType: rawData.BetType,
      BetSettingType: rawData.BetSettingType,
      MinBet: _utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(rawData.MinBet),
      MaxBet: _utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(rawData.MaxBet),
      MaxPerMatch: _utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(rawData.MaxPerMatch),
      BetValueMin: rawData.BetValueMin,
      BetValueMax: rawData.BetValueMax,
      MaxPerMatchMin: rawData.MaxPerMatchMin,
      MaxPerMatchMax: rawData.MaxPerMatchMax,
      OldMinBet: rawData.MinBet,
      OldMaxBet: rawData.MaxBet,
      OldMaxPerMatch: rawData.MaxPerMatch
    };
    row.OriginalMinBet = row.MinBet;
    row.OriginalMaxBet = row.MaxBet;
    row.OriginalMaxPerMatch = row.MaxPerMatch;
    return row;
  }

  getSubmitRowData(row) {
    return {
      Id: row.Id,
      BetType: row.BetType,
      BetSettingType: row.BetSettingType,
      MinBet: _utility__WEBPACK_IMPORTED_MODULE_1__["default"].getValueAsNumber(row.MinBet),
      MaxBet: _utility__WEBPACK_IMPORTED_MODULE_1__["default"].getValueAsNumber(row.MaxBet),
      MaxPerMatch: _utility__WEBPACK_IMPORTED_MODULE_1__["default"].getValueAsNumber(row.MaxPerMatch),
      OldMinBet: row.OldMinBet,
      OldMaxBet: row.OldMaxBet,
      OldMaxPerMatch: row.OldMaxPerMatch,
      IsUpdated: row.MinBet !== row.OriginalMinBet || row.MaxBet !== row.OriginalMaxBet || row.MaxPerMatch !== row.OriginalMaxPerMatch
    };
  }

  getSubmitData(rows) {
    const submitData = [];

    for (let i = 0; i < rows.length; i++) {
      const row = rows[i];
      const currentValue = this.getSubmitRowData(row);
      const invalidData = this.validate(currentValue, row);

      if (invalidData !== null) {
        return {
          IsValid: false,
          Data: invalidData
        };
      }

      submitData.push(currentValue);
    }

    return {
      IsValid: true,
      Data: submitData
    };
  }

  validate(currentValue, row) {
    if (!currentValue.IsUpdated) {
      return null;
    }

    const validateMinMaxBet = _validatation__WEBPACK_IMPORTED_MODULE_0__["default"].validateMinMaxBet(currentValue, row);

    if (validateMinMaxBet !== null) {
      return validateMinMaxBet;
    }

    return _validatation__WEBPACK_IMPORTED_MODULE_0__["default"].validateMaxPerMatch(currentValue, row);
  }

  getHeaderName(header) {
    return _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource(header);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataTransfer.js":
/*!**********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataTransfer.js ***!
  \**********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return DataTransfer; });
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/DataService */ "./www/js/source/reactpages/lib/DataService.js");
/* harmony import */ var _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/lib/site */ "./www/js/source/reactpages/lib/site.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/constants.js");
function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }





const Url = {
  [_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitProduct"].SB]: 'SportsBookBetLimit',
  [_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitProduct"].VS]: 'DcsBetLimit',
  [_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitProduct"].VS2]: 'DcsBetLimit',
  [_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitProduct"].LT]: 'DcsBetLimit',
  [_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitProduct"].NG]: 'NumberGameBetLimit',
  [_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitProduct"].KN]: 'RngKenoBetLimit',
  [_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitProduct"].AGC]: 'AGCasinoBetLimit',
  [_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitProduct"].TG]: 'DcsBetLimit',
  [_constants__WEBPACK_IMPORTED_MODULE_3__["BetLimitProduct"].I4D]: 'DcsBetLimit'
};
class DataTransfer {
  static getData(param, callBack) {
    const url = _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_2__["default"].resolveClientUrl(Url[param.ProductId] + '/GetSetting');
    _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_1__["default"].postData(url, param).then(response => {
      const _response$data = response.data,
            {
        Data
      } = _response$data,
            setting = _objectWithoutProperties(_response$data, ["Data"]);

      callBack && callBack(Data, setting);
    }).catch(error => {
      console.log(error.message);
      callBack && callBack([], {});
    });
  }

  static updateData(param, callBack) {
    const url = _jsroot_lib_site__WEBPACK_IMPORTED_MODULE_2__["default"].resolveClientUrl(Url[param.ProductId] + '/UpdateSetting');
    _jsroot_lib_DataService__WEBPACK_IMPORTED_MODULE_1__["default"].postData(url, param).then(response => {
      const result = response.data;
      const isSuccess = Number(result.Code) === 0;
      const title = isSuccess ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MessageSuccess') : _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('Fail');
      callBack && callBack(title, result.Message, isSuccess);
    }).catch(error => {
      console.log(error.message);
      callBack(_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('Error'), _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('ErrorProcessingMsg'), false);
    });
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/gameSettingDataHandling.js":
/*!*********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/gameSettingDataHandling.js ***!
  \*********************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return GameSettingDataHandling; });
/* harmony import */ var _validatation__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./validatation */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/validatation.js");
/* harmony import */ var _dataHandling__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./dataHandling */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataHandling.js");


class GameSettingDataHandling extends _dataHandling__WEBPACK_IMPORTED_MODULE_1__["default"] {
  validate(currentValue, row) {
    if (!currentValue.IsUpdated) {
      return null;
    }

    const validateMinMaxBet = _validatation__WEBPACK_IMPORTED_MODULE_0__["default"].validateMinMaxBet(currentValue, row);

    if (validateMinMaxBet !== null) {
      return validateMinMaxBet;
    }

    return _validatation__WEBPACK_IMPORTED_MODULE_0__["default"].validateMaxBetPerDraw(currentValue, row);
  }

  getHeaderName(header) {
    if (header === 'Sport') {
      return super.getHeaderName('GameType');
    }

    if (header === 'MaxPerMatch') {
      return super.getHeaderName('MaxBetPerDraw');
    }

    return super.getHeaderName(header);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/i4DDataHandling.js":
/*!*************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/i4DDataHandling.js ***!
  \*************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return I4DDataHandling; });
/* harmony import */ var _validatation__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./validatation */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/validatation.js");
/* harmony import */ var _dataHandling__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./dataHandling */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataHandling.js");
/* harmony import */ var _utility__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./utility */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js");



class I4DDataHandling extends _dataHandling__WEBPACK_IMPORTED_MODULE_1__["default"] {
  validate(currentValue, row) {
    if (!currentValue.IsUpdated) {
      return null;
    }

    return _validatation__WEBPACK_IMPORTED_MODULE_0__["default"].validateMinMaxBet(currentValue, row);
  }

  getSubmitRowData(row) {
    return {
      Id: row.Id,
      BetType: row.BetType,
      BetSettingType: row.BetSettingType,
      MinBet: _utility__WEBPACK_IMPORTED_MODULE_2__["default"].getValueAsNumber(row.MinBet),
      MaxBet: _utility__WEBPACK_IMPORTED_MODULE_2__["default"].getValueAsNumber(row.MaxBet),
      MaxPerMatch: _utility__WEBPACK_IMPORTED_MODULE_2__["default"].getValueAsNumber(row.MaxBet),
      OldMinBet: row.OldMinBet,
      OldMaxBet: row.OldMaxBet,
      OldMaxPerMatch: row.OldMaxPerMatch,
      IsUpdated: row.MinBet !== row.OriginalMinBet || row.MaxBet !== row.OriginalMaxBet
    };
  }

  getHeaderName(header) {
    if (header === 'Sport') {
      return super.getHeaderName('GameType');
    }

    return super.getHeaderName(header);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/ngDataHandling.js":
/*!************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/ngDataHandling.js ***!
  \************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return NGDataHandling; });
/* harmony import */ var _validatation__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./validatation */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/validatation.js");
/* harmony import */ var _utility__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./utility */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js");
/* harmony import */ var _dataHandling__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./dataHandling */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/dataHandling.js");



class NGDataHandling extends _dataHandling__WEBPACK_IMPORTED_MODULE_2__["default"] {
  getViewRowData(rawData) {
    let row = super.getViewRowData(rawData);
    row.MaxPerBall = _utility__WEBPACK_IMPORTED_MODULE_1__["default"].formatValue(rawData.MaxPerBall);
    row.OldMaxPerBall = rawData.MaxPerBall;
    row.MaxPerBallMax = rawData.MaxPerBallMax;
    row.OriginalMaxPerBall = row.MaxPerBall;
    return row;
  }

  getSubmitRowData(row) {
    let data = super.getSubmitRowData(row);
    data.MaxPerBall = _utility__WEBPACK_IMPORTED_MODULE_1__["default"].getValueAsNumber(row.MaxPerBall);
    data.OldMaxPerBall = row.OldMaxPerBall;
    data.IsUpdated = data.IsUpdated || row.MaxPerBall !== row.OriginalMaxPerBall;
    return data;
  }

  getSubmitData(rows) {
    const submitData = [];

    for (let i = 0; i < rows.length; i++) {
      const row = rows[i];
      const currentValue = this.getSubmitRowData(row);
      const invalidData = this.validate(currentValue, row);

      if (invalidData !== null) {
        return {
          IsValid: false,
          Data: invalidData
        };
      }

      submitData.push(currentValue);
    }

    return {
      IsValid: true,
      Data: submitData
    };
  }

  validate(currentValue, row) {
    const validatationResult = super.validate(currentValue, row);

    if (validatationResult !== null) {
      return validatationResult;
    }

    return _validatation__WEBPACK_IMPORTED_MODULE_0__["default"].validateMaxPerBall(currentValue, row);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js":
/*!*****************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/utility.js ***!
  \*****************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return Utility; });
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/numberFormat */ "./www/js/source/reactpages/lib/numberFormat.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/constants.js");



const ConfirmationMsg = {
  [_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].SB]: {
    Msg: 'UpdateSportsbookBetSettings'
  },
  [_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].VS]: {
    Msg: 'UpdateProductBetSettings',
    Product: 'ProductVS'
  },
  [_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].VS2]: {
    Msg: 'UpdateProductBetSettings',
    Product: 'ProductVS2'
  },
  [_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].LT]: {
    Msg: 'UpdateProductBetSettings',
    Product: 'ProductLT'
  },
  [_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].NG]: {
    Msg: 'UpdateNumberGameBetSettings'
  },
  [_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].KN]: {
    Msg: 'UpdateProductBetSettings',
    Product: 'ProductKN'
  },
  [_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].AGC]: {
    Msg: 'UpdateAgCasinoBetSettings'
  },
  [_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].TG]: {
    Msg: 'UpdateProductBetSettings',
    Product: 'ProductTG'
  },
  [_constants__WEBPACK_IMPORTED_MODULE_2__["BetLimitProduct"].I4D]: {
    Msg: 'UpdateProductBetSettings',
    Product: 'ProductI4D'
  }
};
class Utility {
  static getConfirmationMsg(productId) {
    const message = _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource(ConfirmationMsg[productId].Msg);

    if (!ConfirmationMsg[productId].Product) {
      return message;
    }

    return message.replace('{0}', _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource(ConfirmationMsg[productId].Product));
  }

  static formatValue(number) {
    return _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_1__["default"].parseToString(number);
  }

  static getValueAsNumber(text) {
    const number = parseFloat(text.replace(/,/g, ''));
    return !isNaN(number) && isFinite(number) ? _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_1__["default"].round(number, 2) : NaN;
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/validatation.js":
/*!**********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/validatation.js ***!
  \**********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return Validatation; });
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/lib/constants.js");


class Validatation {
  static validateMinMaxBet(current, limitData) {
    const {
      MinBet,
      MaxBet,
      MaxPerMatch
    } = current;
    const Id = current.Id === _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitSport"].I4D ? current.Id : current.BetType;

    if (isNaN(MinBet)) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MinBet,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('IncorrectMinBet')
      };
    }

    if (isNaN(MaxBet)) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxBet,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('IncorrectMaxBet')
      };
    }

    if (limitData.BetValueMin > 0 && MinBet < limitData.BetValueMin) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MinBet,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MinBetGreaterThanOrEqual') + ' ' + limitData.BetValueMin
      };
    }

    if (limitData.BetValueMax > 0 && MaxBet > limitData.BetValueMax) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxBet,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MaxBetLessThanOrEqual') + ' ' + limitData.BetValueMax
      };
    }

    if (MinBet > MaxBet) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MinBet,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MinBetCanNotGreaterThanMaxBet') + ' ' + MaxBet
      };
    }

    return null;
  }

  static validateMaxPerMatch(current, limitData) {
    const {
      MaxBet,
      MaxPerMatch,
      Id
    } = current;

    if (isNaN(MaxPerMatch)) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxPerMatch,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('IncorrectMaxPerMatch')
      };
    }

    if (limitData.MaxPerMatchMax > 0 && MaxPerMatch > limitData.MaxPerMatchMax) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxPerMatch,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MaxBetMatchMoreThanOrEqual') + ' ' + limitData.MaxPerMatchMax
      };
    }

    if (limitData.MaxPerMatchMin > 0 && MaxPerMatch < limitData.MaxPerMatchMin) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxPerMatch,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MaxBetMatchLessThanOrEqual') + ' ' + limitData.MaxPerMatchMin
      };
    }

    if (MaxBet > MaxPerMatch) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxBet,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MaxBetCanNotGreaterThanMaxPerMatch') + ' ' + MaxPerMatch
      };
    }

    return null;
  }

  static validateMaxBetPerDraw(current, limitData) {
    const {
      MaxBet,
      MaxPerMatch,
      Id
    } = current;

    if (isNaN(MaxPerMatch)) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxPerMatch,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('IncorrectMaxBetPerDraw')
      };
    }

    if (limitData.MaxPerMatchMax > 0 && MaxPerMatch > limitData.MaxPerMatchMax) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxPerMatch,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MaxBetPerDrawMoreThanOrEqual') + ' ' + limitData.MaxPerMatchMax
      };
    }

    if (limitData.MaxPerMatchMin > 0 && MaxPerMatch < limitData.MaxPerMatchMin) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxPerMatch,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MaxBetPerDrawLessThanOrEqual') + ' ' + limitData.MaxPerMatchMin
      };
    }

    if (MaxBet > MaxPerMatch) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxBet,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MaxBetCanNotGreaterThanMaxBetPerDraw') + ' ' + MaxPerMatch
      };
    }

    return null;
  }

  static validateMaxPerBall(current, limitData) {
    const {
      MaxPerBall,
      Id
    } = current;

    if (isNaN(MaxPerBall)) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxPerBall,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('IncorrectMaxPerBall')
      };
    }

    if (limitData.MaxPerBallMax > 0 && MaxPerBall > limitData.MaxPerBallMax) {
      return {
        Id: Id,
        Type: _constants__WEBPACK_IMPORTED_MODULE_1__["BetLimitType"].MaxPerBall,
        Message: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MaxPerBallLessThanOrEqual') + ' ' + limitData.MaxPerBallMax
      };
    }

    return null;
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/reducers/alertReducer.js":
/*!***************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/reducers/alertReducer.js ***!
  \***************************************************************************************/
/*! exports provided: alertOption */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "alertOption", function() { return alertOption; });
/* harmony import */ var _actions_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/actions/types.js");
/* harmony import */ var _jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");


const alertOption = Object(_jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_1__["default"])({}, {
  [_actions_types__WEBPACK_IMPORTED_MODULE_0__["SET_ALERT_OPTION"]](state, action) {
    return Object.assign({}, state, action.data);
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/reducers/index.js":
/*!********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/betlimit/reducers/index.js ***!
  \********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _jsroot_reducers_requestParamReducer__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/reducers/requestParamReducer */ "./www/js/source/reactpages/reducers/requestParamReducer.js");
/* harmony import */ var _jsroot_reducers_loaderReducer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/reducers/loaderReducer */ "./www/js/source/reactpages/reducers/loaderReducer.js");
/* harmony import */ var _alertReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./alertReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/reducers/alertReducer.js");




/* harmony default export */ __webpack_exports__["default"] = (Object(redux__WEBPACK_IMPORTED_MODULE_0__["combineReducers"])(Object.assign({}, _jsroot_reducers_loaderReducer__WEBPACK_IMPORTED_MODULE_2__, _jsroot_reducers_requestParamReducer__WEBPACK_IMPORTED_MODULE_1__, _alertReducer__WEBPACK_IMPORTED_MODULE_3__)));

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

/***/ "./www/js/source/reactpages/components/Balloon.jsx":
/*!*********************************************************!*\
  !*** ./www/js/source/reactpages/components/Balloon.jsx ***!
  \*********************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _Overlay__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./Overlay */ "./www/js/source/reactpages/components/Overlay.jsx");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_2__);
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }





class Balloon extends react__WEBPACK_IMPORTED_MODULE_0__["PureComponent"] {
  constructor(props) {
    super(props);
    this.box = props.forwardedRef;
    this.state = {
      top: 0,
      left: 0
    };
  }

  componentDidUpdate(prevProps) {
    const {
      targetTop,
      targetLeft,
      width
    } = this.props;

    if (!prevProps.isOpen && this.props.isOpen && (prevProps.targetTop !== targetTop || prevProps.targetLeft !== targetLeft || prevProps.msg !== this.props.msg)) {
      this.setState({
        left: targetLeft - width / 2,
        top: targetTop - (this.box.current ? this.box.current.clientHeight : 0) - 20
      });
    }
  }

  render() {
    const {
      msg,
      targetTop,
      targetLeft,
      width,
      isOpen
    } = this.props;
    const {
      top,
      left
    } = this.state;
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "balloon",
      style: {
        top: top,
        left: left,
        display: isOpen ? 'block' : 'none'
      },
      ref: this.box
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "balloon-msg",
      style: {
        width: width
      }
    }, msg), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "balloon-arrow-border",
      style: {
        left: (width - 10) / 2
      }
    }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "balloon-arrow",
      style: {
        left: (width - 10) / 2
      }
    }));
  }

}

_defineProperty(Balloon, "propTypes", {
  isOpen: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.bool,
  msg: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string,
  targetTop: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.number,
  targetLeft: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.number,
  width: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.number
});

_defineProperty(Balloon, "defaultProps", {
  isOpen: false,
  msg: '',
  targetTop: 0,
  targetLeft: 0,
  width: 0
});

const balloonWithRef = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.forwardRef((props, ref) => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Balloon, _extends({}, props, {
    forwardedRef: ref
  }));
});
/* harmony default export */ __webpack_exports__["default"] = (balloonWithRef);

/***/ }),

/***/ "./www/js/source/reactpages/components/CheckBox.jsx":
/*!**********************************************************!*\
  !*** ./www/js/source/reactpages/components/CheckBox.jsx ***!
  \**********************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);


const CheckBox = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.forwardRef((props, ref) => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "checkbox-group",
    ref: ref
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
    type: "checkbox",
    className: "checkbox",
    id: props.id,
    checked: props.checked,
    disabled: props.disabled,
    onChange: () => props.onChange(!props.checked)
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
    htmlFor: props.id
  }, props.label));
});
CheckBox.propTypes = {
  id: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string.isRequired,
  label: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  checked: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  disabled: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  onChange: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func
};
CheckBox.defaultProps = {
  label: '',
  checked: true,
  disabled: false,
  onChange: function () {}
};
/* harmony default export */ __webpack_exports__["default"] = (CheckBox);

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

/***/ "./www/js/source/reactpages/components/Overlay.jsx":
/*!*********************************************************!*\
  !*** ./www/js/source/reactpages/components/Overlay.jsx ***!
  \*********************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return Overlay; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(classnames__WEBPACK_IMPORTED_MODULE_2__);
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }




const wrapperClasses = {
  OVERLAY: "overlay",
  OVERLAY_OPEN: "overlay-open"
};
class Overlay extends react__WEBPACK_IMPORTED_MODULE_0__["Component"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "_unRegisterEvents", () => {
      const {
        closeOnOutsideClick
      } = this.props;

      if (closeOnOutsideClick) {
        document.removeEventListener('click', this._handleClickOutSide);
      }
    });

    _defineProperty(this, "_handleClickOutSide", event => {
      const clickOutside = this.containerElement && !this.containerElement.contains(event.target);

      if (clickOutside && this.state.isOpening) {
        this._close();
      }
    });

    this.containerElement = null;
    this.refHandlers = {
      container: ref => {
        this.containerElement = ref;
      }
    };
    this.state = {
      isOpening: this.props.isOpening
    };
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.props.isOpening !== prevProps.isOpening) {
      const {
        onOpen
      } = this.props;
      this.props.isOpening && onOpen && onOpen();
      this.setState({
        isOpening: this.props.isOpening
      });
      window.setTimeout(() => {
        if (this.props.isOpening) {
          this._registerEvents();
        } else {
          this._unRegisterEvents();
        }
      }, 0);
    }
  }

  componentWillUnmount() {
    this._unRegisterEvents();
  }

  _registerEvents() {
    const {
      closeOnOutsideClick
    } = this.props;

    if (closeOnOutsideClick) {
      document.addEventListener('click', this._handleClickOutSide);
    }
  }

  _close() {
    const {
      onClose
    } = this.props;
    this.setState({
      isOpening: false
    }, () => onClose && onClose());
  }

  render() {
    const {
      className,
      children
    } = this.props;
    const mergedClassName = classnames__WEBPACK_IMPORTED_MODULE_2___default()(wrapperClasses.OVERLAY, {
      [wrapperClasses.OVERLAY_OPEN]: this.state.isOpening
    }, className);
    const style = {
      display: this.state.isOpening ? "block" : "none"
    };
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: mergedClassName,
      style: style,
      ref: this.refHandlers.container
    }, children);
  }

}

_defineProperty(Overlay, "propTypes", {
  closeOnOutsideClick: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  isOpening: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  onOpen: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onClose: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func
});

_defineProperty(Overlay, "defaultProps", {
  closeOnOutsideClick: true,
  isOpening: false
});

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

/***/ "./www/js/source/reactpages/components/sweet-alert/icon.js":
/*!*****************************************************************!*\
  !*** ./www/js/source/reactpages/components/sweet-alert/icon.js ***!
  \*****************************************************************/
/*! exports provided: ICON, errorIconMarkup, warningIconMarkup, successIconMarkup */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ICON", function() { return ICON; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "errorIconMarkup", function() { return errorIconMarkup; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "warningIconMarkup", function() { return warningIconMarkup; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "successIconMarkup", function() { return successIconMarkup; });
const ICON = 'swal-icon';
const errorIconMarkup = () => {
  const icon = `${ICON}--error`;
  const line = `${icon}__line`;
  const markup = `
    <div class="${icon}__x-mark">
      <span class="${line} ${line}--left"></span>
      <span class="${line} ${line}--right"></span>
    </div>
  `;
  return markup;
};
const warningIconMarkup = () => {
  const icon = `${ICON}--warning`;
  return `
    <span class="${icon}__body">
      <span class="${icon}__dot"></span>
    </span>
  `;
};
const successIconMarkup = () => {
  const icon = `${ICON}--success`;
  return `
    <span class="${icon}__line ${icon}__line--long"></span>
    <span class="${icon}__line ${icon}__line--tip"></span>

    <div class="${icon}__ring"></div>
    <div class="${icon}__hide-corners"></div>
  `;
};

/***/ }),

/***/ "./www/js/source/reactpages/components/sweet-alert/index.jsx":
/*!*******************************************************************!*\
  !*** ./www/js/source/reactpages/components/sweet-alert/index.jsx ***!
  \*******************************************************************/
/*! exports provided: SweetAlert, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SweetAlert", function() { return SweetAlert; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _popup__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../popup */ "./www/js/source/reactpages/components/popup/index.jsx");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _icon__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./icon */ "./www/js/source/reactpages/components/sweet-alert/icon.js");
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }





const PREDEFINED_ICONS = ["error", "warning", "success", "info"];
const ICON_CONTENTS = {
  error: Object(_icon__WEBPACK_IMPORTED_MODULE_3__["errorIconMarkup"])(),
  warning: Object(_icon__WEBPACK_IMPORTED_MODULE_3__["warningIconMarkup"])(),
  success: Object(_icon__WEBPACK_IMPORTED_MODULE_3__["successIconMarkup"])()
};
class SweetAlert extends react__WEBPACK_IMPORTED_MODULE_0___default.a.Component {
  constructor(props) {
    super(props);
  }

  _buildButtonGroup(buttons) {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "al-button-container"
    }, buttons.map((btnItem, index) => {
      const {
        text
      } = btnItem,
            attrs = _objectWithoutProperties(btnItem, ["text"]);

      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", _extends({
        key: index,
        type: "button"
      }, attrs), text);
    }));
  }

  _buildIcon() {
    const {
      icon
    } = this.props;
    const className = `${_icon__WEBPACK_IMPORTED_MODULE_3__["ICON"]} ${_icon__WEBPACK_IMPORTED_MODULE_3__["ICON"]}--${icon}`;

    if (PREDEFINED_ICONS.includes(icon)) {
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: className,
        dangerouslySetInnerHTML: {
          __html: ICON_CONTENTS[icon]
        }
      });
    }

    return null;
  }

  render() {
    const _this$props = this.props,
          {
      buttons,
      children
    } = _this$props,
          props = _objectWithoutProperties(_this$props, ["buttons", "children"]);

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "sweet-alert"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_popup__WEBPACK_IMPORTED_MODULE_1__["default"], props, this._buildIcon(), children, this._buildButtonGroup(buttons)));
  }

}
SweetAlert.propTypes = {
  width: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.oneOfType([prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.number, prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string]),
  height: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.oneOfType([prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.number, prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string]),
  title: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string,
  isShow: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.bool,
  onClose: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.func,
  isHtml: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.bool,
  buttons: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.arrayOf(prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.object)
};
SweetAlert.defaultProps = {
  width: 400,
  height: 200,
  isShow: false,
  isHtml: false,
  title: '',
  buttons: []
};
/* harmony default export */ __webpack_exports__["default"] = (SweetAlert);

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

/***/ "./www/js/source/reactpages/lib/numberFormat.js":
/*!******************************************************!*\
  !*** ./www/js/source/reactpages/lib/numberFormat.js ***!
  \******************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return NumberFormat; });
class NumberFormat {
  static formatDecimal(value) {
    return this.format(value, 2);
  }

  static formatInteger(value) {
    if (typeof value !== "number" || isNaN(value)) {
      return "";
    }

    return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
  }

  static format(value, digits = 0) {
    if (typeof value !== "number" || isNaN(value)) {
      return "";
    }

    const roundedValue = this.round(value, digits);
    return roundedValue.toFixed(digits).replace(/(\d)(?=(\d{3})+(\.|$))/g, "$1,");
  }

  static round(value, digits) {
    const sign = value < 0 ? -1 : 1;
    const absoluteValue = Math.abs(value);
    const rounded = Number(Math.round(absoluteValue + 'e' + digits) + 'e-' + digits);
    return sign * rounded;
  }

  static parseToString(number) {
    return number.toString().replace(/(\d)(?=(\d{3})+(\.|$))/g, "$1,");
  }

  static roundDownFormatDecimal(value) {
    return this.roundDownFormat(value, 2);
  }

  static roundDownFormat(value, digits = 0) {
    if (typeof value !== "number" || isNaN(value)) {
      return "";
    }

    const roundedValue = this.roundDown(value, digits);
    return roundedValue.toFixed(digits).replace(/(\d)(?=(\d{3})+(\.|$))/g, "$1,");
  }

  static roundDown(value, digits) {
    const sign = value < 0 ? -1 : 1;
    const absoluteValue = Math.abs(value);
    const rounded = Number(Math.floor(absoluteValue + 'e' + digits) + 'e-' + digits);
    return sign * rounded;
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
//# sourceMappingURL=betlimit.chunk.js.map