using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace SunTzu.Core.AOP
{
    public class RetryHandler : ICallHandler
    {
        private int retryTime = 0;
        private int order;

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            RetryAttribute retryAttribute = (RetryAttribute) input.MethodBase.GetCustomAttributes(typeof(RetryAttribute), false)[0];

            IMethodReturn returnValue = DoInvoke(input, getNext);

            if (returnValue.Exception != null && IsTypeMatch(returnValue.Exception.GetType(), retryAttribute))
            {
                if (retryTime >= retryAttribute.RetryMax)
                    return returnValue;

                retryTime++;

                return Invoke(input, getNext);
            }

            return returnValue;
        }

        private bool IsTypeMatch(Type exceptionType, RetryAttribute retryAttribute)
        {
            return exceptionType == retryAttribute.Type
                || exceptionType.IsSubclassOf(retryAttribute.Type);
        }

        private IMethodReturn DoInvoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            return getNext()(input, getNext);
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }
    }
}
