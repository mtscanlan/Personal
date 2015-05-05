var path = require("path");

// Routes the request, so that a proper file can be found for it.
var route = function(pathname) {
	
    var path = process.cwd() + "/"; // Sets up the path to the current directory, the one that will contain the pages.
    var corrected = false; // If the file ending has been corrected before hand

	// If it should be routed to the home page.
    if (pathname === "" || pathname === "index" || pathname === "home" || pathname === "index.html" || pathname === "home.html") {
        path += "index.html"; // Sets it to the index page
        corrected = true;
    } else {
        // If it isn't any of those, then just appends the pathname
        path += pathname;
    }

    // Splits it using "." seperator. If the lenght of the split is only one
    // then no file type has been specified, and so one will be generated
    var pathSplit = pathname.split(".");

	// If the split leaves length one then appends .html to the end.
    if (pathSplit.length === 1 && corrected === false) {
        path += ".html";

    }
    console.log("Path is : " + path);
    return path; // Returns the path.

};

exports.route = route;