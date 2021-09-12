const path = require("path");
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

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
			},
		]
	},
	plugins: [
		new MiniCssExtractPlugin({
			filename: "MudBlazor.Markdown.min.css"
		})
	],
};