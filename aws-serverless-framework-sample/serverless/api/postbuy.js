'use strict';

var AWS = require('aws-sdk');
module.exports.handler = (event, context, callback) => {
  console.log(event);
   var docClient = new AWS.DynamoDB.DocumentClient();
   var table = process.env.BUY_ORDERS;
   var body = JSON.parse(event.body);
   var exchange = body.itemA + ":" + body.itemB;
   var price = body.price;
   var params = {
       TableName: table,
       Item: {
           "exchange": exchange,
           "price" : price,
           "statue" : "PENDING"
       }
   };
   console.log("Adding " + JSON.stringify(params) + " to " + table);
   docClient.put(params, function(err, data) {
       if (err) {
           callback(null, {
             statusCode: 500,
             body: JSON.stringify({
               Error: err
             }),
           });
       } else {
         callback(null, {
           statusCode: 200,
           body: JSON.stringify({
             Item: params.Item
           }),
         });
       }
   });
};
