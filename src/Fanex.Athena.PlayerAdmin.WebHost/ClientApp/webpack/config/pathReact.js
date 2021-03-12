var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "www/css/build",
    layout: "./www/css/source/reactpages/main.scss",
    subsidiaryperformance: "./www/css/source/reactpages/pages/subsidiaryperformance/index.scss",
    licenseewlbytimezone: "./www/css/source/reactpages/pages/licenseewl-by-timezone/index.scss",
    notification: "./www/css/source/reactpages/pages/notification/index.scss",
    exchangewl: "./www/css/source/reactpages/pages/exchangewl/index.scss",
    speciallicenseewl: "./www/css/source/reactpages/pages/speciallicenseewl/index.scss",
    bettingticketscenter: "./www/css/source/reactpages/pages/bettingticketscenter/index.scss",
    betlist: "./www/css/source/reactpages/pages/betlist/index.scss",
    casinoturnover: "./www/css/source/reactpages/pages/casinoturnover/index.scss",
    playeradmin: "./www/css/source/reactpages/pages/playeradmin/index.scss",
    accountInfo: "./www/css/source/reactpages/pages/playeradmin/account-info.scss",
    playerrunningbetlist: "./www/css/source/reactpages/pages/playeradmin/running-betlist.scss",
    playerbetlimit: "./www/css/source/reactpages/pages/playeradmin/betlimit.scss",
    playerwinloss: "./www/css/source/reactpages/pages/playeradmin/playerwinloss.scss",
    statement: "./www/css/source/reactpages/pages/playeradmin/statement.scss",
    detailtransaction: "./www/css/source/reactpages/pages/playeradmin/detailtransaction.scss",
    dailybets: "./www/css/source/reactpages/pages/dailybets/index.scss",
    exchangevoided: "./www/css/source/reactpages/pages/exchangevoided/index.scss",
    playersegmentation: "./www/css/source/reactpages/pages/customer/player-segmentation/index.scss"
};

var scriptPaths = {
    build: "www/js/build",

    languageResource: ["./www/js/source/reactpages/lib/langHelper.js",
        "./www/js/source/common/languages/ViewResources.js",
    ],
    langCN: "./www/js/source/common/languages/ViewResources.zh-CN.js",
    langHans: "./www/js/source/common/languages/ViewResources.zh-Hans.js",
    langTW: "./www/js/source/common/languages/ViewResources.zh-TW.js",
    /*React page*/
    subsidiaryperformance: "./www/js/source/reactpages/pages/subsidiaryperformance/components/index.jsx",
    licenseewlbytimezone: "./www/js/source/reactpages/pages/licenseewl-by-timezone/components/index.jsx",
    notification: "./www/js/source/reactpages/pages/notification/components/resultAmendment/index.jsx",
    exchangewl: "./www/js/source/reactpages/pages/exchangewl/components/index.jsx",
    speciallicenseewl: "./www/js/source/reactpages/pages/speciallicenseewl/components/index.jsx",
    bettingticketscenter_advancedsearch: "./www/js/source/reactpages/pages/bettingticketscenter/advancedsearch/components/index.jsx",
    bettingticketscenter_singlecustomer: "./www/js/source/reactpages/pages/bettingticketscenter/singlecustomer/components/index.jsx",
    bettingticketscenter_refno: "./www/js/source/reactpages/pages/bettingticketscenter/refno/components/index.jsx",
    bettingticketscenter_running: "./www/js/source/reactpages/pages/bettingticketscenter/running/components/index.jsx",
    casinoturnover: "./www/js/source/reactpages/pages/casinoturnover/components/index.jsx",

    playerlist: "./www/js/source/reactpages/Module/PlayerAdmin/playerlist/components/index.jsx",
    accountinfo: "./www/js/source/reactpages/Module/PlayerAdmin/account-info/components/index.jsx",
    playerrunningbetlist: "./www/js/source/reactpages/Module/PlayerAdmin/running-betlist/components/index.jsx",
    playerbetlimit: "./www/js/source/reactpages/Module/PlayerAdmin/betlimit/components/index.jsx",
    singleCustomer: "./www/js/source/reactpages/Module/PlayerAdmin/single-customer/components/index.jsx",
    playerwinloss: "./www/js/source/reactpages/Module/PlayerAdmin/playerwinloss/components/index.jsx",
    statement: "./www/js/source/reactpages/Module/PlayerAdmin/statement/components/index.jsx",
    detailtransaction: "./www/js/source/reactpages/Module/PlayerAdmin/detailtransaction/components/index.jsx",

    dailybets: "./www/js/source/reactpages/pages/dailybets/components/index.jsx",
    exchangevoided: "./www/js/source/reactpages/pages/exchangevoided/components/index.jsx",

    playersegmentation: "./www/js/source/reactpages/pages/customer/player-segmentation/tabs/index.js",
    playersegmentation_betlist: "./www/js/source/reactpages/pages/customer/player-segmentation/betlist/index.js",
};
module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};