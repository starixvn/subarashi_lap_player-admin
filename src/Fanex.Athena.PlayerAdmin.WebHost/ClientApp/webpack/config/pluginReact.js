const webpack = require("webpack");
const WebpackBuildNotifierPlugin = require("webpack-build-notifier");
const Visualizer = require("webpack-visualizer-plugin2"); // ✅ Dùng phiên bản mới cho Webpack 5

let _buildNumber = 1;

function generatePlugins() {
    return [
        new Visualizer({
            filename: "./wwwroot/webpack-stats-react.html"
        }),
        new webpack.IgnorePlugin({
            resourceRegExp: /^\.\/locale$/,
            contextRegExp: /moment/
        }),
        new WebpackBuildNotifierPlugin({
            title: "React Webpack Build",
            suppressSuccess: true
        }),
        {
            apply: (compiler) => {
                compiler.hooks.done.tap("BuildNotifier", (stats) => {
                    setTimeout(() => {
                        console.log(`\n ////////// Build version: #${_buildNumber++} (${new Date().toLocaleString()}).//////////\n`);
                    }, 150);
                });
            }
        }
    ];
}

module.exports = generatePlugins;