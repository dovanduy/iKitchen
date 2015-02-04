using System;
using System.Collections.Generic;
using System.Reflection;
using SunTzu.Core.AOP;

namespace SunTzu.Core.Cache
{
    public class CacheMetaLoader
    {
        internal bool IsMatchProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(CacheKeyAttribute), false).Length > 0;
        }

        [Cache("System", "CacheKeyMeta", CacheMode.LOCAL)]
        internal virtual PropertyInfo[] FindCacheKeyProperties([CacheKey]Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            return Array.FindAll(propertyInfos, IsMatchProperty);
        }

    }
}
