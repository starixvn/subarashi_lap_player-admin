const path = require("path");
const { merge } = require("webpack-merge");
const TerserPlugin = require("terser-webpack-plugin");

const pathConfig = require("../config/path");
const alias = require("../config/alias");
const rules = require("../config/rule");
const pluginGenerator = require("../config/plugin");

function generateConfig(environment) {
    var jsPath = `${pathConfig.scriptPaths.build}/${environment}`;
    var cssPath = `${pathConfig.cssPaths.build}/${environment}`;

    return {
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
            [`${jsPath}/language-th-TH`]: pathConfig.scriptPaths.languageTH,
            [`${jsPath}/language-id-ID`]: pathConfig.scriptPaths.languageID,
            [`${jsPath}/pages/playercommission/multiplemovegroup`]: [pathConfig.scriptPaths.playercomissionmultiplemovegroup],
            [`${jsPath}/pages/playercommission/multipleremovefromgroup`]: [pathConfig.scriptPaths.playercomissionmultipleremovefromgroup],
            [`${jsPath}/pages/playercommission/multipleupdategroup`]: [pathConfig.scriptPaths.playercomissionmultipleupdategroup],
            [`${jsPath}/pages/playercommission/singlemovegroup`]: [pathConfig.scriptPaths.playercomissionsinglemovegroup],
            [`${jsPath}/pages/playercommission/singleremovefromgroup`]: [pathConfig.scriptPaths.playercomissionsingleremovefromgroup],
            [`${jsPath}/pages/playercommission/singleupdategroup`]: [pathConfig.scriptPaths.playercomissionsingleupdategroup],
            [`${jsPath}/pages/playerBatchUpdate/specialLevel`]: [pathConfig.scriptPaths.playerBatchUpdateSpecialLevel],
            [`${jsPath}/pages/playerBatchUpdate/oddsGroup`]: [pathConfig.scriptPaths.playerBatchUpdateOddsGroup]
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
                new TerserPlugin({
                    parallel: true,
                    terserOptions: {
                        format: { comments: false },
                        compress: { drop_console: true }
                    },
                    extractComments: false
                })
            ],
            splitChunks: {
                chunks: 'all',
                minSize: 30000,
                minChunks: 2,
                maxAsyncRequests: 5,
                maxInitialRequests: 3,
                automaticNameDelimiter: '~',
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
}

module.exports = generateConfig;