using System.Collections.Generic;

namespace SunTzu.Core.Cache.Settings
{
    public class CachePolicys
    {
        private List<LocalCachePolicy> localCachePolicys = new List<LocalCachePolicy>();
        private List<RemoteCachePolicy> remoteCachePolicys = new List<RemoteCachePolicy>();

        public List<LocalCachePolicy> LocalCachePolicys
        {
            get { return localCachePolicys; }
            set { localCachePolicys = value; }
        }

        public List<RemoteCachePolicy> RemoteCachePolicys
        {
            get { return remoteCachePolicys; }
            set { remoteCachePolicys = value; }
        }
    }
}