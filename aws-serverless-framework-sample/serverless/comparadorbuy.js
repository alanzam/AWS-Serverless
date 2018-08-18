'use strict';

var AWS = require('aws-sdk');

exports.handler = (event, context, callback) => {
    console.log("EventType: " + event.Records[0].eventName);
    console.log("Object: " + event.Records[0].dynamodb);
    var obj = event.Records[0].dynamodb;
    var docClient = new AWS.DynamoDB.DocumentClient()
    var table = process.env.SELL_ORDERS;
    var exchange = obj.Keys.exchange.S;
    var price = obj.Keys.price.S;
    var params = {
        TableName: table,
        Key:{
          "exchange": exchange,
          "price" : price
        },
        UpdateExpression: "set status = :s",
        ExpressionAttributeValues:{
            ":s":"RESOLVED"
        },
        ReturnValues:"UPDATED_NEW"
    };
    console.log("Updating the item...");
    docClient.update(params, function(err, data) {
        if (err) {
            console.error("Unable to update item. Error JSON:", JSON.stringify(err, null, 2));
        } else {
            console.log("UpdateItem succeeded:", JSON.stringify(data, null, 2));
        }
        callback();
    });

  }
