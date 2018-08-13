using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.DynamoDBv2.Model;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using TodoApp.AWSServices;
using Amazon.Lambda.CloudWatchLogsEvents;
using TodoApp.CommonServices;

namespace TodoApp.DatabaseFunctions
{
    public class NoteDatabaseFunctions
    {
        public async Task NotifyNoteChange(DynamoDBEvent dynamoEvent, ILambdaContext context)
        {
			context.Logger.LogLine($"Beginning to process {dynamoEvent.Records.Count} records...");

            foreach (var record in dynamoEvent.Records)
            {
                context.Logger.LogLine($"Event ID: {record.EventID}");
                context.Logger.LogLine($"Event Name: {record.EventName}");

                string streamRecordJson = SerializeStreamRecord(record.Dynamodb);
                context.Logger.LogLine($"DynamoDB Record:");
                context.Logger.LogLine(streamRecordJson );
				await new SimpleEmailService().SendEmail(record.EventName, $"{streamRecordJson}");

			}

            context.Logger.LogLine("Stream processing complete.");
		}

        private string SerializeStreamRecord(StreamRecord streamRecord)
        {
            var _jsonSerializer = new JsonSerializer();
            using (var writer = new StringWriter())
            {
                _jsonSerializer.Serialize(writer, streamRecord);
                return writer.ToString();
            }
        }

        public async Task MoveToS3(ScheduledEvent scheduledEvent, ILambdaContext context)
        {
            await Task.Delay(5);
            Console.WriteLine($"Log content - {JsonConvert.SerializeObject(scheduledEvent)}");
        }
        
    }
}