using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.ViewModels.Validations
{
    public class EditEventDTOValidator : AbstractValidator<EditEventDTO>
    {
        public EditEventDTOValidator()
        {
            RuleFor(m => m.title).NotEmpty().WithMessage("Title is Required").WithErrorCode("1004");
            RuleFor(m => m.description).NotNull().WithMessage("Description is null").WithErrorCode("1011");
        }
        
    }
}
