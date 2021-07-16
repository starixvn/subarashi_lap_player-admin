var webpack = require('webpack');
var WebpackBuildNotifierPlugin = require('webpack-build-notifier');
var Visualizer = require('webpack-visualizer-plugin');

function generatePlugins(environment) {
    var commonPlugins = [
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery",
            jquery: 'jquery',
            'window.$': 'jquery',
            "window.jQuery": 'jquery',
            "root.jQuery": 'jquery'
        }),
         new WebpackBuildNotifierPlugin({
             title: "New UI Webpack Build",
             suppressSuccess: true
        }),
        new Visualizer({
            filename: './webpack-stats-newui.html'
        })
    ];

    return commonPlugins;
}

module.exports = generatePlugins;