using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace SunTzu.Core.AOP
{
    public class AssemblyNamesMatchingRule : IMatchingRule
    {
        private readonly string[] assemblyNames;

        public AssemblyNamesMatchingRule(string[] assemblyNames)
        {
            this.assemblyNames = assemblyNames;
        }

        #region IMatchingRule Members

        public bool Matches(MethodBase member)
        {
            foreach (string assemblyName in assemblyNames)
            {
                if (member.ReflectedType.AssemblyQualifiedName.StartsWith(assemblyName))
                    return true;
            }
            return false;
        }

        #endregion
    }
}