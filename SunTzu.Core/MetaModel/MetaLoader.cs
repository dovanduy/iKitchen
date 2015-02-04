using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using SunTzu.Core.Data;

namespace SunTzu.Core.MetaModel
{
    public static class MetaLoader
    {
        private static List<MetaEntity> MetaEntityList = new List<MetaEntity>();

        public static void LoadMetaFromConfigFile(string configFile)
        {
            if (File.Exists(configFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<MetaEntity>), "");
                using (var stream = File.OpenRead(configFile))
                {
                    var list = (List<MetaEntity>)serializer.Deserialize(stream);
                    MetaEntityList.Clear();
                    MetaEntityList.AddRange(list);
                }
            }
        }

        private static MetaEntity GetMetaEntityByEntity(IEntity entity)
        {
            var metaEntity = MetaEntityList.FirstOrDefault(c => c.TypeName == entity.GetType().Name);
            if (metaEntity != null)
            {
                metaEntity = CreateDefaultMetaEntity(entity);
                MetaEntityList.Add(metaEntity);
            }
            return metaEntity;
        }

        public static MetaEntity GetMetaEntity(this IEntity entity)
        {
            return GetMetaEntityByEntity(entity);
        }

        public static MetaEntity CreateDefaultMetaEntity(IEntity entity)
        {
            var metaEntity = new MetaEntity();
            metaEntity.TypeName = entity.GetType().Name;
            metaEntity.DefaultSortMetaField = MetaField.Id;

            var properties = entity.GetType().GetProperties();
            metaEntity.DisplayColumnCountInList = properties.Count();
            metaEntity.IsEnable = true;
            foreach (var property in properties)
            {
                metaEntity.MetaFieldList.Add(CreateDefaultMetaField(property));

            }

            return metaEntity;
        }

        public static MetaField CreateDefaultMetaField(PropertyInfo property)
        {
            var metaField = new MetaField();

            metaField.FieldName = property.Name;
            metaField.FieldType = property.PropertyType.Name;
            if (property.Name.EndsWith("Id"))
            {
                // the property is foreign key or of a dictionary value
                var name = property.Name.Substring(0, property.Name.Length - 2);
                metaField.DisplayLabel = name;
                metaField.DropDownEntityName = name;
                metaField.DropDownValueField = "Id";
                metaField.DropDownTextField = "Title";
                // metaField.ForeignKeyFieldName = "Id";
                // metaField.ForeignKeyOfEntityName = name;
            }
            else
            {
                metaField.DisplayLabel = property.Name;
            }
            metaField.IsEnable = true;
            metaField.IsReadOnly = false;
            metaField.IsShowInDetail = true;
            metaField.IsShowInEdit = true;
            metaField.IsShowInList = true;
            
            return metaField;
        }
    }
}
