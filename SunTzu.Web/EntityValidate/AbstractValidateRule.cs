using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    public abstract class AbstractValidateRule : IValidateRule
    {
        #region IValidateRule Members

        public abstract bool DoValidate(object value);

        public string FieldName
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public ValidateConfigRule ValidateConfigRule
        {
            get;
            set;
        }

        public abstract string GetValidateString();

        #endregion
    }
}
