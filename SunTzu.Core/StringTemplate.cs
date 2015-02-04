using System.Collections.Generic;
using System.IO;
using Commons.Collections;
using Microsoft.Practices.Unity;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;

namespace SunTzu.Utility
{
    public class StringTemplate
    {         
        private readonly VelocityEngine velocityEngine = new VelocityEngine();

        [Dependency("TemplateBaseDirectory")]
        public string BaseDirectory
        {
            set
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + value;
                ExtendedProperties extendedProperties = new ExtendedProperties();
                extendedProperties.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, path);
                velocityEngine.Init(extendedProperties);
            }
        }


        public virtual string Process(string source, Dictionary<string, object> context)
        {
            VelocityContext velocityContext = new VelocityContext();
            foreach (KeyValuePair<string, object> pair in context)
            {
                velocityContext.Put(pair.Key, pair.Value);
            }
            StringWriter writer = new StringWriter();
            velocityEngine.Evaluate(velocityContext, writer, "", source);
            return writer.GetStringBuilder().ToString();
        }

        public virtual string Process(Dictionary<string, object> context,string templateFile)
        {
            VelocityContext velocityContext = new VelocityContext();
            foreach (KeyValuePair<string, object> pair in context)
            {
                velocityContext.Put(pair.Key, pair.Value);
            }
            Template template = velocityEngine.GetTemplate(templateFile);
            StringWriter writer = new StringWriter();
            template.Merge(velocityContext, writer);
            return writer.ToString();
        }
    }
}