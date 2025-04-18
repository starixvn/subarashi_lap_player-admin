const { merge } = require("webpack-merge");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const utils = require("./utils");
const pathConfig = require("../config/path");

const baseWebpackConfigGenerator = require("./webpack.base.conf");

const environment = "dev";
const baseConfigs = baseWebpackConfigGenerator(environment);
const cssRootPath = `${pathConfig.cssPaths.build}/${environment}`;
const jsRootPath = `${pathConfig.scriptPaths.build}/${environment}`;

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