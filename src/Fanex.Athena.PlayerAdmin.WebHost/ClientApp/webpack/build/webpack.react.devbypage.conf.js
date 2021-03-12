const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const devConfig = require('./webpack.react.dev.conf.js');
var pathConfig = require('../config/pathReact');

var environment = "dev";
var jsPath = `${pathConfig.scriptPaths.build}/${environment}/reactpages`;
var cssPath = `${pathConfig.cssPaths.build}/${environment}/reactpages`;
const entry = {
    [`${jsPath}/lib/langHelper`]: pathConfig.scriptPaths.languageResource,
    [`${jsPath}/lib/lang-zh-CN`]: pathConfig.scriptPaths.langCN,
    [`${cssPath}/vendor`]: pathConfig.cssPaths.layout,

    [`${jsPath}/report/dailybets`]: pathConfig.scriptPaths.dailybets,
    [`${cssPath}/dailybets/index`]: pathConfig.cssPaths.dailybets,
}

const devByPageConfig = {
    ...devConfig,
    entry,
    plugins: [
        new MiniCssExtractPlugin({ filename: "[name].css", chunkFilename: '[id].css' })
    ]
};

module.exports = devByPageConfig;