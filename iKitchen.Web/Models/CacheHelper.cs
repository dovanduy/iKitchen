using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iKitchen.Linq;
using SunTzu.Core.Data;

namespace iKitchen.Web.Models
{
    public static class CacheHelper<T> where T : class, IEntity, new()
    {
        private static iKitchenDataContext db
        {
            get
            {
                return DataContextManager.GetContext();
            }
        }

        private static List<T> cachedList = null;

        public static List<T> GetAll()
        {
            if (cachedList == null)
            {
                cachedList = db.Set<T>().ToList();
            }
            return cachedList;
        }

        public static void Clear()
        {
            cachedList = null;
        }

        public static T GetById(int id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);
            return entity ?? new T();
        }
    }

    public static class CacheHelperApplicationUser
    {
        private static List<ApplicationUser> cachedList = null;

        public static List<ApplicationUser> GetAll()
        {
            if (cachedList == null)
            {
                var db = new ApplicationDbContext();
                cachedList = db.Users.Where(c => c.State == 0).ToList();
            }
            return cachedList;
        }

        public static void Clear()
        {
            cachedList = null;
        }

        public static ApplicationUser GetById(string id)
        {
            var entity = GetAll().FirstOrDefault(c => c.Id == id);
            return entity ?? new ApplicationUser();
        }
    }
}