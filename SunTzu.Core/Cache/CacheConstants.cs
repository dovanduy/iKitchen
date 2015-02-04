namespace SunTzu.Core.Cache
{
    public enum CacheMode
    {
        LOCAL,
        REMOTE,
        SESSION,
        REQUEST,
        MEMREMOTE
    }

    public enum CacheType
    {
        FETCH,
        CLEAR,
        UPDATE
    }
}