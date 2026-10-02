const { src, dest, series } = require("gulp");
const rename = require("gulp-rename");
const minifyCss = require("gulp-clean-css");

function cssMain() {
	return src("Resources/*.css")
		.pipe(minifyCss())
		.pipe(rename({ extname: ".min.css" }))
		.pipe(dest("wwwroot"));
}

function jsMain() {
	return src("Resources/MudBlazor.Markdown.js")
		.pipe(rename({ extname: ".min.js" }))
		.pipe(dest("wwwroot"));
}

exports.default = series(cssMain, jsMain);
