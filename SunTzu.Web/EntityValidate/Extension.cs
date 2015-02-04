using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunTzu.Core.Data;

namespace SunTzu.Web.EntityValidate
{
    public static class Extension
    {
        public static ValidateResult Validate(this IEntity entity)
        {
            var entityValidator = RuleContainer.GetValidator(entity.GetType().Name);
            if(entityValidator != null)
            {
                return entityValidator.Validate(entity);
            }
            return ValidateResult.CreateValidResult();
        }
    }
}
