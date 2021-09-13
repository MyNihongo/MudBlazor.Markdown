const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const TerserPlugin = require("terser-webpack-plugin");

module.exports = {
	entry: [
		 "./Resources/MudBlazor.Markdown.js",
		 "./Resources/MudBlazor.Markdown.css"
		],
	output: {
		path: path.resolve(__dirname, "wwwroot"),
		filename: "MudBlazor.Markdown.min.js",
		publicPath: "/"
	},
	mode: "production",
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
		minimize: true,
		minimizer: [
			new CssMinimizerPlugin(),
			new TerserPlugin({ parallel: true })
		]
	},
	plugins: [
		new MiniCssExtractPlugin({
			filename: "MudBlazor.Markdown.min.css"
		})
	]
};