using System;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity;

namespace SunTzu.Core.AOP
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LockAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return container.Resolve<LockCallHandler>();
        }
    }
}
