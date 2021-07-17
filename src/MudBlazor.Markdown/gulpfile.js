const { src, dest } = require("gulp");
const rename = require("gulp-rename");
const minifyCss = require("gulp-minify-css");

function css() {
	return src("Resources/*css")
		.pipe(minifyCss())
		.pipe(rename({ extname: ".min.css" }))
		.pipe(dest("wwwroot"));
}

exports.default = css