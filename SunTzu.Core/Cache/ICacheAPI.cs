namespace SunTzu.Core.Cache
{
    public interface ICacheAPI
    {
        void Add(string groupName, string key, object value);

        void Remove(string groupName, string key);

        object Get(string groupName, string key);

        bool Contains(string groupName, string key);

        long Size(string groupName);

        void Flush(string groupName);

        void Update(string groupName, string key, object value);
    }
}