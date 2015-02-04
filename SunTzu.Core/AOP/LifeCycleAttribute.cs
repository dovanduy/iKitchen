using System;

namespace SunTzu.Core.AOP
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LifeCycleAttribute : Attribute
    {
        private LifeCycle lifeCycle;

        public LifeCycle LifeCycle
        {
            get { return lifeCycle; }
            set { lifeCycle = value; }
        }

        public LifeCycleAttribute(LifeCycle lifeCycle)
        {
            this.lifeCycle = lifeCycle;
        }
    }

    public enum LifeCycle
    {
        Singleton,
        Request
    }
}
