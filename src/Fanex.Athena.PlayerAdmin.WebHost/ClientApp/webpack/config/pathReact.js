var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "wwwroot/css",
    
    layout: "./ClientApp/src/css/main.scss",
    betlist: "./ClientApp/src/css/pages/betlist/index.scss",
    bettingticketscenter: "./ClientApp/src/css/pages/betlist/bettingticketscenter.scss",
    playeradmin: "./ClientApp/src/css/pages/index.scss",
    accountInfo: "./ClientApp/src/css/pages/account-info.scss",
    playerrunningbetlist: "./ClientApp/src/css/pages/running-betlist.scss",
    playerbetlimit: "./ClientApp/src/css/pages/betlimit.scss",
    playerwinloss: "./ClientApp/src/css/pages/playerwinloss.scss",
    statement: "./ClientApp/src/css/pages/statement.scss",
    detailtransaction: "./ClientApp/src/css/pages/detailtransaction.scss",
    userguide: "./ClientApp/src/css/pages/userguide.scss"
};

var scriptPaths = {
    build: "wwwroot/js",

    languageResource: [
        "./ClientApp/src/js/lib/languages/ViewResources.js"
    ],
    langCN: "./ClientApp/src/js/lib/languages/ViewResources.zh-CN.js",
    langVN: "./ClientApp/src/js/lib/languages/ViewResources.vi-VN.js",

    playerlist: "./ClientApp/src/js/pages/playerlist/components/index.jsx",
    accountinfo: "./ClientApp/src/js/pages/account-info/components/index.jsx",
    playerrunningbetlist: "./ClientApp/src/js/pages/running-betlist/components/index.jsx",
    playerbetlimit: "./ClientApp/src/js/pages/betlimit/components/index.jsx",
    singleCustomer: "./ClientApp/src/js/pages/single-customer/components/index.jsx",
    playerwinloss: "./ClientApp/src/js/pages/playerwinloss/components/index.jsx",
    statement: "./ClientApp/src/js/pages/statement/components/index.jsx",
    detailtransaction: "./ClientApp/src/js/pages/detailtransaction/components/index.jsx",

    playercomissionsingleupdategroup: "./ClientApp/src/js/pages/playercommission/singleupdategroup.js",
};
module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};