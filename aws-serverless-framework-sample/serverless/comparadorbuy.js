'use strict';

exports.handler = (event, context, callback) => {
    console.log("EventType: " + event.Records[0].eventName);
    console.log("Object: " + event.Records[0].dynamodb);
    callback();
  }
