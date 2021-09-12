const path = require("path");
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");

module.exports = {
	entry: [
		"./Resources/Npm.js",
		 "./Resources/MudBlazor.Markdown.js",
		 "./Resources/MudBlazor.Markdown.css"
		],
	output: {
		path: path.resolve(__dirname, "wwwroot"),
		filename: "MudBlazor.Markdown.min.js",
		publicPath: "/"
	},
	module: {
		rules: [
			{
				test: /\.css$/,
				use: [
					MiniCssExtractPlugin.loader,
					"css-loader"
				]
			}
		]
	},
	optimization: {
		minimizer: [
			new CssMinimizerPlugin(),
			new UglifyJsPlugin()
		]
	},
	plugins: [
		new MiniCssExtractPlugin({
			filename: "MudBlazor.Markdown.min.css"
		})
	]
};