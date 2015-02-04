using System.Web;
using Microsoft.Practices.Unity;
using SunTzu.Core.Authorization;
using SunTzu.Web.EntityValidate;

namespace iKitchen.Web.Models
{
    public class WebInitializer
    {
        public void InitializeContainer(UnityContainer container)
        {
            HttpContext.Current.Application["Container"] = container;
            // register container
            container.RegisterInstance<UnityContainer>(container);

            // register type
            //container.RegisterType<IAuthorization, LoginHelper>();
            container.RegisterType<IAuthorization, AccountHelper>();
            
            // register lifetime cycle

            // load Entity Validator Config
            //ValidateContainer.LoadValidateConfig();
        }

        public void InitializeMetaModel()
        {

        }
    }
}