using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using SunTzu.Core.Cache;

namespace SunTzu.Core.AOP
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : HandlerAttribute
    {
        public const string LOCAL_CACHE = "Local_Cache";
        public const string REMOTE_CACHE = "Remote_Cache";
        public const string SESSION = "Session";
        public const string REQUEST = "Request";
        public const string MEM_REMOTE_CACHE = "Mem_Remote_Cache";

        private CacheMode cacheMode;
        private CacheType cacheType = CacheType.FETCH;
        private string group;
        private string name;

        public CacheAttribute(string group, string name, CacheMode cacheMode)
        {
            this.group = group;
            this.name = name;
            this.cacheMode = cacheMode;
        }

        public CacheAttribute(string group, string name, CacheMode cacheMode, CacheType cacheType)
        {
            this.group = group;
            this.name = name;
            this.cacheMode = cacheMode;
            this.cacheType = cacheType;
        }

        public string Name
        {
            get { return name; }
        }

        public string Group
        {
            get { return group; }
        }

        public CacheMode CacheMode
        {
            get { return cacheMode; }
        }

        public CacheType CacheType
        {
            get { return cacheType; }
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return container.Resolve<CacheCallHandler>();
        }
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class CacheKeyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class CacheValueAttribute : Attribute
    {
    }
}