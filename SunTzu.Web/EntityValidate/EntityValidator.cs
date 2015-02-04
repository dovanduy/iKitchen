using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunTzu.Core.Data;

namespace SunTzu.Web.EntityValidate
{
    public class EntityValidator : IEntityValidator
    {
        public string Type { get; set; }

        private List<IValidateRule> validateRules = new List<IValidateRule>();

        public List<IValidateRule> ValidateRules
        {
            get { return validateRules; }
            set { validateRules = value; }
        }
        

        #region IEntityValidator Members

        public ValidateResult Validate(IEntity entity)
        {
            bool isValid = true;

            return new ValidateResult(isValid, null, "");
        }

        #endregion

        public static EntityValidator CreateDummy(string type)
        {
            return new EntityValidator() { Type = type };
        }
    }
}
