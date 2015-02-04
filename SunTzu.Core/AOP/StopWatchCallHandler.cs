using System.Text;
using log4net;
using Microsoft.Practices.Unity.InterceptionExtension;
using SunTzu.Utility;

namespace SunTzu.Core.AOP
{
    public class StopWatchCallHandler : ICallHandler
    {
        private static readonly ILog logger = LogManager.GetLogger("StopWatch");

        #region ICallHandler Members

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            string methodName = input.MethodBase.ReflectedType + "." + input.MethodBase.Name;
            StringBuilder param = new StringBuilder();
            for (int i = 0; i < input.Arguments.Count; i++)
            {
                param.Append(input.Arguments.GetParameterInfo(i).Name + "=" + input.Arguments[i] + ",");
            }
            logger.Debug("Start Invoke, method=" + methodName + ", params=" + param);
            
            StopWatch stopWatch = new StopWatch();

            IMethodReturn methodReturn = getNext().Invoke(input, getNext);

            stopWatch.Stop();
            logger.Debug("End Invoke, method=" + methodName + ", ElapsedMs=" + stopWatch.ElapsedMs());

            return methodReturn;
        }

        public int Order
        {
            get { return 0; }
            set { }
        }

        #endregion
    }
}