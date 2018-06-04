using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.ViewModels
{
    public class EventSubscriberDTO
    {
        public int id { get; set; }
        public UserDTO subscriber { get; set; }
        [JsonProperty(PropertyName ="event")]
        public EventDTO targetEvent { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}
