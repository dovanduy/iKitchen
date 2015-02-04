using System.Text;
using log4net;
using Microsoft.Practices.Unity.InterceptionExtension;
using SunTzu.Utility;
using Microsoft.Practices.Unity;

namespace SunTzu.Core.AOP
{
    public class LockCallHandler : ICallHandler
    {
        private static readonly ILog logger = LogManager.GetLogger("Lock");

        #region ICallHandler Members

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            string methodName = input.MethodBase.ReflectedType + "." + input.MethodBase.Name;

            if (!isLockEnabled)
            {
                logger.Debug("Ignored lock, method=" + methodName + ", SynRoot=" + input.MethodBase);
                return getNext().Invoke(input, getNext);
            }
            
            StopWatch stopWatch = new StopWatch();
            logger.Debug("Waiting for Lock, method=" + methodName + ", SynRoot=" + input.MethodBase);
            IMethodReturn methodReturn = null;
            lock (input.MethodBase)
            {
                stopWatch.Stop();
                logger.Debug("Got Lock, WaitedMs=" + stopWatch.ElapsedMs());                
                methodReturn = getNext().Invoke(input, getNext);
            }
            logger.Debug("Released Lock, method=" + methodName + ", SynRoot=" + input.MethodBase);            

            return methodReturn;
        }

        public int Order
        {
            get { return 1; }
            set { }
        }

        private bool isLockEnabled = false;
        [Dependency("EnableLock")]
        public string EnableLock
        {
            set { isLockEnabled = value.Equals("Y", System.StringComparison.OrdinalIgnoreCase); }
        }
    
        #endregion
    }
}