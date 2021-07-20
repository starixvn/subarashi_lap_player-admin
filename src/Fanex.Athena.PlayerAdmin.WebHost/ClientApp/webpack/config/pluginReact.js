var webpack = require('webpack');
var WebpackBuildNotifierPlugin = require('webpack-build-notifier');
var Visualizer = require('webpack-visualizer-plugin');

let _buildNumber = 1;
function generatePlugins() {
    var commonPlugins = [
        new Visualizer({
            filename: './wwwroot/webpack-stats-react.html'
        }),
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
        new WebpackBuildNotifierPlugin({
            title: "React Webpack Build",
            suppressSuccess: true
        }),
         function () {
             this.plugin('done', function (stats) {
                 setTimeout(function () {
                     console.log('\n ////////// Build version: #' + (_buildNumber++) + ' (' + new Date().toLocaleString() + ').//////////\n');
                 }, 150);
             });
         }
    ];

    return commonPlugins;
}

module.exports = generatePlugins;