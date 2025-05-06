const { merge } = require("webpack-merge");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const utils = require("./utils");
const pathConfig = require("../config/path");

const baseWebpackConfigGenerator = require("./webpack.base.conf");

const environment = "pro";
const baseConfigs = baseWebpackConfigGenerator(environment);
const cssRootPath = `${pathConfig.cssPaths.build}/${environment}`;
const jsRootPath = `${pathConfig.scriptPaths.build}/${environment}`;

module.exports = merge(
    baseConfigs,
    utils.setDefinePlugin("process.env.NODE_ENV", "production"),
    utils.extractCSS(true),
    {
        mode: "production",
        plugins: [
            new CleanWebpackPlugin({
                cleanOnceBeforeBuildPatterns: [jsRootPath, cssRootPath]
            })
        ]
    }
);