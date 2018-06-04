using FluentValidation.Attributes;
using LiveQ.Api.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.ViewModels
{
    [Validator(typeof(EditEventDTOValidator))]
    public class EditEventDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}
