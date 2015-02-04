using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    public class ValidateResult
    {
        public bool IsValid { get; private set; }

        public List<string> InvalidFields { get; private set; }

        public string Message { get; private set; }

        public ValidateResult(bool isValid, IEnumerable<string> invalidFiels, string message)
        {
            IsValid = isValid;
            InvalidFields = new List<string>();
            if(invalidFiels != null)
            {
                InvalidFields.AddRange(invalidFiels);
            }
            Message = message;
        }

        public static ValidateResult CreateValidResult()
        {
            return new ValidateResult(true, null, "Valid");
        }
    }
}
