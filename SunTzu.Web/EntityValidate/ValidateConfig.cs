using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SunTzu.Web.EntityValidate
{
    public class ValidateConfig
    {
        [XmlElement("Field")]
        public List<ValidateConfigField> Fields { get; set; }
        [XmlAttribute("EntityType")]
        public string EntityType { get; set; }
    }
}
