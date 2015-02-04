using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace SunTzu.Core.AOP
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RetryAttribute : HandlerAttribute
    {
        private int retryMax;
        private Type type;

        public RetryAttribute(Type type, int retryMax)
        {
            this.retryMax = retryMax;
            this.type = type;
        }

        public int RetryMax
        {
            get { return retryMax; }
        }

        public Type Type
        {
            get { return type; }
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return container.Resolve<RetryHandler>();
        }
    }

}
