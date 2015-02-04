using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Practices.Unity;
using SunTzu.Utility;

namespace SunTzu.Core.Cache.Settings
{
    public class CacheSettingManager
    {
        private string cacheSettingFile;

        private CachePolicys policys;

        private Dictionary<string, LocalCachePolicy> localCachePolicys;
        private Dictionary<string, RemoteCachePolicy> remoteCachePolicys;

        [Dependency("CacheSettingFile")]
        public string CacheSettingFile
        {
            set { cacheSettingFile = value; }
        }

        public CachePolicys GetSettings()
        {
            Monitor.Enter(this);
            if (policys == null)
            {
                string xml = File.ReadAllText(string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, cacheSettingFile));
                policys = (CachePolicys)XMLUtility.Deserialize(xml, typeof(CachePolicys));
                localCachePolicys = new Dictionary<string, LocalCachePolicy>();
                foreach (LocalCachePolicy localCachePolicy in policys.LocalCachePolicys)
                {
                    localCachePolicys.Add(localCachePolicy.PolicyName.ToLower(), localCachePolicy);
                }
                remoteCachePolicys = new Dictionary<string, RemoteCachePolicy>();
                foreach (RemoteCachePolicy remoteCachePolicy in policys.RemoteCachePolicys)
                {
                    remoteCachePolicys.Add(remoteCachePolicy.PolicyName.ToLower(), remoteCachePolicy);
                }
            }
            Monitor.Exit(this);
            return policys;
        }

        public LocalCachePolicy GetLocalCachePolicy(string policyName)
        {
            GetSettings();
            string key = (!string.IsNullOrEmpty(policyName)) && localCachePolicys.ContainsKey(policyName.ToLower()) ? policyName.ToLower() : "default";
            return localCachePolicys[key];
        }

        public RemoteCachePolicy GetRemoteCachePolicy(string policyName)
        {
            GetSettings();
            string key = (!string.IsNullOrEmpty(policyName)) && remoteCachePolicys.ContainsKey(policyName.ToLower()) ? policyName.ToLower() : "default";
            return remoteCachePolicys[key];
        }
    }
}
