using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iKitchen.Linq;

namespace iKitchen.Web.Models
{
    public class iKitchenConfig
    {
        private static SystemConfiguration systemConfiguration = null;

        public static SystemConfiguration SystemConfiguration
        {
            get
            {
                if (systemConfiguration == null)
                {
                    systemConfiguration = DataContextManager.GetContext().SystemConfiguration.Single();
                }
                return systemConfiguration;
            }
            set { systemConfiguration = value; }
        }


    }
}