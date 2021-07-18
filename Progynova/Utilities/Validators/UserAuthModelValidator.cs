using System.Collections.Generic;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Progynova.Models.Request;

namespace Progynova.Utilities.Validators
{
    public class UserAuthModelValidator : AbstractValidator<UserAuthModel>, IModelValidator
    {
        public UserAuthModelValidator()
        {
            CascadeMode = CascadeMode.Stop;
        }

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if ()
            {
                
            }
        }
    }
}