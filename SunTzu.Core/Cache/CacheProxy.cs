using System;
using System.Collections.Generic;
using log4net;

namespace SunTzu.Core.Cache
{
    public class CacheProxy : ICacheAPI
    {
        private static readonly ILog logger = LogManager.GetLogger("Cache");
        private readonly ICacheAPI cacheApi;
        private readonly CacheMode cacheMode;
        private CacheStatistics cacheStatistics;

        public CacheProxy(ICacheAPI cacheApi, CacheMode cacheMode, CacheStatistics cacheStatistics)
        {
            this.cacheApi = cacheApi;
            this.cacheMode = cacheMode;
            this.cacheStatistics = cacheStatistics;
        }

        public CacheStatistics CacheStatistics
        {
            set { cacheStatistics = value; }
        }

        #region ICacheAPI Members

        public void Add(string groupName, string key, object value)
        {
            try
            {
                cacheApi.Add(groupName, key, value);
            }
            catch (Exception ex)
            {
                logger.Error(cacheMode + " Cache Error", ex);
                cacheStatistics.AddErrorCount(cacheMode, groupName);
            }
        }

        public void Remove(string groupName, string key)
        {
            try
            {
                cacheApi.Remove(groupName, key);
            }
            catch (Exception ex)
            {
                logger.Error(cacheMode + " Cache Error", ex);
                cacheStatistics.AddErrorCount(cacheMode, groupName);
            }
        }

        public object Get(string groupName, string key)
        {
            try
            {
                return cacheApi.Get(groupName, key);
            }
            catch (Exception)
            {
                cacheStatistics.AddErrorCount(cacheMode, groupName);
                throw;
            }
        }

        public bool Contains(string groupName, string key)
        {
            try
            {
                return cacheApi.Contains(groupName, key);
            }
            catch (Exception ex)
            {
                logger.Error(cacheMode + " Cache Error", ex);
                cacheStatistics.AddErrorCount(cacheMode, groupName);
                return false;
            }
        }

        public long Size(string groupName)
        {
            try
            {
                return cacheApi.Size(groupName);
            }
            catch (Exception ex)
            {
                logger.Error(cacheMode + " Cache Error", ex);
                cacheStatistics.AddErrorCount(cacheMode, groupName);
                return 0;
            }
        }

        public void Flush(string groupName)
        {
            try
            {
                cacheApi.Flush(groupName);
                Dictionary<string, CacheHitCount> cacheHitCount = cacheStatistics.ChooseCacheHitCount(cacheMode);
                lock (cacheHitCount)
                {
                    if (cacheHitCount.ContainsKey(groupName))
                        cacheHitCount.Remove(groupName);
                }
            }
            catch (Exception ex)
            {
                logger.Error(cacheMode + " Cache Error", ex);
                cacheStatistics.AddErrorCount(cacheMode, groupName);
            }
        }

        public void Update(string groupName, string key, object value)
        {
            try
            {
                cacheApi.Update(groupName, key, value);
            }
            catch (Exception ex)
            {
                logger.Error(cacheMode + " Cache Error", ex);
                cacheStatistics.AddErrorCount(cacheMode, groupName);
            }
        }

        #endregion
    }
}