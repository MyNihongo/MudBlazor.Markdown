const { src, dest, series } = require("gulp");
const rename = require("gulp-rename");
const minifyCss = require("gulp-minify-css");
const minifyJs = require("gulp-minify");

function css() {
    return src("Resources/*.css")
        .pipe(minifyCss())
        .pipe(rename({ extname: ".min.css" }))
        .pipe(dest("wwwroot"));
}

function js() {
    return src("Resources/*.js")
        .pipe(minifyJs({ noSource: true, ext: { min: ".min.js" } }))
        .pipe(dest("wwwroot"));
}

exports.default = series(css, js);
