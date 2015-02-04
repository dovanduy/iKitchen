using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SunTzu.Web.EntityValidate
{
    public class ValidateConfigField
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlElement("Rule")]
        public List<ValidateConfigRule> Rules { get; set; }
    }
}
