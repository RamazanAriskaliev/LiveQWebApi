using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.ViewModels
{
    public class EventDTO
    {
        public EventDTO()
        {
            subscribers = new List<EventSubscriberDTO>();
        }
        public int id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string timeLimit { get; set; }
        public string title { get; set; }
        public List<EventSubscriberDTO> subscribers { get; set; }
        public UserDTO creator { get; set; }
    }
}
