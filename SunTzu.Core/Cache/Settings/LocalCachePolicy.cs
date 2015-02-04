namespace SunTzu.Core.Cache.Settings
{
    public class LocalCachePolicy
    {
        private string policyName;
        private int expiredSeconds;
        private int maxElements;
        private int absoluteExpirationTimeInSecond;

        public string PolicyName
        {
            get { return policyName; }
            set { policyName = value; }
        }

        public int ExpiredSeconds
        {
            get { return expiredSeconds; }
            set { expiredSeconds = value; }
        }

        public int MaxElements
        {
            get { return maxElements; }
            set { maxElements = value; }
        }

        public int AbsoluteExpirationTimeInSecond
        {
            get { return absoluteExpirationTimeInSecond; }
            set { absoluteExpirationTimeInSecond = value; }
        }
    }
}
