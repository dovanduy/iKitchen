namespace SunTzu.Core.Cache.Settings
{
    public class RemoteCachePolicy
    {
        private string policyName;
        private int absoluteExpirationTimeInSecond;
        private string cacheStoreName;

        public string PolicyName
        {
            get { return policyName; }
            set { policyName = value; }
        }

        public int AbsoluteExpirationTimeInSecond
        {
            get { return absoluteExpirationTimeInSecond; }
            set { absoluteExpirationTimeInSecond = value; }
        }

        public string CacheStoreName
        {
            get { return cacheStoreName; }
            set { cacheStoreName = value; }
        }
    }
}
