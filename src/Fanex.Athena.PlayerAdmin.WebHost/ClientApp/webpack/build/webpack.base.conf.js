var path = require('path');
var merge = require('webpack-merge');

var pathConfig = require('../config/path');
var alias = require('../config/alias');
var rules = require('../config/rule');
var pluginGenerator = require('../config/plugin');
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");

function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}`;
    var cssPath = `${pathConfig.cssPaths.build}/${environment}`;

    var commonConfigs = {
        resolve: {
            alias: alias,
            modules: [
                path.join(__dirname, "ClientApp"),
                "node_modules"
            ]
        },
        entry: {
            [`${cssPath}/main`]: pathConfig.cssPaths.main,
            [`${cssPath}/pages/reports/reportjqgridlayout`]: pathConfig.cssPaths.reportjqgridlayout,
            [`${cssPath}/pages/playercommission/index`]: pathConfig.cssPaths.playercomission,
            [`${cssPath}/pages/playerBatchUpdate/index`]: pathConfig.cssPaths.playerBatchUpdate,

            [`${jsPath}/language`]: pathConfig.scriptPaths.language,
            [`${jsPath}/language-zh-CN`]: pathConfig.scriptPaths.languageCN,
            [`${jsPath}/language-vi-VN`]: pathConfig.scriptPaths.languageVN,
            [`${jsPath}/pages/playercommission/multiplemovegroup`]: [pathConfig.scriptPaths.playercomissionmultiplemovegroup],
            [`${jsPath}/pages/playercommission/multipleremovefromgroup`]: [pathConfig.scriptPaths.playercomissionmultipleremovefromgroup],
            [`${jsPath}/pages/playercommission/multipleupdategroup`]: [pathConfig.scriptPaths.playercomissionmultipleupdategroup],
            [`${jsPath}/pages/playercommission/singlemovegroup`]: [pathConfig.scriptPaths.playercomissionsinglemovegroup],
            [`${jsPath}/pages/playercommission/singleremovefromgroup`]: [pathConfig.scriptPaths.playercomissionsingleremovefromgroup],
            [`${jsPath}/pages/playercommission/singleupdategroup`]: [pathConfig.scriptPaths.playercomissionsingleupdategroup],
            [`${jsPath}/pages/playerBatchUpdate/specialLevel`]: [pathConfig.scriptPaths.playerBatchUpdateSpecialLevel],
            [`${jsPath}/pages/playerBatchUpdate/oddsGroup`]: [pathConfig.scriptPaths.playerBatchUpdateOddsGroup],
            [`${jsPath}/pages/playerBatchUpdate/baStatus`]: [pathConfig.scriptPaths.playerBatchUpdateBAStatus]
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].chunk.js"
        },
        module: {
            rules: rules.commonRules
        },
        plugins: pluginGenerator(),
        stats: {
            assets: false,
            colors: true,
            version: false,
            hash: false,
            timings: true,
            chunks: false,
            chunkModules: false,
            children: false
        },
        optimization: {
            minimizer: [
                new UglifyJsPlugin({
                    sourceMap: true
                })
            ],
            splitChunks: {
                chunks: 'all',
                minSize: 30000,
                maxSize: 0,
                minChunks: 2,
                maxAsyncRequests: 5,
                maxInitialRequests: 3,
                automaticNameDelimiter: '~',
                name: true,
                cacheGroups: {
                    // disable default cache group
                    default: false,
                    vendors: false,
                    common: {
                        name: `${jsPath}/pages/lib/common`,
                        test: /(node_modules|ClientApp[\\/]src[\\/]jquery-pages[\\/]js[\\/]vendors|ClientApp[\\/]src[\\/]jquery-pages[\\/]js[\\/]common)/,
                        minChunks: 2,
                        priority: -10
                    },
                    jqgrid: {
                        name: `${jsPath}/pages/lib/jqgrid`,
                        test: /(ClientApp[\\/]src[\\/]jquery-pages[\\/]js[\\/]lib[\\/]jqgrid)/,
                        minChunks: 2,
                        priority: -15
                    }
                }
            }
        }
    };

    return commonConfigs;
}

module.exports = generateConfig;
