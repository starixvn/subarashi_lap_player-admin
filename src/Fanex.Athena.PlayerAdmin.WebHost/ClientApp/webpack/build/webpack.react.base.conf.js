var alias = require('../config/aliasReact');
var pathConfig = require('../config/pathReact');
var pluginGenerator = require('../config/pluginReact');
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");

function generateConfig() {
    var jsPath = pathConfig.scriptPaths.build;
    var cssPath = pathConfig.cssPaths.build;
    var commonConfigs = {
        resolve: {
            alias: alias,
            extensions: ['.js', '.jsx']
        },
        entry: {
            /*js*/
            [`${jsPath}/lib/langHelper`]: pathConfig.scriptPaths.languageResource,
            [`${jsPath}/lib/lang-zh-CN`]: pathConfig.scriptPaths.langCN,

            [`${cssPath}/betlist/index`]: pathConfig.cssPaths.betlist,
            [`${cssPath}/betlist/bettingticketscenter`]: pathConfig.cssPaths.bettingticketscenter,

            [`${jsPath}/playerlist`]: pathConfig.scriptPaths.playerlist,
            [`${jsPath}/accountinfo`]: pathConfig.scriptPaths.accountinfo,
            [`${jsPath}/runningbetlist`]: pathConfig.scriptPaths.playerrunningbetlist,
            [`${jsPath}/betlimit`]: pathConfig.scriptPaths.playerbetlimit,
            [`${jsPath}/single-customer`]: pathConfig.scriptPaths.singleCustomer,
            [`${jsPath}/playerwinloss`]: pathConfig.scriptPaths.playerwinloss,
            [`${jsPath}/statement`]: pathConfig.scriptPaths.statement,
            [`${jsPath}/detailtransaction`]: pathConfig.scriptPaths.detailtransaction,

            // /*css*/
            [`${cssPath}/vendor`]: pathConfig.cssPaths.layout,
            [`${cssPath}/index`]: pathConfig.cssPaths.playeradmin,
            [`${cssPath}/account-info`]: pathConfig.cssPaths.accountInfo,
            [`${cssPath}/running-betlist`]: pathConfig.cssPaths.playerrunningbetlist,
            [`${cssPath}/betlimit`]: pathConfig.cssPaths.playerbetlimit,
            [`${cssPath}/playerwinloss`]: pathConfig.cssPaths.playerwinloss,
            [`${cssPath}/statement`]: pathConfig.cssPaths.statement,
            [`${cssPath}/detailtransaction`]: pathConfig.cssPaths.detailtransaction,
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].chunk.js",
            chunkFilename: jsPath + '/[name].chunk.js',
        },
        module: {
            rules: [{
                test: /\.(js|jsx)$/,
                loader: ["babel-loader"],
                exclude: /node_modules/
            }]
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
            children: true
        },
        node: {
            fs: 'empty'
        },
        externals: [
            { './jszip': 'jszip' },
            { './ods': 'ods' },
            { './cptable': 'var cptable' }
        ],
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
                name: false,
                cacheGroups: {
                    // disable default cache group
                    default: false,
                    vendors: false,
                    common: {
                        name: 'lib/vendor',
                        test: /(node_modules)/,
                        minChunks: 2,
                        priority: -10
                    }
                }
            }
        }
    };

    return commonConfigs;
}

module.exports = generateConfig;