using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    static class RuleContainer
    {
        private static List<EntityValidator> allValidators = null;

        public static EntityValidator GetValidator(string type)
        {
            if (allValidators == null)
            {

            }
            return null;
        }

        private static void LoadRules()
        {
        }
    }
}
