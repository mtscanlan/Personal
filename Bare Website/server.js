var http = require("http");
var url = require("url");

// Starts the server
var Start = function(route, serve, reqtype) {

    // Lauched when there is a request.
    var onRequest = function(request, response) {

        // Extracts the pathname from the url
        var pathname = url.parse(request.url).pathname;

        // Removes the starting "/". If this fails, then that means the request
        // was without
        // the "/", and so does not affect it.
        try {
            pathname = pathname.substring(1, pathname.length);
        } catch (err) {}

        // Responds to all requests apart from that for favicon.ico
        if (pathname !== "favicon.ico") {
            console.log("Request has been recieved for " + pathname);

            // Gets the path from the router
            var path = route(pathname);
            console.log("Path has been generated");
            var html = "";
			var type = "plain";
			
			try {
				// Gets html or whatever will be written from the server
				html = serve(path);
				console.log("Html has been generated");

				// Gets the type from the pageserver
				type = reqtype(path);
				console.log("Filetype has been found");
			} catch (err2) {
				path = route("404.html");
				html = serve(path);;
				type = "html";
				console.log("Reading file was unsuccesful.");
			}

            // Writes what type of data will be sent. Dynamically sets file ending.
            response.writeHead(200, {
                "Content-Type" : "text/" + type
            });
            response.write(html);
            // ends connection
            response.end();
            console.log("Request answered successfully");
        }
		else 
		{
			console.log("Request handler 'favicon' was called.");
			var img = serve('./images/favicon.ico');
			response.writeHead(200, {"Content-Type": "image/x-icon"});
			response.end(img,'binary');
		}

    };

    http.createServer(onRequest).listen(42850);
    console.log("Server has been started");
};

exports.Start = Start;