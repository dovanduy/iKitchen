using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace iKitchen.Linq
{
    public class DataContextManager
    {
        [ThreadStatic]
        private static iKitchenDataContext dataContext = null;

        public static iKitchenDataContext GetContext()
        {
            if (dataContext == null)
            {
                dataContext = new iKitchenDataContext();
            }
            return dataContext;
        }

        /// <summary>
        /// 重置DataContext对象
        /// </summary>
        public static void Clear()
        {           
            dataContext = null;
        }
    }
}
