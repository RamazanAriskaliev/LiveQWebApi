using System;
using System.Collections.Generic;
using System.Text;

namespace LiveQ.Api.Models
{
    public class Creator : AppUser
    {
        public ICollection<Event> CreatedEvents { get; set; }
    }
}
