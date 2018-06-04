using FluentValidation.Attributes;
using LiveQ.Api.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.ViewModels
{
    [Validator(typeof(CreateEventDTOValidator))]
    public class CreateEventDTO
    {
        public string id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string timeLimit { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}
