const { merge } = require("webpack-merge");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const utils = require("./utils");
const pathConfig = require("../config/pathReact");
const baseWebpackConfigGenerator = require("./webpack.react.base.conf");

const baseConfigs = baseWebpackConfigGenerator();
const cssRootPath = pathConfig.cssPaths.build;
const jsRootPath = pathConfig.scriptPaths.build;

module.exports = merge(
    baseConfigs,
    utils.setDefinePlugin("process.env.NODE_ENV", "development"),
    {
        mode: "development",
        devtool: "source-map",
        watch: true,
        plugins: [
            new CleanWebpackPlugin({
                cleanOnceBeforeBuildPatterns: [jsRootPath, cssRootPath]
            })
        ]
    },
    utils.extractCSS(false)
);