'use strict';

var AWS = require('aws-sdk');
module.exports.handler = (event, context, callback) => {
  console.log(event);
   var docClient = new AWS.DynamoDB.DocumentClient();
   var table = process.env.BUY_ORDERS;
   var exchange = event.body.itemA + ":" + event.body.itemB;
   var price = event.body.price;
   var params = {
       TableName: table,
       Item: {
           "exchange": exchange,
           "price" : price
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
