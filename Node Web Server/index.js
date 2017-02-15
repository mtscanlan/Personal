console.log("Creating server");

// Gets the required components
var server = require("./server.js");
var router = require("./router.js");
var pageserver = require("./pageserver.js");

// Starts the server.
server.Start(router.route, pageserver.serve, pageserver.reqtype);