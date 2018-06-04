using FluentValidation;
using LiveQ.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.ViewModels.Validations
{
    public class CreateEventDTOValidator : AbstractValidator<CreateEventDTO>
    {
        public CreateEventDTOValidator()
        {
            RuleFor(m => m.end).NotEmpty().WithMessage("End date is required").WithErrorCode("1001");
            RuleFor(m => m.start).NotEmpty().WithMessage("Start Date is Required").WithErrorCode("1002");
            RuleFor(m => m.timeLimit).NotEmpty().WithMessage("Time limit is Required").WithErrorCode("1003");
            RuleFor(m => m.title).NotEmpty().WithMessage("Title is Required").WithErrorCode("1004");
            RuleFor(m => m.description).NotNull().WithMessage("Description is null").WithErrorCode("1011");

            RuleFor(m => m.start).Must(BeAValidDate).WithMessage("StartDate invalid date/time").WithErrorCode("1005");
            RuleFor(m => m.end).Must(BeAValidDate).WithMessage("EndDate invalid date/time").WithErrorCode("1006");
            RuleFor(m => m.timeLimit).Must(BeAValidTimeSpan).WithMessage("timeLimit invalid TimeSpan").WithErrorCode("1007");

            RuleFor(m => DateConverter.ToDateTime(m.end)).GreaterThanOrEqualTo(m => DateConverter.ToDateTime(m.start)).WithErrorCode("1008").WithMessage("End date must after Start date");
            RuleFor(m => DateConverter.ToDateTime(m.start)).GreaterThanOrEqualTo(DateTime.Now).WithErrorCode("1009").WithMessage("Start date must be greater than or equal 'now'");
            RuleFor(m => DateConverter.ToDateTime(m.end)).GreaterThan(m=>(DateConverter.ToDateTime(m.start).Add(TimeSpan.Parse(m.timeLimit)))).WithErrorCode("1010").WithMessage("Time limit must be limited by th end time");
        }

        private bool BeAValidDate(string value)
        {
            long date;
            return long.TryParse(value, out date);
        }
        
        private bool BeAValidTimeSpan(string value)
        {
            TimeSpan timeSpan;
            return TimeSpan.TryParse(value, out timeSpan);
        }
        
    }
}
