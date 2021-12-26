const { src, dest, series } = require("gulp");
const webpack = require("webpack-stream");
const rename = require("gulp-rename");
const minifyCss = require("gulp-minify-css");

function css() {
    return src("Resources/*.css")
        .pipe(minifyCss())
        .pipe(rename({ extname: ".min.css" }))
        .pipe(dest("wwwroot"));
}

function js() {
    return src("Resources/MudBlazor.Markdown.js")
        .pipe(webpack())
        .pipe(rename({ basename: "MudBlazor.Markdown", extname: ".min.js" }))
        .pipe(dest("wwwroot"));
}

exports.default = series(css, js);
