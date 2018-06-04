using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.ViewModels.Validations
{
    public class ValidationResultModel
    {

        public string message { get; }
        public string status { get; }
        public string code { get; }

        public List<ValidationError> data { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            status = "error";
            code = "1001";
            message = "Validation Failed";
            data = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }
}
