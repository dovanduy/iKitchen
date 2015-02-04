using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunTzu.Utility;
using System.Web;
using System.Configuration;
using System.IO;
using log4net;
using System.Reflection;

namespace SunTzu.Web.EntityValidate
{
    public static class ValidateContainer
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool isLoaded = false;
        private static List<ValidateConfig> validateConfigs = new List<ValidateConfig>();
        private static List<EntityValidator> entityValidators = new List<EntityValidator>();

        public static EntityValidator GetEntityValidate(string entityType)
        {
            if (entityValidators.Count == 0 && !isLoaded)
            {
                LoadValidateConfig();
            }
            return entityValidators.FirstOrDefault(c => c.Type == entityType);
        }

        public static void LoadValidateConfig()
        {
            var validateConfigFolder = ConfigurationManager.AppSettings["ValidateConfigFolder"] ?? "~/ValidateConfig";
            if (validateConfigFolder.IsNotNullOrEmpty())
            {
                validateConfigFolder = HttpContext.Current.Server.MapPath(validateConfigFolder);
                var directory = new DirectoryInfo(validateConfigFolder);
                if (directory.Exists)
                {
                    var files = directory.GetFiles("*.config");
                    foreach (var file in files)
                    {
                        try
                        {
                            var validateConfig = XMLUtility.CreateInstanceFromXml<ValidateConfig>(file.FullName);
                            if (validateConfigs.Any(c => c.EntityType == validateConfig.EntityType))
                            {
                                // 支持重复定义同类型的验证逻辑
                                validateConfigs.First(c => c.EntityType == validateConfig.EntityType)
                                    .Fields.AddRange(validateConfig.Fields);
                            }
                            else
                            {
                                validateConfigs.Add(validateConfig);
                            }
                        }
                        catch (Exception)
                        {
                            // todo: log here
                        }
                    }
                }
            }

            if (validateConfigs.Count > 0)
            {
                GenerateEntityValidators();
            }

            isLoaded = true;
        }

        private static void GenerateEntityValidators()
        {
            foreach (var validateConfig in validateConfigs)
            {
                var entityValidator = new EntityValidator();
                entityValidator.Type = validateConfig.EntityType;
                foreach (var field in validateConfig.Fields)
                {
                    foreach (var rule in field.Rules)
                    {
                        IValidateRule validateRule = CreateRule(rule, field.Name);
                        entityValidator.ValidateRules.Add(validateRule);
                    }
                }
                entityValidators.Add(entityValidator);
            }
        }

        private static IValidateRule CreateRule(ValidateConfigRule rule, string fieldName)
        {
            IValidateRule validateRule = null;

            try
            {
                var type = Type.GetType(string.Format("SunTzu.Web.EntityValidate." + rule.Type + "Rule"));
                validateRule = Activator.CreateInstance(type) as IValidateRule;
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            if (validateRule == null)
            {
                logger.DebugFormat("Create ValidateRule failed! ValidateConfigRule={0}, fieldName={1}", rule, fieldName);
                validateRule = new DummyRule();
            }
            validateRule.FieldName = fieldName;
            validateRule.ErrorMessage = rule.ErrorMessage;
            return validateRule;
        }
    }
}
