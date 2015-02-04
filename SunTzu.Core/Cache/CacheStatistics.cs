using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace SunTzu.Core.Cache
{
    [Serializable]
    public class CacheStatistics
    {
        private readonly Dictionary<CacheMode, Dictionary<string, CacheHitCount>> cacheStats = new Dictionary<CacheMode, Dictionary<string, CacheHitCount>>();

        private int maxCacheErrorLimit;

        [Dependency("MaxCacheErrorLimit")]
        public string CacheErrorLimit
        {
            set { maxCacheErrorLimit = int.Parse(value); }
        }

        public CacheStatistics()
        {
            cacheStats.Add(CacheMode.LOCAL, new Dictionary<string, CacheHitCount>());
            cacheStats.Add(CacheMode.REMOTE, new Dictionary<string, CacheHitCount>());
            cacheStats.Add(CacheMode.MEMREMOTE, new Dictionary<string, CacheHitCount>());
        }

        public Dictionary<string, CacheHitCount> ChooseCacheHitCount(CacheMode cacheMode)
        {
            if (cacheStats.ContainsKey(cacheMode))
            {
                return cacheStats[cacheMode];
            }
            return null;
        }

        public void AddHitCount(CacheMode cacheMode, string groupName)
        {
            Dictionary<string, CacheHitCount> cacheHitCount = ChooseCacheHitCount(cacheMode);
            if (cacheHitCount != null)
            {
                lock (cacheHitCount)
                {
                    if (cacheHitCount.ContainsKey(groupName))
                    {
                        cacheHitCount[groupName].Hit++;
                    }
                    else
                    {
                        cacheHitCount[groupName] = new CacheHitCount(1, 0, maxCacheErrorLimit);
                    }
                }
            }
        }

        public void AddUnhitCount(CacheMode cacheMode, string groupName)
        {
            Dictionary<string, CacheHitCount> cacheHitCount = ChooseCacheHitCount(cacheMode);
            if (cacheHitCount != null)
            {
                lock (cacheHitCount)
                {
                    if (cacheHitCount.ContainsKey(groupName))
                    {
                        cacheHitCount[groupName].Unhit++;
                    }
                    else
                    {
                        cacheHitCount[groupName] = new CacheHitCount(0, 1, maxCacheErrorLimit);
                    }
                }
            }
        }

        public void AddErrorCount(CacheMode cacheMode, string groupName)
        {
            Dictionary<string, CacheHitCount> cacheHitCount = ChooseCacheHitCount(cacheMode);
            if (cacheHitCount != null)
            {
                lock (cacheHitCount)
                {
                    if (cacheHitCount.ContainsKey(groupName))
                    {
                        cacheHitCount[groupName].ErrorCount++;
                    }
                    else
                    {
                        cacheHitCount[groupName] = new CacheHitCount(0, 0, 1, maxCacheErrorLimit);
                    }
                }
            }
        }

        public bool IsError(CacheMode cacheMode, string groupName)
        {
            Dictionary<string, CacheHitCount> cacheHitCount = ChooseCacheHitCount(cacheMode);
            if (cacheHitCount != null)
            {
                if (!cacheHitCount.ContainsKey(groupName))
                    return false;

                return cacheHitCount[groupName].ErrorCount >= cacheHitCount[groupName].MaxErrorLimit;
            }

            return false;
        }
    }

    [Serializable]
    public class CacheHitCount
    {
        private int errorCount;
        private int hit;
        private int unhit;
        private int maxErrorLimit = 10;

        public CacheHitCount(int hit, int unhit, int maxErrorLimit)
        {
            this.hit = hit;
            this.unhit = unhit;
            this.maxErrorLimit = maxErrorLimit;
            this.errorCount = 0;
        }

        public CacheHitCount(int hit, int unhit, int errorCount, int maxErrorLimit)
        {
            this.hit = hit;
            this.unhit = unhit;
            this.errorCount = errorCount;
            this.maxErrorLimit = maxErrorLimit;
        }

        public int Hit
        {
            get { return hit; }
            set { hit = value; }
        }

        public int Unhit
        {
            get { return unhit; }
            set { unhit = value; }
        }

        public int ErrorCount
        {
            get { return errorCount; }
            set { errorCount = value; }
        }

        public int MaxErrorLimit
        {
            get { return maxErrorLimit; }
            set { maxErrorLimit = value; }
        }

        public int TotalHit
        {
            get { return hit + unhit; }
        }
    }
}