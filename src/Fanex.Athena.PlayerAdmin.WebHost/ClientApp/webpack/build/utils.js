const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

exports.setDefinePlugin = function (key, value) {
    return {
        plugins: [
            new webpack.DefinePlugin({
                [key]: JSON.stringify(value)
            })
        ]
    };
};

exports.extractCSS = function(isProduction) {
    return {
        plugins: [
                new MiniCssExtractPlugin({ filename: "[name].css", chunkFilename: '[id].css'})
        ],
        module: {
            rules: [
                {
                    test: /\.(sc|c)ss$/,
                    use: [
                        MiniCssExtractPlugin.loader,
                        {
                            loader: "css-loader",
                            options: { url: false }
                        },
                        "sass-loader"
                    ]
                }
            ]
        }
    };
};