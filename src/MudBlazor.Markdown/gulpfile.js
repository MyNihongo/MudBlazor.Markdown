const { src, dest, series } = require("gulp");
const webpack = require("webpack-stream");
const rename = require("gulp-rename");
const minifyCss = require("gulp-clean-css");
const changeCase = require("change-case");

function cssMain() {
	return src("Resources/*.css")
		.pipe(minifyCss())
		.pipe(rename({ extname: ".min.css" }))
		.pipe(dest("wwwroot"));
}

function cssCodeStyles() {
	return src("Resources/CodeStyles/src/**/*.css")
		.pipe(minifyCss({ level: { 1: { specialComments: "0" } } }))
		.pipe(rename(function (path) {
			path.dirname = changeCase.camelCase(path.dirname);
			path.extname = ".min.css";
		}))
		.pipe(dest("wwwroot/code-styles"));
}

function js() {
	return src("Resources/MudBlazor.Markdown.js")
		.pipe(webpack())
		.pipe(rename({ basename: "MudBlazor.Markdown", extname: ".min.js" }))
		.pipe(dest("wwwroot"));
}

exports.default = series(cssMain, cssCodeStyles, js);
