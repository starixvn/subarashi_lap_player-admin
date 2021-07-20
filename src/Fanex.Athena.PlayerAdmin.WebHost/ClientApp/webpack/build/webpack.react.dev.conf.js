var merge = require('webpack-merge');
var utils = require('./utils');
var baseWebpackConfigGenerator = require('./webpack.react.base.conf');
var CleanWebpackPlugin = require('clean-webpack-plugin');

var pathConfig = require('../config/pathReact');
var baseConfigs = baseWebpackConfigGenerator();

var cssRootPath = pathConfig.cssPaths.build;
var jsRootPath = pathConfig.scriptPaths.build;

module.exports = merge(baseConfigs,
    utils.setDefinePlugin('process.env.NODE_ENV', 'development'),
    {
        mode: 'development',
        devtool: 'source-map',
        watch: true,
        plugins: [
            new CleanWebpackPlugin([`${jsRootPath}/`], {
                root: process.cwd()
            }),
            new CleanWebpackPlugin([`${cssRootPath}/`], {
                root: process.cwd()
            }),
        ]
    },

    utils.extractCSS(false)
);
