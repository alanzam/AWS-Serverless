using System;

namespace TodoApp.CommonServices
{
    public class NoteModel
    {
        public string Id {get; set;}
        public string Name {get; set;}
        public DateTime CreatedOn {get; set;}
        public bool Done {get; set;}
    }
}