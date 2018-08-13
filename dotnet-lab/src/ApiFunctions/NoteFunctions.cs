using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using TodoApp.AWSServices;
using TodoApp.CommonServices;
namespace TodoApp.ApiFunctions 
{

    public class NoteFunctions
    {
        public async Task<APIGatewayProxyResponse> AddNote(APIGatewayProxyRequest request, ILambdaContext context)
		{
			context.Logger.LogLine("Get Request\n");
            var noteR = JsonConvert.DeserializeObject<NoteRequest>(request.Body);
            var note = await new DynamoService().AddNote(new NoteModel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = noteR.Name,
                CreatedOn = DateTime.UtcNow,
                Done = false
            });
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(note),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
		}

        public async Task<APIGatewayProxyResponse> GetNotes(APIGatewayProxyRequest request, ILambdaContext context)
		{
			context.Logger.LogLine("Get Request\n");
            var notes = await new DynamoService().GetNotes();
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(notes),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
		}

        public async Task<APIGatewayProxyResponse> EditNote(APIGatewayProxyRequest request, ILambdaContext context)
		{
			context.Logger.LogLine("Get Request\n");
            var noteR = JsonConvert.DeserializeObject<NoteRequest>(request.Body);
            var note = await new DynamoService().UpdateNote(new NoteModel()
            {
                Id = noteR.Id,
                Name = noteR.Name,
                CreatedOn = DateTime.UtcNow,
                Done = false
            });
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(note),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
		}

        public async Task<APIGatewayProxyResponse> DeleteNote(APIGatewayProxyRequest request, ILambdaContext context)
		{
			context.Logger.LogLine("Get Request\n");
            var noteR = JsonConvert.DeserializeObject<NoteRequest>(request.Body);
            var note = await new DynamoService().DeleteNote(noteR.Id);
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(note),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
		}
    }
}