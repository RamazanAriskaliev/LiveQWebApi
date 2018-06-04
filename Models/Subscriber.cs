using System;
using System.Collections.Generic;
using System.Text;

namespace LiveQ.Api.Models
{
    public class Subscriber : AppUser
    {
        public ICollection<EventSubscriber> SubscribedEvents { get; set; }
    }
}
