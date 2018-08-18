'use strict';

exports.handler = function(event, context, callback) {

    console.log("request: " + JSON.stringify(event));

    var response = {
        statusCode: responseCode,
        headers: {
            "x-custom-header" : "my custom header value"
        },
        body: JSON.stringify(responseBody)
    };
    console.log("response: " + JSON.stringify(response))
    callback(null, response);
};
