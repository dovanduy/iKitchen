using System;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using SunTzu.Core.Cache;

namespace SunTzu.Core.AOP
{
    public class CacheCallHandler : ICallHandler
    {
        private static readonly ILog logger = LogManager.GetLogger("Cache");

        private int order;
        private ICacheAPI localCache;
        private ICacheAPI remoteCache;
        private ICacheAPI session;
        private ICacheAPI request;
        private ICacheAPI memRemoteCache;
        private CacheStatistics cacheStatistics;
        private CacheMetaLoader cacheMetaLoader;

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            CacheMode cacheMode = GetCacheMode(input.MethodBase);
            ICacheAPI cacheAPI = ChooseCacheAPI(cacheMode);
            if (cacheAPI == null)
            {
                logger.Error("No cache used");
                return getNext()(input, getNext);
            }

            string cacheImplName = cacheAPI.GetType().Name;


            cacheAPI = new CacheProxy(cacheAPI, cacheMode, cacheStatistics);

            CacheType cacheType = GetCacheType(input.MethodBase);

            string cacheGroup = GetCacheGroup(input.MethodBase);
            string cacheKey = CreateCacheKey(input.MethodBase, input.Arguments);

            logger.Info(string.Format("Choose cache api as {0}, cacheGroup={1}, cacheKey={2}", cacheImplName, cacheGroup, cacheKey));

            if (cacheMode.Equals(CacheMode.REMOTE) && cacheStatistics.IsError(cacheMode, cacheGroup))
            {
                logger.Error("Remote cache error occured too many, skip cache, get data from DB.");
                return getNext()(input, getNext);
            }

            if (CacheType.FETCH.Equals(cacheType))
            {
                object obj = null;
                try
                {
                    obj = cacheAPI.Get(cacheGroup, cacheKey);
                }
                catch (Exception ex)
                {
                    logger.Error("get cache failed, cachetype=" + cacheType,ex);
                    return getNext()(input, getNext);
                }

                if (obj != null)
                {
                    logger.Info("Hitting cache, cacheGroup=" + cacheGroup + ", cacheKey=" + cacheKey);
                    cacheStatistics.AddHitCount(cacheMode, cacheGroup);
                    return input.CreateMethodReturn(obj, null);
                }
                else
                {
                    cacheStatistics.AddUnhitCount(cacheMode, cacheGroup);
                    IMethodReturn methodReturn = getNext()(input, getNext);
                    if (methodReturn.Exception == null && methodReturn.ReturnValue != null)
                    {
                        cacheAPI.Add(cacheGroup, cacheKey, methodReturn.ReturnValue);
                        logger.Info("Adding into cache, cacheGroup=" + cacheGroup + ", cacheKey=" + cacheKey);
                    }
                    return methodReturn;
                }
            }
            else if (CacheType.CLEAR.Equals(cacheType))
            {
                IMethodReturn methodReturn = getNext()(input, getNext);
                cacheAPI.Remove(cacheGroup, cacheKey);
                return methodReturn;
            }
            else if(CacheType.UPDATE.Equals(cacheType))
            {
                object cacheValue = GetCacheValue(input.Arguments);
                if (cacheValue != null)
                {
                    cacheAPI.Update(cacheGroup, cacheKey, cacheValue);
                    logger.Info("Update cache, cacheGroup=" + cacheGroup + ", cacheKey=" + cacheKey);
                }
                return getNext()(input, getNext);
            }
            else
            {
                logger.Error("Invalid cache type, cachetype=" + cacheType);
                return getNext()(input, getNext);
            }
        }

        private CacheMode GetCacheMode(MethodBase methodBase)
        {
            CacheAttribute[] cacheAttrs = (CacheAttribute[])methodBase.GetCustomAttributes(typeof(CacheAttribute), false);
            CacheMode cacheMode = cacheAttrs[0].CacheMode;
            return cacheMode;
        }

        private ICacheAPI ChooseCacheAPI(CacheMode cacheMode)
        {
            switch (cacheMode)
            {
                case CacheMode.LOCAL:
                    return localCache;
                case CacheMode.REMOTE:
                    return remoteCache;
                case CacheMode.MEMREMOTE:
                    return memRemoteCache;
                case CacheMode.SESSION:
                    return session;
                case CacheMode.REQUEST:
                    return request;
                default:
                    return null;
            }
        }

        private CacheType GetCacheType(MethodBase methodBase)
        {
            CacheAttribute[] cacheAttrs = (CacheAttribute[])methodBase.GetCustomAttributes(typeof(CacheAttribute), false);
            return cacheAttrs[0].CacheType;
        }

        private string GetCacheGroup(MethodBase methodBase)
        {
            CacheAttribute[] cacheAttrs = (CacheAttribute[])methodBase.GetCustomAttributes(typeof(CacheAttribute), false);
            return cacheAttrs[0].Group;
        }

        object GetCacheValue(IParameterCollection inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                ParameterInfo parameterInfo = inputs.GetParameterInfo(i);
                CacheValueAttribute[] attributes =
                    (CacheValueAttribute[])parameterInfo.GetCustomAttributes(typeof(CacheValueAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return inputs[i];
                }
            }
            return null;
        }

        string CreateCacheKey(MethodBase methodBase, IParameterCollection inputs)
        {
            StringBuilder cacheKey = new StringBuilder();
            CacheAttribute[] cacheAttrs = (CacheAttribute[]) methodBase.GetCustomAttributes(typeof (CacheAttribute), false);
            cacheKey.Append(cacheAttrs[0].Name);
            int index = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                ParameterInfo parameterInfo = inputs.GetParameterInfo(i);
                CacheKeyAttribute[] attributes = (CacheKeyAttribute[]) parameterInfo.GetCustomAttributes(typeof (CacheKeyAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    if (index > 0)
                        cacheKey.Append(";");
                    else
                        cacheKey.Append("[");
                    if (parameterInfo.ParameterType.IsPrimitive || parameterInfo.ParameterType == typeof(Type))
                    {
                        cacheKey.Append(parameterInfo.Name).Append("=").Append(inputs[i]);
                        index++;
                    }
                    else
                    {
                        PropertyInfo[] properties = cacheMetaLoader.FindCacheKeyProperties(parameterInfo.ParameterType);
                        if (properties.Length == 0)
                        {
                            cacheKey.Append(parameterInfo.Name).Append("=").Append(inputs[i]);
                            index++;
                        }
                        else
                        {
                            foreach (PropertyInfo propertyInfo in properties)
                            {
                                if (index > 0)
                                    cacheKey.Append(";");
                                cacheKey.Append(propertyInfo.Name).Append("=").Append(
                                    propertyInfo.GetValue(inputs[i], null));
                                index++;
                            }
                        }
                    }
                }
            }
            if (index > 0)
                cacheKey.Append("]");

            return cacheKey.ToString().Replace(" ","").ToUpper();
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        [Dependency(CacheAttribute.LOCAL_CACHE)]
        public ICacheAPI LocalCache
        {
            get { return localCache; }
            set { localCache = value; }
        }

        [Dependency(CacheAttribute.REMOTE_CACHE)]
        public ICacheAPI RemoteCache
        {
            get { return remoteCache; }
            set { remoteCache = value; }
        }

        [Dependency(CacheAttribute.SESSION)]
        public ICacheAPI Session
        {
            get { return session; }
            set { session = value; }
        }

        [Dependency(CacheAttribute.REQUEST)]
        public ICacheAPI Request
        {
            get { return request; }
            set { request = value; }
        }

        [Dependency(CacheAttribute.MEM_REMOTE_CACHE)]
        public ICacheAPI MemRemoteCache
        {
            get { return memRemoteCache; }
            set { memRemoteCache = value; }
        }

        

        [Dependency]
        public CacheStatistics CacheStatistics
        {
            get { return cacheStatistics; }
            set { cacheStatistics = value; }
        }

        [Dependency]
        public CacheMetaLoader CacheMetaLoader
        {
            get { return cacheMetaLoader; }
            set { cacheMetaLoader = value; }
        }
    }
}