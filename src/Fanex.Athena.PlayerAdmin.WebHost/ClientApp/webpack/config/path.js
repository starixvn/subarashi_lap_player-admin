var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "wwwroot/jquery-pages/css",

    main: ["./ClientApp/src/jquery-pages/css/main.scss"],
    reportjqgridlayout: "./ClientApp/src/jquery-pages/css/pages/common/reportjqgridlayout.main.scss",
    playercomission: "./ClientApp/src/jquery-pages/css/pages/playercommission/index.scss",
};

var scriptPaths = {
    build: "wwwroot/jquery-pages/js",

    language: "common/languages/ViewResources.js",
    languageCN: "common/languages/ViewResources.zh-CN.js",
    
    playercomissionmultiplemovegroup: "./ClientApp/src/jquery-pages/js/pages/playercommission/multiplemovegroup.js",
    playercomissionmultipleremovefromgroup: "./ClientApp/src/jquery-pages/js/pages/playercommission/multipleremovefromgroup.js",
    playercomissionmultipleupdategroup: "./ClientApp/src/jquery-pages/js/pages/playercommission/multipleupdategroup.js",
    playercomissionsinglemovegroup: "./ClientApp/src/jquery-pages/js/pages/playercommission/singlemovegroup.js",
    playercomissionsingleremovefromgroup: "./ClientApp/src/jquery-pages/js/pages/playercommission/singleremovefromgroup.js",
    playercomissionsingleupdategroup: "./ClientApp/src/jquery-pages/js/pages/playercommission/singleupdategroup.js"
};

module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};
