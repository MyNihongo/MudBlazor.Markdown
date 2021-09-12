const path = require("path");

module.exports = {
	entry: ["./Resources/Npm.js", "./Resources/MudBlazor.Markdown.js"],
	output: {
		path: path.resolve(__dirname, "wwwroot"),
		filename: "MudBlazor.Markdown.min.js"
	}
};