using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.ViewModels
{
    public class SubscriberDTO : UserDTO
    {
        public List<EventDTO> events { get; set; }
    }
}
