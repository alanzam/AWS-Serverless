using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using Newtonsoft.Json;
using TodoApp.CommonServices;
using System;
using System.Net;

namespace TodoApp.AWSServices
{
    public class DynamoService
    {
        private AmazonDynamoDBClient _client { get; set; } = new AmazonDynamoDBClient();
        private string _tableName = Environment.GetEnvironmentVariable("TableName");
        private NoteModel ConvertToNote(Dictionary<string, AttributeValue> dic)
        {
            long dateTicks;
            if (!long.TryParse(dic["CreatedOn"].N, out dateTicks))
                dateTicks = 0;
            var note = new NoteModel()
            {
                Id = dic["Id"].S,
                Name = dic["Name"].S,
                CreatedOn = new DateTime(dateTicks),
                Done = dic["Done"].BOOL
            };
            return note;
        }
        public async Task<NoteModel> AddNote(NoteModel note)
        {
            var obj = new Dictionary<string, AttributeValue>()
            {
                { "Id", new AttributeValue() { S = note.Id } },
                { "Name", new AttributeValue() { S = note.Name }},
                { "CreatedOn", new AttributeValue() { N = DateTime.UtcNow.Ticks.ToString() }},
                { "Done", new AttributeValue() { BOOL = note.Done }}
            };
            var response = await _client.PutItemAsync(_tableName, obj);
            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not add Note, {response.HttpStatusCode}");
            return note;
        }

        public async Task<NoteModel> UpdateNote(NoteModel note)
        {
            var key = new Dictionary<string, AttributeValue>()
            {
                { "Id", new AttributeValue() { S = note.Id } }
            };
            var obj = new Dictionary<string, AttributeValueUpdate>()
            {
                { "Name", new AttributeValueUpdate(new AttributeValue() { S = note.Name }, AttributeAction.PUT)},
                { "Done", new AttributeValueUpdate(new AttributeValue() { BOOL = note.Done }, AttributeAction.PUT)}
            };
            var response = await _client.UpdateItemAsync(_tableName, key, obj);
            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not add Note, {response.HttpStatusCode}");
            return note;
        }

        public async Task<NoteModel> GetNote(string id)
        {
            var response = await _client.GetItemAsync(_tableName, new Dictionary<string, AttributeValue>()
            {
                { "Id", new AttributeValue(id) }
            });
            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not get Note, {response.HttpStatusCode}");
            var noteDic = response.Item;
            return ConvertToNote(noteDic);
        }

        public async Task<List<NoteModel>> GetNotes()
        {
            var response = await _client.ScanAsync(_tableName, new List<string> { "Id", "Name", "CreatedOn", "Done" } );
            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not get Notes, {response.HttpStatusCode}");
            var noteDics = response.Items;
            var notes = new List<NoteModel>();
            foreach (var dic in noteDics)
            {
                var note = ConvertToNote(dic);
                notes.Add(note);
            }
            return notes;
        } 

        public async Task<NoteModel> DeleteNote(string id)
        {
            var response = await _client.DeleteItemAsync(_tableName, new Dictionary<string, AttributeValue>()
            {
                { "Id", new AttributeValue(id) }
            });
            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not delete Note, {response.HttpStatusCode}");
            return ConvertToNote(response.Attributes);
        }

    }
}