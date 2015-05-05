var path = require("path");
var fs = require("fs");

// Serves the page by returning the html.
var serve = function(path) {
    var html = "";
    html = fs.readFileSync(path);
    return html;
};

// returns the type of file aka filename extension
var reqtype = function(path) {

    var type = "";
    var pathSplit = path.split(".");
	
	// If for some reason no ending, output as plain text file
    if (pathSplit === 1) {
        type = "plain";
    } else {
        type = pathSplit[1]; // Returns thee part after the "." seperator
    }

    return type;
};

exports.serve = serve;
exports.reqtype = reqtype;