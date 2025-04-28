const webpack = require("webpack");
const RemoveEmptyScriptsPlugin = require('webpack-remove-empty-scripts');
const WebpackBuildNotifierPlugin = require("webpack-build-notifier");
const Visualizer = require("webpack-visualizer-plugin2");

function generatePlugins() {
    return [
        new webpack.IgnorePlugin({
            resourceRegExp: /^\.\/locale$/,
            contextRegExp: /moment/
        }),
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            jquery: "jquery",
            "window.$": "jquery",
            "window.jQuery": "jquery",
            "root.jQuery": "jquery"
        }),
        new RemoveEmptyScriptsPlugin(),
        new WebpackBuildNotifierPlugin({
            title: "New UI Webpack Build",
            suppressSuccess: true
        }),
        new Visualizer({
            filename: "./wwwroot/webpack-stats-newui.html"
        })
    ];
}

module.exports = generatePlugins;