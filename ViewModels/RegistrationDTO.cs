using LiveQ.Api.ViewModels.Validations;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LiveQ.Api.ViewModels
{
    [Validator(typeof(RegistrationViewModelValidator))]
    public class RegistrationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName {get;set;}
        public string LastName {get;set;}
    }
}
