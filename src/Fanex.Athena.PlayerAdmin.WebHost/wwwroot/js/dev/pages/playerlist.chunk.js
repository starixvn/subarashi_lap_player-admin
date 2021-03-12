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
/******/ 		"www/js/build/dev/reactpages/playeradmin/playerlist": 0
/******/ 	};
/******/
/******/ 	var deferredModules = [];
/******/
/******/ 	// script path function
/******/ 	function jsonpScriptSrc(chunkId) {
/******/ 		return __webpack_require__.p + "www/js/build/dev/reactpages/" + ({"lib/xlsx-style-sparta":"lib/xlsx-style-sparta"}[chunkId]||chunkId) + ".chunk.js"
/******/ 	}
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
/******/ 	// This file contains only the entry chunk.
/******/ 	// The chunk loading function for additional chunks
/******/ 	__webpack_require__.e = function requireEnsure(chunkId) {
/******/ 		var promises = [];
/******/
/******/
/******/ 		// JSONP chunk loading for javascript
/******/
/******/ 		var installedChunkData = installedChunks[chunkId];
/******/ 		if(installedChunkData !== 0) { // 0 means "already installed".
/******/
/******/ 			// a Promise means "currently loading".
/******/ 			if(installedChunkData) {
/******/ 				promises.push(installedChunkData[2]);
/******/ 			} else {
/******/ 				// setup Promise in chunk cache
/******/ 				var promise = new Promise(function(resolve, reject) {
/******/ 					installedChunkData = installedChunks[chunkId] = [resolve, reject];
/******/ 				});
/******/ 				promises.push(installedChunkData[2] = promise);
/******/
/******/ 				// start chunk loading
/******/ 				var script = document.createElement('script');
/******/ 				var onScriptComplete;
/******/
/******/ 				script.charset = 'utf-8';
/******/ 				script.timeout = 120;
/******/ 				if (__webpack_require__.nc) {
/******/ 					script.setAttribute("nonce", __webpack_require__.nc);
/******/ 				}
/******/ 				script.src = jsonpScriptSrc(chunkId);
/******/
/******/ 				// create error before stack unwound to get useful stacktrace later
/******/ 				var error = new Error();
/******/ 				onScriptComplete = function (event) {
/******/ 					// avoid mem leaks in IE.
/******/ 					script.onerror = script.onload = null;
/******/ 					clearTimeout(timeout);
/******/ 					var chunk = installedChunks[chunkId];
/******/ 					if(chunk !== 0) {
/******/ 						if(chunk) {
/******/ 							var errorType = event && (event.type === 'load' ? 'missing' : event.type);
/******/ 							var realSrc = event && event.target && event.target.src;
/******/ 							error.message = 'Loading chunk ' + chunkId + ' failed.\n(' + errorType + ': ' + realSrc + ')';
/******/ 							error.name = 'ChunkLoadError';
/******/ 							error.type = errorType;
/******/ 							error.request = realSrc;
/******/ 							chunk[1](error);
/******/ 						}
/******/ 						installedChunks[chunkId] = undefined;
/******/ 					}
/******/ 				};
/******/ 				var timeout = setTimeout(function(){
/******/ 					onScriptComplete({ type: 'timeout', target: script });
/******/ 				}, 120000);
/******/ 				script.onerror = script.onload = onScriptComplete;
/******/ 				document.head.appendChild(script);
/******/ 			}
/******/ 		}
/******/ 		return Promise.all(promises);
/******/ 	};
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
/******/ 	// on error function for async loading
/******/ 	__webpack_require__.oe = function(err) { console.error(err); throw err; };
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
/******/ 	deferredModules.push(["./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/index.jsx","lib/common"]);
/******/ 	// run deferred modules when ready
/******/ 	return checkDeferredModules();
/******/ })
/************************************************************************/
/******/ ({

/***/ "./cptable":
/*!**************************!*\
  !*** external "cptable" ***!
  \**************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = cptable;

/***/ }),

/***/ "./jszip":
/*!************************!*\
  !*** external "jszip" ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = jszip;

/***/ }),

/***/ "./ods":
/*!**********************!*\
  !*** external "ods" ***!
  \**********************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ods;

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/commonAction.js":
/*!****************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/commonAction.js ***!
  \****************************************************************************************/
/*! exports provided: setSelectedPlayer */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setSelectedPlayer", function() { return setSelectedPlayer; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");

const setSelectedPlayer = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_SELECTED_PLAYER"],
    data
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/filterHandlerAction.js":
/*!***********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/filterHandlerAction.js ***!
  \***********************************************************************************************/
/*! exports provided: searchPlayer, getSinglePlayer, setParam, setAdvancedSearch, setCollapseFilter, getSuggestionList */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "searchPlayer", function() { return searchPlayer; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getSinglePlayer", function() { return getSinglePlayer; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setParam", function() { return setParam; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setAdvancedSearch", function() { return setAdvancedSearch; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setCollapseFilter", function() { return setCollapseFilter; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getSuggestionList", function() { return getSuggestionList; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
/* harmony import */ var _jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");
/* harmony import */ var _lib_services_playerQueryService__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../lib/services/playerQueryService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerQueryService.js");
/* harmony import */ var _commonAction__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./commonAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/commonAction.js");
/* harmony import */ var _gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./gridHandlerAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridHandlerAction.js");
/* harmony import */ var _lib_constants_js__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../lib/constants.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../lib/helpers/playerDataBuilder */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerDataBuilder.js");
/* harmony import */ var _lib_services_playerSuggestionService__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../lib/services/playerSuggestionService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerSuggestionService.js");
/* harmony import */ var _playerDetailHandlerAction__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./playerDetailHandlerAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/playerDetailHandlerAction.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }











const processBeforeGettingData = dispatch => {
  dispatch(Object(_commonAction__WEBPACK_IMPORTED_MODULE_3__["setSelectedPlayer"])(null));
  dispatch(Object(_gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__["setIsFetchingData"])(true));
  dispatch(Object(_jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_1__["toggleLoader"])(true));
  dispatch(Object(_playerDetailHandlerAction__WEBPACK_IMPORTED_MODULE_8__["setSelectedTabName"])(null));
  dispatch(Object(_playerDetailHandlerAction__WEBPACK_IMPORTED_MODULE_8__["setSelectedSubTabName"])(null));
};

const searchPlayer = param => dispatch => {
  const service = new _lib_services_playerQueryService__WEBPACK_IMPORTED_MODULE_2__["default"]();
  processBeforeGettingData(dispatch);
  dispatch(setParam(param));
  service.getPlayerList(param).then(result => {
    const {
      playerList,
      totalItem
    } = result;
    dispatch(Object(_gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__["setPlayerList"])(playerList));
    dispatch(Object(_gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__["setIsFetchingData"])(false));

    const pagingOption = _objectSpread(_objectSpread({}, _lib_constants_js__WEBPACK_IMPORTED_MODULE_5__["defaultPagingOption"]), {}, {
      PageIndex: param.pageIndex,
      PageSize: param.pageSize,
      TotalItem: totalItem
    });

    dispatch(Object(_gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__["setPagingOption"])(pagingOption));

    if (playerList.length === 1) {
      const selectedPlayer = Object(_lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_6__["buildSelectedPlayer"])(playerList[0]);
      dispatch(Object(_commonAction__WEBPACK_IMPORTED_MODULE_3__["setSelectedPlayer"])(selectedPlayer));
    }

    dispatch(Object(_jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_1__["toggleLoader"])(false));
  }).catch(() => {
    dispatch(Object(_jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_1__["toggleLoader"])(false));
  });
};
const getSinglePlayer = param => dispatch => {
  const service = new _lib_services_playerQueryService__WEBPACK_IMPORTED_MODULE_2__["default"]();
  processBeforeGettingData(dispatch);
  service.getSinglePlayer(param).then(result => {
    if (result) {
      dispatch(Object(_gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__["setPlayerList"])([result]));
      dispatch(Object(_gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__["setIsFetchingData"])(false));

      const pagingOption = _objectSpread(_objectSpread({}, _lib_constants_js__WEBPACK_IMPORTED_MODULE_5__["defaultPagingOption"]), {}, {
        TotalItem: 1
      });

      dispatch(Object(_gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__["setPagingOption"])(pagingOption));
      const selectedPlayer = Object(_lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_6__["buildSelectedPlayer"])(result);
      dispatch(Object(_commonAction__WEBPACK_IMPORTED_MODULE_3__["setSelectedPlayer"])(selectedPlayer));
    }

    dispatch(Object(_jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_1__["toggleLoader"])(false));
  }).catch(() => {
    dispatch(Object(_jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_1__["toggleLoader"])(false));
  });
};
const setParam = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_PARAM"],
    data
  };
};
const setAdvancedSearch = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_ADVANCED_SEARCH"],
    data
  };
};
const setCollapseFilter = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_COLLAPSE_FILTER"],
    data
  };
};
const getSuggestionList = param => () => {
  const suggestionService = new _lib_services_playerSuggestionService__WEBPACK_IMPORTED_MODULE_7__["default"]();
  return suggestionService.getSuggestionList(param);
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridBaStatusAction.js":
/*!**********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridBaStatusAction.js ***!
  \**********************************************************************************************/
/*! exports provided: updateBaStatus */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "updateBaStatus", function() { return updateBaStatus; });
/* harmony import */ var _lib_services_playerCommandService__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../lib/services/playerCommandService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerCommandService.js");

const updateBaStatus = param => () => {
  const commandService = new _lib_services_playerCommandService__WEBPACK_IMPORTED_MODULE_0__["default"]();
  return commandService.updateBaStatus(param);
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridHandlerAction.js":
/*!*********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridHandlerAction.js ***!
  \*********************************************************************************************/
/*! exports provided: setDisplayedColumnList, setPlayerList, setCurrentActivePlayer, setPagingOption, exportExcel, setIsFetchingData, setSweetAlertOption */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setDisplayedColumnList", function() { return setDisplayedColumnList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setPlayerList", function() { return setPlayerList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setCurrentActivePlayer", function() { return setCurrentActivePlayer; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setPagingOption", function() { return setPagingOption; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "exportExcel", function() { return exportExcel; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setIsFetchingData", function() { return setIsFetchingData; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setSweetAlertOption", function() { return setSweetAlertOption; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
/* harmony import */ var _lib_services_playerQueryService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../lib/services/playerQueryService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerQueryService.js");


const setDisplayedColumnList = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_DISPLAYED_COLUMN_LIST"],
    data
  };
};
const setPlayerList = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_PLAYER_LIST"],
    data
  };
};
const setCurrentActivePlayer = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_CURRENT_ACTIVE_PLAYER"],
    data
  };
};
const setPagingOption = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_PAGING"],
    data
  };
};
const exportExcel = (playerList, param, displayedColumn, hasCommPermissionRead) => () => {
  const service = new _lib_services_playerQueryService__WEBPACK_IMPORTED_MODULE_1__["default"]();
  service.exportExcel(playerList, param, displayedColumn, hasCommPermissionRead);
};
const setIsFetchingData = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_IS_FETCHING_DATA"],
    data
  };
};
const setSweetAlertOption = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_SWEET_ALERT_OPTION"],
    data
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridOddsGroupAction.js":
/*!***********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridOddsGroupAction.js ***!
  \***********************************************************************************************/
/*! exports provided: setOddsGroupDropdownPosition, updateOddsGroup */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setOddsGroupDropdownPosition", function() { return setOddsGroupDropdownPosition; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "updateOddsGroup", function() { return updateOddsGroup; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
/* harmony import */ var _lib_services_playerCommandService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../lib/services/playerCommandService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerCommandService.js");


const setOddsGroupDropdownPosition = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_ODDSGROUP_POSITION"],
    data
  };
};
const updateOddsGroup = param => () => {
  const commandService = new _lib_services_playerCommandService__WEBPACK_IMPORTED_MODULE_1__["default"]();
  return commandService.updateOddsGroup(param);
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridPlayerStatusAction.js":
/*!**************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridPlayerStatusAction.js ***!
  \**************************************************************************************************/
/*! exports provided: setStatusDropdownPosition, updatePlayerStatus */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setStatusDropdownPosition", function() { return setStatusDropdownPosition; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "updatePlayerStatus", function() { return updatePlayerStatus; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
/* harmony import */ var _lib_services_playerCommandService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../lib/services/playerCommandService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerCommandService.js");


const setStatusDropdownPosition = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_STATUS_POSITION"],
    data
  };
};
const updatePlayerStatus = param => () => {
  const commandService = new _lib_services_playerCommandService__WEBPACK_IMPORTED_MODULE_1__["default"]();
  return commandService.updatePlayerStatus(param);
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridSpecialLevelAction.js":
/*!**************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridSpecialLevelAction.js ***!
  \**************************************************************************************************/
/*! exports provided: setSpecialLevelDropdownPosition, updateSpecialLevel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setSpecialLevelDropdownPosition", function() { return setSpecialLevelDropdownPosition; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "updateSpecialLevel", function() { return updateSpecialLevel; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
/* harmony import */ var _lib_services_playerCommandService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../lib/services/playerCommandService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerCommandService.js");


const setSpecialLevelDropdownPosition = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_SPECIAL_LEVEL_POSITION"],
    data
  };
};
const updateSpecialLevel = param => () => {
  const commandService = new _lib_services_playerCommandService__WEBPACK_IMPORTED_MODULE_1__["default"]();
  return commandService.updateSpecialLevel(param);
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridUserNameAction.js":
/*!**********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridUserNameAction.js ***!
  \**********************************************************************************************/
/*! exports provided: setContextMenu */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setContextMenu", function() { return setContextMenu; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");

const setContextMenu = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_CONTEXT_MENU"],
    data
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js":
/*!*********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js ***!
  \*********************************************************************************/
/*! exports provided: ActionCreators */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ActionCreators", function() { return ActionCreators; });
/* harmony import */ var _jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/actions/loaderAction */ "./www/js/source/reactpages/actions/loaderAction.js");
/* harmony import */ var _filterHandlerAction__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./filterHandlerAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/filterHandlerAction.js");
/* harmony import */ var _playerDetailHandlerAction__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./playerDetailHandlerAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/playerDetailHandlerAction.js");
/* harmony import */ var _commonAction__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./commonAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/commonAction.js");
/* harmony import */ var _gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./gridHandlerAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridHandlerAction.js");
/* harmony import */ var _gridBaStatusAction__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./gridBaStatusAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridBaStatusAction.js");
/* harmony import */ var _gridOddsGroupAction__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./gridOddsGroupAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridOddsGroupAction.js");
/* harmony import */ var _gridSpecialLevelAction__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./gridSpecialLevelAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridSpecialLevelAction.js");
/* harmony import */ var _gridPlayerStatusAction__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./gridPlayerStatusAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridPlayerStatusAction.js");
/* harmony import */ var _gridUserNameAction__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./gridUserNameAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridUserNameAction.js");
/* harmony import */ var _siteSettingAction__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./siteSettingAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/siteSettingAction.js");
/* harmony import */ var _lrfFiltersAction__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ./lrfFiltersAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/lrfFiltersAction.js");












const ActionCreators = Object.assign({}, _jsroot_actions_loaderAction__WEBPACK_IMPORTED_MODULE_0__, _commonAction__WEBPACK_IMPORTED_MODULE_3__, _filterHandlerAction__WEBPACK_IMPORTED_MODULE_1__, _playerDetailHandlerAction__WEBPACK_IMPORTED_MODULE_2__, _gridHandlerAction__WEBPACK_IMPORTED_MODULE_4__, _gridBaStatusAction__WEBPACK_IMPORTED_MODULE_5__, _gridOddsGroupAction__WEBPACK_IMPORTED_MODULE_6__, _gridSpecialLevelAction__WEBPACK_IMPORTED_MODULE_7__, _gridPlayerStatusAction__WEBPACK_IMPORTED_MODULE_8__, _gridUserNameAction__WEBPACK_IMPORTED_MODULE_9__, _siteSettingAction__WEBPACK_IMPORTED_MODULE_10__, _lrfFiltersAction__WEBPACK_IMPORTED_MODULE_11__);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/lrfFiltersAction.js":
/*!********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/lrfFiltersAction.js ***!
  \********************************************************************************************/
/*! exports provided: getLRFFilters */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getLRFFilters", function() { return getLRFFilters; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
/* harmony import */ var _lib_services_lrfFiltersService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../lib/services/lrfFiltersService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/lrfFiltersService.js");


const getLRFFilters = () => dispatch => {
  const service = new _lib_services_lrfFiltersService__WEBPACK_IMPORTED_MODULE_1__["default"]();
  service.fetchData().then(response => {
    dispatch(setLRFFilters(response.data));
  }).catch(() => {
    dispatch(setLRFFilters({
      CustomerClass: []
    }));
  });
};

const setLRFFilters = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_LRF_FILTERS"],
    data
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/playerDetailHandlerAction.js":
/*!*****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/playerDetailHandlerAction.js ***!
  \*****************************************************************************************************/
/*! exports provided: getLink, getTabList, setSelectedPlayerIdList, setSelectedTabName, setSelectedSubTabName */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getLink", function() { return getLink; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getTabList", function() { return getTabList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setSelectedPlayerIdList", function() { return setSelectedPlayerIdList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setSelectedTabName", function() { return setSelectedTabName; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "setSelectedSubTabName", function() { return setSelectedSubTabName; });
/* harmony import */ var _lib_services_playerQueryService__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../lib/services/playerQueryService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerQueryService.js");
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");


const getLink = (systemType, action) => () => {
  const service = new _lib_services_playerQueryService__WEBPACK_IMPORTED_MODULE_0__["default"]();
  return service.getLink(systemType, action);
};
const getTabList = () => () => {
  const service = new _lib_services_playerQueryService__WEBPACK_IMPORTED_MODULE_0__["default"]();
  return service.getTabList();
};
const setSelectedPlayerIdList = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_1__["SET_SELECTED_PLAYERID_LIST"],
    data
  };
};
const setSelectedTabName = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_1__["SET_SELECTED_TAB_NAME"],
    data
  };
};
const setSelectedSubTabName = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_1__["SET_SELECTED_SUBTAB_NAME"],
    data
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/siteSettingAction.js":
/*!*********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/siteSettingAction.js ***!
  \*********************************************************************************************/
/*! exports provided: getSiteSetting */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getSiteSetting", function() { return getSiteSetting; });
/* harmony import */ var _types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
/* harmony import */ var _gridHandlerAction__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./gridHandlerAction */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/gridHandlerAction.js");
/* harmony import */ var _lib_helpers_gridColumnBuilder__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../lib/helpers/gridColumnBuilder */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/gridColumnBuilder.js");
/* harmony import */ var _lib_services_siteSettingService__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../lib/services/siteSettingService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/siteSettingService.js");




const getSiteSetting = (licType, isSubsidiary, hasCommPermissionRead) => dispatch => {
  const service = new _lib_services_siteSettingService__WEBPACK_IMPORTED_MODULE_3__["default"]();
  service.getSetting().then(response => {
    const {
      IsAllowedCustomerComm,
      IsAllowedBA
    } = response.data;
    const isAllowedBA = !isSubsidiary && IsAllowedBA;

    if (isAllowedBA) {
      const columnList = Object(_lib_helpers_gridColumnBuilder__WEBPACK_IMPORTED_MODULE_2__["buildColumnList"])(licType, isAllowedBA, hasCommPermissionRead);
      dispatch(Object(_gridHandlerAction__WEBPACK_IMPORTED_MODULE_1__["setDisplayedColumnList"])(columnList));
    }

    dispatch(setSiteSetting({
      isAllowedCustomerComm: IsAllowedCustomerComm,
      isAllowedBA
    }));
  }).catch(() => {
    dispatch(setSiteSetting({
      isAllowedCustomerComm: false,
      isAllowedBA: false
    }));
  });
};

const setSiteSetting = data => {
  return {
    type: _types__WEBPACK_IMPORTED_MODULE_0__["SET_SITE_SETTING"],
    data
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js":
/*!*********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js ***!
  \*********************************************************************************/
/*! exports provided: SET_PARAM, SET_PLAYER_LIST, SET_SELECTED_PLAYER, SET_SELECTED_PLAYERID_LIST, SET_ADVANCED_SEARCH, SET_COLLAPSE_FILTER, SET_DISPLAYED_COLUMN_LIST, SET_CURRENT_ACTIVE_PLAYER, SET_PAGING, SET_SELECTED_TAB_NAME, SET_SELECTED_SUBTAB_NAME, SET_IS_FETCHING_DATA, SET_ODDSGROUP_POSITION, SET_SPECIAL_LEVEL_POSITION, SET_STATUS_POSITION, SET_CONTEXT_MENU, SET_SWEET_ALERT_OPTION, SET_SITE_SETTING, SET_LRF_FILTERS */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_PARAM", function() { return SET_PARAM; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_PLAYER_LIST", function() { return SET_PLAYER_LIST; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_PLAYER", function() { return SET_SELECTED_PLAYER; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_PLAYERID_LIST", function() { return SET_SELECTED_PLAYERID_LIST; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_ADVANCED_SEARCH", function() { return SET_ADVANCED_SEARCH; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_COLLAPSE_FILTER", function() { return SET_COLLAPSE_FILTER; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_DISPLAYED_COLUMN_LIST", function() { return SET_DISPLAYED_COLUMN_LIST; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_CURRENT_ACTIVE_PLAYER", function() { return SET_CURRENT_ACTIVE_PLAYER; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_PAGING", function() { return SET_PAGING; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_TAB_NAME", function() { return SET_SELECTED_TAB_NAME; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SELECTED_SUBTAB_NAME", function() { return SET_SELECTED_SUBTAB_NAME; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_IS_FETCHING_DATA", function() { return SET_IS_FETCHING_DATA; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_ODDSGROUP_POSITION", function() { return SET_ODDSGROUP_POSITION; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SPECIAL_LEVEL_POSITION", function() { return SET_SPECIAL_LEVEL_POSITION; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_STATUS_POSITION", function() { return SET_STATUS_POSITION; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_CONTEXT_MENU", function() { return SET_CONTEXT_MENU; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SWEET_ALERT_OPTION", function() { return SET_SWEET_ALERT_OPTION; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_SITE_SETTING", function() { return SET_SITE_SETTING; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SET_LRF_FILTERS", function() { return SET_LRF_FILTERS; });
const SET_PARAM = 'PLAYER_ADMIN/SET_PARAM';
const SET_PLAYER_LIST = 'PLAYER_ADMIN/SET_PLAYER_LIST';
const SET_SELECTED_PLAYER = 'PLAYER_ADMIN/SET_SELECTED_PLAYER';
const SET_SELECTED_PLAYERID_LIST = 'PLAYER_ADMIN/SET_SELECTED_PLAYERID_LIST';
const SET_ADVANCED_SEARCH = 'PLAYER_ADMIN/SET_ADVANCED_SEARCH';
const SET_COLLAPSE_FILTER = 'PLAYER_ADMIN/SET_COLLAPSE_FILTER';
const SET_DISPLAYED_COLUMN_LIST = 'PLAYER_ADMIN/SET_DISPLAYED_COLUMN_LIST';
const SET_CURRENT_ACTIVE_PLAYER = 'PLAYER_ADMIN/SET_CURRENT_ACTIVE_PLAYER';
const SET_PAGING = 'PLAYER_ADMIN/SET_PAGING';
const SET_SELECTED_TAB_NAME = 'PLAYER_ADMIN/SET_SELECTED_TAB_NAME';
const SET_SELECTED_SUBTAB_NAME = 'PLAYER_ADMIN/SET_SELECTED_SUBTAB_NAME';
const SET_IS_FETCHING_DATA = 'PLAYER_ADMIN/SET_IS_FETCHING_DATA';
const SET_ODDSGROUP_POSITION = 'PLAYER_ADMIN/SET_ODDSGROUP_POSITION';
const SET_SPECIAL_LEVEL_POSITION = 'PLAYER_ADMIN/SET_SPECIAL_LEVEL_POSITION';
const SET_STATUS_POSITION = 'PLAYER_ADMIN/SET_STATUS_POSITION';
const SET_CONTEXT_MENU = 'PLAYER_ADMIN/SET_CONTEXT_MENU';
const SET_SWEET_ALERT_OPTION = 'PLAYER_ADMIN/SET_SWEET_ALERT_OPTION';
const SET_SITE_SETTING = 'PLAYER_ADMIN/SET_SITE_SETTING';
const SET_LRF_FILTERS = 'PLAYER_ADMIN/SET_LRF_FILTERS';

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/batch-update/batch-update-manager.js":
/*!*****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/batch-update/batch-update-manager.js ***!
  \*****************************************************************************************************/
/*! exports provided: batchUpdateManager */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "batchUpdateManager", function() { return batchUpdateManager; });
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js");
/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _jsroot_Module_lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/Module/lib/factoryCommonService */ "./www/js/source/reactpages/Module/lib/factoryCommonService.js");
/* harmony import */ var timers__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! timers */ "./node_modules/timers-browserify/main.js");
/* harmony import */ var timers__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(timers__WEBPACK_IMPORTED_MODULE_3__);




const batchUpdateManager = {
  editCommMenu: [{
    value: 1,
    label: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MoveCommissionGroup')
  }, {
    value: 2,
    label: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('UpdateCommissionValue')
  }, {
    value: 3,
    label: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('RemoveFromGroup')
  }],
  popupBatchUpdateOption_1: {
    url: 'PlayerCommission/MultipleMoveGroup',
    title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('BatchUpdate') + " - " + _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('MoveCommissionGroup'),
    width: 800,
    height: 600
  },
  popupBatchUpdateOption_2: {
    url: 'PlayerCommission/MultipleUpdateGroup',
    title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('BatchUpdate') + " - " + _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('UpdateCommissionValue'),
    width: 800,
    height: 600
  },
  popupBatchUpdateOption_3: {
    url: 'PlayerCommission/MultipleRemoveFromGroup',
    title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('BatchUpdate') + " - " + _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__["default"].getSingleResource('RemoveFromGroup'),
    width: 600,
    height: 400
  },
  updateTableInterval: null,
  getLink: function (action) {
    const lrfSystemId = 1;
    const commonService = _jsroot_Module_lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_2__["default"].createInstance();
    return commonService.getLink(lrfSystemId, action);
  },
  bindMenuEvent: function (handler) {
    var searchInput = jquery__WEBPACK_IMPORTED_MODULE_1___default()(".ddl-batch-update .Select-input");
    searchInput.find(" > input").attr("readonly", "readonly");
    jquery__WEBPACK_IMPORTED_MODULE_1___default()(".ddl-batch-update .Select-value").off("click").on("click", () => {
      searchInput.focus();
    });

    if (batchUpdateManager.updateTableInterval) {
      Object(timers__WEBPACK_IMPORTED_MODULE_3__["clearInterval"])(batchUpdateManager.updateTableInterval);
    }

    batchUpdateManager.updateTableInterval = setInterval(() => {
      var result = window.top["playerComissionResult"];

      if (result && result.isSuccess) {
        var newGroup = result.newGroup;
        result.custIds.split(",").forEach(e => {
          var col = jquery__WEBPACK_IMPORTED_MODULE_1___default()(".td-comm-" + e);
          col.html(newGroup);
          col.attr("title", newGroup);
        });
      }

      if (result && result.isCancel) {
        handler.setState({
          batchUpdateSelectedOption: 0,
          popupBatchUpdateShow: false
        });
      }

      window.top["playerComissionResult"] = null;
    }, 200);
  },
  bindGridEvents: function () {
    var checkAll = jquery__WEBPACK_IMPORTED_MODULE_1___default()("#chk-cust-0");
    var checkCusts = jquery__WEBPACK_IMPORTED_MODULE_1___default()(".chk-cust");
    checkAll.prop("checked", false);
    checkCusts.prop("checked", false);
    checkAll.off("click").on("click", function () {
      var isChecked = jquery__WEBPACK_IMPORTED_MODULE_1___default()(this).is(":checked");
      checkCusts.prop("checked", isChecked);
      var selectedLength = jquery__WEBPACK_IMPORTED_MODULE_1___default()(".chk-cust:checked").length;
      batchUpdateManager.showHideBatchUpdateMenu(selectedLength > 1);
    });
    checkCusts.each((i, e) => {
      jquery__WEBPACK_IMPORTED_MODULE_1___default()(e).off("click").on("click", () => {
        var selectedLength = jquery__WEBPACK_IMPORTED_MODULE_1___default()(".chk-cust:checked").length;
        var isChecked = checkCusts.length == jquery__WEBPACK_IMPORTED_MODULE_1___default()(".chk-cust:checked").length;
        checkAll.prop("checked", isChecked);
        batchUpdateManager.showHideBatchUpdateMenu(selectedLength > 1);
      });
    });
    batchUpdateManager.showHideBatchUpdateMenu(false);
  },
  showHideBatchUpdateMenu: function (isShow) {
    isShow ? jquery__WEBPACK_IMPORTED_MODULE_1___default()(".ddl-batch-update").removeClass("hide") : jquery__WEBPACK_IMPORTED_MODULE_1___default()(".ddl-batch-update").addClass("hide");
  },
  getSelectCusts: function () {
    var checkCusts = jquery__WEBPACK_IMPORTED_MODULE_1___default()(".chk-cust:checked");
    var custIds = [];
    var custNames = [];
    checkCusts.each((i, e) => {
      var $this = jquery__WEBPACK_IMPORTED_MODULE_1___default()(e);
      custIds.push($this.attr("data-custid"));
      custNames.push($this.attr("data-username"));
    });
    return {
      custIds,
      custNames
    };
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
    var {
      custIds,
      custNames
    } = batchUpdateManager.getSelectCusts();
    var query = "?" + jquery__WEBPACK_IMPORTED_MODULE_1___default.a.param({
      t: timestamps,
      custIds: custIds.join(","),
      custNames: custNames.join(",")
    });
    var opt = jquery__WEBPACK_IMPORTED_MODULE_1___default.a.extend({}, batchUpdateManager["popupBatchUpdateOption_" + selectedOption]);
    opt.url = batchUpdateManager.getLink(opt.url + query);
    return opt;
  }
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/AppContainer.jsx":
/*!********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/AppContainer.jsx ***!
  \********************************************************************************************/
/*! exports provided: AppContainer, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppContainer", function() { return AppContainer; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _Content__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Content */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/Content.jsx");
/* harmony import */ var _FormFilter__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./FormFilter */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/FormFilter.jsx");
/* harmony import */ var _jsroot_components_Loader__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/components/Loader */ "./www/js/source/reactpages/components/Loader.jsx");
/* harmony import */ var _jsroot_layouts_AppLayout__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/layouts/AppLayout */ "./www/js/source/reactpages/layouts/AppLayout.jsx");
/* harmony import */ var _PlayerDetail__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./PlayerDetail */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/PlayerDetail.jsx");
/* harmony import */ var _reducers_filterHandlerReducer__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../reducers/filterHandlerReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/filterHandlerReducer.js");








const AppContainer = props => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_layouts_AppLayout__WEBPACK_IMPORTED_MODULE_5__["default"], null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_Loader__WEBPACK_IMPORTED_MODULE_4__["default"], {
    isLoading: props.isLoading
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_FormFilter__WEBPACK_IMPORTED_MODULE_3__["default"], {
    hasCommPermissionRead: props.hasCommPermissionRead
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_Content__WEBPACK_IMPORTED_MODULE_2__["default"], null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_PlayerDetail__WEBPACK_IMPORTED_MODULE_6__["default"], null));
};

function mapStateToProps(state) {
  const {
    isLoading,
    hasCommPermissionRead
  } = state;
  return Object.assign({}, {
    isLoading,
    hasCommPermissionRead
  });
}

/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps)( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(AppContainer)));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/Content.jsx":
/*!***************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/Content.jsx ***!
  \***************************************************************************************/
/*! exports provided: Content, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Content", function() { return Content; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @9softstudio/react-grid */ "./node_modules/@9softstudio/react-grid/dist/index.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _content_header_GridCaption__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./content-header/GridCaption */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/content-header/GridCaption.jsx");
/* harmony import */ var _DataGrid__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./DataGrid */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/DataGrid.jsx");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }








const Content = props => {
  const {
    pagingOption
  } = props;

  if (pagingOption.TotalItem === null) {
    return null;
  }

  const handlePaging = (pageIndex, pageSize) => {
    const {
      param,
      searchPlayer
    } = props;

    const newParam = _objectSpread(_objectSpread({}, param), {}, {
      pageIndex,
      pageSize
    });

    searchPlayer(newParam);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "result-content"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_3__["Pager"], {
    pageOption: pagingOption,
    onPaging: handlePaging
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_content_header_GridCaption__WEBPACK_IMPORTED_MODULE_4__["default"], null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_DataGrid__WEBPACK_IMPORTED_MODULE_5__["default"], null));
};

function mapDispatchToProps(dispatch) {
  const {
    searchPlayer
  } = _actions__WEBPACK_IMPORTED_MODULE_6__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    searchPlayer
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    pagingOption,
    param
  } = state;
  return Object.assign({}, {
    pagingOption,
    param
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(Content));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/DataGrid.jsx":
/*!****************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/DataGrid.jsx ***!
  \****************************************************************************************/
/*! exports provided: DataGrid, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DataGrid", function() { return DataGrid; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @9softstudio/react-grid */ "./node_modules/@9softstudio/react-grid/dist/index.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _grid_ContextMenu__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./grid/ContextMenu */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/ContextMenu.jsx");
/* harmony import */ var _grid_SpecialLevelDropdown__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./grid/SpecialLevelDropdown */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/SpecialLevelDropdown.jsx");
/* harmony import */ var _lib_helpers_gridHelper__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../lib/helpers/gridHelper */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/gridHelper.js");
/* harmony import */ var _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../lib/services/gridInteractionService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js");
/* harmony import */ var _grid_OddsGroupDropdown__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./grid/OddsGroupDropdown */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/OddsGroupDropdown.jsx");
/* harmony import */ var _grid_PlayerRow__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./grid/PlayerRow */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/PlayerRow.jsx");
/* harmony import */ var _grid_StatusDropdown__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ./grid/StatusDropdown */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/StatusDropdown.jsx");
/* harmony import */ var _lib_helpers_utils__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ../lib/helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
/* harmony import */ var _jsroot_components_NoDataTitle__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @jsroot/components/NoDataTitle */ "./www/js/source/reactpages/components/NoDataTitle.jsx");
/* harmony import */ var _components_sweet_alert__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! ../../../../components/sweet-alert */ "./www/js/source/reactpages/components/sweet-alert/index.jsx");
/* harmony import */ var _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ../batch-update/batch-update-manager */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/batch-update/batch-update-manager.js");
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

















const MAX_WIDTH_PAGE = 1920;
class DataGrid extends react__WEBPACK_IMPORTED_MODULE_0__["PureComponent"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "handleSort", (sortBy, sortOrder) => {
      const {
        playerList,
        setPlayerList,
        toggleLoader
      } = this.props;
      toggleLoader(true);
      setTimeout(() => {
        const sortedPlayerList = _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_8__["default"].sortPlayerList(playerList, sortBy, sortOrder);
        setPlayerList(sortedPlayerList);
        toggleLoader(false);
      }, 0);
    });

    this.maxWidth = Math.min(document.body.clientWidth, MAX_WIDTH_PAGE);
  }

  componentDidMount() {
    _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_15__["batchUpdateManager"].bindGridEvents();
  }

  componentDidUpdate() {
    _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_15__["batchUpdateManager"].bindGridEvents();
  }

  _renderDataSection(dataList, displayedColumn, canSelectPlayers, hasCommPermissionRead) {
    const dataRows = [];

    for (let i = 0; i < dataList.length; i++) {
      const dataItem = dataList[i];
      const rowProps = {
        dataItem,
        displayedColumn,
        canSelectPlayers,
        hasCommPermissionRead
      };
      dataRows.push( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_grid_PlayerRow__WEBPACK_IMPORTED_MODULE_10__["default"], _extends({
        key: dataItem.LicenseeCustId
      }, rowProps)));
    }

    return dataRows;
  }

  render() {
    const {
      playerList,
      selectedPlayer,
      isFetchingData,
      displayedColumnList,
      sweetAlertOption,
      hasCommPermissionEdit,
      hasCommPermissionRead
    } = this.props;

    if (!playerList) {
      return null;
    }

    if (!playerList.length) {
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_NoDataTitle__WEBPACK_IMPORTED_MODULE_13__["default"], null);
    }

    const dataList = selectedPlayer ? [playerList.find(item => item.LicenseeCustId === selectedPlayer.licenseeCustId)] : playerList;
    const displayedColumn = Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_12__["buildDisplayedColumnObject"])(displayedColumnList);
    const canSelectPlayers = hasCommPermissionEdit;
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_4__["Table"], {
      minWidth: 650,
      autoWidth: true,
      maxWidth: this.maxWidth,
      minWidth: 1300,
      autoHeight: true,
      header: _lib_helpers_gridHelper__WEBPACK_IMPORTED_MODULE_7__["default"].renderHeader(displayedColumn, canSelectPlayers, hasCommPermissionRead),
      body: this._renderDataSection(dataList, displayedColumn, canSelectPlayers, hasCommPermissionRead),
      containerPadding: 30,
      shouldResetScrollPosition: isFetchingData,
      onSort: this.handleSort
    }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_grid_ContextMenu__WEBPACK_IMPORTED_MODULE_5__["default"], {
      hasCommPermissionRead: hasCommPermissionRead
    }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_grid_SpecialLevelDropdown__WEBPACK_IMPORTED_MODULE_6__["default"], null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_grid_OddsGroupDropdown__WEBPACK_IMPORTED_MODULE_9__["default"], null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_grid_StatusDropdown__WEBPACK_IMPORTED_MODULE_11__["default"], null), sweetAlertOption && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_components_sweet_alert__WEBPACK_IMPORTED_MODULE_14__["default"], sweetAlertOption));
  }

}

function mapDispatchToProps(dispatch) {
  const {
    setPlayerList,
    toggleLoader
  } = _actions__WEBPACK_IMPORTED_MODULE_3__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setPlayerList,
    toggleLoader
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    playerList,
    selectedPlayer,
    displayedColumnList,
    isFetchingData,
    sweetAlertOption,
    hasCommPermissionEdit,
    hasCommPermissionRead
  } = state;
  return Object.assign({}, {
    playerList,
    selectedPlayer,
    displayedColumnList,
    isFetchingData,
    sweetAlertOption,
    hasCommPermissionEdit,
    hasCommPermissionRead
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(DataGrid));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/FormFilter.jsx":
/*!******************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/FormFilter.jsx ***!
  \******************************************************************************************/
/*! exports provided: FormFilter, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "FormFilter", function() { return FormFilter; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _actions_index_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../actions/index.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _jsroot_components_CheckBox__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/components/CheckBox */ "./www/js/source/reactpages/components/CheckBox.jsx");
/* harmony import */ var _filter_AdvancedSearch__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./filter/AdvancedSearch */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/AdvancedSearch.jsx");
/* harmony import */ var _filter_SearchOption__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./filter/SearchOption */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/SearchOption.jsx");
/* harmony import */ var _filter_AthenaSearch__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./filter/AthenaSearch */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/AthenaSearch.jsx");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _jsroot_components_collapsible_panel__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @jsroot/components/collapsible-panel */ "./www/js/source/reactpages/components/collapsible-panel/index.jsx");
/* harmony import */ var _jsroot_components_UserGuide__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @jsroot/components/UserGuide */ "./www/js/source/reactpages/components/UserGuide.jsx");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _filter_UserNameInput__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ./filter/UserNameInput */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/UserNameInput.jsx");
/* harmony import */ var _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @jsroot/lib/QueryString */ "./www/js/source/reactpages/lib/QueryString.js");
/* harmony import */ var _jsroot_lib_htmlHelper__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! @jsroot/lib/htmlHelper */ "./www/js/source/reactpages/lib/htmlHelper.js");
/* harmony import */ var _userGuide_LicAthenaUserGuide__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ../../userGuide/LicAthenaUserGuide */ "./www/js/source/reactpages/Module/PlayerAdmin/userGuide/LicAthenaUserGuide.jsx");
/* harmony import */ var _userGuide_LicApiUserGuide__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ../../userGuide/LicApiUserGuide */ "./www/js/source/reactpages/Module/PlayerAdmin/userGuide/LicApiUserGuide.jsx");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



















class FormFilter extends react__WEBPACK_IMPORTED_MODULE_0__["PureComponent"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "handleSubmit", event => {
      event.preventDefault();
      this.searchPlayer();
    });

    _defineProperty(this, "handlePressEnter", event => {
      if (event.charCode === 13) {
        this.searchPlayer();
      }
    });

    _defineProperty(this, "handleChange", event => {
      const {
        name,
        value
      } = event.target;

      if (name === "searchOption") {
        this.setState({
          isMatchPrecise: value == 4
        });
      }

      this.setState({
        [name]: value
      });
    });

    _defineProperty(this, "handleSpecificInputChange", (name, value) => {
      if (name === "customerClass") {
        this.setState({
          changeOnCustomerClass: true
        });
      }

      this.setState({
        [name]: value
      });
    });

    _defineProperty(this, "handleUserNameChange", value => {
      this.userNameRef.current.value = value;
    });

    this.allOddsGroupString = Object.keys(_lib_constants__WEBPACK_IMPORTED_MODULE_8__["oddsGroupListWithStyle"]).join(',');
    this.allCurrencyString = props.currencyList.map(c => c.Id).join(',');
    this.allSpecialLevel = Object.keys(_lib_constants__WEBPACK_IMPORTED_MODULE_8__["specialLevelListWithColor"]).map(item => item).join(',');
    this.userGuide = this._renderUserGuide();
    this.userNameRef = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createRef();
    this.state = {
      bankAccountNumber: '',
      email: '',
      currencyIdList: this.allCurrencyString,
      oddsGroup: this.allOddsGroupString,
      specialLevel: this.allSpecialLevel,
      baFilter: _lib_constants__WEBPACK_IMPORTED_MODULE_8__["baStatusList"].All,
      searchOption: _lib_constants__WEBPACK_IMPORTED_MODULE_8__["searchOptions"].matchPrecise,
      isMatchPrecise: true,
      customerClass: "",
      changeOnCustomerClass: false
    };
  }

  componentDidMount() {
    const ibcCustId = Number(_jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_13__["default"].getParam(window.location.href, 'custId'));
    const licenseeCustId = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_13__["default"].getParam(window.location.href, 'licenseeCustId');

    if (ibcCustId || licenseeCustId) {
      const param = {
        ibcCustId,
        licenseeCustId
      };
      this.props.getSinglePlayer(param);
      const userName = _jsroot_lib_QueryString__WEBPACK_IMPORTED_MODULE_13__["default"].getParam(window.location.href, 'userName');
      this.userNameRef.current.value = userName;
    }

    const {
      type,
      isSubsidiary,
      hasCommPermissionRead
    } = this.props;
    this.props.getSiteSetting(type, isSubsidiary, hasCommPermissionRead);
    this.props.getLRFFilters();
  }

  componentDidUpdate(prevProps, prevState) {
    const {
      isAdvancedSearch
    } = this.props;

    if (prevProps.isAdvancedSearch !== isAdvancedSearch && !isAdvancedSearch) {
      this.setState({
        currencyIdList: this.allCurrencyString,
        oddsGroup: this.allOddsGroupString,
        specialLevel: this.allSpecialLevel,
        baFilter: _lib_constants__WEBPACK_IMPORTED_MODULE_8__["baStatusList"].All,
        customerClass: this.props.lrfFilters.CustomerClass.join(','),
        changeOnCustomerClass: false
      });
    }
  }

  searchPlayer() {
    const {
      email
    } = this.state;

    if (email && !new RegExp(_lib_constants__WEBPACK_IMPORTED_MODULE_8__["emailPattern"]).test(email)) {
      window.alert(_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].getSingleResource('InvalidEmail'));
      return;
    }

    const userName = this.userNameRef.current.value;
    const {
      pagingOption,
      searchPlayer,
      currencyList
    } = this.props;

    const _this$state = this.state,
          {
      specialLevel,
      currencyIdList,
      oddsGroup
    } = _this$state,
          restParam = _objectWithoutProperties(_this$state, ["specialLevel", "currencyIdList", "oddsGroup"]);

    const actualSpecialLevel = specialLevel.trim() === '' ? this.allSpecialLevel : specialLevel;
    const actualCurrencyIdList = currencyIdList.trim().split(',').length === currencyList.length ? '' : currencyIdList;
    const actualOddsGroup = oddsGroup.trim().split(',').length === Object.keys(_lib_constants__WEBPACK_IMPORTED_MODULE_8__["oddsGroupListWithStyle"]).length ? '' : oddsGroup;
    const actualCustomerClass = restParam.customerClass.trim().split(',').length === this.props.lrfFilters.CustomerClass.length ? '' : restParam.customerClass;

    const param = _objectSpread(_objectSpread({}, restParam), {}, {
      userName: Object(_jsroot_lib_htmlHelper__WEBPACK_IMPORTED_MODULE_14__["encode"])(userName),
      currencyIdList: actualCurrencyIdList,
      oddsGroup: actualOddsGroup,
      specialLevel: actualSpecialLevel,
      customerClass: actualCustomerClass,
      pageSize: pagingOption.PageSize,
      pageIndex: 1
    });

    searchPlayer(param);
  }

  _renderContentUserGuide() {
    const currentLang = _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].currentLanguage;
    return this.props.type === _lib_constants__WEBPACK_IMPORTED_MODULE_8__["licenseeType"].nonApi ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_userGuide_LicAthenaUserGuide__WEBPACK_IMPORTED_MODULE_15__["default"], {
      lang: currentLang
    }) : /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_userGuide_LicApiUserGuide__WEBPACK_IMPORTED_MODULE_16__["default"], {
      lang: currentLang
    });
  }

  _renderUserGuide() {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_UserGuide__WEBPACK_IMPORTED_MODULE_10__["default"], {
      title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].getSingleResource("UserGuide"),
      contentTitle: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].getSingleResource("PlayerAdministration"),
      isHtml: false,
      content: this._renderContentUserGuide()
    });
  }

  _getUserNameLabel() {
    return this.props.type === _lib_constants__WEBPACK_IMPORTED_MODULE_8__["licenseeType"].nonApi ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].getSingleResource("Username") : `${_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].getSingleResource("Username")}/${_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].getSingleResource("VendorMemberID")}`;
  }

  render() {
    const _this$state2 = this.state,
          {
      bankAccountNumber,
      email,
      searchOption
    } = _this$state2,
          advancedSearchParam = _objectWithoutProperties(_this$state2, ["bankAccountNumber", "email", "searchOption"]);

    const {
      type,
      isAdvancedSearch,
      isFilterCollapsing,
      currencyList,
      getSuggestionList,
      siteSetting
    } = this.props;
    const {
      currencyIdList,
      oddsGroup,
      specialLevel
    } = advancedSearchParam;
    const isValidParam = !!currencyIdList && !!oddsGroup && !!specialLevel && !(this.state.changeOnCustomerClass && !this.state.customerClass);
    const iconToggle = {
      tag: 'div',
      className: isFilterCollapsing ? 'chevron down' : 'chevron up',
      style: {}
    };
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_collapsible_panel__WEBPACK_IMPORTED_MODULE_9__["default"], {
      className: "page-filter",
      iconToggle: iconToggle,
      stickyFooter: this.userGuide,
      onCollapse: () => this.props.setCollapseFilter(true),
      onExpand: () => this.props.setCollapseFilter(false)
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "row"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "col-12 col-md-4 col-sm-4 col-lg-3 col-xl-3"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "form-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
      className: "label-text"
    }, this._getUserNameLabel(), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
      className: this.state.isMatchPrecise ? "" : "hide"
    }, "/", _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].getSingleResource("SystemId")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_filter_SearchOption__WEBPACK_IMPORTED_MODULE_6__["default"], {
      value: searchOption,
      onChange: this.handleChange
    })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_filter_UserNameInput__WEBPACK_IMPORTED_MODULE_12__["default"], {
      ref: this.userNameRef,
      onSelectItem: value => this.handleUserNameChange(value),
      minChar: 3,
      getSuggestionList: getSuggestionList,
      searchOption: searchOption,
      onKeyPress: this.handlePressEnter,
      onChange: value => this.handleUserNameChange(value)
    }))), type === _lib_constants__WEBPACK_IMPORTED_MODULE_8__["licenseeType"].nonApi && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_filter_AthenaSearch__WEBPACK_IMPORTED_MODULE_7__["default"], {
      onChange: this.handleChange,
      bankAccountNumber: bankAccountNumber,
      email: email,
      onKeyPress: this.handlePressEnter
    }), isAdvancedSearch && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_filter_AdvancedSearch__WEBPACK_IMPORTED_MODULE_5__["default"], {
      currencyList: currencyList,
      param: advancedSearchParam,
      isAllowedBA: siteSetting.isAllowedBA,
      onChange: this.handleSpecificInputChange,
      lrfFilters: this.props.lrfFilters
    }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "form-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "button-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", {
      type: "button",
      className: "btn btn-submit",
      disabled: !isValidParam,
      onClick: this.handleSubmit
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].getSingleResource("Submit"))))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "form-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_CheckBox__WEBPACK_IMPORTED_MODULE_4__["default"], {
      id: "isAdvancedSearch",
      checked: isAdvancedSearch,
      onChange: checked => this.props.setAdvancedSearch(checked),
      label: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_11__["default"].getSingleResource("AdvancedSearch")
    })))));
  }

}

function mapDispatchToProps(dispatch) {
  const {
    searchPlayer,
    setAdvancedSearch,
    setCollapseFilter,
    getSuggestionList,
    getSinglePlayer,
    getSiteSetting,
    getLRFFilters
  } = _actions_index_js__WEBPACK_IMPORTED_MODULE_2__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_3__["bindActionCreators"])({
    searchPlayer,
    setAdvancedSearch,
    setCollapseFilter,
    getSuggestionList,
    getSinglePlayer,
    getSiteSetting,
    getLRFFilters
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    type,
    isAdvancedSearch,
    isFilterCollapsing,
    pagingOption,
    currencyList,
    siteSetting,
    isSubsidiary,
    lrfFilters
  } = state;
  return Object.assign({}, {
    type,
    isAdvancedSearch,
    isFilterCollapsing,
    pagingOption,
    currencyList,
    siteSetting,
    isSubsidiary,
    lrfFilters
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(FormFilter));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/PlayerDetail.jsx":
/*!********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/PlayerDetail.jsx ***!
  \********************************************************************************************/
/*! exports provided: PlayerDetail, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PlayerDetail", function() { return PlayerDetail; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }






class PlayerDetail extends react__WEBPACK_IMPORTED_MODULE_0__["PureComponent"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "handleClickMenu", (event, tabName) => {
      event.preventDefault();
      this.props.setSelectedTabName(tabName);
      this.props.setSelectedSubTabName(null);
    });

    this._tabList = this.props.getTabList();
  }

  _getDefaultSelectedTab() {
    return this._tabList && this._tabList.length && this._tabList[0].Name;
  }

  _buildActionUrl(systemType, action, tabName) {
    const {
      selectedPlayer
    } = this.props;
    const {
      ibcCustId,
      licenseeCustId,
      currencyId,
      currency,
      userName,
      createdDate
    } = selectedPlayer;
    const custIdQueryString = `custId=${ibcCustId}&licenseeCustId=${licenseeCustId}`;
    const nameValuePairString = custIdQueryString + `&currencyId=${currencyId}&currencyName=${currency}&userName=${userName}`;
    const queryString = (action.indexOf('?') !== -1 ? '&' : '?') + nameValuePairString;
    let urlByPlayer = action + queryString;

    if (tabName === _lib_constants__WEBPACK_IMPORTED_MODULE_4__["tabNames"].playerInfo) {
      urlByPlayer += `&createdDate=${encodeURIComponent(createdDate)}`;
    }

    return this.props.getLink(systemType, urlByPlayer);
  }

  _renderTabList(tabList) {
    const {
      selectedPlayer
    } = this.props;

    const selectedTabName = this.props.selectedTabName || this._getDefaultSelectedTab();

    const {
      isDeposit,
      isArchived
    } = selectedPlayer;
    let renderedTabList = !isDeposit || isArchived ? tabList.filter(item => !_lib_constants__WEBPACK_IMPORTED_MODULE_4__["excludedTabNameWithNonDepositAndArchived"].includes(item.Name)) : tabList;

    if (selectedPlayer.isClosed) {
      renderedTabList = renderedTabList.filter(item => !_lib_constants__WEBPACK_IMPORTED_MODULE_4__["excludedTabNameWithClosed"].includes(item.Name));
    }

    return renderedTabList.map(item => {
      const actionUrl = this._buildActionUrl(item.SystemType, item.Action, item.Name);

      const isActive = selectedTabName === item.Name;
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", {
        className: "tab",
        key: item.Name
      }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("a", {
        className: isActive ? "active" : "",
        href: actionUrl,
        onClick: event => this.handleClickMenu(event, item.Name)
      }, item.Text));
    });
  }

  _getFrameHeight() {
    const {
      isAdvancedSearch,
      isFilterCollapsing
    } = this.props;
    let omittedHeight;

    if (isFilterCollapsing) {
      omittedHeight = _lib_constants__WEBPACK_IMPORTED_MODULE_4__["heightSlicing"].whenCollapsing;
    } else {
      const key = isAdvancedSearch ? 'withAdvancedSearch' : 'default';
      omittedHeight = _lib_constants__WEBPACK_IMPORTED_MODULE_4__["heightSlicing"][key];
    }

    return window.innerHeight - omittedHeight;
  }

  _getActionUrl() {
    const {
      selectedTabName,
      selectedSubTabName
    } = this.props;
    const tabItem = selectedTabName ? this._tabList.find(t => t.Name === selectedTabName) : this._tabList[0];
    const action = selectedSubTabName ? `${tabItem.Action}?timestamp=${new Date().getTime()}&subTab=${selectedSubTabName}` : `${tabItem.Action}?timestamp=${new Date().getTime()}`;
    return tabItem && this._buildActionUrl(tabItem.SystemType, action, tabItem.Name);
  }

  render() {
    const {
      selectedPlayer
    } = this.props;

    if (!selectedPlayer) {
      return null;
    }

    if (this._tabList.length === 0) {
      return null;
    }

    const src = this._getActionUrl();

    if (!src) {
      return null;
    }

    const frameHeight = this._getFrameHeight();

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "pa-detail-container"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "page-filter-tab"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "header-tab"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "content-tab"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", {
      className: "tabs"
    }, this._renderTabList(this._tabList)))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "tab-panel"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "pa-frame-container"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("iframe", {
      id: "paDetailFrame",
      style: {
        height: frameHeight
      },
      name: "paDetailFrame",
      frameBorder: "0",
      src: src
    })))));
  }

}

function mapDispatchToProps(dispatch) {
  const {
    getLink,
    getTabList,
    setSelectedTabName,
    setSelectedSubTabName
  } = _actions__WEBPACK_IMPORTED_MODULE_3__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    getLink,
    getTabList,
    setSelectedTabName,
    setSelectedSubTabName
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    selectedPlayer,
    isAdvancedSearch,
    isFilterCollapsing,
    selectedTabName,
    selectedSubTabName
  } = state;
  return Object.assign({}, {
    selectedPlayer,
    isAdvancedSearch,
    isFilterCollapsing,
    selectedTabName,
    selectedSubTabName
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(PlayerDetail));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/content-header/GridCaption.jsx":
/*!**********************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/content-header/GridCaption.jsx ***!
  \**********************************************************************************************************/
/*! exports provided: GridCaption, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "GridCaption", function() { return GridCaption; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_select__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-select */ "./node_modules/react-select/dist/react-select.es.js");
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _jsroot_components_ReportTitle__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/components/ReportTitle */ "./www/js/source/reactpages/components/ReportTitle.jsx");
/* harmony import */ var _9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @9softstudio/react-multiselect */ "./node_modules/@9softstudio/react-multiselect/dist/index.js");
/* harmony import */ var _9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_6__);
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../../lib/helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
/* harmony import */ var _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../../batch-update/batch-update-manager */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/batch-update/batch-update-manager.js");
/* harmony import */ var _jsroot_components_popup__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @jsroot/components/popup */ "./www/js/source/reactpages/components/popup/index.jsx");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }












class GridCaption extends react__WEBPACK_IMPORTED_MODULE_0___default.a.Component {
  constructor(props) {
    super(props);

    _defineProperty(this, "handleChangeDisplayedColumn", (currentItem, selectedItemsKey) => {
      const {
        displayedColumnList,
        setDisplayedColumnList
      } = this.props;
      const selectedKeyList = selectedItemsKey.split(',');
      const newColumnList = displayedColumnList.map(item => {
        const isShow = selectedKeyList.indexOf(item.key) !== -1;
        return item.show === isShow ? item : _objectSpread(_objectSpread({}, item), {}, {
          show: isShow
        });
      });
      setDisplayedColumnList(newColumnList);
    });

    _defineProperty(this, "handleChangeBatchUpdateMenu", selectedOption => {
      this.setState({
        batchUpdateSelectedOption: selectedOption
      });
      _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_9__["batchUpdateManager"].handleMenuSelected(this, selectedOption);
    });

    _defineProperty(this, "handleExportExcel", () => {
      const {
        playerList,
        param,
        displayedColumnList,
        exportExcel,
        selectedPlayer,
        hasCommPermissionRead
      } = this.props;

      if (displayedColumnList.filter(x => x.show).length === 0) {
        window.alert(_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource("NoInfo"));
        return;
      }

      const displayedColumn = Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__["buildDisplayedColumnObject"])(displayedColumnList);
      const dataList = selectedPlayer ? [playerList.find(item => item.LicenseeCustId === selectedPlayer.licenseeCustId)] : playerList;
      exportExcel(dataList, param, displayedColumn, hasCommPermissionRead);
    });

    this.initBatchUpdate();
  }

  initBatchUpdate() {
    this.batchUpdateMenuRef = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createRef();
    var s = {
      batchUpdateSelectedOption: 0
    };
    var menu = [];

    for (var i = 0; i < _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_9__["batchUpdateManager"].editCommMenu.length; i++) {
      var m = _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_9__["batchUpdateManager"].editCommMenu[i];
      s.popupBatchUpdateShow = false;
      menu.push(m);
    }

    s.batchUpdateMenu = menu;
    this.state = s;
  }

  componentDidMount() {
    _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_9__["batchUpdateManager"].bindMenuEvent(this);
  }

  _buildDisplayedColumnList() {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_6___default.a, {
      id: "columnList",
      className: "selection-column",
      language: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].currentLanguage,
      dataSource: this.props.displayedColumnList,
      keyField: "key",
      valueField: "value",
      statusField: "show",
      onChange: this.handleChangeDisplayedColumn
    });
  }

  _buildBatchUpdateMenu() {
    const hasCommPermissionEdit = this.props.hasCommPermissionEdit == 1;

    if (!hasCommPermissionEdit) {
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null);
    }

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_select__WEBPACK_IMPORTED_MODULE_1__["default"], {
      instanceId: "ddl-batch-update",
      ref: this.batchUpdateMenuRef,
      className: "ddl-batch-update",
      options: this.state.batchUpdateMenu,
      value: this.state.batchUpdateSelectedOption,
      onChange: selectedOption => this.handleChangeBatchUpdateMenu(selectedOption),
      placeholder: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('BatchUpdate')
    });
  }

  _buildLeftPart() {
    const {
      selectedPlayer,
      toggleLoader
    } = this.props;

    if (selectedPlayer) {
      const {
        userName,
        currency
      } = selectedPlayer;
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
        className: "link",
        onClick: () => {
          toggleLoader(true);
          setTimeout(() => {
            this.props.setSelectedPlayer(null);
            toggleLoader(false);
          }, 10);
        }
      }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('PreviousSearchResult')), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("i", {
        className: "chevron right icon-separator"
      }), " ", _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('Player'), ": ", userName, " ", currency && `(${currency})`));
    } else {
      const {
        playerCount
      } = this.props;
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
        key: "searchResult"
      }, `${_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('SearchResult')}: ${playerCount} ${_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('Player')}(s)`);
    }
  }

  _buildRightPart() {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
      className: "caret-block"
    }, this._buildBatchUpdateMenu(), this._buildDisplayedColumnList(), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
      className: "icon-excel-wrapper",
      onClick: this.handleExportExcel
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("i", {
      className: "icon-excel",
      title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('ExportToExcel')
    })));
  }

  render() {
    const Content = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, this._buildLeftPart(), this._buildRightPart());

    if (this.state.batchUpdateSelectedOption == 0) {
      return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_ReportTitle__WEBPACK_IMPORTED_MODULE_5__["default"], {
        tag: "div",
        content: Content
      });
    }

    const opt = _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_9__["batchUpdateManager"].getPopupOption(this.state.batchUpdateSelectedOption);
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_popup__WEBPACK_IMPORTED_MODULE_10__["default"], {
      id: "popup-comm-group",
      isShow: this.state.popupBatchUpdateShow,
      iframeUrl: opt.url,
      title: opt.title,
      width: opt.width,
      height: opt.height,
      onClose: () => _batch_update_batch_update_manager__WEBPACK_IMPORTED_MODULE_9__["batchUpdateManager"].closePopup(this, this.state.batchUpdateSelectedOption)
    }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_ReportTitle__WEBPACK_IMPORTED_MODULE_5__["default"], {
      tag: "div",
      content: Content
    }));
  }

}

function mapDispatchToProps(dispatch) {
  const {
    setSelectedPlayer,
    setDisplayedColumnList,
    toggleLoader,
    exportExcel
  } = _actions__WEBPACK_IMPORTED_MODULE_4__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_3__["bindActionCreators"])({
    setSelectedPlayer,
    setDisplayedColumnList,
    toggleLoader,
    exportExcel
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    pagingOption,
    selectedPlayer,
    displayedColumnList,
    param,
    playerList,
    hasCommPermissionEdit,
    hasCommPermissionRead
  } = state;
  const playerCount = pagingOption.TotalItem || playerList && playerList.length || 0;
  return Object.assign({}, {
    playerList,
    playerCount,
    selectedPlayer,
    displayedColumnList,
    param,
    hasCommPermissionEdit,
    hasCommPermissionRead
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps, mapDispatchToProps)(GridCaption));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/AdvancedSearch.jsx":
/*!*****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/AdvancedSearch.jsx ***!
  \*****************************************************************************************************/
/*! exports provided: AdvancedSearch, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AdvancedSearch", function() { return AdvancedSearch; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @9softstudio/react-multiselect */ "./node_modules/@9softstudio/react-multiselect/dist/index.js");
/* harmony import */ var _9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var react_select__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! react-select */ "./node_modules/react-select/dist/react-select.es.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _lib_helpers_utils__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../lib/helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }








class AdvancedSearch extends react__WEBPACK_IMPORTED_MODULE_0___default.a.Component {
  constructor(props) {
    super(props);

    _defineProperty(this, "handleChange", (name, value) => {
      this.props.onChange(name, value);
    });

    _defineProperty(this, "handleChangeSingleDropdown", (name, item) => {
      this.props.onChange(name, item.value);
    });

    this.oddsGroupList = Object.keys(_lib_constants__WEBPACK_IMPORTED_MODULE_4__["oddsGroupListWithStyle"]).map(item => ({
      Id: item,
      Value: item,
      Status: true
    }));
    this.specialLevelList = Object.keys(_lib_constants__WEBPACK_IMPORTED_MODULE_4__["specialLevelListWithColor"]).map(item => ({
      Id: item,
      Value: Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_6__["getSpecialLevelText"])(item),
      Status: true
    }));
    this.baStatusList = Object.keys(_lib_constants__WEBPACK_IMPORTED_MODULE_4__["baStatusList"]).map(item => ({
      value: _lib_constants__WEBPACK_IMPORTED_MODULE_4__["baStatusList"][item],
      label: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_5__["default"].getSingleResource(item)
    }));
    this.customerClassList = props.lrfFilters.CustomerClass.map(item => ({
      Id: item,
      Value: Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_6__["getCustomerClassText"])(item),
      Status: true
    }));
  }

  _createSelectData(originalList, selectedItemString) {
    const selectedItemList = selectedItemString.split(',').filter(x => x !== '');
    return originalList.map(item => {
      const {
        Id,
        Value
      } = item;
      return {
        Id,
        Value,
        Status: selectedItemList.indexOf(Id) !== -1
      };
    });
  }

  render() {
    const {
      currencyList,
      param,
      isAllowedBA
    } = this.props;
    const {
      currencyIdList,
      oddsGroup,
      specialLevel,
      baFilter
    } = param;

    const convertedCurrencyList = this._createSelectData(currencyList, currencyIdList);

    const oddsGroupList = this._createSelectData(this.oddsGroupList, oddsGroup);

    const specialLevelList = this._createSelectData(this.specialLevelList, specialLevel);

    const selectedBaStatus = isAllowedBA ? this.baStatusList.find(x => x.value === baFilter) : null;
    const selectedCustomerClass = param.changeOnCustomerClass ? param.customerClass : this.props.lrfFilters.CustomerClass.join(",");

    const ddlCustomerClass = this._createSelectData(this.customerClassList, selectedCustomerClass);

    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "form-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
      className: "label-text"
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_5__["default"].getSingleResource("Currency")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_1___default.a, {
      id: "currency",
      dataSource: convertedCurrencyList,
      keyField: "Id",
      valueField: "Value",
      statusField: "Status",
      onChange: (currentItem, selectedItemsKey) => this.handleChange('currencyIdList', selectedItemsKey)
    }))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "form-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
      className: "label-text"
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_5__["default"].getSingleResource("OddsGroup")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_1___default.a, {
      id: "oddsGroup",
      dataSource: oddsGroupList,
      keyField: "Id",
      valueField: "Value",
      statusField: "Status",
      onChange: (currentItem, selectedItemsKey) => this.handleChange('oddsGroup', selectedItemsKey)
    }))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "col-12 col-md-3 col-sm-3 col-lg-2 col-xl-1"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "form-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
      className: "label-text"
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_5__["default"].getSingleResource("SpecialLevel")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_1___default.a, {
      id: "specialLevel",
      dataSource: specialLevelList,
      keyField: "Id",
      valueField: "Value",
      statusField: "Status",
      onChange: (currentItem, selectedItemsKey) => this.handleChange('specialLevel', selectedItemsKey)
    }))), selectedBaStatus !== null && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "col-12 col-md-3 col-sm-3 col-lg-2 col-xl-1"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "form-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
      className: "label-text"
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_5__["default"].getSingleResource("BA")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_select__WEBPACK_IMPORTED_MODULE_3__["default"], {
      instanceId: "ddlBaStatus",
      name: "form-field-name",
      value: selectedBaStatus,
      options: this.baStatusList,
      onChange: item => this.handleChangeSingleDropdown('baFilter', item)
    }))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "col-12 col-md-3 col-sm-3 col-lg-2 col-xl-1"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "form-group"
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
      className: "label-text"
    }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_5__["default"].getSingleResource("CustomerClass")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_multiselect__WEBPACK_IMPORTED_MODULE_1___default.a, {
      id: "ddl-customer-class",
      dataSource: ddlCustomerClass,
      keyField: "Id",
      valueField: "Value",
      statusField: "Status",
      onChange: (currentItem, selectedItemsKey) => this.handleChange('customerClass', selectedItemsKey)
    }))));
  }

}
const propTypes = {
  param: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.shape({
    currencyIdList: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string.isRequired,
    oddsGroup: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string.isRequired,
    specialLevel: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string.isRequired,
    baFilter: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.number.isRequired,
    customerClass: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.string.isRequired
  }).isRequired,
  onChange: prop_types__WEBPACK_IMPORTED_MODULE_2___default.a.func.isRequired
};
AdvancedSearch.propTypes = propTypes;
/* harmony default export */ __webpack_exports__["default"] = (AdvancedSearch);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/AthenaSearch.jsx":
/*!***************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/AthenaSearch.jsx ***!
  \***************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");




const AthenaSearch = props => {
  const {
    bankAccountNumber,
    email,
    onChange,
    onKeyPress
  } = props;
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "col-12 col-md-4 col-sm-4 col-lg-3 col-xl-2"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "form-group"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
    className: "label-text"
  }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource("BankAccountNumber")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
    className: "input-text",
    id: "bankAccountNumber",
    name: "bankAccountNumber",
    type: "text",
    value: bankAccountNumber,
    autoComplete: "off",
    onChange: onChange,
    onKeyPress: onKeyPress,
    placeholder: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('PlaceholderBankAccount')
  }))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "col-12 col-md-4 col-sm-4 col-lg-3 col-xl-2"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "form-group"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
    className: "label-text"
  }, _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource("Email")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
    className: "input-text",
    id: "email",
    name: "email",
    type: "text",
    value: email,
    autoComplete: "off",
    onChange: onChange,
    onKeyPress: onKeyPress,
    placeholder: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_2__["default"].getSingleResource('PlaceholderEmail')
  }))));
};

const propTypes = {
  bankAccountNumber: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string.isRequired,
  email: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string.isRequired,
  onChange: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  onKeyPress: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired
};
AthenaSearch.propTypes = propTypes;
/* harmony default export */ __webpack_exports__["default"] = (/*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(AthenaSearch));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/SearchOption.jsx":
/*!***************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/SearchOption.jsx ***!
  \***************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/components/dropdown */ "./www/js/source/reactpages/components/dropdown/index.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");





const SearchOption = props => {
  const {
    value,
    onChange
  } = props;
  const [isOpen, setIsOpen] = Object(react__WEBPACK_IMPORTED_MODULE_0__["useState"])(false);

  const handleToggleDropdown = () => {
    setIsOpen(!isOpen);
  };

  const handleClose = () => {
    setIsOpen(false);
  };

  const toggleButton = {
    className: 'toggle-outline toggle-outline-light'
  };
  const selectedValue = value || _lib_constants__WEBPACK_IMPORTED_MODULE_2__["searchOptions"].matchPrecise;
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_1__["DropdownButton"], {
    className: "search-option",
    isOpen: isOpen,
    toggleButton: toggleButton,
    onToggle: handleToggleDropdown,
    buttonLabel: /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("i", {
      className: "material-icons"
    }, "settings"),
    onClose: handleClose
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_1__["DropdownItem"], {
    tag: "div"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
    className: "selection-item"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
    type: "radio",
    name: "searchOption",
    value: _lib_constants__WEBPACK_IMPORTED_MODULE_2__["searchOptions"].matchPrecise,
    checked: selectedValue === _lib_constants__WEBPACK_IMPORTED_MODULE_2__["searchOptions"].matchPrecise,
    onChange: onChange
  }), "\xA0\xA0", _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_3__["default"].getSingleResource('MatchPrecise'))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_1__["DropdownItem"], {
    tag: "div"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
    className: "selection-item"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
    type: "radio",
    name: "searchOption",
    value: _lib_constants__WEBPACK_IMPORTED_MODULE_2__["searchOptions"].matchPrefix,
    checked: selectedValue === _lib_constants__WEBPACK_IMPORTED_MODULE_2__["searchOptions"].matchPrefix,
    onChange: onChange
  }), "\xA0\xA0", _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_3__["default"].getSingleResource('MatchPrefix'), " \"", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
    className: "keyword"
  }, "test"), "\"ABCD")), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_1__["DropdownItem"], {
    tag: "div"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", {
    className: "selection-item"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
    type: "radio",
    name: "searchOption",
    value: _lib_constants__WEBPACK_IMPORTED_MODULE_2__["searchOptions"].matchCase,
    checked: selectedValue === _lib_constants__WEBPACK_IMPORTED_MODULE_2__["searchOptions"].matchCase,
    onChange: onChange
  }), "\xA0\xA0", _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_3__["default"].getSingleResource('MatchCase'), " AB\"", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
    className: "keyword"
  }, "test"), "\"CD")));
};

/* harmony default export */ __webpack_exports__["default"] = (SearchOption);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/UserNameInput.jsx":
/*!****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/filter/UserNameInput.jsx ***!
  \****************************************************************************************************/
/*! exports provided: UserNameInput, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "UserNameInput", function() { return UserNameInput; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/components/dropdown */ "./www/js/source/reactpages/components/dropdown/index.js");
/* harmony import */ var _jsroot_components_utils_timer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/components/utils/timer */ "./www/js/source/reactpages/components/utils/timer.js");
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }




const UserNameInput = /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.forwardRef((props, userNameRef) => {
  const {
    onSelectItem,
    minChar,
    getSuggestionList,
    searchOption,
    onChange
  } = props,
        inputAttributes = _objectWithoutProperties(props, ["onSelectItem", "minChar", "getSuggestionList", "searchOption", "onChange"]);

  const [isOpenSuggestion, setIsOpenSuggestion] = Object(react__WEBPACK_IMPORTED_MODULE_0__["useState"])(false);
  const [suggestionList, setSuggestionList] = Object(react__WEBPACK_IMPORTED_MODULE_0__["useState"])([]);
  const [dropdownStyle, setDropdownStyle] = Object(react__WEBPACK_IMPORTED_MODULE_0__["useState"])(null);
  const cachedDataRef = Object(react__WEBPACK_IMPORTED_MODULE_0__["useRef"])({});

  const handleChange = () => {
    const element = userNameRef.current;

    if (!element) {
      return;
    }

    const rect = element.getBoundingClientRect();
    const style = {
      left: rect.left,
      top: rect.top + rect.height,
      width: rect.width,
      position: 'fixed'
    };

    const handleSuggestionResult = resultData => {
      setDropdownStyle(style);
      setSuggestionList(resultData);
      const hasData = resultData && resultData.length;
      setIsOpenSuggestion(!!hasData);
    };

    const {
      value
    } = element;

    if (value.length >= minChar) {
      const cachedData = cachedDataRef.current;
      const cacheKey = value + '_' + searchOption;

      if (cachedData[cacheKey]) {
        const result = cachedData[cacheKey];
        handleSuggestionResult(result);
      } else {
        const param = {
          keyword: value,
          searchType: searchOption
        };
        getSuggestionList(param).then(response => {
          const result = response.data || [];
          handleSuggestionResult(result);
          cachedData[cacheKey] = result;
        });
      }
    } else {
      setIsOpenSuggestion(false);
      setSuggestionList([]);
    }

    onChange && onChange(value);
  };

  const debounceChange = _jsroot_components_utils_timer__WEBPACK_IMPORTED_MODULE_2__["default"].debounce(handleChange, 300);

  const handleSelectSuggestionItem = value => {
    onSelectItem && onSelectItem(value);
    handleCloseDropdown();
  };

  const handleCloseDropdown = () => {
    setIsOpenSuggestion(false);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "pa-username"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", _extends({
    className: "input-text",
    id: "userName",
    name: "userName",
    type: "text",
    ref: userNameRef,
    autoComplete: "off",
    onChange: debounceChange
  }, inputAttributes)), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "dropdown"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_1__["DropdownMenu"], {
    isOpen: isOpenSuggestion,
    style: dropdownStyle
  }, suggestionList.map(item => {
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_1__["DropdownItem"], {
      key: item,
      onClick: () => handleSelectSuggestionItem(item)
    }, item);
  }))));
});
/* harmony default export */ __webpack_exports__["default"] = (UserNameInput);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/BaStatusCell.jsx":
/*!*************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/BaStatusCell.jsx ***!
  \*************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @9softstudio/react-grid */ "./node_modules/@9softstudio/react-grid/dist/index.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _jsroot_components_Switcher__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/components/Switcher */ "./www/js/source/reactpages/components/Switcher.jsx");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../lib/services/gridInteractionService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js");
/* harmony import */ var _lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../../lib/helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
/* harmony import */ var _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../../../../../lib/sweetAlert */ "./www/js/source/reactpages/lib/sweetAlert.js");











const BaStatusCell = props => {
  const {
    dataItem,
    setPlayerList,
    toggleLoader,
    updateBaStatus,
    setSweetAlertOption,
    playerList,
    hasBaPermission
  } = props;
  const {
    IbcCustId,
    BaStatus,
    OddsGroup,
    IsArchived,
    LicenseeCustId
  } = dataItem;

  const handleChangeBaStatus = event => {
    event.preventDefault();

    if (!hasBaPermission) {
      return;
    }

    const handleCloseAlert = () => {
      setSweetAlertOption(null);
    };

    const isBa = event.target.checked;
    const resourceName = isBa ? 'AlertTurnOnBAMsg' : 'AlertTurnOffBAMsg';
    const confirmMessage = _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__["default"].getSingleResource(resourceName);
    const cancelProps = {
      onClick: handleCloseAlert
    };
    const confirmProps = {
      onClick: () => {
        const param = {
          custId: IbcCustId,
          isBa,
          LicenseeCustId
        };
        toggleLoader(true);
        updateBaStatus(param).then(response => {
          const {
            Success,
            Message,
            ErrorMessage,
            ExtendedData
          } = response.data;

          if (Success) {
            const oddsGroup = ExtendedData || OddsGroup;
            const newPlayerList = _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_7__["default"].updatePlayerWithBaStatus(playerList, IbcCustId, isBa, oddsGroup);
            setPlayerList(newPlayerList);
          }

          const resultMessage = Success ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__["default"].getSingleResource('UpdateSuccess') : Message || _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__["default"].getSingleResource(ErrorMessage);
          const displayIcon = Success ? _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].success : _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].error;
          const alertOpt = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultAlertOption"])(resultMessage, handleCloseAlert, null, displayIcon);
          setSweetAlertOption(alertOpt);
          toggleLoader(false);
        }).catch(error => {
          console.log(error.message);
          const message = Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__["getErrorMessageForUpdating"])(error);
          const alertOpt = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultAlertOption"])(message, handleCloseAlert, null, _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].error);
          setSweetAlertOption(alertOpt);
          toggleLoader(false);
        });
      }
    };
    const confirmOption = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultConfirmOption"])(confirmMessage, handleCloseAlert, confirmProps, cancelProps);
    setSweetAlertOption(confirmOption);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_4__["Cell"], {
    className: "center-align" + `${!hasBaPermission ? ' ba-disabled' : ''}`
  }, !IsArchived && BaStatus !== undefined && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_Switcher__WEBPACK_IMPORTED_MODULE_5__["default"], {
    disabled: !hasBaPermission,
    checked: BaStatus,
    onChange: handleChangeBaStatus
  }));
};

function mapDispatchToProps(dispatch) {
  const {
    setPlayerList,
    toggleLoader,
    updateBaStatus,
    setSweetAlertOption
  } = _actions__WEBPACK_IMPORTED_MODULE_3__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setPlayerList,
    toggleLoader,
    updateBaStatus,
    setSweetAlertOption
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    playerList,
    hasBaPermission
  } = state;
  return Object.assign({}, {
    playerList,
    hasBaPermission
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(BaStatusCell));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/ContextMenu.jsx":
/*!************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/ContextMenu.jsx ***!
  \************************************************************************************************/
/*! exports provided: ContextMenu, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ContextMenu", function() { return ContextMenu; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/components/dropdown */ "./www/js/source/reactpages/components/dropdown/index.js");
/* harmony import */ var _lib_helpers_contextMenuHelper__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../lib/helpers/contextMenuHelper */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/contextMenuHelper.js");






const ContextMenu = props => {
  const {
    setSelectedPlayer,
    currentActivePlayer,
    setSelectedTabName,
    setSelectedSubTabName,
    getTabList,
    contextMenu,
    type,
    hasCommPermissionRead
  } = props;
  const {
    position,
    isOpen: isOpenContextMenu
  } = contextMenu;
  const [isOpen, setIsOpen] = Object(react__WEBPACK_IMPORTED_MODULE_0__["useState"])(false);

  const handleMouseEnter = () => {
    setIsOpen(true);
  };

  const handleMouseLeave = () => {
    setIsOpen(isOpenContextMenu);
  };

  const handleClickMenu = (tabName, subTabName) => {
    setSelectedTabName(tabName);
    setSelectedSubTabName(subTabName || '');
    const {
      ibcCustId,
      userName,
      currency,
      isDeposit,
      isArchived,
      licenseeCustId,
      currencyId,
      createdDate
    } = currentActivePlayer;
    const selectedPlayer = {
      ibcCustId,
      userName,
      currency,
      isDeposit,
      isArchived,
      licenseeCustId,
      currencyId,
      createdDate
    };
    setSelectedPlayer(selectedPlayer);
  };

  const style = position && {
    top: position.y,
    left: position.x
  };
  let activeMenuList = [];

  if (currentActivePlayer) {
    const tabList = getTabList();
    activeMenuList = Object(_lib_helpers_contextMenuHelper__WEBPACK_IMPORTED_MODULE_5__["getActiveTabList"])(currentActivePlayer, tabList, type, hasCommPermissionRead);
  }

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "dropdown context-menu",
    style: style
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_4__["DropdownMenu"], {
    onMouseEnter: handleMouseEnter,
    onMouseLeave: handleMouseLeave,
    isOpen: isOpenContextMenu || isOpen
  }, activeMenuList.map(item => {
    const selectedTabName = item.tabName;
    const subTabName = item.subTabName;
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_4__["DropdownItem"], {
      key: item.key,
      onClick: () => handleClickMenu(selectedTabName, subTabName)
    }, item.name);
  })));
};

function mapDispatchToProps(dispatch) {
  const {
    setSelectedPlayer,
    setSelectedTabName,
    setSelectedSubTabName,
    getTabList
  } = _actions__WEBPACK_IMPORTED_MODULE_3__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setSelectedPlayer,
    setSelectedTabName,
    setSelectedSubTabName,
    getTabList
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    currentActivePlayer,
    contextMenu,
    type
  } = state;
  return Object.assign({}, {
    currentActivePlayer,
    contextMenu,
    type
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(ContextMenu));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/HeaderCustomerClassCell.jsx":
/*!************************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/HeaderCustomerClassCell.jsx ***!
  \************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return CustomerClassCell; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _jsroot_components_popup_Popup__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/components/popup/Popup */ "./www/js/source/reactpages/components/popup/Popup.jsx");
/* harmony import */ var _jsroot_Module_lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @jsroot/Module/lib/factoryCommonService */ "./www/js/source/reactpages/Module/lib/factoryCommonService.js");
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }





class CustomerClassCell extends react__WEBPACK_IMPORTED_MODULE_0___default.a.Component {
  constructor(props) {
    super(props);

    _defineProperty(this, "showCustomerClassInformation", e => {
      this.setState({
        isShowCustomerClassInformation: true
      });
      e.stopPropagation();
      return false;
    });

    _defineProperty(this, "closeCustomerClassInfomation", () => {
      this.setState({
        isShowCustomerClassInformation: false
      });
    });

    _defineProperty(this, "clickOnPopup", e => {
      e.stopPropagation();
      return false;
    });

    this.state = {
      isShowCustomerClassInformation: false
    };
  }

  render() {
    const commonService = _jsroot_Module_lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_3__["default"].createInstance();
    const lrfSystemId = 1;
    var url = "CustomerClass/CustomerClassInformation?time=" + new Date().getTime();
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, this.props.text, "\xA0\xA0", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      class: "icon-info",
      onClick: this.showCustomerClassInformation
    }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_popup_Popup__WEBPACK_IMPORTED_MODULE_2__["default"], {
      width: 1000,
      height: 600,
      isShow: this.state.isShowCustomerClassInformation,
      title: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('CustomerClassInformation'),
      onClose: this.closeCustomerClassInfomation,
      onClick: this.clickOnPopup,
      isHtml: "false",
      iframeUrl: commonService.getLink(lrfSystemId, url)
    }));
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/OddsGroupCell.jsx":
/*!**************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/OddsGroupCell.jsx ***!
  \**************************************************************************************************/
/*! exports provided: OddsGroupCell, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OddsGroupCell", function() { return OddsGroupCell; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _jsroot_components_DropdownLabel__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/components/DropdownLabel */ "./www/js/source/reactpages/components/DropdownLabel.jsx");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../lib/services/gridInteractionService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js");
/* harmony import */ var _lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../lib/helpers/playerDataBuilder */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerDataBuilder.js");








const OddsGroupCell = props => {
  const {
    dataItem,
    oddsGroupDropdownPosition,
    setCurrentActivePlayer,
    setOddsGroupDropdownPosition,
    hasOddsGroupPermission
  } = props;
  const value = dataItem.OddsGroup;
  const style = _lib_constants__WEBPACK_IMPORTED_MODULE_3__["oddsGroupListWithStyle"][value];
  const canUpdate = hasOddsGroupPermission && !dataItem.BaStatus;

  const handleToggleOddsGroup = event => {
    if (!canUpdate) {
      return;
    }

    const currentActivePlayer = Object(_lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_7__["buildCurrentActivePlayer"])(dataItem);
    setCurrentActivePlayer(currentActivePlayer);
    const dropdownName = 'oddsGroupDropdown';
    const newPosition = _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_6__["default"].getNextDropdownPosition(event.target, oddsGroupDropdownPosition, dropdownName);
    setOddsGroupDropdownPosition(newPosition);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_DropdownLabel__WEBPACK_IMPORTED_MODULE_4__["default"], {
    className: canUpdate ? '' : 'disabled',
    text: value,
    labelStyle: style,
    onClick: handleToggleOddsGroup
  });
};

function mapDispatchToProps(dispatch) {
  const {
    setCurrentActivePlayer,
    setOddsGroupDropdownPosition
  } = _actions__WEBPACK_IMPORTED_MODULE_5__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setCurrentActivePlayer,
    setOddsGroupDropdownPosition
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    oddsGroupDropdownPosition,
    hasOddsGroupPermission
  } = state;
  return Object.assign({}, {
    oddsGroupDropdownPosition,
    hasOddsGroupPermission
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(OddsGroupCell));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/OddsGroupDropdown.jsx":
/*!******************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/OddsGroupDropdown.jsx ***!
  \******************************************************************************************************/
/*! exports provided: OddsGroupDropdown, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OddsGroupDropdown", function() { return OddsGroupDropdown; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @jsroot/components/dropdown */ "./www/js/source/reactpages/components/dropdown/index.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../lib/services/gridInteractionService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../../lib/helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
/* harmony import */ var _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../../../../../lib/sweetAlert */ "./www/js/source/reactpages/lib/sweetAlert.js");










const OddsGroupDropdown = props => {
  const {
    oddsGroupDropdownPosition,
    setOddsGroupDropdownPosition
  } = props;
  const isOpen = oddsGroupDropdownPosition !== null;
  const style = oddsGroupDropdownPosition && {
    top: oddsGroupDropdownPosition.y,
    left: oddsGroupDropdownPosition.x
  };

  const handleClose = () => {
    setOddsGroupDropdownPosition(null);
  };

  const {
    currentActivePlayer,
    updateOddsGroup,
    playerList,
    setPlayerList,
    toggleLoader,
    setSweetAlertOption
  } = props;

  const handleCloseAlert = () => {
    setSweetAlertOption(null);
  };

  const cancelProps = {
    onClick: handleCloseAlert
  };

  const handleChangeOddsGroup = (event, value) => {
    event.preventDefault();
    const confirmMessage = _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__["default"].getSingleResource('AlertSetOddsGroupForCustomerMsg').replace('{0}', value);
    const confirmProps = {
      onClick: () => {
        if (currentActivePlayer && value !== currentActivePlayer.oddsGroup) {
          const {
            ibcCustId,
            licenseeCustId,
            oddsGroup
          } = currentActivePlayer;
          const param = {
            custId: ibcCustId,
            oddsGroup: value,
            oldValue: oddsGroup,
            licenseeCustId
          };
          toggleLoader(true);
          updateOddsGroup(param).then(response => {
            const {
              Success,
              Message,
              ErrorMessage
            } = response.data;

            if (Success) {
              const newPlayerList = _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_5__["default"].updatePlayerItem(playerList, ibcCustId, value, 'OddsGroup');
              setPlayerList(newPlayerList);
            }

            const resultMessage = Success ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__["default"].getSingleResource('UpdateSuccess') : Message || _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__["default"].getSingleResource(ErrorMessage);
            const displayIcon = Success ? _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].success : _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].error;
            const alertOpt = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultAlertOption"])(resultMessage, handleCloseAlert, null, displayIcon);
            setSweetAlertOption(alertOpt);
            toggleLoader(false);
          }).catch(error => {
            console.log(error.message);
            const message = Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__["getErrorMessageForUpdating"])(error);
            const alertOpt = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultAlertOption"])(message, handleCloseAlert, null, _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].error);
            setSweetAlertOption(alertOpt);
            toggleLoader(false);
          });
        } else {
          handleCloseAlert();
        }

        handleClose();
      }
    };
    const confirmOption = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultConfirmOption"])(confirmMessage, handleCloseAlert, confirmProps, cancelProps);
    setSweetAlertOption(confirmOption);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "dropdown oddsgroup-menu",
    style: style
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_3__["DropdownMenu"], {
    isOpen: isOpen,
    onClose: handleClose
  }, Object.keys(_lib_constants__WEBPACK_IMPORTED_MODULE_4__["oddsGroupListWithStyle"]).map(item => {
    const style = _lib_constants__WEBPACK_IMPORTED_MODULE_4__["oddsGroupListWithStyle"][item];
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_3__["DropdownItem"], {
      key: item,
      className: "oddsgroup-item",
      style: style,
      onClick: e => handleChangeOddsGroup(e, item)
    }, item);
  })));
};

function mapDispatchToProps(dispatch) {
  const {
    setOddsGroupDropdownPosition,
    updateOddsGroup,
    setPlayerList,
    toggleLoader,
    setSweetAlertOption
  } = _actions__WEBPACK_IMPORTED_MODULE_7__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setOddsGroupDropdownPosition,
    updateOddsGroup,
    setPlayerList,
    toggleLoader,
    setSweetAlertOption
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    oddsGroupDropdownPosition,
    currentActivePlayer,
    playerList
  } = state;
  return Object.assign({}, {
    oddsGroupDropdownPosition,
    currentActivePlayer,
    playerList
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(OddsGroupDropdown));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/PlayerRow.jsx":
/*!**********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/PlayerRow.jsx ***!
  \**********************************************************************************************/
/*! exports provided: PlayerRow, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PlayerRow", function() { return PlayerRow; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @9softstudio/react-grid */ "./node_modules/@9softstudio/react-grid/dist/index.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @jsroot/lib/numberFormat */ "./www/js/source/reactpages/lib/numberFormat.js");
/* harmony import */ var _UserNameCell__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./UserNameCell */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/UserNameCell.jsx");
/* harmony import */ var _OddsGroupCell__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./OddsGroupCell */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/OddsGroupCell.jsx");
/* harmony import */ var _SpecialLevelCell__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./SpecialLevelCell */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/SpecialLevelCell.jsx");
/* harmony import */ var _StatusCell__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./StatusCell */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/StatusCell.jsx");
/* harmony import */ var _BaStatusCell__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ./BaStatusCell */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/BaStatusCell.jsx");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");













const PlayerRow = props => {
  const {
    dataItem,
    displayedColumn,
    canSelectPlayers,
    hasCommPermissionRead
  } = props;
  const {
    LicenseeCustId,
    SystemId,
    RealName,
    Currency,
    CurrentBalance,
    OutstandingBalance,
    LastTransaction,
    Email,
    OddsGroup,
    SpecialLevel,
    IsDeposit,
    IsArchived,
    CommissionGroup,
    VendorMemberId
  } = dataItem;
  const canShowMainColumn = IsDeposit && !IsArchived;
  const commissionGroup = CommissionGroup || _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_12__["default"].getSingleResource('None');
  const canEditCommissionGpoup = IsDeposit && !IsArchived && dataItem.Status != _lib_constants__WEBPACK_IMPORTED_MODULE_4__["playerStatusByName"].Closed;
  const customerClass = dataItem.CustomerClass == "0" || !dataItem.CustomerClass ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_12__["default"].getSingleResource('None') : dataItem.CustomerClass;
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Row"], {
    key: LicenseeCustId
  }, canSelectPlayers && displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].Index] && !canEditCommissionGpoup && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    className: "center-align"
  }), canSelectPlayers && displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].Index] && canEditCommissionGpoup && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    className: "center-align"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
    type: "checkbox",
    id: "chk-cust-" + LicenseeCustId,
    "data-username": dataItem.UserName,
    "data-custid": dataItem.IbcCustId,
    className: "chk-cust"
  })), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].Status] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_StatusCell__WEBPACK_IMPORTED_MODULE_10__["default"], {
    dataItem: dataItem
  })), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].UserName] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_UserNameCell__WEBPACK_IMPORTED_MODULE_7__["default"], {
    dataItem: dataItem,
    hasCommPermissionRead: hasCommPermissionRead
  })), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].SystemId] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    title: SystemId
  }, SystemId), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].VendorMemberId] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], null, VendorMemberId), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].RealName] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], null, RealName), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].Currency] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    className: "center-align"
  }, Currency), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].CurrentBalance] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    className: "right-align",
    title: canShowMainColumn ? _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_6__["default"].roundDownFormatDecimal(CurrentBalance) : undefined
  }, canShowMainColumn && _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_6__["default"].roundDownFormatDecimal(CurrentBalance)), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].OutstandingBalance] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    className: "right-align",
    title: canShowMainColumn ? _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_6__["default"].roundDownFormatDecimal(OutstandingBalance) : undefined
  }, canShowMainColumn && _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_6__["default"].roundDownFormatDecimal(OutstandingBalance)), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].OddsGroup] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    className: "center-align"
  }, canShowMainColumn && OddsGroup !== undefined && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_OddsGroupCell__WEBPACK_IMPORTED_MODULE_8__["default"], {
    dataItem: dataItem
  })), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].BaStatus] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_BaStatusCell__WEBPACK_IMPORTED_MODULE_11__["default"], {
    dataItem: dataItem
  }), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].SpecialLevel] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    className: "center-align"
  }, canShowMainColumn && SpecialLevel !== undefined && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_SpecialLevelCell__WEBPACK_IMPORTED_MODULE_9__["default"], {
    dataItem: dataItem
  })), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].LastTransaction] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], null, LastTransaction), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].Email] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], null, Email), hasCommPermissionRead && displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].CommissionGroup] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    className: "text-center td-comm-" + dataItem.IbcCustId,
    title: commissionGroup
  }, commissionGroup), displayedColumn[_lib_constants__WEBPACK_IMPORTED_MODULE_4__["fields"].CustomerClass] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_5__["Cell"], {
    className: "text-center"
  }, customerClass));
};

function mapDispatchToProps(dispatch) {
  const {
    setSelectedPlayer
  } = _actions__WEBPACK_IMPORTED_MODULE_3__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setSelectedPlayer
  }, dispatch);
}

/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(null, mapDispatchToProps)(PlayerRow));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/SpecialLevelCell.jsx":
/*!*****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/SpecialLevelCell.jsx ***!
  \*****************************************************************************************************/
/*! exports provided: SpecialLevelCell, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SpecialLevelCell", function() { return SpecialLevelCell; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _jsroot_components_DropdownLabel__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @jsroot/components/DropdownLabel */ "./www/js/source/reactpages/components/DropdownLabel.jsx");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../lib/services/gridInteractionService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js");
/* harmony import */ var _lib_helpers_utils__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../lib/helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
/* harmony import */ var _lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../../lib/helpers/playerDataBuilder */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerDataBuilder.js");









const SpecialLevelCell = props => {
  const {
    dataItem,
    specialLevelDropdownPosition,
    setCurrentActivePlayer,
    setSpecialLevelDropdownPosition,
    hasSpecialLevelPermission
  } = props;
  const value = dataItem.SpecialLevel;
  const style = {
    backgroundColor: _lib_constants__WEBPACK_IMPORTED_MODULE_3__["specialLevelListWithColor"][value]
  };

  const handleToggleSpecialLevel = event => {
    if (!hasSpecialLevelPermission) {
      return;
    }

    const currentActivePlayer = Object(_lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_8__["buildCurrentActivePlayer"])(dataItem);
    setCurrentActivePlayer(currentActivePlayer);
    const dropdownName = 'specialLevelDropdown';
    const newPosition = _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_6__["default"].getNextDropdownPosition(event.target, specialLevelDropdownPosition, dropdownName);
    setSpecialLevelDropdownPosition(newPosition);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_DropdownLabel__WEBPACK_IMPORTED_MODULE_4__["default"], {
    className: hasSpecialLevelPermission ? '' : 'disabled',
    text: Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_7__["getSpecialLevelText"])(value),
    labelStyle: style,
    onClick: handleToggleSpecialLevel
  });
};

function mapDispatchToProps(dispatch) {
  const {
    setCurrentActivePlayer,
    setSpecialLevelDropdownPosition
  } = _actions__WEBPACK_IMPORTED_MODULE_5__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setCurrentActivePlayer,
    setSpecialLevelDropdownPosition
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    specialLevelDropdownPosition,
    hasSpecialLevelPermission
  } = state;
  return Object.assign({}, {
    specialLevelDropdownPosition,
    hasSpecialLevelPermission
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(SpecialLevelCell));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/SpecialLevelDropdown.jsx":
/*!*********************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/SpecialLevelDropdown.jsx ***!
  \*********************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @jsroot/components/dropdown */ "./www/js/source/reactpages/components/dropdown/index.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../lib/services/gridInteractionService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../../lib/helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
/* harmony import */ var _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../../../../../lib/sweetAlert */ "./www/js/source/reactpages/lib/sweetAlert.js");











const SpecialLevelDropdown = props => {
  const {
    specialLevelDropdownPosition,
    setSpecialLevelDropdownPosition
  } = props;
  const isOpen = specialLevelDropdownPosition !== null;
  const style = specialLevelDropdownPosition && {
    top: specialLevelDropdownPosition.y,
    left: specialLevelDropdownPosition.x
  };

  const handleClose = () => {
    setSpecialLevelDropdownPosition(null);
  };

  const {
    currentActivePlayer,
    updateSpecialLevel,
    playerList,
    setPlayerList,
    toggleLoader,
    setSweetAlertOption
  } = props;

  const handleCloseAlert = () => {
    setSweetAlertOption(null);
  };

  const cancelProps = {
    onClick: handleCloseAlert
  };

  const handleChangeOddsGroup = (event, value) => {
    event.preventDefault();
    const resourceName = Number(value) === 0 ? 'AlertRemoveSpecialLevelForCustomerMsg' : 'AlertSetSpecialLevelForCustomerMsg';
    const confirmMessage = _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__["default"].getSingleResource(resourceName);
    const confirmProps = {
      onClick: () => {
        if (currentActivePlayer && value !== currentActivePlayer.specialLevel) {
          const {
            ibcCustId,
            licenseeCustId,
            specialLevel
          } = currentActivePlayer;
          const param = {
            custId: ibcCustId,
            specialLevel: value,
            oldValue: specialLevel,
            licenseeCustId
          };
          toggleLoader(true);
          updateSpecialLevel(param).then(response => {
            const {
              Success,
              Message,
              ErrorMessage
            } = response.data;

            if (Success) {
              const newPlayerList = _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_5__["default"].updatePlayerItem(playerList, ibcCustId, value, 'SpecialLevel');
              setPlayerList(newPlayerList);
            }

            const resultMessage = Success ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__["default"].getSingleResource('UpdateSuccess') : Message || _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_6__["default"].getSingleResource(ErrorMessage);
            const displayIcon = Success ? _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].success : _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].error;
            const alertOpt = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultAlertOption"])(resultMessage, handleCloseAlert, null, displayIcon);
            setSweetAlertOption(alertOpt);
            toggleLoader(false);
          }).catch(error => {
            console.log(error.message);
            const message = Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__["getErrorMessageForUpdating"])(error);
            const alertOpt = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultAlertOption"])(message, handleCloseAlert, null, _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].error);
            setSweetAlertOption(alertOpt);
            toggleLoader(false);
          });
        } else {
          handleCloseAlert();
        }

        handleClose();
      }
    };
    const confirmOption = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultConfirmOption"])(confirmMessage, handleCloseAlert, confirmProps, cancelProps);
    setSweetAlertOption(confirmOption);
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "dropdown special-level-menu",
    style: style
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_3__["DropdownMenu"], {
    isOpen: isOpen,
    onClose: handleClose
  }, Object.keys(_lib_constants__WEBPACK_IMPORTED_MODULE_4__["specialLevelListWithColor"]).map(item => {
    const color = _lib_constants__WEBPACK_IMPORTED_MODULE_4__["specialLevelListWithColor"][item];
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_3__["DropdownItem"], {
      key: item,
      className: "special-level-item",
      style: {
        backgroundColor: color
      },
      onClick: e => handleChangeOddsGroup(e, item)
    }, Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__["getSpecialLevelText"])(item));
  })));
};

function mapDispatchToProps(dispatch) {
  const {
    setSpecialLevelDropdownPosition,
    updateSpecialLevel,
    setPlayerList,
    toggleLoader,
    setSweetAlertOption
  } = _actions__WEBPACK_IMPORTED_MODULE_7__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setSpecialLevelDropdownPosition,
    updateSpecialLevel,
    setPlayerList,
    toggleLoader,
    setSweetAlertOption
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    specialLevelDropdownPosition,
    currentActivePlayer,
    playerList
  } = state;
  return Object.assign({}, {
    specialLevelDropdownPosition,
    currentActivePlayer,
    playerList
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(SpecialLevelDropdown));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/StatusCell.jsx":
/*!***********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/StatusCell.jsx ***!
  \***********************************************************************************************/
/*! exports provided: StatusCell, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "StatusCell", function() { return StatusCell; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../lib/services/gridInteractionService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js");
/* harmony import */ var _lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../lib/helpers/playerDataBuilder */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerDataBuilder.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");








const StatusCell = props => {
  const {
    dataItem,
    statusDropdownPosition,
    setCurrentActivePlayer,
    setStatusDropdownPosition,
    type
  } = props;
  const {
    Status,
    IsOnline,
    IsDeposit,
    IsArchived
  } = dataItem;
  const statusName = _lib_constants__WEBPACK_IMPORTED_MODULE_3__["playerStatus"][Status];
  const classname = 'status-text status-' + Status;
  const canUpdate = type === _lib_constants__WEBPACK_IMPORTED_MODULE_3__["licenseeType"].nonApi && !IsArchived && Status !== _lib_constants__WEBPACK_IMPORTED_MODULE_3__["playerStatusByName"].Closed;

  const handleToggleStatus = event => {
    const currentActivePlayer = Object(_lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_6__["buildCurrentActivePlayer"])(dataItem);
    setCurrentActivePlayer(currentActivePlayer);
    const dropdownName = 'statusDropdown';
    const newPosition = _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_5__["default"].getNextDropdownPosition(event.target, statusDropdownPosition, dropdownName);
    setStatusDropdownPosition(newPosition);
  };

  const shouldShowOnlineStatus = IsDeposit && Status === _lib_constants__WEBPACK_IMPORTED_MODULE_3__["playerStatusByName"].Active;
  const onlineText = _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('Online');
  const offlineText = _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('Offline');
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "status-cell"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
    className: classname
  }, statusName || _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('Unknown')), shouldShowOnlineStatus && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
    className: "status-text"
  }, IsOnline ? `(${onlineText})` : `(${offlineText})`), statusName && canUpdate && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", {
    type: "button",
    className: "btn-outline btn-status-menu",
    onClick: handleToggleStatus
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("i", {
    className: "site-caret"
  })));
};

function mapDispatchToProps(dispatch) {
  const {
    setCurrentActivePlayer,
    setStatusDropdownPosition
  } = _actions__WEBPACK_IMPORTED_MODULE_4__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setCurrentActivePlayer,
    setStatusDropdownPosition
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    statusDropdownPosition,
    type
  } = state;
  return Object.assign({}, {
    statusDropdownPosition,
    type
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(StatusCell));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/StatusDropdown.jsx":
/*!***************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/StatusDropdown.jsx ***!
  \***************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @jsroot/components/dropdown */ "./www/js/source/reactpages/components/dropdown/index.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../lib/services/gridInteractionService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ../../lib/helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
/* harmony import */ var _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../../../../../lib/sweetAlert */ "./www/js/source/reactpages/lib/sweetAlert.js");











const StatusDropdown = props => {
  const {
    statusDropdownPosition,
    setStatusDropdownPosition
  } = props;
  const isOpen = statusDropdownPosition !== null;
  const style = statusDropdownPosition && {
    top: statusDropdownPosition.y,
    left: statusDropdownPosition.x
  };

  const handleClose = () => {
    setStatusDropdownPosition(null);
  };

  const {
    currentActivePlayer,
    playerList,
    setPlayerList,
    toggleLoader,
    updatePlayerStatus,
    setSweetAlertOption
  } = props;

  const handleCloseAlert = () => {
    setSweetAlertOption(null);
  };

  const cancelProps = {
    onClick: handleCloseAlert
  };

  const handleChangeOddsGroup = (event, value) => {
    event.preventDefault();
    const messageList = _lib_constants__WEBPACK_IMPORTED_MODULE_4__["messageUpdateState"][value] || [];
    const confirmMessage = messageList.length ? /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", null, messageList[0], /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("br", null), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
      className: "warning"
    }, messageList[1])) : null;
    const confirmProps = {
      onClick: () => {
        if (currentActivePlayer) {
          const {
            licenseeCustId
          } = currentActivePlayer;
          const param = {
            custId: licenseeCustId,
            status: value
          };
          toggleLoader(true);
          updatePlayerStatus(param).then(response => {
            const {
              IsSuccessful
            } = response.data;

            if (IsSuccessful) {
              const {
                ibcCustId
              } = currentActivePlayer;
              const playerId = ibcCustId || licenseeCustId;
              const newPlayerList = _lib_services_gridInteractionService__WEBPACK_IMPORTED_MODULE_5__["default"].updatePlayerItem(playerList, playerId, value, 'Status');
              setPlayerList(newPlayerList);
            }

            const resultMessage = IsSuccessful ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('UpdateSuccess') : _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_7__["default"].getSingleResource('PlayerAdminUpdateFailedMsg');
            const displayIcon = IsSuccessful ? _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].success : _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].error;
            const alertOpt = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultAlertOption"])(resultMessage, handleCloseAlert, null, displayIcon);
            setSweetAlertOption(alertOpt);
            toggleLoader(false);
          }).catch(error => {
            console.log(error.message);
            const message = Object(_lib_helpers_utils__WEBPACK_IMPORTED_MODULE_8__["getErrorMessageForUpdating"])(error);
            ;
            const alertOpt = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultAlertOption"])(message, handleCloseAlert, null, _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].error);
            setSweetAlertOption(alertOpt);
            toggleLoader(false);
          });
        }

        handleClose();
      }
    };
    const customAlertOption = {
      width: 480,
      height: 220
    };
    const confirmOption = Object(_lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["generateDefaultConfirmOption"])(confirmMessage, handleCloseAlert, confirmProps, cancelProps, _lib_sweetAlert__WEBPACK_IMPORTED_MODULE_9__["predefinedIcons"].warning, customAlertOption);
    setSweetAlertOption(confirmOption);
  };

  const displayedStatusList = currentActivePlayer && _lib_constants__WEBPACK_IMPORTED_MODULE_4__["displayedPlayerStatusList"][currentActivePlayer.status];

  if (!displayedStatusList) {
    return null;
  }

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "dropdown status-menu",
    style: style
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_3__["DropdownMenu"], {
    isOpen: isOpen,
    onClose: handleClose
  }, displayedStatusList.map(item => {
    const statusName = _lib_constants__WEBPACK_IMPORTED_MODULE_4__["playerStatus"][item];
    const classname = 'status-item ' + statusName;
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_jsroot_components_dropdown__WEBPACK_IMPORTED_MODULE_3__["DropdownItem"], {
      key: item,
      className: classname,
      onClick: e => handleChangeOddsGroup(e, item)
    }, statusName);
  })));
};

function mapDispatchToProps(dispatch) {
  const {
    setStatusDropdownPosition,
    setPlayerList,
    toggleLoader,
    updatePlayerStatus,
    setSweetAlertOption
  } = _actions__WEBPACK_IMPORTED_MODULE_6__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setStatusDropdownPosition,
    setPlayerList,
    toggleLoader,
    updatePlayerStatus,
    setSweetAlertOption
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    statusDropdownPosition,
    currentActivePlayer,
    playerList
  } = state;
  return Object.assign({}, {
    statusDropdownPosition,
    currentActivePlayer,
    playerList
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(StatusDropdown));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/UserNameCell.jsx":
/*!*************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/UserNameCell.jsx ***!
  \*************************************************************************************************/
/*! exports provided: UserNameCell, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "UserNameCell", function() { return UserNameCell; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../actions */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/index.js");
/* harmony import */ var _lib_helpers_gridHelper__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../../lib/helpers/gridHelper */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/gridHelper.js");
/* harmony import */ var _lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../../lib/helpers/playerDataBuilder */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerDataBuilder.js");
/* harmony import */ var _lib_helpers_contextMenuHelper__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../../lib/helpers/contextMenuHelper */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/contextMenuHelper.js");
/* harmony import */ var _lib_constants__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../../lib/constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }









const itemContextMenuHeight = 28;
const UserNameCell = props => {
  const {
    dataItem,
    hasCommPermissionRead,
    setSelectedPlayer,
    setCurrentActivePlayer,
    setContextMenu,
    contextMenu,
    setSelectedTabName,
    setSelectedSubTabName,
    getTabList,
    type
  } = props;

  const handleSelectPlayer = event => {
    event.preventDefault();
    const newSelectedPlayer = Object(_lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_5__["buildSelectedPlayer"])(dataItem);
    setSelectedPlayer(newSelectedPlayer);
    setSelectedTabName(null);
    setSelectedSubTabName(null);
  };

  const handleEnterContextMenu = event => {
    event.preventDefault();
    const {
      isOpen
    } = contextMenu;

    if (!isOpen) {
      const currentActivePlayer = Object(_lib_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_5__["buildCurrentActivePlayer"])(dataItem);
      const tabList = getTabList();
      const activeMenuList = Object(_lib_helpers_contextMenuHelper__WEBPACK_IMPORTED_MODULE_6__["getActiveTabList"])(currentActivePlayer, tabList, type, hasCommPermissionRead);
      const contextMenuHeight = itemContextMenuHeight * activeMenuList.length;
      const rect = event.target.getBoundingClientRect();
      const position = {
        x: rect.left - 10,
        y: _lib_helpers_gridHelper__WEBPACK_IMPORTED_MODULE_4__["default"].calculateTopPosition(contextMenuHeight, rect.top)
      };
      setContextMenu({
        position,
        isOpen: true
      });
      setCurrentActivePlayer(currentActivePlayer);
    }
  };

  const handleLeaveContextMenu = event => {
    event.preventDefault();

    if (contextMenu.isOpen) {
      setContextMenu(_objectSpread(_objectSpread({}, contextMenu), {}, {
        isOpen: false
      }));
    }
  };

  const {
    Status
  } = dataItem;
  const isClosedOrArchived = Status === _lib_constants__WEBPACK_IMPORTED_MODULE_7__["playerStatusByName"].Closed || Status === _lib_constants__WEBPACK_IMPORTED_MODULE_7__["playerStatusByName"].Archived;
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "username-cell"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", {
    className: "btn-default btn-link username-link",
    onClick: handleSelectPlayer
  }, dataItem.UserName), !isClosedOrArchived && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("button", {
    className: "btn-default btn-link btn-icon cell-menu",
    onMouseEnter: handleEnterContextMenu,
    onMouseLeave: handleLeaveContextMenu
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("i", {
    className: "material-icons"
  }, "edit")));
};

function mapDispatchToProps(dispatch) {
  const {
    setSelectedPlayer,
    setCurrentActivePlayer,
    setContextMenu,
    setSelectedTabName,
    setSelectedSubTabName,
    getTabList
  } = _actions__WEBPACK_IMPORTED_MODULE_3__["ActionCreators"];
  return Object(redux__WEBPACK_IMPORTED_MODULE_2__["bindActionCreators"])({
    setSelectedPlayer,
    setCurrentActivePlayer,
    setContextMenu,
    setSelectedTabName,
    setSelectedSubTabName,
    getTabList
  }, dispatch);
}

function mapStateToProps(state) {
  const {
    contextMenu,
    type
  } = state;
  return Object.assign({}, {
    contextMenu,
    type
  });
}

;
/* harmony default export */ __webpack_exports__["default"] = (Object(react_redux__WEBPACK_IMPORTED_MODULE_1__["connect"])(mapStateToProps, mapDispatchToProps)(UserNameCell));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/index.jsx":
/*!*************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/index.jsx ***!
  \*************************************************************************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _AppContainer_jsx__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./AppContainer.jsx */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/AppContainer.jsx");
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! react-redux */ "./node_modules/react-redux/es/index.js");
/* harmony import */ var _reducers__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../reducers */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/index.js");
/* harmony import */ var _jsroot_lib_store__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @jsroot/lib/store */ "./www/js/source/reactpages/lib/store.js");
/* harmony import */ var _lib_helpers_gridColumnBuilder__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../lib/helpers/gridColumnBuilder */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/gridColumnBuilder.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }








const {
  type
} = window.initialState;
const hasBaPermission = window._pConfig.hasBaPermission === 1;
const hasOddsGroupPermission = window._pConfig.hasOddsGroupPermission === 1;
const hasSpecialLevelPermission = window._pConfig.hasSpecialLevelPermission === 1;
const hasCommPermissionRead = window._pConfig.hasCommPermissionRead === 1;
const hasCommPermissionEdit = window._pConfig.hasCommPermissionEdit === 1;
const displayedColumnList = Object(_lib_helpers_gridColumnBuilder__WEBPACK_IMPORTED_MODULE_6__["buildDefaultColumnList"])(type, hasCommPermissionRead);
const siteSetting = {
  isAllowedCustomerComm: false,
  isAllowedBA: false
};
const lrfFilters = {
  CustomerClass: []
};

const defaultState = _objectSpread(_objectSpread({}, window.initialState), {}, {
  displayedColumnList,
  siteSetting,
  hasBaPermission,
  hasOddsGroupPermission,
  hasSpecialLevelPermission,
  hasCommPermissionRead,
  hasCommPermissionEdit,
  lrfFilters
});

const store = _jsroot_lib_store__WEBPACK_IMPORTED_MODULE_5__["default"].createStore(_reducers__WEBPACK_IMPORTED_MODULE_4__["default"], defaultState);

const App = () => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_redux__WEBPACK_IMPORTED_MODULE_3__["Provider"], {
    store: store
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_AppContainer_jsx__WEBPACK_IMPORTED_MODULE_2__["default"], null));
};

Object(react_dom__WEBPACK_IMPORTED_MODULE_1__["render"])( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(App, null), document.getElementById("app-container"));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js":
/*!*********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js ***!
  \*********************************************************************************/
/*! exports provided: specialLevelListWithColor, oddsGroupListWithStyle, licenseeType, fields, columnList, contextMenuKeys, tabNames, excludedTabNameWithNonDepositAndArchived, excludedTabNameWithClosed, contextMenuList, heightSlicing, searchOptions, defaultPagingOption, playerStatus, playerStatusByName, displayedPlayerStatusList, messageUpdateState, baStatusList, headerText, gridItemDropdownHeight, stringTypeFields, apiInfo, apiUpdateInfo, apiSiteSetting, apiSuggestion, excludedContextMenuWithNonDeposit, excludedContextMenuWithClosed, excludedContextMenuWithArchived, excludedContextMenuWithApiLicensee, playerTypes, emailPattern, maxLapPageSize */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "specialLevelListWithColor", function() { return specialLevelListWithColor; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "oddsGroupListWithStyle", function() { return oddsGroupListWithStyle; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "licenseeType", function() { return licenseeType; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "fields", function() { return fields; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "columnList", function() { return columnList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "contextMenuKeys", function() { return contextMenuKeys; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "tabNames", function() { return tabNames; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "excludedTabNameWithNonDepositAndArchived", function() { return excludedTabNameWithNonDepositAndArchived; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "excludedTabNameWithClosed", function() { return excludedTabNameWithClosed; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "contextMenuList", function() { return contextMenuList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "heightSlicing", function() { return heightSlicing; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "searchOptions", function() { return searchOptions; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "defaultPagingOption", function() { return defaultPagingOption; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "playerStatus", function() { return playerStatus; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "playerStatusByName", function() { return playerStatusByName; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "displayedPlayerStatusList", function() { return displayedPlayerStatusList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "messageUpdateState", function() { return messageUpdateState; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "baStatusList", function() { return baStatusList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "headerText", function() { return headerText; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "gridItemDropdownHeight", function() { return gridItemDropdownHeight; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "stringTypeFields", function() { return stringTypeFields; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "apiInfo", function() { return apiInfo; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "apiUpdateInfo", function() { return apiUpdateInfo; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "apiSiteSetting", function() { return apiSiteSetting; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "apiSuggestion", function() { return apiSuggestion; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "excludedContextMenuWithNonDeposit", function() { return excludedContextMenuWithNonDeposit; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "excludedContextMenuWithClosed", function() { return excludedContextMenuWithClosed; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "excludedContextMenuWithArchived", function() { return excludedContextMenuWithArchived; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "excludedContextMenuWithApiLicensee", function() { return excludedContextMenuWithApiLicensee; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "playerTypes", function() { return playerTypes; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "emailPattern", function() { return emailPattern; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "maxLapPageSize", function() { return maxLapPageSize; });
/* harmony import */ var _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../constants/configInfo */ "./www/js/source/reactpages/Module/constants/configInfo.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");


const specialLevelListWithColor = {
  0: '#fcfcfc',
  1: '#00ff00',
  2: '#008000',
  3: '#8b4513',
  4: '#daa520',
  5: '#006699',
  6: '#666633',
  7: '#a52a2a',
  8: '#0099ff',
  9: '#cc6600',
  10: '#ff0000'
};
const oddsGroupListWithStyle = {
  A: {
    backgroundColor: '#006400',
    color: '#fff'
  },
  B: {
    backgroundColor: '#0099ff',
    color: '#fff'
  },
  C: null,
  D: null,
  E: null,
  F: null
};
const licenseeType = {
  api: 0,
  nonApi: 1
};
const fields = {
  Index: 'Index',
  Status: 'Status',
  UserName: 'UserName',
  SystemId: 'SystemId',
  VendorMemberId: 'VendorMemberId',
  RealName: 'RealName',
  Currency: 'Currency',
  CurrentBalance: 'CurrentBalance',
  OutstandingBalance: 'OutstandingBalance',
  OddsGroup: 'OddsGroup',
  BaStatus: 'BaStatus',
  SpecialLevel: 'SpecialLevel',
  LastTransaction: 'LastTransaction',
  Email: 'Email',
  CommissionGroup: 'CommissionGroup',
  CustomerClass: 'CustomerClass'
};
const columnList = [{
  key: fields.Index,
  value: '#',
  show: true
}, {
  key: fields.Status,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Status'),
  show: true
}, {
  key: fields.UserName,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Username'),
  show: true
}, {
  key: fields.SystemId,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('SystemId'),
  show: true
}, {
  key: fields.VendorMemberId,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('VendorMemberID'),
  show: true
}, {
  key: fields.RealName,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('RealName'),
  show: true
}, {
  key: fields.Currency,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Currency'),
  show: true
}, {
  key: fields.CurrentBalance,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('CurrentBalance'),
  show: true
}, {
  key: fields.OutstandingBalance,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('OutstandingBalance'),
  show: true
}, {
  key: fields.OddsGroup,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('OddsGroup'),
  show: true
}, {
  key: fields.BaStatus,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('BAStatus'),
  show: true
}, {
  key: fields.SpecialLevel,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('SpecialLevel'),
  show: true
}, {
  key: fields.LastTransaction,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('LastTransfer'),
  show: true
}, {
  key: fields.Email,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Email'),
  show: true
}, {
  key: fields.CommissionGroup,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('CommissionGroup'),
  show: true
}, {
  key: fields.CustomerClass,
  value: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('CustomerClass'),
  show: true
}];
const contextMenuKeys = {
  bankInformation: 'bankInformation',
  note: 'note',
  password: 'password',
  fundtransferLimit: 'fundtransferLimit',
  betLimit: 'betLimit',
  statement: 'statement',
  personalMessage: 'personalMessage',
  commissionGroup: 'commissionGroup'
};
const tabNames = {
  playerInfo: 'lblPlayerAdminPlayerInfo',
  statement: 'lblPlayerAdminPlayerStatement',
  betlist: 'lblPlayerAdminBetList',
  betLimit: 'lblPlayerAdminBetLimit',
  setting: 'lblPlayerAdminSetting',
  fundTransferLimit: 'FundTransferLimit',
  password: 'Password',
  bankInfo: 'BankInfo',
  personalMessage: 'PersonalMessage',
  detailTransaction: 'lblPlayerDetailTransaction',
  commissionGroup: 'lblCommissionGroup'
};
const excludedTabNameWithNonDepositAndArchived = [tabNames.statement, tabNames.betlist, tabNames.betLimit, tabNames.detailTransaction, tabNames.commissionGroup];
const excludedTabNameWithClosed = [tabNames.commissionGroup];
const contextMenuList = [{
  key: contextMenuKeys.note,
  name: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Note'),
  tabName: 'lblPlayerAdminNote'
}, {
  key: contextMenuKeys.password,
  name: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Password'),
  tabName: tabNames.setting,
  subTabName: tabNames.password
}, {
  key: contextMenuKeys.bankInformation,
  name: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('BankInformation'),
  tabName: tabNames.setting,
  subTabName: tabNames.bankInfo
}, {
  key: contextMenuKeys.fundtransferLimit,
  name: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('FundtransferLimit'),
  tabName: tabNames.setting,
  subTabName: tabNames.fundTransferLimit
}, {
  key: contextMenuKeys.betLimit,
  name: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('BetLimit'),
  tabName: tabNames.betLimit
}, {
  key: contextMenuKeys.statement,
  name: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Statement'),
  tabName: tabNames.statement
}, {
  key: contextMenuKeys.personalMessage,
  name: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('PersonalMessage'),
  tabName: 'lblPlayerAdminMessage',
  subTabName: tabNames.personalMessage
}, {
  key: contextMenuKeys.commissionGroup,
  name: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('CommissionGroup'),
  tabName: 'lblCommissionGroup'
}];
const heightSlicing = {
  default: 300,
  withAdvancedSearch: 357,
  whenCollapsing: 235
};
const searchOptions = {
  matchPrefix: '1',
  matchCase: '2',
  matchWholeWord: '3',
  matchPrecise: '4'
};
const defaultPagingOption = {
  PageIndex: 1,
  PageSize: 500,
  PageList: [500],
  TotalItem: null
};
const playerStatus = {
  '0': _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Active'),
  '1': _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Locked'),
  '2': _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Closed'),
  '3': _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Archived')
};
const playerStatusByName = {
  'Active': 0,
  'Locked': 1,
  'Closed': 2,
  'Archived': 3
};
const displayedPlayerStatusList = {
  [playerStatusByName.Active]: [playerStatusByName.Closed, playerStatusByName.Locked],
  [playerStatusByName.Locked]: [playerStatusByName.Active, playerStatusByName.Closed]
};
const messageUpdateState = {
  [playerStatusByName.Locked]: [_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('UpdateToLockedConfirmMsg'), _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('WarningToLockedMsg')],
  [playerStatusByName.Closed]: [_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('UpdateToClosedConfirmMsg'), _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('WarningToClosedMsg')],
  [playerStatusByName.Active]: [_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('UpdateToActiveConfirmMsg'), _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('WarningToActiveMsg')]
};
const baStatusList = {
  'All': 0,
  'On': 1,
  'Off': 2
};
const headerText = {
  Index: '',
  Status: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Status'),
  UserName: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Username'),
  SystemId: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('SystemId'),
  VendorMemberId: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('VendorMemberID'),
  RealName: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('RealName'),
  Currency: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Currency'),
  CurrentBalance: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('CurrentBalance'),
  OutstandingBalance: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('OutstandingBalance'),
  OddsGroup: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('OddsGroup'),
  BaStatus: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('BAStatus'),
  SpecialLevel: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('SpecialLevel'),
  LastTransaction: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('LastTransfer'),
  Email: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Email'),
  CommissionGroup: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('CommissionGroup'),
  CustomerClass: _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('CustomerClass')
};
const gridItemDropdownHeight = {
  'specialLevelDropdown': 231,
  'oddsGroupDropdown': 126,
  'statusDropdown': 56
};
const stringTypeFields = [fields.UserName, fields.RealName, fields.Currency, fields.OddsGroup];
const apiInfo = {
  lrfSearch: {
    action: 'api/PlayerList/Search',
    type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].lrf
  },
  lrfDataById: {
    action: 'api/PlayerList/GetPlayerListById',
    type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].lrf
  },
  lapSearch: {
    action: 'PlayerAdminContainer/Search',
    type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].newAdmin
  },
  lapDataById: {
    action: 'PlayerAdminContainer/GetPlayerListById',
    type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].newAdmin
  },
  lapGetPlayerByLicenseeId: {
    action: 'PlayerAdminContainer/GetPlayerById',
    type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].newAdmin
  }
};
const apiUpdateInfo = {
  updateBaStatus: {
    action: 'api/PlayerBaStatus/Update',
    type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].lrf
  },
  updateOddsGroup: {
    action: 'api/PlayerOddsGroup/Update',
    type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].lrf
  },
  updateSpecialLevel: {
    action: 'api/PlayerSpecialLevel/Update',
    type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].lrf
  },
  updatePlayerStatus: {
    action: 'PlayerAdminContainer/UpdatePlayerStatus',
    type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].newAdmin
  }
};
const apiSiteSetting = {
  type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].lrf,
  action: 'api/SiteSetting/GetSetting'
};
const apiSuggestion = {
  action: 'PlayerAdminContainer/GetSuggestionList',
  type: _constants_configInfo__WEBPACK_IMPORTED_MODULE_0__["systemType"].newAdmin
};
const excludedContextMenuWithNonDeposit = [contextMenuKeys.betLimit, contextMenuKeys.statement, contextMenuKeys.commissionGroup];
const excludedContextMenuWithClosed = [contextMenuKeys.password, contextMenuKeys.commissionGroup];
const excludedContextMenuWithArchived = [contextMenuKeys.password, contextMenuKeys.betLimit, contextMenuKeys.statement, contextMenuKeys.commissionGroup];
const excludedContextMenuWithApiLicensee = [contextMenuKeys.password, contextMenuKeys.bankInformation, contextMenuKeys.personalMessage];
const playerTypes = {
  nonDeposit: 1,
  deposit: 2
};
const emailPattern = '^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$';
const maxLapPageSize = 250;

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/contextMenuHelper.js":
/*!*************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/contextMenuHelper.js ***!
  \*************************************************************************************************/
/*! exports provided: isShowContextMenuItem, getActiveTabList */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isShowContextMenuItem", function() { return isShowContextMenuItem; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getActiveTabList", function() { return getActiveTabList; });
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");

const isShowContextMenuItem = (item, isDeposit, isArchived, isClosed, hasCommPermissionRead) => {
  if (!isDeposit) {
    return !_constants__WEBPACK_IMPORTED_MODULE_0__["excludedContextMenuWithNonDeposit"].includes(item.key);
  }

  if (isClosed) {
    return !_constants__WEBPACK_IMPORTED_MODULE_0__["excludedContextMenuWithClosed"].includes(item.key);
  }

  if (isArchived) {
    return !_constants__WEBPACK_IMPORTED_MODULE_0__["excludedContextMenuWithArchived"].includes(item.key);
  }

  if (item.key === _constants__WEBPACK_IMPORTED_MODULE_0__["contextMenuKeys"].commissionGroup) {
    return hasCommPermissionRead;
  }

  return true;
};
const getActiveTabList = (player, tabList, licType, hasCommPermissionRead) => {
  const {
    isDeposit,
    isArchived,
    status
  } = player;
  return _constants__WEBPACK_IMPORTED_MODULE_0__["contextMenuList"].filter(item => {
    if (licType === _constants__WEBPACK_IMPORTED_MODULE_0__["licenseeType"].api && _constants__WEBPACK_IMPORTED_MODULE_0__["excludedContextMenuWithApiLicensee"].includes(item.key)) {
      return false;
    }

    const isClosedPlayer = status === _constants__WEBPACK_IMPORTED_MODULE_0__["playerStatusByName"].Closed;
    return !!tabList.find(t => item.tabName === t.Name) && isShowContextMenuItem(item, isDeposit, isArchived, isClosedPlayer, hasCommPermissionRead);
  });
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/gridColumnBuilder.js":
/*!*************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/gridColumnBuilder.js ***!
  \*************************************************************************************************/
/*! exports provided: buildDefaultColumnList, buildColumnList */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "buildDefaultColumnList", function() { return buildDefaultColumnList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "buildColumnList", function() { return buildColumnList; });
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


const excludedKeyForApiType = [_constants__WEBPACK_IMPORTED_MODULE_0__["fields"].RealName, _constants__WEBPACK_IMPORTED_MODULE_0__["fields"].Email, _constants__WEBPACK_IMPORTED_MODULE_0__["fields"].LastTransaction];
const excludedKeyForAthenaType = [_constants__WEBPACK_IMPORTED_MODULE_0__["fields"].VendorMemberId];
const buildDefaultColumnList = (licType, hasCommPermissionRead) => {
  const isApiType = licType === _constants__WEBPACK_IMPORTED_MODULE_0__["licenseeType"].api;
  return _constants__WEBPACK_IMPORTED_MODULE_0__["columnList"].filter(item => {
    const show = item.key === _constants__WEBPACK_IMPORTED_MODULE_0__["fields"].CommissionGroup ? hasCommPermissionRead : item.key !== _constants__WEBPACK_IMPORTED_MODULE_0__["fields"].BaStatus;
    const showByLicenseeType = isApiType ? !excludedKeyForApiType.includes(item.key) : !excludedKeyForAthenaType.includes(item.key);
    return show && showByLicenseeType;
  }).map(item => _objectSpread({}, item));
};
const buildColumnList = (licType, isAllowedBA, hasCommPermissionRead) => {
  const isApiType = licType === _constants__WEBPACK_IMPORTED_MODULE_0__["licenseeType"].api;
  return _constants__WEBPACK_IMPORTED_MODULE_0__["columnList"].filter(item => {
    if (item.key === _constants__WEBPACK_IMPORTED_MODULE_0__["fields"].BaStatus) {
      return isAllowedBA;
    } else if (item.key === _constants__WEBPACK_IMPORTED_MODULE_0__["fields"].CommissionGroup) {
      return hasCommPermissionRead;
    } else {
      return isApiType ? !excludedKeyForApiType.includes(item.key) : !excludedKeyForAthenaType.includes(item.key);
    }
  }).map(item => _objectSpread({}, item));
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/gridHelper.js":
/*!******************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/gridHelper.js ***!
  \******************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return GridHelper; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @9softstudio/react-grid */ "./node_modules/@9softstudio/react-grid/dist/index.js");
/* harmony import */ var _9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _components_grid_HeaderCustomerClassCell__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../components/grid/HeaderCustomerClassCell */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/grid/HeaderCustomerClassCell.jsx");




class GridHelper {
  static renderHeader(displayedColumn, canSelectPlayers, hasCommPermissionRead) {
    const {
      Index,
      Status,
      UserName,
      SystemId,
      VendorMemberId,
      RealName,
      Currency,
      CurrentBalance,
      OutstandingBalance,
      OddsGroup,
      BaStatus,
      SpecialLevel,
      LastTransaction,
      Email,
      CommissionGroup,
      CustomerClass
    } = _constants__WEBPACK_IMPORTED_MODULE_2__["fields"];
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderRow"], {
      style: {
        height: 32
      }
    }, canSelectPlayers && displayedColumn[Index] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 30
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", {
      type: "checkbox",
      id: "chk-cust-0",
      "data-username": ""
    })), displayedColumn[Status] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 80,
      sortBy: Status
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].Status), displayedColumn[UserName] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 120,
      sortBy: UserName
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].UserName), displayedColumn[SystemId] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 100
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].SystemId), displayedColumn[VendorMemberId] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 70
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].VendorMemberId), displayedColumn[RealName] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 80,
      sortBy: RealName
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].RealName), displayedColumn[Currency] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 60,
      sortBy: Currency
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].Currency), displayedColumn[CurrentBalance] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 80,
      sortBy: CurrentBalance
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].CurrentBalance), displayedColumn[OutstandingBalance] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 80,
      sortBy: OutstandingBalance
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].OutstandingBalance), displayedColumn[OddsGroup] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 50,
      sortBy: OddsGroup
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].OddsGroup), displayedColumn[BaStatus] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 50,
      sortBy: BaStatus
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].BaStatus), displayedColumn[SpecialLevel] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 50,
      sortBy: SpecialLevel
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].SpecialLevel), displayedColumn[LastTransaction] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 60,
      sortBy: LastTransaction
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].LastTransaction), displayedColumn[Email] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 100
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].Email), hasCommPermissionRead && displayedColumn[CommissionGroup] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 50
    }, _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].CommissionGroup), displayedColumn[CustomerClass] && /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_9softstudio_react_grid__WEBPACK_IMPORTED_MODULE_1__["HeaderCell"], {
      colWidth: 80,
      sortBy: CustomerClass
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_components_grid_HeaderCustomerClassCell__WEBPACK_IMPORTED_MODULE_3__["default"], {
      text: _constants__WEBPACK_IMPORTED_MODULE_2__["headerText"].CustomerClass
    })));
  }

  static calculateTopPosition(dropdownHeight = 0, currentTop) {
    const innerHeight = window.innerHeight;
    const expectedHeight = dropdownHeight + currentTop + 20;
    return expectedHeight > innerHeight ? currentTop - dropdownHeight : currentTop;
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerDataBuilder.js":
/*!*************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerDataBuilder.js ***!
  \*************************************************************************************************/
/*! exports provided: buildSelectedPlayer, buildLapPlayerList, buildCurrentActivePlayer */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "buildSelectedPlayer", function() { return buildSelectedPlayer; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "buildLapPlayerList", function() { return buildLapPlayerList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "buildCurrentActivePlayer", function() { return buildCurrentActivePlayer; });
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

const buildSelectedPlayer = dataItem => {
  const {
    IbcCustId,
    UserName,
    Currency,
    IsDeposit,
    IsArchived,
    LicenseeCustId,
    CurrencyId,
    CreatedDate
  } = dataItem;
  return {
    ibcCustId: IbcCustId,
    licenseeCustId: LicenseeCustId,
    userName: UserName,
    currency: Currency,
    isDeposit: IsDeposit,
    isArchived: IsArchived,
    currencyId: CurrencyId,
    createdDate: CreatedDate,
    commissionGroup: dataItem.CommissionGroup,
    customerClass: dataItem.CustomerClass,
    isClosed: dataItem.Status == 2
  };
};
const buildLapPlayerList = playerList => {
  if (!playerList) {
    return [];
  }

  return playerList.map(item => {
    const {
      CustId,
      UserName,
      CurrentBalance,
      Currency,
      Status,
      RealName,
      email,
      isOnline,
      IBCCustId,
      isArchived,
      LastTransaction,
      CurrencyCode,
      OutBalance,
      IBCUserName,
      Vender_Member_ID,
      CreatedDateString
    } = item;
    return {
      IbcCustId: IBCCustId,
      LicenseeCustId: CustId,
      UserName,
      SystemId: IBCUserName,
      VendorMemberId: Vender_Member_ID,
      Currency: CurrencyCode,
      CurrentBalance,
      OutstandingBalance: OutBalance,
      Status,
      RealName,
      LastTransaction,
      Email: email,
      IsOnline: isOnline,
      IsDeposit: IBCCustId > 0,
      IsArchived: !!isArchived,
      CurrencyId: Currency,
      CreatedDate: CreatedDateString,
      CommissionGroup: item.CommissionGroup,
      CustomerClass: item.CustomerClass
    };
  });
};
const buildCurrentActivePlayer = dataItem => {
  const {
    IbcCustId,
    UserName,
    Currency,
    Status,
    OddsGroup,
    SpecialLevel,
    IsDeposit,
    IsArchived,
    LicenseeCustId,
    CurrencyId,
    CreatedDate
  } = dataItem;
  return {
    ibcCustId: IbcCustId,
    licenseeCustId: LicenseeCustId,
    userName: UserName,
    currency: Currency,
    status: Status,
    oddsGroup: OddsGroup,
    specialLevel: SpecialLevel,
    isDeposit: IsDeposit,
    isArchived: IsArchived,
    currencyId: CurrencyId,
    createdDate: CreatedDate,
    commissionGroup: dataItem.CommissionGroup,
    customerClass: dataItem.CustomerClass,
    isClosed: dataItem.Status == 2
  };
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerListDataHelper.js":
/*!****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerListDataHelper.js ***!
  \****************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return PlayerListDataHelper; });
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");

class PlayerListDataHelper {
  static mergePlayerList(lrfList, lapList) {
    if (!lrfList || !lapList) {
      return [];
    }

    const lrfDic = this.toLrfPlayerDic(lrfList);
    return lapList.reduce((result, lapItem) => {
      const lrfItem = lrfDic[lapItem.IBCCustId] || {};
      const playerItem = this.mergePlayerItem(lrfItem, lapItem);
      result.push(playerItem);
      return result;
    }, []);
  }

  static mergePlayerItem(lrfItem, lapItem) {
    const {
      OddsGroup,
      BaStatus,
      SpecialLevel
    } = lrfItem;
    const {
      Status,
      RealName,
      email,
      isOnline,
      IBCCustId,
      isArchived,
      LastTransaction,
      CurrencyCode,
      OutBalance,
      IBCUserName,
      Vender_Member_ID,
      CreatedDateString
    } = lapItem;
    return {
      IbcCustId: IBCCustId,
      LicenseeCustId: lapItem.CustId,
      UserName: lapItem.UserName,
      SystemId: IBCUserName,
      VendorMemberId: Vender_Member_ID,
      Currency: CurrencyCode,
      CurrentBalance: lapItem.CurrentBalance,
      OutstandingBalance: OutBalance,
      OddsGroup,
      BaStatus,
      SpecialLevel,
      Status,
      RealName,
      LastTransaction,
      Email: email,
      IsOnline: isOnline,
      IsDeposit: IBCCustId > 0,
      IsArchived: !!isArchived,
      CurrencyId: lapItem.Currency,
      CreatedDate: CreatedDateString,
      CommissionGroup: lrfItem.CommissionGroup,
      CustomerClass: lrfItem.CustomerClass
    };
  }

  static toLrfPlayerDic(lrfList) {
    const lrfPlayerDic = lrfList.reduce((result, currentItem) => {
      result[currentItem.IbcCustId] = currentItem;
      return result;
    }, {});
    return lrfPlayerDic;
  }

  static createEmptyResult() {
    return {
      playerList: [],
      totalItem: 0
    };
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js":
/*!*************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js ***!
  \*************************************************************************************/
/*! exports provided: getSpecialLevelText, getBaStatusText, getPlayerStatus, buildDisplayedColumnObject, isNonDepositOrArchived, getNoneDepositOrArchivedList, getCustIdListFromLapData, getErrorMessageForUpdating, getCustomerClassText */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getSpecialLevelText", function() { return getSpecialLevelText; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getBaStatusText", function() { return getBaStatusText; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getPlayerStatus", function() { return getPlayerStatus; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "buildDisplayedColumnObject", function() { return buildDisplayedColumnObject; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isNonDepositOrArchived", function() { return isNonDepositOrArchived; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getNoneDepositOrArchivedList", function() { return getNoneDepositOrArchivedList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getCustIdListFromLapData", function() { return getCustIdListFromLapData; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getErrorMessageForUpdating", function() { return getErrorMessageForUpdating; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "getCustomerClassText", function() { return getCustomerClassText; });
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
/* harmony import */ var _jsroot_lib_htmlHelper__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/lib/htmlHelper */ "./www/js/source/reactpages/lib/htmlHelper.js");



const getSpecialLevelText = value => {
  return value === undefined || value === null ? '' : Number(value) === 0 ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('None') : value;
};
const getBaStatusText = value => {
  return value ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('On') : _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Off');
};
const getPlayerStatus = value => {
  const displayedValue = _constants__WEBPACK_IMPORTED_MODULE_0__["playerStatus"][value];
  return displayedValue || '';
};
const buildDisplayedColumnObject = displayedColumnList => {
  return displayedColumnList.reduce((accumulator, currentItem) => {
    accumulator[currentItem.key] = currentItem.show;
    return accumulator;
  }, {});
};
const isNonDepositOrArchived = lapItem => lapItem.IBCCustId === 0 || lapItem.isArchived;
const getNoneDepositOrArchivedList = lapPlayerList => {
  return !!lapPlayerList && !!lapPlayerList.length ? lapPlayerList.filter(player => isNonDepositOrArchived(player)) : [];
};
const getCustIdListFromLapData = lapPlayerList => {
  return lapPlayerList.reduce((result, item) => {
    if (!isNonDepositOrArchived(item)) {
      result.push(item.IBCCustId);
    }

    return result;
  }, []);
};
const getErrorMessageForUpdating = errorInfo => {
  const defaultKey = 'HttpError_Default';

  if (!errorInfo) {
    return _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource(defaultKey);
  }

  const resourceKey = isUnauthorizedResponse(errorInfo) ? 'FunctionDeniedMsg' : defaultKey;
  return Object(_jsroot_lib_htmlHelper__WEBPACK_IMPORTED_MODULE_2__["decode"])(_jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource(resourceKey));
};

const isUnauthorizedResponse = errorInfo => {
  if (!errorInfo.response) {
    return false;
  }

  const {
    status
  } = errorInfo.response;
  return status === 403 || status === 401;
};

const getCustomerClassText = value => {
  return value === "0" || value == "" || value == undefined ? _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('None') : value;
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/reportExcel.js":
/*!***********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/reportExcel.js ***!
  \***********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return ReportExcel; });
/* harmony import */ var _jsroot_lib_excel__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/excel */ "./www/js/source/reactpages/lib/excel.js");
/* harmony import */ var _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/numberFormat */ "./www/js/source/reactpages/lib/numberFormat.js");
/* harmony import */ var _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/constants/excelStyle */ "./www/js/source/reactpages/constants/excelStyle.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _helpers_utils__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! jquery */ "./node_modules/jquery/dist/jquery.js");
/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_5__);






const HEADER_ROW_INDEX = 0;
const DATA_ROW_INDEX = 1;
const MAIN_SECTION_STYLE = 'mainSectionStyle';
const displayedTextByField = {
  [_constants__WEBPACK_IMPORTED_MODULE_3__["fields"].SpecialLevel]: value => Object(_helpers_utils__WEBPACK_IMPORTED_MODULE_4__["getSpecialLevelText"])(value),
  [_constants__WEBPACK_IMPORTED_MODULE_3__["fields"].BaStatus]: value => Object(_helpers_utils__WEBPACK_IMPORTED_MODULE_4__["getBaStatusText"])(value),
  [_constants__WEBPACK_IMPORTED_MODULE_3__["fields"].Status]: value => Object(_helpers_utils__WEBPACK_IMPORTED_MODULE_4__["getPlayerStatus"])(value)
};
const fieldListWithEmptyValueByStatus = [_constants__WEBPACK_IMPORTED_MODULE_3__["fields"].CurrentBalance, _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].OutstandingBalance, _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].OddsGroup, _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].BaStatus, _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].SpecialLevel];
class ReportExcel {
  constructor(data, displayedColumn, hasCommPermissionRead, title) {
    this.data = data;
    this.title = title;
    this.displayedColumn = displayedColumn;
    this.hasCommPermissionRead = hasCommPermissionRead;
    this.worksheet = {};
    this.currentRowIndex = DATA_ROW_INDEX;
    this.columnConfigs = this._getcolumnConfigs();
  }

  export(onFinished) {
    _jsroot_lib_excel__WEBPACK_IMPORTED_MODULE_0__["default"].process(() => {
      this._createHeader();

      this._createBodyData();

      this._createExcelConfig();

      _jsroot_lib_excel__WEBPACK_IMPORTED_MODULE_0__["default"].export(this.worksheet, this.title);
      onFinished && onFinished();
    });
  }

  _createHeader() {
    const totalColumn = this.columnConfigs.length;

    for (let i = 0; i < totalColumn; i++) {
      const field = this.columnConfigs[i].field;
      const address = _jsroot_lib_excel__WEBPACK_IMPORTED_MODULE_0__["default"].getCellAddress({
        c: i,
        r: HEADER_ROW_INDEX
      });
      this.worksheet[address] = {
        t: "s",
        v: _constants__WEBPACK_IMPORTED_MODULE_3__["headerText"][field] || '',
        s: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["headerCellStyle"]
      };
    }
  }

  _createBodyData() {
    const totalColumn = this.columnConfigs.length;

    for (let i = 0; i < this.data.length; i++) {
      const item = this.data[i];

      for (let j = 0; j < totalColumn; j++) {
        const address = _jsroot_lib_excel__WEBPACK_IMPORTED_MODULE_0__["default"].getCellAddress({
          c: j,
          r: this.currentRowIndex
        });
        this.worksheet[address] = this._buildDataRow(item, j);
      }

      this.currentRowIndex++;
    }
  }

  _buildDataRow(item, colIndex) {
    const columnConfig = this.columnConfigs[colIndex];
    const shouldHideValue = !item.IsDeposit || item.IsArchived;

    if (shouldHideValue && fieldListWithEmptyValueByStatus.indexOf(columnConfig.field) !== -1) {
      return {
        t: 's',
        v: '',
        s: columnConfig[MAIN_SECTION_STYLE]
      };
    }

    let value = item[columnConfig.field];

    if (columnConfig.field === _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].CurrentBalance || columnConfig.field === _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].OutstandingBalance) {
      value = _jsroot_lib_numberFormat__WEBPACK_IMPORTED_MODULE_1__["default"].roundDown(value, 2);
    }

    if (columnConfig.field === _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].CommissionGroup) {
      value = jquery__WEBPACK_IMPORTED_MODULE_5___default()(".td-comm-" + item.IbcCustId).html();
    }

    if (columnConfig.field === _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].CustomerClass) {
      value = Object(_helpers_utils__WEBPACK_IMPORTED_MODULE_4__["getCustomerClassText"])(value);
    }

    const displayedTextFunc = displayedTextByField[columnConfig.field];
    const displayedValue = this._shouldSetEmpty(value) ? '' : displayedTextFunc ? displayedTextFunc(value) : value;
    return {
      t: _jsroot_lib_excel__WEBPACK_IMPORTED_MODULE_0__["default"].getCellType(displayedValue),
      v: displayedValue,
      s: columnConfig[MAIN_SECTION_STYLE]
    };
  }

  _shouldSetEmpty(value) {
    return value === undefined || value === null;
  }

  _createExcelConfig() {
    const cellStart = _jsroot_lib_excel__WEBPACK_IMPORTED_MODULE_0__["default"].getCellAddress({
      r: 0,
      c: 0
    });
    const cellEnd = _jsroot_lib_excel__WEBPACK_IMPORTED_MODULE_0__["default"].getCellAddress({
      r: this.currentRowIndex,
      c: this.columnConfigs.length
    });
    this.worksheet["!ref"] = `${cellStart}:${cellEnd}`;
    this.worksheet['!cols'] = this.columnConfigs.map(x => ({
      wpx: x.width
    }));
  }

  _getcolumnConfigs() {
    var result = [{
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].Status,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].left,
      width: 60
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].UserName,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].left,
      width: 200
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].SystemId,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].left,
      width: 200
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].VendorMemberId,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].left,
      width: 150
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].RealName,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].left,
      width: 150
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].Currency,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].center,
      width: 80
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].CurrentBalance,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].decimal,
      width: 130
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].OutstandingBalance,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].decimal,
      width: 130
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].OddsGroup,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].center,
      width: 70
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].BaStatus,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].center,
      width: 70
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].SpecialLevel,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].center,
      width: 70
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].LastTransaction,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].left,
      width: 150
    }, {
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].Email,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].left,
      width: 200
    }];

    if (this.hasCommPermissionRead) {
      result.push({
        field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].CommissionGroup,
        [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].center,
        width: 120
      });
    }

    result.push({
      field: _constants__WEBPACK_IMPORTED_MODULE_3__["fields"].CustomerClass,
      [MAIN_SECTION_STYLE]: _jsroot_constants_excelStyle__WEBPACK_IMPORTED_MODULE_2__["bodyCellStyle"].center,
      width: 120
    });
    return result.filter(x => !!this.displayedColumn[x.field]);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js":
/*!*******************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/gridInteractionService.js ***!
  \*******************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return GridInteractionService; });
/* harmony import */ var _helpers_gridHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../helpers/gridHelper */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/gridHelper.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _jsroot_lib_dataReducer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @jsroot/lib/dataReducer */ "./www/js/source/reactpages/lib/dataReducer.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }




class GridInteractionService {
  static getNextDropdownPosition(element, currentPosition, dropdownItemName) {
    if (!element || currentPosition) {
      return null;
    }

    const dropdownHeight = _constants__WEBPACK_IMPORTED_MODULE_1__["gridItemDropdownHeight"][dropdownItemName];
    const rect = element.getBoundingClientRect();
    return {
      x: rect.left,
      y: _helpers_gridHelper__WEBPACK_IMPORTED_MODULE_0__["default"].calculateTopPosition(dropdownHeight, rect.top + 20)
    };
  }

  static updatePlayerItem(playerList, custId, value, fieldName) {
    let itemIndex = playerList.findIndex(x => x.IbcCustId === custId);

    if (itemIndex === -1) {
      itemIndex = playerList.findIndex(x => x.LicenseeCustId === custId);
    }

    if (itemIndex !== -1) {
      const newPlayerItem = _objectSpread(_objectSpread({}, playerList[itemIndex]), {}, {
        [fieldName]: value
      });

      return Object(_jsroot_lib_dataReducer__WEBPACK_IMPORTED_MODULE_2__["updateObjectInArray"])(playerList, itemIndex, newPlayerItem);
    }

    return [...playerList];
  }

  static updatePlayerWithBaStatus(playerList, ibcCustId, isBa, oddsGroup) {
    const itemIndex = playerList.findIndex(x => x.IbcCustId === ibcCustId);

    const newPlayerItem = _objectSpread(_objectSpread({}, playerList[itemIndex]), {}, {
      BaStatus: isBa,
      OddsGroup: oddsGroup
    });

    return Object(_jsroot_lib_dataReducer__WEBPACK_IMPORTED_MODULE_2__["updateObjectInArray"])(playerList, itemIndex, newPlayerItem);
  }

  static sortPlayerList(dataList, sortBy, sortOrder) {
    if (!dataList || dataList.length === 0) {
      return [];
    }

    const isSortAscending = sortOrder.toLowerCase() === 'asc';
    const newDataList = [...dataList];
    const isTringType = _constants__WEBPACK_IMPORTED_MODULE_1__["stringTypeFields"].includes(sortBy);
    const isDateTimeField = sortBy === _constants__WEBPACK_IMPORTED_MODULE_1__["fields"].LastTransaction;
    newDataList.sort((currentItem, nextItem) => {
      const currentValue = currentItem[sortBy];
      const nextValue = nextItem[sortBy];

      if (isTringType) {
        return this.sortStringValue(currentValue || '', nextValue || '', isSortAscending);
      }

      if (isDateTimeField) {
        const currentItemDate = currentValue ? new Date(currentValue) : 0;
        const nextItemDate = nextValue ? new Date(nextValue) : 0;
        const comparingResult = (this.isValidDate(currentItemDate) ? currentItemDate : 0) - (this.isValidDate(nextItemDate) ? nextItemDate : 0);
        return isSortAscending ? comparingResult : -comparingResult;
      }

      var cEmpty = currentValue === undefined || currentValue === null || currentValue === '';
      var nEmpty = nextValue === undefined || nextValue === null || nextValue === '';

      if (cEmpty || nEmpty) {
        var c = cEmpty ? 0 : 1;
        var n = nEmpty ? 0 : 1;
        return isSortAscending ? c - n : n - c;
      }

      var diff = currentValue - nextValue;

      if (isNaN(diff)) {
        return this.sortStringValue(currentValue.toString(), nextValue.toString(), isSortAscending);
      }

      return isSortAscending ? diff : -diff;
    });
    return newDataList;
  }

  static sortStringValue(current, next, isSortAscending) {
    const currentUpperCase = current.toUpperCase();
    const nextUpperCase = next.toUpperCase();
    const sortResult = currentUpperCase > nextUpperCase ? 1 : currentUpperCase < nextUpperCase ? -1 : 0;
    return isSortAscending ? sortResult : -sortResult;
  }

  static isValidDate(d) {
    return d instanceof Date && !isNaN(d);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/lapPlayerQueryService.js":
/*!******************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/lapPlayerQueryService.js ***!
  \******************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return LapPlayerQueryService; });
/* harmony import */ var _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../helpers/playerListDataHelper */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerListDataHelper.js");
/* harmony import */ var _helpers_utils__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../helpers/utils */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/utils.js");
/* harmony import */ var _helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../helpers/playerDataBuilder */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerDataBuilder.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }





class LapPlayerQueryService {
  static fetchData(param, commonService, apiInfo) {
    const {
      lrfDataById,
      lapSearch
    } = apiInfo;
    let request = null;

    if (this._shouldBatchRequest(param)) {
      request = this._batchRequest(param, _constants__WEBPACK_IMPORTED_MODULE_3__["maxLapPageSize"], commonService, lapSearch);
    } else {
      request = commonService.fetchData(lapSearch.type, lapSearch.action, _objectSpread({}, param));
    }

    return request.then(lapResponse => {
      const {
        PlayerList: lapPlayerList = [],
        TotalItem: lapTotalItem = 0
      } = lapResponse.data;

      if (!lapPlayerList || !lapPlayerList.length) {
        return _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__["default"].createEmptyResult();
      }

      return this._fetchFromLrf(param, lrfDataById, lapPlayerList, commonService, lapTotalItem);
    }).catch(() => _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__["default"].createEmptyResult());
    ;
  }

  static _shouldBatchRequest(param) {
    const {
      isMatchPrecise,
      bankAccountNumber,
      userName,
      email,
      pageSize
    } = param;
    return (!userName || !isMatchPrecise) && !bankAccountNumber && !email && pageSize > _constants__WEBPACK_IMPORTED_MODULE_3__["maxLapPageSize"];
  }

  static _batchRequest(param, maxLapPageSize, commonService, lapSearch) {
    const promises = this._buildBatchPromise(param, maxLapPageSize, commonService, lapSearch);

    return commonService.fetchMultiple(promises, (...responses) => {
      const lapTotalItem = responses[0].data.TotalItem;
      const offset = (param.pageIndex - 1) * param.pageSize;
      const lapPlayerList = responses.reduce((result, current) => {
        const currentPlayerList = current.data.PlayerList;

        if (offset + result.length + currentPlayerList.length > lapTotalItem) {
          return result;
        }

        return [...result, ...currentPlayerList];
      }, []);
      return {
        data: {
          PlayerList: lapPlayerList,
          TotalItem: lapTotalItem
        }
      };
    });
  }

  static _buildBatchPromise(param, maxLapPageSize, commonService, lapSearch) {
    const {
      pageIndex,
      pageSize
    } = param;
    const batchSize = pageSize / maxLapPageSize;
    const pageIndexStart = batchSize * (pageIndex - 1);
    const promises = [];

    for (let batchIndex = 1; batchIndex <= batchSize; batchIndex++) {
      const batchParam = _objectSpread(_objectSpread({}, param), {}, {
        pageIndex: pageIndexStart + batchIndex,
        pageSize: maxLapPageSize
      });

      promises.push(commonService.fetchData(lapSearch.type, lapSearch.action, _objectSpread({}, batchParam)).catch(error => {
        console.log(error.message);
        window.alert(LangHelper.getSingleResource('HttpError_Default'));
        return {
          data: {
            PlayerList: [],
            TotalItem: 0
          }
        };
      }));
    }

    return promises;
  }

  static _fetchFromLrf(param, lrfDataById, lapPlayerList, commonService, lapTotalItem) {
    const noneDepositOrArchivedList = Object(_helpers_utils__WEBPACK_IMPORTED_MODULE_1__["getNoneDepositOrArchivedList"])(lapPlayerList);
    const depositIdList = Object(_helpers_utils__WEBPACK_IMPORTED_MODULE_1__["getCustIdListFromLapData"])(lapPlayerList);

    if (depositIdList.length === 0) {
      return {
        playerList: Object(_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_2__["buildLapPlayerList"])(noneDepositOrArchivedList),
        totalItem: lapTotalItem
      };
    }

    const lrfParam = {
      currencyIdList: param.currencyIdList,
      idList: depositIdList.join(','),
      specialLevel: param.specialLevel,
      oddsGroup: param.oddsGroup,
      baFilter: param.baFilter,
      customerClass: param.customerClass,
      pageSize: param.pageSize,
      pageIndex: 1
    };
    return commonService.fetchData(lrfDataById.type, lrfDataById.action, lrfParam).then(lrfResponse => {
      const {
        DataList: lrfPlayerList
      } = lrfResponse.data;

      if (!lrfPlayerList || !lrfPlayerList.length) {
        return {
          playerList: Object(_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_2__["buildLapPlayerList"])(noneDepositOrArchivedList),
          totalItem: lapTotalItem - lapPlayerList.length + noneDepositOrArchivedList.length
        };
      }

      const lrfPlayerDic = _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__["default"].toLrfPlayerDic(lrfPlayerList);
      const filteredLapPlayerList = lapPlayerList.filter(player => Object(_helpers_utils__WEBPACK_IMPORTED_MODULE_1__["isNonDepositOrArchived"])(player) || !!lrfPlayerDic[player.IBCCustId]);
      const playerList = _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__["default"].mergePlayerList(lrfPlayerList, filteredLapPlayerList);
      return {
        playerList,
        totalItem: lapTotalItem - lapPlayerList.length + filteredLapPlayerList.length
      };
    }).catch(() => {
      return {
        playerList: Object(_helpers_playerDataBuilder__WEBPACK_IMPORTED_MODULE_2__["buildLapPlayerList"])(nonDepositOrArchivedList),
        totalItem: nonDepositOrArchivedList.length
      };
    });
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/lrfFiltersService.js":
/*!**************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/lrfFiltersService.js ***!
  \**************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return LrfFiltersService; });
/* harmony import */ var _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../../lib/factoryCommonService */ "./www/js/source/reactpages/Module/lib/factoryCommonService.js");
/* harmony import */ var _constants_configInfo__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../../../constants/configInfo */ "./www/js/source/reactpages/Module/constants/configInfo.js");


class LrfFiltersService {
  constructor() {
    this.commonService = _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__["default"].createInstance();
  }

  fetchData() {
    return this.commonService.fetchData(_constants_configInfo__WEBPACK_IMPORTED_MODULE_1__["systemType"].lrf, 'api/PlayerList/LRFFilters', {});
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/lrfPlayerQueryService.js":
/*!******************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/lrfPlayerQueryService.js ***!
  \******************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return LrfPlayerQueryService; });
/* harmony import */ var _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../helpers/playerListDataHelper */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerListDataHelper.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


class LrfPlayerQueryService {
  static fetchData(param, commonService, apiInfo) {
    const {
      lrfSearch,
      lapDataById
    } = apiInfo;
    return commonService.fetchData(lrfSearch.type, lrfSearch.action, _objectSpread({}, param)).then(lrfResponse => {
      const {
        DataList: lrfPlayerList,
        TotalItem
      } = lrfResponse.data;

      if (!lrfPlayerList || !lrfPlayerList.length) {
        return _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__["default"].createEmptyResult();
      }

      return this._fetchFromLap(lrfPlayerList, lapDataById, commonService, TotalItem);
    }).catch(() => _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__["default"].createEmptyResult());
  }

  static _fetchFromLap(lrfPlayerList, lapDataById, commonService, lrfTotalItem) {
    const lapParam = {
      idList: lrfPlayerList.map(x => x.IbcCustId)
    };
    return commonService.fetchData(lapDataById.type, lapDataById.action, lapParam).then(lapResponse => {
      const {
        PlayerList: lapPlayerList
      } = lapResponse.data;

      if (!lapPlayerList || !lapPlayerList.length) {
        return _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__["default"].createEmptyResult();
      }

      const playerList = _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__["default"].mergePlayerList(lrfPlayerList, lapPlayerList);
      return {
        playerList,
        totalItem: lrfTotalItem
      };
    }).catch(() => _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_0__["default"].createEmptyResult());
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerCommandService.js":
/*!*****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerCommandService.js ***!
  \*****************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return PlayerCommandService; });
/* harmony import */ var _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../../lib/factoryCommonService */ "./www/js/source/reactpages/Module/lib/factoryCommonService.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



class PlayerCommandService {
  constructor() {
    this.commonService = _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__["default"].createInstance();
  }

  updateBaStatus(param) {
    const {
      updateBaStatus
    } = _constants__WEBPACK_IMPORTED_MODULE_1__["apiUpdateInfo"];
    return this.commonService.fetchData(updateBaStatus.type, updateBaStatus.action, _objectSpread({}, param));
  }

  updateOddsGroup(param) {
    const {
      updateOddsGroup
    } = _constants__WEBPACK_IMPORTED_MODULE_1__["apiUpdateInfo"];
    return this.commonService.fetchData(updateOddsGroup.type, updateOddsGroup.action, _objectSpread({}, param));
  }

  updateSpecialLevel(param) {
    const {
      updateSpecialLevel
    } = _constants__WEBPACK_IMPORTED_MODULE_1__["apiUpdateInfo"];
    return this.commonService.fetchData(updateSpecialLevel.type, updateSpecialLevel.action, _objectSpread({}, param));
  }

  updatePlayerStatus(param) {
    const {
      updatePlayerStatus
    } = _constants__WEBPACK_IMPORTED_MODULE_1__["apiUpdateInfo"];
    return this.commonService.fetchData(updatePlayerStatus.type, updatePlayerStatus.action, _objectSpread({}, param));
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerItemQueryService.js":
/*!*******************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerItemQueryService.js ***!
  \*******************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return PlayerItemQueryService; });
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../helpers/playerListDataHelper */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/helpers/playerListDataHelper.js");


class PlayerItemQueryService {
  static getPlayerById(param, commonService, apiInfo) {
    const {
      lrfDataById,
      lapGetPlayerByLicenseeId
    } = apiInfo;
    const {
      ibcCustId,
      licenseeCustId
    } = param;
    const lrfParam = {
      idList: ibcCustId,
      specialLevel: Object.keys(_constants__WEBPACK_IMPORTED_MODULE_0__["specialLevelListWithColor"]).map(item => item).join(','),
      pageSize: _constants__WEBPACK_IMPORTED_MODULE_0__["defaultPagingOption"].PageSize,
      pageIndex: 1
    };
    const lapParam = {
      custId: licenseeCustId
    };
    const lrfPromise = ibcCustId > 0 ? commonService.fetchData(lrfDataById.type, lrfDataById.action, lrfParam) : null;
    const lapPromise = commonService.fetchData(lapGetPlayerByLicenseeId.type, lapGetPlayerByLicenseeId.action, lapParam);
    return this._getResult(lrfPromise, lapPromise, commonService);
  }

  static _getResult(lrfPromise, lapPromise, commonService) {
    return commonService.fetchMultiple([lrfPromise, lapPromise], (lrfResponse, lapResponse) => {
      const {
        DataList: lrfPlayerList = []
      } = lrfResponse && lrfResponse.data || {};
      const {
        Player
      } = lapResponse.data;
      const lapPlayerList = Player ? [Player] : [];
      const playerList = _helpers_playerListDataHelper__WEBPACK_IMPORTED_MODULE_1__["default"].mergePlayerList(lrfPlayerList, lapPlayerList);
      return playerList.length ? playerList[0] : null;
    }).catch(error => {
      console.log(error.message);
      return null;
    });
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerQueryService.js":
/*!***************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerQueryService.js ***!
  \***************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return PlayerQueryService; });
/* harmony import */ var _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../../lib/factoryCommonService */ "./www/js/source/reactpages/Module/lib/factoryCommonService.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _reportExcel__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../reportExcel */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/reportExcel.js");
/* harmony import */ var _queryServiceFactory__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./queryServiceFactory */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/queryServiceFactory.js");
/* harmony import */ var _playerItemQueryService__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./playerItemQueryService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerItemQueryService.js");





class PlayerQueryService {
  constructor() {
    this.commonService = _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__["default"].createInstance();
  }

  getTabList() {
    return this.commonService.getTabList();
  }

  getPlayerList(param) {
    const playerListService = _queryServiceFactory__WEBPACK_IMPORTED_MODULE_3__["default"].getPlayerListService(param, this.commonService);
    return playerListService.fetchData(param, this.commonService, _constants__WEBPACK_IMPORTED_MODULE_1__["apiInfo"]);
  }

  getSinglePlayer(param) {
    return _playerItemQueryService__WEBPACK_IMPORTED_MODULE_4__["default"].getPlayerById(param, this.commonService, _constants__WEBPACK_IMPORTED_MODULE_1__["apiInfo"]);
  }

  getLink(systemType, action) {
    return this.commonService.getLink(systemType, action);
  }

  exportExcel(playerList, param, displayedColumn, hasCommPermissionRead, fileName = 'PlayerList') {
    const reportExcel = new _reportExcel__WEBPACK_IMPORTED_MODULE_2__["default"](playerList, displayedColumn, hasCommPermissionRead, fileName);
    reportExcel.export();
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerSuggestionService.js":
/*!********************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/playerSuggestionService.js ***!
  \********************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return PlayerSuggestionService; });
/* harmony import */ var _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../../lib/factoryCommonService */ "./www/js/source/reactpages/Module/lib/factoryCommonService.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



class PlayerSuggestionService {
  constructor() {
    this.commonService = _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__["default"].createInstance();
  }

  getSuggestionList(param) {
    const configOption = {
      method: 'get'
    };
    return this.commonService.fetchData(_constants__WEBPACK_IMPORTED_MODULE_1__["apiSuggestion"].type, _constants__WEBPACK_IMPORTED_MODULE_1__["apiSuggestion"].action, _objectSpread({}, param), configOption);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/queryServiceFactory.js":
/*!****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/queryServiceFactory.js ***!
  \****************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return QueryServiceFactory; });
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
/* harmony import */ var _lapPlayerQueryService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./lapPlayerQueryService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/lapPlayerQueryService.js");
/* harmony import */ var _lrfPlayerQueryService__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./lrfPlayerQueryService */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/lrfPlayerQueryService.js");



class QueryServiceFactory {
  static getPlayerListService(param) {
    const {
      bankAccountNumber,
      email
    } = param;

    if (bankAccountNumber || email) {
      return _lapPlayerQueryService__WEBPACK_IMPORTED_MODULE_1__["default"];
    } else if (this._isChangeAdvancedSearch(param)) {
      return _lrfPlayerQueryService__WEBPACK_IMPORTED_MODULE_2__["default"];
    } else {
      return _lapPlayerQueryService__WEBPACK_IMPORTED_MODULE_1__["default"];
    }
  }

  static _isChangeAdvancedSearch(param) {
    const {
      baFilter,
      currencyIdList,
      oddsGroup,
      specialLevel
    } = param;
    return Number(baFilter) !== 0 || currencyIdList !== '' || oddsGroup !== '' || Object.keys(_constants__WEBPACK_IMPORTED_MODULE_0__["specialLevelListWithColor"]).length !== specialLevel.split(',').length || param.customerClass != "";
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/siteSettingService.js":
/*!***************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/services/siteSettingService.js ***!
  \***************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return SiteSettingService; });
/* harmony import */ var _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../../lib/factoryCommonService */ "./www/js/source/reactpages/Module/lib/factoryCommonService.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../constants */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");


class SiteSettingService {
  constructor() {
    this.commonService = _lib_factoryCommonService__WEBPACK_IMPORTED_MODULE_0__["default"].createInstance();
  }

  getSetting() {
    return this.commonService.fetchData(_constants__WEBPACK_IMPORTED_MODULE_1__["apiSiteSetting"].type, _constants__WEBPACK_IMPORTED_MODULE_1__["apiSiteSetting"].action, {});
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/filterHandlerReducer.js":
/*!*************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/filterHandlerReducer.js ***!
  \*************************************************************************************************/
/*! exports provided: param, playerList, type, currencyList, hasBaPermission, hasOddsGroupPermission, hasSpecialLevelPermission, hasCommPermissionEdit, hasCommPermissionRead, isSubsidiary, isAdvancedSearch, isFilterCollapsing */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "param", function() { return param; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "playerList", function() { return playerList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "type", function() { return type; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "currencyList", function() { return currencyList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "hasBaPermission", function() { return hasBaPermission; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "hasOddsGroupPermission", function() { return hasOddsGroupPermission; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "hasSpecialLevelPermission", function() { return hasSpecialLevelPermission; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "hasCommPermissionEdit", function() { return hasCommPermissionEdit; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "hasCommPermissionRead", function() { return hasCommPermissionRead; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isSubsidiary", function() { return isSubsidiary; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isAdvancedSearch", function() { return isAdvancedSearch; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isFilterCollapsing", function() { return isFilterCollapsing; });
/* harmony import */ var _lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
/* harmony import */ var _actions_types_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const param = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])({}, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_PARAM"]](preState, action) {
    return _objectSpread({}, action.data);
  }

});
const playerList = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(null, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_PLAYER_LIST"]](preState, action) {
    return action.data && [...action.data];
  }

});
const type = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(null, {});
const currencyList = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])([], {});
const hasBaPermission = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(false, {});
const hasOddsGroupPermission = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(false, {});
const hasSpecialLevelPermission = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(false, {});
const hasCommPermissionEdit = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(false, {});
const hasCommPermissionRead = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(false, {});
const isSubsidiary = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(false, {});
const isAdvancedSearch = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(false, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_ADVANCED_SEARCH"]](preState, action) {
    return action.data;
  }

});
const isFilterCollapsing = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(false, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_COLLAPSE_FILTER"]](preState, action) {
    return action.data;
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridHandlerReducer.js":
/*!***********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridHandlerReducer.js ***!
  \***********************************************************************************************/
/*! exports provided: displayedColumnList, currentActivePlayer, pagingOption, isFetchingData, sweetAlertOption */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "displayedColumnList", function() { return displayedColumnList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "currentActivePlayer", function() { return currentActivePlayer; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "pagingOption", function() { return pagingOption; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "isFetchingData", function() { return isFetchingData; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "sweetAlertOption", function() { return sweetAlertOption; });
/* harmony import */ var _lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
/* harmony import */ var _actions_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../actions/types */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
/* harmony import */ var _lib_constants_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../lib/constants.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/lib/constants.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }




const displayedColumnList = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])([], {
  [_actions_types__WEBPACK_IMPORTED_MODULE_1__["SET_DISPLAYED_COLUMN_LIST"]](preState, action) {
    return [...action.data];
  }

});
const currentActivePlayer = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(null, {
  [_actions_types__WEBPACK_IMPORTED_MODULE_1__["SET_CURRENT_ACTIVE_PLAYER"]](preState, action) {
    return action.data && _objectSpread({}, action.data);
  }

});
const pagingOption = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(_lib_constants_js__WEBPACK_IMPORTED_MODULE_2__["defaultPagingOption"], {
  [_actions_types__WEBPACK_IMPORTED_MODULE_1__["SET_PAGING"]](preState, action) {
    return _objectSpread({}, action.data);
  }

});
const isFetchingData = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(false, {
  [_actions_types__WEBPACK_IMPORTED_MODULE_1__["SET_IS_FETCHING_DATA"]](preState, action) {
    return action.data;
  }

});
const sweetAlertOption = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(null, {
  [_actions_types__WEBPACK_IMPORTED_MODULE_1__["SET_SWEET_ALERT_OPTION"]](preState, action) {
    return action.data && _objectSpread({}, action.data);
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridOddsGroupReducer.js":
/*!*************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridOddsGroupReducer.js ***!
  \*************************************************************************************************/
/*! exports provided: oddsGroupDropdownPosition */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "oddsGroupDropdownPosition", function() { return oddsGroupDropdownPosition; });
/* harmony import */ var _jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
/* harmony import */ var _actions_types_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const oddsGroupDropdownPosition = Object(_jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(null, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_ODDSGROUP_POSITION"]](preState, action) {
    return action.data && _objectSpread({}, action.data);
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridPlayerStatusReducer.js":
/*!****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridPlayerStatusReducer.js ***!
  \****************************************************************************************************/
/*! exports provided: statusDropdownPosition */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "statusDropdownPosition", function() { return statusDropdownPosition; });
/* harmony import */ var _jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
/* harmony import */ var _actions_types_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const statusDropdownPosition = Object(_jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(null, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_STATUS_POSITION"]](preState, action) {
    return action.data && _objectSpread({}, action.data);
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridSpecialLevelReducer.js":
/*!****************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridSpecialLevelReducer.js ***!
  \****************************************************************************************************/
/*! exports provided: specialLevelDropdownPosition */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "specialLevelDropdownPosition", function() { return specialLevelDropdownPosition; });
/* harmony import */ var _jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
/* harmony import */ var _actions_types_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const specialLevelDropdownPosition = Object(_jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(null, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_SPECIAL_LEVEL_POSITION"]](preState, action) {
    return action.data && _objectSpread({}, action.data);
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridUserNameReducer.js":
/*!************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridUserNameReducer.js ***!
  \************************************************************************************************/
/*! exports provided: contextMenu */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "contextMenu", function() { return contextMenu; });
/* harmony import */ var _jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
/* harmony import */ var _actions_types_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const contextMenu = Object(_jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])({}, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_CONTEXT_MENU"]](preState, action) {
    return _objectSpread({}, action.data);
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/index.js":
/*!**********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/index.js ***!
  \**********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! redux */ "./node_modules/redux/es/redux.js");
/* harmony import */ var _filterHandlerReducer_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./filterHandlerReducer.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/filterHandlerReducer.js");
/* harmony import */ var _playerDetailHandlerReducer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./playerDetailHandlerReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/playerDetailHandlerReducer.js");
/* harmony import */ var _reducers_loaderReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../../../reducers/loaderReducer */ "./www/js/source/reactpages/reducers/loaderReducer.js");
/* harmony import */ var _gridHandlerReducer__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./gridHandlerReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridHandlerReducer.js");
/* harmony import */ var _gridOddsGroupReducer__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./gridOddsGroupReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridOddsGroupReducer.js");
/* harmony import */ var _gridSpecialLevelReducer__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./gridSpecialLevelReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridSpecialLevelReducer.js");
/* harmony import */ var _gridPlayerStatusReducer__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./gridPlayerStatusReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridPlayerStatusReducer.js");
/* harmony import */ var _gridUserNameReducer__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./gridUserNameReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/gridUserNameReducer.js");
/* harmony import */ var _siteSettingReducer__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./siteSettingReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/siteSettingReducer.js");
/* harmony import */ var _lrfFiltersReducer__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./lrfFiltersReducer */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/lrfFiltersReducer.js");











/* harmony default export */ __webpack_exports__["default"] = (Object(redux__WEBPACK_IMPORTED_MODULE_0__["combineReducers"])(Object.assign({}, _filterHandlerReducer_js__WEBPACK_IMPORTED_MODULE_1__, _reducers_loaderReducer__WEBPACK_IMPORTED_MODULE_3__, _playerDetailHandlerReducer__WEBPACK_IMPORTED_MODULE_2__, _gridHandlerReducer__WEBPACK_IMPORTED_MODULE_4__, _gridOddsGroupReducer__WEBPACK_IMPORTED_MODULE_5__, _gridSpecialLevelReducer__WEBPACK_IMPORTED_MODULE_6__, _gridPlayerStatusReducer__WEBPACK_IMPORTED_MODULE_7__, _gridUserNameReducer__WEBPACK_IMPORTED_MODULE_8__, _siteSettingReducer__WEBPACK_IMPORTED_MODULE_9__, _lrfFiltersReducer__WEBPACK_IMPORTED_MODULE_10__)));

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/lrfFiltersReducer.js":
/*!**********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/lrfFiltersReducer.js ***!
  \**********************************************************************************************/
/*! exports provided: lrfFilters */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "lrfFilters", function() { return lrfFilters; });
/* harmony import */ var _jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @jsroot/lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
/* harmony import */ var _actions_types_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const defaultLRFFilters = {
  CustomerClass: []
};
const lrfFilters = Object(_jsroot_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(defaultLRFFilters, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_LRF_FILTERS"]](preState, action) {
    return _objectSpread({}, action.data);
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/playerDetailHandlerReducer.js":
/*!*******************************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/playerDetailHandlerReducer.js ***!
  \*******************************************************************************************************/
/*! exports provided: selectedPlayer, selectedPlayerIdList, selectedTabName, selectedSubTabName */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "selectedPlayer", function() { return selectedPlayer; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "selectedPlayerIdList", function() { return selectedPlayerIdList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "selectedTabName", function() { return selectedTabName; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "selectedSubTabName", function() { return selectedSubTabName; });
/* harmony import */ var _lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
/* harmony import */ var _actions_types_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const selectedPlayer = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(null, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_SELECTED_PLAYER"]](preState, action) {
    return action.data && _objectSpread({}, action.data);
  }

});
const selectedPlayerIdList = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])([], {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_SELECTED_PLAYERID_LIST"]](preState, action) {
    return [...action.data];
  }

});
const selectedTabName = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])('', {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_SELECTED_TAB_NAME"]](preState, action) {
    return action.data;
  }

});
const selectedSubTabName = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])('', {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_SELECTED_SUBTAB_NAME"]](preState, action) {
    return action.data;
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/siteSettingReducer.js":
/*!***********************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/playerlist/reducers/siteSettingReducer.js ***!
  \***********************************************************************************************/
/*! exports provided: siteSetting */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "siteSetting", function() { return siteSetting; });
/* harmony import */ var _lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../../../lib/createReducer.js */ "./www/js/source/reactpages/lib/createReducer.js");
/* harmony import */ var _actions_types_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../actions/types.js */ "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/actions/types.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const defaultSiteSetting = {
  isAllowedBA: false,
  isAllowedCustomerComm: false
};
const siteSetting = Object(_lib_createReducer_js__WEBPACK_IMPORTED_MODULE_0__["default"])(defaultSiteSetting, {
  [_actions_types_js__WEBPACK_IMPORTED_MODULE_1__["SET_SITE_SETTING"]](preState, action) {
    return _objectSpread({}, action.data);
  }

});

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/userGuide/LicApiUserGuide.jsx":
/*!***********************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/userGuide/LicApiUserGuide.jsx ***!
  \***********************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _components_BaseUserGuide__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../components/BaseUserGuide */ "./www/js/source/reactpages/Module/components/BaseUserGuide.jsx");



const EngContent = _ => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "General Info"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "This function shows all players information along with their settings."))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "Search Filters"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Username:"), " searched by Username (Support prefix search)."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Currency:"), " select the currency of players that you want to manage.", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "All:"), " all supported currencies will be listed out."))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Odds Group:"), " view players of an odds group."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Special Level:"), " view players of a specific special level."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "BA:"), " view players of a BA."))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "Report Guidelines"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Status:"), " view or set status for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Username:"), " view player username and number of shortcut options including:", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Note"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Fundtransfer Limit"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Bet Limit"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Statement"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Current Balances: "), " indicate available amount to place bets."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Outstanding Balance:"), " indicate total stakes of all running bets."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Odds Group:"), " view or set odds group for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "BA Status:"), " switch on/off BA status for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Special Level:"), " view or set a special level for player."))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "Player Details"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Player Info:"), " view player\u2019s profile and activity record.", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Player Betting Statistics"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Fast Cross-Link"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Statement: "), " view daily statements of player. All transactions are summarized and then put into a daily record. Each daily record balance will be summed up to the current date as the latest balance."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Detail Transaction:"), " view player balance changes and sort by time."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Bet List:"), " lists out non-settled tickets of player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Fund Transfer:"), " view transfer log and wallets balance for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Bet Limit:"), " view bet setting for Sportsbook, Number Game, Virtual Sports, RNG Keno, Lottery and other products."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Note:"), " view or edit the note for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Log:"), " view update log of player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Setting:"), " set fund transfer limit."))));
};

const ZnCNContent = _ => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "\u4E00\u822C\u4FE1\u606F"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u672C\u62A5\u8868\u663E\u793A\u6240\u6709\u4F1A\u5458\u6570\u636E\u5305\u542B\u4F1A\u5458\u8BBE\u5B9A\u3002"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "\u641C\u7D22\u8FC7\u6EE4\u5668"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u7528\u6237\u540D:"), " \u641C\u5BFB\u7528\u6237\u540D (\u652F\u63F4\u6A21\u7CCA\u641C\u5BFB)\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u8D27\u5E01:"), " \u4EE5\u4F1A\u5458\u8D27\u5E01\u6765\u9009\u62E9\u62A5\u8868\u5448\u73B0\u5185\u5BB9\u3002", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u5168\u90E8:"), " \u62A5\u8868\u5448\u73B0\u6240\u6709\u652F\u6301\u8D27\u5E01\u3002"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Odds Group:"), " \u67E5\u770B\u73A9\u5BB6\u7684 odds group\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u7279\u6B8A\u5C42\u7EA7:"), " \u67E5\u9605\u7279\u6B8A\u5C42\u7EA7\u91CC\u7684\u73A9\u5BB6\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "BA\u72B6\u6001:"), " \u67E5\u770B\u73A9\u5BB6\u7684BA\u72B6\u6001\u3002"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "\u62A5\u544A\u6307\u5357"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u72B6\u6001:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u72B6\u6001\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u7528\u6237\u540D:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u7528\u6237\u540D\u548C\u5FEB\u6377\u529F\u80FD\u9009\u9879\uFF0C\u5305\u62EC\uFF1A", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u8BB0\u5F55"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u8F6C\u5E10\u989D\u5EA6"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u6295\u6CE8\u9650\u989D"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u5E10\u76EE"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u76EE\u524D\u4F59\u989D:"), " \u663E\u793A\u53EF\u7528\u4E8E\u6295\u6CE8\u7684\u91D1\u989D\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u672A\u7ED3\u4F59\u989D:"), " \u663E\u793A\u6240\u6709\u6295\u6CE8\u4E2D\u7684\u603B\u6295\u6CE8\u989D\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Odds Group:"), " \u663E\u793A\u6216\u8BBE\u5B9A\u73A9\u5BB6\u7684odds group\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "BA\u72B6\u6001:"), " \u5F00/\u5173\u73A9\u5BB6\u7684BA\u72B6\u6001\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u7279\u6B8A\u5C42\u7EA7:"), " \u663E\u793A\u6216\u8BBE\u5B9A\u73A9\u5BB6\u7684\u7279\u6B8A\u5C42\u7EA7\u3002"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "\u73A9\u5BB6\u8BE6\u60C5"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u73A9\u5BB6\u8D44\u8BAF:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u4E2A\u4EBA\u8D44\u6599\u548C\u6D3B\u52A8\u8BB0\u5F55\u3002", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u73A9\u5BB6\u6295\u6CE8\u7EDF\u8BA1"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u5FEB\u901F\u8FDE\u7ED3"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u5E10\u76EE:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u6BCF\u65E5\u62A5\u8868\u3002\u6C47\u603B\u6240\u6709\u4EA4\u6613\uFF0C\u7136\u540E\u8BB0\u5165\u6BCF\u65E5\u8BB0\u5F55\u3002\u6BCF\u4E2A\u6BCF\u65E5\u8BB0\u5F55\u7684\u4F59\u989D\u5C06\u88AB\u7D2F\u8BA1\u5230\u5F53\u524D\u65E5\u671F\u4F5C\u4E3A\u6700\u540E\u7684\u4F59\u989D\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u989D\u5EA6\u8BB0\u5F55:"), " \u67E5\u770B\u73A9\u5BB6\u4F59\u989D\u53D8\u5316\u5E76\u6309\u65F6\u95F4\u6392\u5E8F\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u6295\u6CE8\u5386\u53F2:"), " \u5217\u51FA\u73A9\u5BB6\u7684\u672A\u7ED3\u7B97\u6CE8\u5355\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u8F6C\u5E10:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u8F6C\u5E10\u65E5\u5FD7\u548C\u94B1\u5305\u4F59\u989D\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u6295\u6CE8\u9650\u989D:"), " \u67E5\u770B\u4F53\u80B2\u535A\u5F69\uFF0C\u6570\u5B57\u6E38\u620F\uFF0C\u865A\u62DF\u4F53\u80B2\uFF0C\u5FEB\u4E50\u5F69\uFF0C\u5F69\u7968\u548C\u5176\u4ED6\u4EA7\u54C1\u7684\u6295\u6CE8\u8BBE\u7F6E\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u8BB0\u5F55:"), " \u67E5\u770B\u548C\u7F16\u8F91\u73A9\u5BB6\u8BB0\u5F55\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u65E5\u5FD7:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u66F4\u65B0\u65E5\u5FD7\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u8BBE\u5B9A:"), " \u8BBE\u7F6E\u8F6C\u8D26\u9650\u989D\u3002"))));
};

const Contents = {
  engContent: props => /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(EngContent, props),
  znCNContent: props => /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(ZnCNContent, props)
};

const LicApiUserGuide = props => {
  const {
    lang
  } = props;
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_components_BaseUserGuide__WEBPACK_IMPORTED_MODULE_1__["default"], {
    lang: lang,
    contents: Contents
  }));
};

/* harmony default export */ __webpack_exports__["default"] = (LicApiUserGuide);

/***/ }),

/***/ "./www/js/source/reactpages/Module/PlayerAdmin/userGuide/LicAthenaUserGuide.jsx":
/*!**************************************************************************************!*\
  !*** ./www/js/source/reactpages/Module/PlayerAdmin/userGuide/LicAthenaUserGuide.jsx ***!
  \**************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _components_BaseUserGuide__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../components/BaseUserGuide */ "./www/js/source/reactpages/Module/components/BaseUserGuide.jsx");



const EngContent = _ => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "General Info"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "This function shows all players information along with their settings."))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "Search Filters"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Username:"), " searched by Username (Support prefix search)."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Bank Account Number:"), " searched by exactly bank account number."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Email:"), " searched by exactly Email."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Currency:"), " select the currency of players that you want to manage.", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "All:"), " all supported currencies will be listed out."))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Odds Group:"), " view players of an odds group."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Special Level:"), " view players of a specific special level."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "BA:"), " view players of a BA."))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "Report Guidelines"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Status:"), " view or set status for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Username:"), " view player username and number of shortcut options including:", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Note"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Password"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Bank Information"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Fundtransfer Limit"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Bet Limit"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Statement"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Personal Message"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Current Balances: "), " indicate available amount to place bets."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Outstanding Balance:"), " indicate total stakes of all running bets."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Odds Group:"), " view or set odds group for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "BA Status:"), " switch on/off BA status for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Special Level:"), " view or set a special level for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Last Transfer:"), " indicate player\u2019s last transfer time."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Email:"), " indicate player\u2019s Email."))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "Player Details"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Player Info:"), " view player\u2019s profile, bank information and activity record.", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Player Betting Statistics"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "Fast Cross-Link"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Statement: "), " view daily statements of player. All transactions are summarized and then put into a daily record. Each daily record balance will be summed up to the current date as the latest balance."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Detail Transaction:"), " view player balance changes and sort by time."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Bet List:"), " lists out non-settled tickets of player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Fund Transfer:"), " view transfer log and wallets balance for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Bet Limit:"), " view bet setting for Sportsbook, Number Game, Virtual Sports, RNG Keno, Lottery and other products."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Note:"), " view or edit the note for player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Log:"), " view update log of player."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Setting:"), " set password, fund transfer limit and bank info."), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Message:"), " view normal/ special announcements and add the personal message."))));
};

const ZnCNContent = _ => {
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "\u4E00\u822C\u4FE1\u606F"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u672C\u62A5\u8868\u663E\u793A\u6240\u6709\u4F1A\u5458\u6570\u636E\u5305\u542B\u4F1A\u5458\u8BBE\u5B9A\u3002"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "\u641C\u7D22\u8FC7\u6EE4\u5668"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u7528\u6237\u540D:"), " \u641C\u5BFB\u7528\u6237\u540D (\u652F\u63F4\u6A21\u7CCA\u641C\u5BFB)\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u94F6\u884C\u8D26\u6237:"), " \u7CBE\u786E\u67E5\u8BE2\u94F6\u884C\u8D26\u6237\u53F7\u7801\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u4FE1\u7BB1:"), " \u7CBE\u786E\u67E5\u8BE2\u7535\u5B50\u90AE\u4EF6\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u8D27\u5E01:"), " \u4EE5\u4F1A\u5458\u8D27\u5E01\u6765\u9009\u62E9\u62A5\u8868\u5448\u73B0\u5185\u5BB9\u3002", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u5168\u90E8:"), " \u62A5\u8868\u5448\u73B0\u6240\u6709\u652F\u6301\u8D27\u5E01\u3002"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Odds Group:"), " \u67E5\u770B\u73A9\u5BB6\u7684 odds group\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u7279\u6B8A\u5C42\u7EA7:"), " \u67E5\u9605\u7279\u6B8A\u5C42\u7EA7\u91CC\u7684\u73A9\u5BB6\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "BA\u72B6\u6001:"), " \u67E5\u770B\u73A9\u5BB6\u7684BA\u72B6\u6001\u3002"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "\u62A5\u544A\u6307\u5357"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u72B6\u6001:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u72B6\u6001\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u7528\u6237\u540D:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u7528\u6237\u540D\u548C\u5FEB\u6377\u529F\u80FD\u9009\u9879\uFF0C\u5305\u62EC\uFF1A", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u8BB0\u5F55"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u5BC6\u7801"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u94F6\u884C\u8D44\u8BAF"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u8F6C\u5E10\u989D\u5EA6"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u6295\u6CE8\u9650\u989D"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u5E10\u76EE"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u4E2A\u4EBA\u8BAF\u606F"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u76EE\u524D\u4F59\u989D:"), " \u663E\u793A\u53EF\u7528\u4E8E\u6295\u6CE8\u7684\u91D1\u989D\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u672A\u7ED3\u4F59\u989D:"), " \u663E\u793A\u6240\u6709\u6295\u6CE8\u4E2D\u7684\u603B\u6295\u6CE8\u989D\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "Odds Group:"), " \u663E\u793A\u6216\u8BBE\u5B9A\u73A9\u5BB6\u7684odds group\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "BA\u72B6\u6001:"), " \u5F00/\u5173\u73A9\u5BB6\u7684BA\u72B6\u6001\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u7279\u6B8A\u5C42\u7EA7:"), " \u663E\u793A\u6216\u8BBE\u5B9A\u73A9\u5BB6\u7684\u7279\u6B8A\u5C42\u7EA7\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u6700\u540E\u8F6C\u5E10:"), " \u663E\u793A\u73A9\u5BB6\u7684\u6700\u540E\u8F6C\u5E10\u7684\u65F6\u95F4\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u4FE1\u7BB1:"), " \u663E\u793A\u73A9\u5BB6\u7684\u4FE1\u7BB1\u3002"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "line"
  }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "rule"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h4", null, "\u73A9\u5BB6\u8BE6\u60C5"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u73A9\u5BB6\u8D44\u8BAF:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u4E2A\u4EBA\u8D44\u6599\uFF0C\u94F6\u884C\u4FE1\u606F\u548C\u6D3B\u52A8\u8BB0\u5F55\u3002", /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("ul", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u73A9\u5BB6\u6295\u6CE8\u7EDF\u8BA1"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, "\u5FEB\u901F\u8FDE\u7ED3"))), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u5E10\u76EE:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u6BCF\u65E5\u62A5\u8868\u3002\u6C47\u603B\u6240\u6709\u4EA4\u6613\uFF0C\u7136\u540E\u8BB0\u5165\u6BCF\u65E5\u8BB0\u5F55\u3002\u6BCF\u4E2A\u6BCF\u65E5\u8BB0\u5F55\u7684\u4F59\u989D\u5C06\u88AB\u7D2F\u8BA1\u5230\u5F53\u524D\u65E5\u671F\u4F5C\u4E3A\u6700\u540E\u7684\u4F59\u989D\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u989D\u5EA6\u8BB0\u5F55:"), " \u67E5\u770B\u73A9\u5BB6\u4F59\u989D\u53D8\u5316\u5E76\u6309\u65F6\u95F4\u6392\u5E8F\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u6295\u6CE8\u5386\u53F2:"), " \u5217\u51FA\u73A9\u5BB6\u7684\u672A\u7ED3\u7B97\u6CE8\u5355\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u8F6C\u5E10:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u8F6C\u5E10\u65E5\u5FD7\u548C\u94B1\u5305\u4F59\u989D\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u6295\u6CE8\u9650\u989D:"), " \u67E5\u770B\u4F53\u80B2\u535A\u5F69\uFF0C\u6570\u5B57\u6E38\u620F\uFF0C\u865A\u62DF\u4F53\u80B2\uFF0C\u57FA\u8BFA\uFF0C\u5F69\u7968\u548C\u5176\u4ED6\u4EA7\u54C1\u7684\u6295\u6CE8\u8BBE\u7F6E\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u8BB0\u5F55:"), " \u67E5\u770B\u548C\u7F16\u8F91\u73A9\u5BB6\u8BB0\u5F55\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u65E5\u5FD7:"), " \u67E5\u770B\u73A9\u5BB6\u7684\u767B\u5165\u8D44\u8BAF\u548C\u66F4\u65B0\u65E5\u5FD7\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u8BBE\u5B9A:"), " \u8BBE\u7F6E\u5BC6\u7801\uFF0C\u8F6C\u8D26\u9650\u989D\u548C\u94F6\u884C\u4FE1\u606F\u3002"), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("li", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("b", null, "\u4FE1\u606F:"), " \u67E5\u770B\u4E00\u822C/\u7279\u522B\u516C\u544A\u5E76\u6DFB\u52A0\u4E2A\u4EBA\u4FE1\u606F\u3002"))));
};

const Contents = {
  engContent: props => /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(EngContent, props),
  znCNContent: props => /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(ZnCNContent, props)
};

const LicAthenaUserGuide = props => {
  const {
    lang
  } = props;
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_components_BaseUserGuide__WEBPACK_IMPORTED_MODULE_1__["default"], {
    lang: lang,
    contents: Contents
  }));
};

/* harmony default export */ __webpack_exports__["default"] = (LicAthenaUserGuide);

/***/ }),

/***/ "./www/js/source/reactpages/Module/components/BaseUserGuide.jsx":
/*!**********************************************************************!*\
  !*** ./www/js/source/reactpages/Module/components/BaseUserGuide.jsx ***!
  \**********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @jsroot/lib/langHelper */ "./www/js/source/reactpages/lib/langHelper.js");



const BaseUserGuide = props => {
  const {
    lang,
    contents
  } = props;

  const renderContentByLanguage = () => {
    if (lang === _jsroot_lib_langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].languageKeys.zhCN) {
      return contents.znCNContent();
    }

    return contents.engContent();
  };

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react__WEBPACK_IMPORTED_MODULE_0___default.a.Fragment, null, renderContentByLanguage());
};

/* harmony default export */ __webpack_exports__["default"] = (BaseUserGuide);

/***/ }),

/***/ "./www/js/source/reactpages/Module/constants/configInfo.js":
/*!*****************************************************************!*\
  !*** ./www/js/source/reactpages/Module/constants/configInfo.js ***!
  \*****************************************************************/
/*! exports provided: tokenUrlTemplate, systemType */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "tokenUrlTemplate", function() { return tokenUrlTemplate; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "systemType", function() { return systemType; });
const pageConfig = window._pConfig;
const tokenUrlTemplate = pageConfig && pageConfig.tokenUrl;
const systemType = {
  internal: 0,
  lrf: 1,
  affiliate: 2,
  newAdmin: 3
};

/***/ }),

/***/ "./www/js/source/reactpages/Module/lib/baseCommonService.js":
/*!******************************************************************!*\
  !*** ./www/js/source/reactpages/Module/lib/baseCommonService.js ***!
  \******************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
class BaseCommonService {
  constructor() {
    if (this.constructor == BaseCommonService) {
      throw new Error("Abstract classes can't be instantiated.");
    }
  }

  getTabList() {
    throw new Error("Method 'getTabList()' must be implemented.");
  }

  getLink(sysType, action) {
    throw new Error("Method 'getLink()' must be implemented.");
  }

  fetchData(sType, action, params, configOption = {}) {
    throw new Error("Method 'fetchData()' must be implemented.");
  }

  fetchMultiple(promiseList, callback) {
    throw new Error("Method 'fetchMultiple()' must be implemented.");
  }

}

/* harmony default export */ __webpack_exports__["default"] = (BaseCommonService);

/***/ }),

/***/ "./www/js/source/reactpages/Module/lib/factoryCommonService.js":
/*!*********************************************************************!*\
  !*** ./www/js/source/reactpages/Module/lib/factoryCommonService.js ***!
  \*********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return FactoryCommonService; });
/* harmony import */ var _lapCommonService__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./lapCommonService */ "./www/js/source/reactpages/Module/lib/lapCommonService.js");

class FactoryCommonService {
  static createInstance() {
    const pageConfig = window._pConfig;

    if (!pageConfig) {
      throw new Error("Missing _pConfig in window");
    }

    const {
      applyApiTokenUrl,
      tabList
    } = pageConfig;

    if (!applyApiTokenUrl) {
      throw new Error("Missing applyApiTokenUrl in window._pConfig");
    }

    if (!window.Identity) {
      throw new Error("Missing Identity object");
    }

    const {
      RootUrl,
      ApplyTokenUrl
    } = window.Identity;

    if (!RootUrl) {
      throw new Error("Missing Identity.RootUrl");
    }

    if (!ApplyTokenUrl) {
      throw new Error("Missing Identity.ApplyTokenUrl");
    }

    const initialConfig = {
      tabList,
      baseUrl: RootUrl,
      applyApiTokenUrl,
      applyLinkTokenUrl: ApplyTokenUrl
    };
    return new _lapCommonService__WEBPACK_IMPORTED_MODULE_0__["default"](initialConfig);
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/Module/lib/lapCommonService.js":
/*!*****************************************************************!*\
  !*** ./www/js/source/reactpages/Module/lib/lapCommonService.js ***!
  \*****************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _baseCommonService__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./baseCommonService */ "./www/js/source/reactpages/Module/lib/baseCommonService.js");
/* harmony import */ var _constants_configInfo__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../constants/configInfo */ "./www/js/source/reactpages/Module/constants/configInfo.js");
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! axios */ "./node_modules/axios/index.js");
/* harmony import */ var axios__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(axios__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var qs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! qs */ "./node_modules/qs/lib/index.js");
/* harmony import */ var qs__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(qs__WEBPACK_IMPORTED_MODULE_3__);
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }





const defaultAjaxConfig = {
  method: 'post',
  headers: {
    'X-Requested-With': 'XMLHttpRequest',
    'content-type': 'application/x-www-form-urlencoded'
  },
  withCredentials: false,
  responseType: 'json',
  cancelToken: new axios__WEBPACK_IMPORTED_MODULE_2___default.a.CancelToken(function (cancel) {})
};

class LapCommonService extends _baseCommonService__WEBPACK_IMPORTED_MODULE_0__["default"] {
  constructor(initialConfig) {
    super();
    const {
      baseUrl,
      applyLinkTokenUrl,
      applyApiTokenUrl,
      tabList
    } = initialConfig;
    this._applyLinkTokenUrl = applyLinkTokenUrl;
    this._applyApiTokenUrl = applyApiTokenUrl;
    this._baseUrl = baseUrl;
    this._tabList = tabList || [];
  }

  getTabList() {
    return [...this._tabList];
  }

  _buildApplyTokenQueryString(sysType, action) {
    return `ActionLink=${encodeURIComponent(action)}&Type=${sysType}`;
  }

  getLink(sysType, action) {
    return sysType === _constants_configInfo__WEBPACK_IMPORTED_MODULE_1__["systemType"].newAdmin ? this._baseUrl + action : `${this._applyLinkTokenUrl}?${this._buildApplyTokenQueryString(sysType, action)}`;
  }

  _buildCommonAjaxConfig(configOption, apiActionUrl, params) {
    const commonConfig = _objectSpread(_objectSpread({}, configOption), {}, {
      url: apiActionUrl
    });

    const configWithParam = this._getConfigParam(commonConfig, params);

    return configWithParam;
  }

  _getConfigParam(config, params) {
    const isGetMethod = config.method.toLowerCase() === 'get';
    const paramConfig = isGetMethod ? {
      params
    } : {
      data: qs__WEBPACK_IMPORTED_MODULE_3___default.a.stringify(params)
    };
    return _objectSpread(_objectSpread({}, config), paramConfig);
  }

  _fetchDataWithToken(sysType, action, params, configOption) {
    const apiToken = `${this._applyApiTokenUrl}?${this._buildApplyTokenQueryString(sysType, action)}`;
    return axios__WEBPACK_IMPORTED_MODULE_2___default.a.get(apiToken).then(response => {
      const {
        Token,
        ApiUrl
      } = response.data;
      const apiActionUrl = `${ApiUrl}/${action}`;

      const commonAjaxConfig = this._buildCommonAjaxConfig(configOption, apiActionUrl, params);

      const headerConfig = _objectSpread(_objectSpread({}, configOption.headers), {}, {
        'X-Auth-Token': Token
      });

      const configWithXToken = _objectSpread(_objectSpread({}, commonAjaxConfig), {}, {
        headers: _objectSpread({}, headerConfig)
      });

      return axios__WEBPACK_IMPORTED_MODULE_2___default()(configWithXToken);
    });
  }

  fetchData(sysType, action, params, configOption = defaultAjaxConfig) {
    const config = _objectSpread(_objectSpread({}, defaultAjaxConfig), configOption);

    if (sysType === _constants_configInfo__WEBPACK_IMPORTED_MODULE_1__["systemType"].newAdmin) {
      const relativeUrl = this._baseUrl + action;

      const ajaxConfig = this._buildCommonAjaxConfig(config, relativeUrl, params);

      return axios__WEBPACK_IMPORTED_MODULE_2___default()(ajaxConfig);
    } else {
      return this._fetchDataWithToken(sysType, action, params, config);
    }
  }

  fetchMultiple(promiseList, callback) {
    if (promiseList && promiseList.length) {
      return axios__WEBPACK_IMPORTED_MODULE_2___default.a.all(promiseList).then(axios__WEBPACK_IMPORTED_MODULE_2___default.a.spread(callback));
    }
  }

}

/* harmony default export */ __webpack_exports__["default"] = (LapCommonService);

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

/***/ "./www/js/source/reactpages/components/DropdownLabel.jsx":
/*!***************************************************************!*\
  !*** ./www/js/source/reactpages/components/DropdownLabel.jsx ***!
  \***************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(classnames__WEBPACK_IMPORTED_MODULE_2__);
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }





const DropdownLabel = props => {
  const {
    className,
    text,
    iconElement,
    labelStyle
  } = props,
        attrs = _objectWithoutProperties(props, ["className", "text", "iconElement", "labelStyle"]);

  const classes = classnames__WEBPACK_IMPORTED_MODULE_2___default()('dropdown-label-container', className, {
    disabled: props.disabled
  });
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", _extends({
    className: classes
  }, attrs), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
    className: "dropdown-label",
    style: labelStyle
  }, text), iconElement);
};

const propTypes = {
  children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node,
  disabled: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  onClick: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  text: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.any,
  iconElement: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.any,
  labelStyle: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object
};
const defaultProps = {
  disabled: false,
  iconElement: /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("i", {
    className: "dropdown-label-icon chevron down"
  })
};
DropdownLabel.propTypes = propTypes;
DropdownLabel.defaultProps = defaultProps;
/* harmony default export */ __webpack_exports__["default"] = (/*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.memo(DropdownLabel));
;

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

/***/ "./www/js/source/reactpages/components/Switcher.jsx":
/*!**********************************************************!*\
  !*** ./www/js/source/reactpages/components/Switcher.jsx ***!
  \**********************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }



const Switcher = props => {
  const attributes = _extends({}, props);

  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
    className: "switch"
  }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("label", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("input", _extends({}, attributes, {
    type: "checkbox"
  })), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
    className: "lever"
  })));
};

/* harmony default export */ __webpack_exports__["default"] = (Switcher);

/***/ }),

/***/ "./www/js/source/reactpages/components/UserGuide.jsx":
/*!***********************************************************!*\
  !*** ./www/js/source/reactpages/components/UserGuide.jsx ***!
  \***********************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return UserGuide; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ "./node_modules/react-dom/index.js");
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _popup_Popup__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./popup/Popup */ "./www/js/source/reactpages/components/popup/Popup.jsx");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_3__);
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }





class UserGuide extends react__WEBPACK_IMPORTED_MODULE_0___default.a.Component {
  constructor(props) {
    super(props);

    _defineProperty(this, "onClickHandler", () => {
      this.setState({
        isShow: true
      });
      this.props.onClick && this.props.onClick();
    });

    _defineProperty(this, "onCloseHandler", () => {
      this.setState({
        isShow: false
      });
    });

    this.state = {
      isShow: false
    };
  }

  render() {
    const {
      className,
      title,
      content,
      contentTitle,
      isHtml
    } = this.props;
    const isShow = this.state.isShow && content && ( /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.isValidElement(content) || content.length > 0) ? true : false;
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("i", {
      className: className,
      onClick: this.onClickHandler,
      title: title
    }), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_popup_Popup__WEBPACK_IMPORTED_MODULE_2__["default"], {
      width: 640,
      height: 480,
      isShow: isShow,
      title: contentTitle,
      onClose: this.onCloseHandler,
      isHtml: isHtml
    }, content));
  }

}

_defineProperty(UserGuide, "propTypes", {
  title: prop_types__WEBPACK_IMPORTED_MODULE_3___default.a.string,
  className: prop_types__WEBPACK_IMPORTED_MODULE_3___default.a.string,
  contentTitle: prop_types__WEBPACK_IMPORTED_MODULE_3___default.a.string,
  isHtml: prop_types__WEBPACK_IMPORTED_MODULE_3___default.a.bool
});

_defineProperty(UserGuide, "defaultProps", {
  className: 'icon-book right',
  isHtml: true
});

;

/***/ }),

/***/ "./www/js/source/reactpages/components/collapsible-panel/CollapsiblePanel.jsx":
/*!************************************************************************************!*\
  !*** ./www/js/source/reactpages/components/collapsible-panel/CollapsiblePanel.jsx ***!
  \************************************************************************************/
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

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }




class CollapsiblePanel extends react__WEBPACK_IMPORTED_MODULE_0___default.a.Component {
  constructor(props) {
    super(props);

    _defineProperty(this, "onClick", () => {
      const newState = !this.state.isCollapsed;
      const {
        onCollapse,
        onExpand
      } = this.props;

      if (newState) {
        onCollapse && onCollapse();
      }

      this.setState({
        isCollapsed: newState
      }, () => {
        window.dispatchEvent(new Event('resize'));

        if (!newState) {
          onExpand && onExpand();
        }
      });
    });

    this.state = {
      isCollapsed: false
    };
  }

  render() {
    const {
      stickyFooter,
      iconContainerStyle,
      iconToggle
    } = this.props;

    const {
      tag,
      className,
      style
    } = iconToggle,
          attrs = _objectWithoutProperties(iconToggle, ["tag", "className", "style"]);

    const IconTag = tag;
    const iconStyle = style || {
      transform: this.state.isCollapsed ? null : 'rotate(180deg)',
      color: '#fff',
      textAlign: 'center',
      background: '#4c6274',
      height: "15px",
      width: "20px",
      float: 'left'
    };
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: `${this.props.className}`
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      style: {
        display: this.state.isCollapsed ? 'none' : 'block'
      }
    }, this.props.children), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "caret-block"
    }, stickyFooter), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
      className: "filter-handle",
      style: iconContainerStyle,
      onClick: this.onClick
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(IconTag, _extends({
      className: className,
      style: iconStyle
    }, attrs))));
  }

}

_defineProperty(CollapsiblePanel, "propTypes", {
  stickyFooter: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node,
  onCollapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onExpand: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func
});

const propTypes = {
  stickyFooter: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node,
  onCollapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onExpand: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  iconToggle: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object
};
const defaultProps = {
  iconContainerStyle: {
    height: '15px',
    width: '20px',
    cursor: 'pointer',
    position: 'absolute',
    left: 'calc(50% - 10px)',
    bottom: 0,
    background: '#4c6274',
    textAlign: 'center'
  },
  iconToggle: {
    tag: 'i',
    className: 'icon-arrow',
    style: null
  }
};
CollapsiblePanel.propTypes = propTypes;
CollapsiblePanel.defaultProps = defaultProps;
/* harmony default export */ __webpack_exports__["default"] = (CollapsiblePanel);

/***/ }),

/***/ "./www/js/source/reactpages/components/collapsible-panel/index.jsx":
/*!*************************************************************************!*\
  !*** ./www/js/source/reactpages/components/collapsible-panel/index.jsx ***!
  \*************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _CollapsiblePanel__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./CollapsiblePanel */ "./www/js/source/reactpages/components/collapsible-panel/CollapsiblePanel.jsx");

/* harmony default export */ __webpack_exports__["default"] = (_CollapsiblePanel__WEBPACK_IMPORTED_MODULE_0__["default"]);

/***/ }),

/***/ "./www/js/source/reactpages/components/dropdown/DropdownButton.jsx":
/*!*************************************************************************!*\
  !*** ./www/js/source/reactpages/components/dropdown/DropdownButton.jsx ***!
  \*************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(classnames__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _DropdownToggle__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./DropdownToggle */ "./www/js/source/reactpages/components/dropdown/DropdownToggle.jsx");
/* harmony import */ var _DropdownMenu__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./DropdownMenu */ "./www/js/source/reactpages/components/dropdown/DropdownMenu.jsx");
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }







const DropdownButton = props => {
  const {
    className,
    children,
    toggleButton,
    dropdownMenu,
    isOpen,
    onOpen,
    onClose,
    onToggle,
    buttonLabel
  } = props,
        attrs = _objectWithoutProperties(props, ["className", "children", "toggleButton", "dropdownMenu", "isOpen", "onOpen", "onClose", "onToggle", "buttonLabel"]);

  const classes = classnames__WEBPACK_IMPORTED_MODULE_2___default()('dropdown', className);
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", _extends({
    className: classes
  }, attrs), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_DropdownToggle__WEBPACK_IMPORTED_MODULE_3__["default"], _extends({}, toggleButton, {
    onClick: onToggle
  }), buttonLabel), /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_DropdownMenu__WEBPACK_IMPORTED_MODULE_4__["default"], _extends({}, dropdownMenu, {
    isOpen: isOpen,
    onOpen: onOpen,
    onClose: onClose
  }), children));
};

const propTypes = {
  buttonLabel: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.any,
  children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node.isRequired,
  className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  isOpen: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  onOpen: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onClose: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onToggle: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  toggleButton: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.shape({
    caret: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
    className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
    disabled: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
    onClick: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
    tag: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
  }),
  dropdownMenu: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.shape({
    tag: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
    children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node.isRequired,
    className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
  })
};
const defaultProps = {
  toggleButton: {
    className: 'toggle-default'
  }
};
DropdownButton.propTypes = propTypes;
DropdownButton.defaultProps = defaultProps;
/* harmony default export */ __webpack_exports__["default"] = (DropdownButton);

/***/ }),

/***/ "./www/js/source/reactpages/components/dropdown/DropdownItem.jsx":
/*!***********************************************************************!*\
  !*** ./www/js/source/reactpages/components/dropdown/DropdownItem.jsx ***!
  \***********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(classnames__WEBPACK_IMPORTED_MODULE_2__);
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }





class DropdownItem extends react__WEBPACK_IMPORTED_MODULE_0__["Component"] {
  constructor(props) {
    super(props);

    _defineProperty(this, "handleClick", e => {
      e.preventDefault();

      if (this.props.disabled) {
        return;
      }

      this.props.onClick && this.props.onClick(e);
    });
  }

  render() {
    const _this$props = this.props,
          {
      tag,
      className,
      active
    } = _this$props,
          props = _objectWithoutProperties(_this$props, ["tag", "className", "active"]);

    const Tag = tag;
    const type = Tag === 'button' && props.onClick ? 'button' : undefined;
    const classes = classnames__WEBPACK_IMPORTED_MODULE_2___default()('dropdown-item', className, {
      disabled: props.disabled,
      active
    });
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Tag, _extends({
      className: classes,
      type: type
    }, props));
  }

}

const propTypes = {
  children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node,
  active: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  disabled: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  tag: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  onClick: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};
const defaultProps = {
  tag: 'button',
  disabled: false
};
DropdownItem.propTypes = propTypes;
DropdownItem.defaultProps = defaultProps;
/* harmony default export */ __webpack_exports__["default"] = (DropdownItem);

/***/ }),

/***/ "./www/js/source/reactpages/components/dropdown/DropdownMenu.jsx":
/*!***********************************************************************!*\
  !*** ./www/js/source/reactpages/components/dropdown/DropdownMenu.jsx ***!
  \***********************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(classnames__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _Overlay__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../Overlay */ "./www/js/source/reactpages/components/Overlay.jsx");
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }






class DropdownMenu extends react__WEBPACK_IMPORTED_MODULE_0__["Component"] {
  constructor(props) {
    super(props);
  }

  render() {
    const _this$props = this.props,
          {
      tag,
      className,
      isOpen,
      onOpen,
      onClose
    } = _this$props,
          attrs = _objectWithoutProperties(_this$props, ["tag", "className", "isOpen", "onOpen", "onClose"]);

    const Tag = tag;
    const classes = classnames__WEBPACK_IMPORTED_MODULE_2___default()('dropdown-menu', className);
    return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_Overlay__WEBPACK_IMPORTED_MODULE_3__["default"], {
      isOpening: isOpen,
      onOpen: onOpen,
      onClose: onClose
    }, /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Tag, _extends({
      className: classes
    }, attrs)));
  }

}

const propTypes = {
  tag: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node.isRequired,
  className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  isOpen: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  onOpen: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onClose: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func
};
const defaultProps = {
  tag: 'div'
};
DropdownMenu.propTypes = propTypes;
DropdownMenu.defaultProps = defaultProps;
/* harmony default export */ __webpack_exports__["default"] = (DropdownMenu);

/***/ }),

/***/ "./www/js/source/reactpages/components/dropdown/DropdownToggle.jsx":
/*!*************************************************************************!*\
  !*** ./www/js/source/reactpages/components/dropdown/DropdownToggle.jsx ***!
  \*************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! prop-types */ "./node_modules/prop-types/index.js");
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! classnames */ "./node_modules/classnames/index.js");
/* harmony import */ var classnames__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(classnames__WEBPACK_IMPORTED_MODULE_2__);
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }





const DropdownToggle = props => {
  const {
    className,
    tag,
    caret
  } = props,
        attrs = _objectWithoutProperties(props, ["className", "tag", "caret"]);

  const classes = classnames__WEBPACK_IMPORTED_MODULE_2___default()('dropdown-toggle', className, {
    'site-caret': caret
  });
  const Tag = tag;
  return /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(Tag, _extends({
    className: classes
  }, attrs));
};

const propTypes = {
  caret: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node,
  className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  disabled: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  onClick: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  tag: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};
const defaultProps = {
  caret: true,
  tag: 'button'
};
DropdownToggle.propTypes = propTypes;
DropdownToggle.defaultProps = defaultProps;
/* harmony default export */ __webpack_exports__["default"] = (DropdownToggle);

/***/ }),

/***/ "./www/js/source/reactpages/components/dropdown/index.js":
/*!***************************************************************!*\
  !*** ./www/js/source/reactpages/components/dropdown/index.js ***!
  \***************************************************************/
/*! exports provided: DropdownToggle, DropdownMenu, DropdownItem, DropdownButton */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _DropdownToggle__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./DropdownToggle */ "./www/js/source/reactpages/components/dropdown/DropdownToggle.jsx");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "DropdownToggle", function() { return _DropdownToggle__WEBPACK_IMPORTED_MODULE_0__["default"]; });

/* harmony import */ var _DropdownMenu__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./DropdownMenu */ "./www/js/source/reactpages/components/dropdown/DropdownMenu.jsx");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "DropdownMenu", function() { return _DropdownMenu__WEBPACK_IMPORTED_MODULE_1__["default"]; });

/* harmony import */ var _DropdownItem__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./DropdownItem */ "./www/js/source/reactpages/components/dropdown/DropdownItem.jsx");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "DropdownItem", function() { return _DropdownItem__WEBPACK_IMPORTED_MODULE_2__["default"]; });

/* harmony import */ var _DropdownButton__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./DropdownButton */ "./www/js/source/reactpages/components/dropdown/DropdownButton.jsx");
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "DropdownButton", function() { return _DropdownButton__WEBPACK_IMPORTED_MODULE_3__["default"]; });







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

/***/ "./www/js/source/reactpages/components/utils/timer.js":
/*!************************************************************!*\
  !*** ./www/js/source/reactpages/components/utils/timer.js ***!
  \************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return Timer; });
const DEFAULT_TIME_DELAY_MILLISECOND = 500;
class Timer {
  static debounce(func, wait) {
    let timeout;
    return function () {
      const context = this,
            args = arguments;

      const later = () => {
        func.apply(context, args);
      };

      clearTimeout(timeout);
      timeout = setTimeout(later, wait || DEFAULT_TIME_DELAY_MILLISECOND);
    };
  }

}

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

/***/ "./www/js/source/reactpages/constants/excelStyle.js":
/*!**********************************************************!*\
  !*** ./www/js/source/reactpages/constants/excelStyle.js ***!
  \**********************************************************/
/*! exports provided: titleCellStyle, headerCellStyle, bodyCellStyle, totalCellStyle, grandTotalCellStyle */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "titleCellStyle", function() { return titleCellStyle; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "headerCellStyle", function() { return headerCellStyle; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "bodyCellStyle", function() { return bodyCellStyle; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "totalCellStyle", function() { return totalCellStyle; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "grandTotalCellStyle", function() { return grandTotalCellStyle; });
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

const titleCellStyle = {
  font: {
    color: {
      rgb: "FFFFFF"
    }
  },
  fill: {
    patternType: "solid",
    fgColor: {
      rgb: "4C6375"
    }
  }
};
const headerCellStyle = {
  font: {
    bold: true,
    color: {
      rgb: "2C2C2C"
    },
    sz: "12"
  },
  alignment: {
    horizontal: "center",
    vertical: "center"
  },
  fill: {
    patternType: "solid",
    fgColor: {
      rgb: "9EB4C3"
    }
  },
  border: {
    left: {
      style: "thin",
      color: {
        rgb: "D6D6D6"
      }
    },
    bottom: {
      style: "thin",
      color: {
        rgb: "D6D6D6"
      }
    }
  }
};
const bodyTextStyle = {
  alignment: {
    vertical: "center"
  }
};
const bodyDecimalStyle = {
  numFmt: "#,##0.00"
};
const bodyCellStyle = {
  rowspan: bodyTextStyle,
  center: {
    alignment: _objectSpread(_objectSpread({}, bodyTextStyle.alignment), {}, {
      horizontal: "center"
    })
  },
  left: {
    alignment: _objectSpread(_objectSpread({}, bodyTextStyle.alignment), {}, {
      horizontal: "left"
    })
  },
  right: {
    alignment: _objectSpread(_objectSpread({}, bodyTextStyle.alignment), {}, {
      horizontal: "right"
    })
  },
  number: {
    numFmt: "#,##0"
  },
  decimal: bodyDecimalStyle,
  negativeDecimal: _objectSpread(_objectSpread({}, bodyDecimalStyle), {}, {
    font: {
      color: {
        rgb: "FF0000"
      }
    }
  }),
  percent: {
    numFmt: '#,##0.00%'
  }
};
const totalTextStyle = {
  font: {
    bold: true
  }
};

const totalDecimalStyle = _objectSpread(_objectSpread({}, totalTextStyle), {}, {
  numFmt: "#,##0.00"
});

const totalCellStyle = {
  default: totalTextStyle,
  rowspan: _objectSpread(_objectSpread({}, totalTextStyle), {}, {
    alignment: {
      horizontal: "center",
      vertical: "center"
    }
  }),
  center: _objectSpread(_objectSpread({}, totalTextStyle), {}, {
    alignment: {
      vertical: "center",
      horizontal: "center"
    }
  }),
  number: _objectSpread(_objectSpread({}, totalTextStyle), {}, {
    numFmt: "#,##0"
  }),
  decimal: totalDecimalStyle,
  negativeDecimal: _objectSpread(_objectSpread({}, totalDecimalStyle), {}, {
    font: {
      bold: true,
      color: {
        rgb: "FF0000"
      }
    }
  })
};
const grandTotalTextStyle = {
  font: {
    bold: true
  },
  fill: {
    patternType: "solid",
    fgColor: {
      rgb: "B8C3C9"
    }
  },
  border: {
    left: {
      style: "thin",
      color: {
        rgb: "D6D6D6"
      }
    }
  }
};

const decimalGrandTotalStyle = _objectSpread(_objectSpread({}, grandTotalTextStyle), {}, {
  numFmt: "#,##0.00"
});

const grandTotalCellStyle = {
  default: grandTotalTextStyle,
  rowspan: _objectSpread(_objectSpread({}, grandTotalTextStyle), {}, {
    alignment: {
      horizontal: "center",
      vertical: "center"
    }
  }),
  center: _objectSpread(_objectSpread({}, grandTotalTextStyle), {}, {
    alignment: {
      horizontal: "center"
    }
  }),
  number: _objectSpread(_objectSpread({}, grandTotalTextStyle), {}, {
    numFmt: "#,##0"
  }),
  decimal: decimalGrandTotalStyle,
  negativeDecimal: _objectSpread(_objectSpread({}, decimalGrandTotalStyle), {}, {
    font: {
      bold: true,
      color: {
        rgb: "FF0000"
      }
    }
  }),
  percent: _objectSpread(_objectSpread({}, grandTotalTextStyle), {}, {
    numFmt: "#,##0.00%"
  })
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

/***/ "./www/js/source/reactpages/lib/dataReducer.js":
/*!*****************************************************!*\
  !*** ./www/js/source/reactpages/lib/dataReducer.js ***!
  \*****************************************************/
/*! exports provided: updateObjectInArray */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "updateObjectInArray", function() { return updateObjectInArray; });
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

const updateObjectInArray = (array, indexItem, updatedItem) => {
  return array.map((item, index) => {
    if (index !== indexItem) {
      return item;
    }

    return _objectSpread(_objectSpread({}, item), updatedItem);
  });
};

/***/ }),

/***/ "./www/js/source/reactpages/lib/excel.js":
/*!***********************************************!*\
  !*** ./www/js/source/reactpages/lib/excel.js ***!
  \***********************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "default", function() { return Excel; });
const EXCEL_TYPE = "xlsx";
const MapExcelType = {
  "number": "n",
  "string": "s",
  "boolean": "b",
  "object": "d"
};
let _XLSX = null;
class Excel {
  static process(callback) {
    if (window.XLSX) {
      _XLSX = window.XLSX;
      callback && callback();
    } else {
      Promise.all(/*! import() | lib/xlsx-style-sparta */[__webpack_require__.e("lib/common"), __webpack_require__.e("lib/xlsx-style-sparta")]).then(__webpack_require__.t.bind(null, /*! xlsx-style-sparta */ "./node_modules/xlsx-style-sparta/xlsx.js", 7)).then(XLSX => {
        _XLSX = XLSX;
        callback && callback();
      });
    }
  }

  static read(file, sheetIndex, callBack) {
    const reader = new FileReader();

    reader.onload = function (e) {
      const data = e.target.result;

      const workbook = _XLSX.read(data, {
        type: 'binary'
      });

      const sheetName = workbook.SheetNames[sheetIndex];
      const worksheet = workbook.Sheets[sheetName];

      const content = _XLSX.utils.sheet_to_json(worksheet);

      const rows = _XLSX.utils.sheet_to_json(worksheet, {
        header: 1,
        defval: '',
        blankrows: true
      });

      const result = {
        header: rows && rows.length ? rows[0] : null,
        content
      };
      callBack && callBack(result);
    };

    reader.readAsBinaryString(file);
  }

  static export(worksheet, fileName) {
    const option = {
      bookType: EXCEL_TYPE,
      bookSST: false,
      type: 'binary'
    };
    const workbook = {
      SheetNames: [fileName],
      Sheets: {},
      Props: {}
    };
    workbook.Sheets[fileName] = worksheet;

    var workbookOutput = _XLSX.write(workbook, option);

    this.download(workbookOutput, fileName);
  }

  static getCellType(value) {
    return MapExcelType[typeof value];
  }

  static getCellAddress(cellObject) {
    return _XLSX.utils.encode_cell(cellObject);
  }

  static download(workbookOut, fileName) {
    const blob = new Blob([this.sheetToArrayBuffer(workbookOut)], {
      type: "application/octet-stream"
    });
    this.downloadFromBolb(blob, fileName);
  }

  static downloadFromBolb(blob, fileName) {
    const fileNameDownload = `${fileName}.${EXCEL_TYPE}`; // IE 10+

    if (typeof navigator !== "undefined" && navigator.msSaveOrOpenBlob) {
      navigator.msSaveOrOpenBlob(blob, fileNameDownload);
    } else {
      const element = window.document.createElement('a');
      element.href = window.URL.createObjectURL(blob);
      element.download = fileNameDownload;
      document.body.appendChild(element);
      element.click();
      document.body.removeChild(element);
    }
  }

  static sheetToArrayBuffer(s) {
    const buffer = new ArrayBuffer(s.length);
    const view = new Uint8Array(buffer);

    for (let i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;

    return buffer;
  }

}

/***/ }),

/***/ "./www/js/source/reactpages/lib/htmlHelper.js":
/*!****************************************************!*\
  !*** ./www/js/source/reactpages/lib/htmlHelper.js ***!
  \****************************************************/
/*! exports provided: encode, decode */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "encode", function() { return encode; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "decode", function() { return decode; });
const encode = text => {
  const p = document.createElement("p");
  p.textContent = text;
  return p.innerHTML;
};
const decode = html => {
  const txt = document.createElement('textarea');
  txt.innerHTML = html;
  return txt.value;
};

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

/***/ "./www/js/source/reactpages/lib/sweetAlert.js":
/*!****************************************************!*\
  !*** ./www/js/source/reactpages/lib/sweetAlert.js ***!
  \****************************************************/
/*! exports provided: predefinedIcons, defaultSubmitButton, defaultCancelButton, defaultOkButton, generateDefaultConfirmOption, generateDefaultAlertOption */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "predefinedIcons", function() { return predefinedIcons; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "defaultSubmitButton", function() { return defaultSubmitButton; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "defaultCancelButton", function() { return defaultCancelButton; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "defaultOkButton", function() { return defaultOkButton; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "generateDefaultConfirmOption", function() { return generateDefaultConfirmOption; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "generateDefaultAlertOption", function() { return generateDefaultAlertOption; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "./node_modules/react/index.js");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _langHelper__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./langHelper */ "./www/js/source/reactpages/lib/langHelper.js");
function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



const predefinedIcons = {
  error: 'error',
  warning: 'warning',
  success: 'success'
};
const defaultSubmitButton = {
  text: _langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Submit'),
  className: 'btn btn-apply'
};
const defaultCancelButton = {
  text: _langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('Cancel'),
  className: 'btn'
};
const defaultOkButton = {
  text: _langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('OK'),
  className: 'btn btn-apply'
};
const generateDefaultConfirmOption = (content, onClose, submitProps, cancelProps, icon = predefinedIcons.warning, customOption = {}) => {
  return _objectSpread({
    buttons: [_objectSpread(_objectSpread({}, defaultSubmitButton), submitProps), _objectSpread(_objectSpread({}, defaultCancelButton), cancelProps)],
    icon,
    isShow: true,
    children: /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
      className: "center-align"
    }, content),
    onClose
  }, customOption);
};
const generateDefaultAlertOption = (content, onClose, onClick, icon = predefinedIcons.success) => {
  return {
    buttons: [{
      text: _langHelper__WEBPACK_IMPORTED_MODULE_1__["default"].getSingleResource('OK'),
      className: 'btn btn-apply',
      onClick: onClick || onClose
    }],
    icon,
    isShow: true,
    children: /*#__PURE__*/react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("p", {
      className: "center-align"
    }, content),
    onClose
  };
};

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

/***/ })

/******/ });
//# sourceMappingURL=playerlist.chunk.js.map