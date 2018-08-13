using System.Collections.Generic;

namespace TodoApp.CommonServices
{
    public class ScheduledEvent
    {
        public string Account {get; set;}
        public string Region {get; set;}
        public string Type {get; set;}
        public string Time {get; set;}
        public string Id {get; set;}
        public string Source {get; set;}
        public List<string> Resources {get; set;}
    }
// {
//       "account": "123456789012",
//   "region": "us-east-1",
//   "detail": {},
//   "detail-type": "Scheduled Event",
//   "source": "aws.events",
//   "time": "1970-01-01T00:00:00Z",
//   "id": "cdc73f9d-aea9-11e3-9d5a-835b769c0d9c",
//   "resources": [
//     "arn:aws:events:us-east-1:123456789012:rule/my-schedule"
//   ]
// }
}