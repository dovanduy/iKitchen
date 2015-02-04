using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SunTzu.Web.EntityValidate
{
    public class ValidateConfigRule
    {
        [XmlAttribute("Type")]
        public string Type { get; set; }

        private decimal min = decimal.MinValue;
        [XmlAttribute("Min")]
        public decimal Min
        {
            get { return min; }
            set { min = value; }
        }

        private decimal max = decimal.MinValue;
        [XmlAttribute("Max")]
        public decimal Max
        {
            get { return max; }
            set { max = value; }
        }

        private int minLength = int.MinValue;
        [XmlAttribute("MinLength")]
        public int MinLength
        {
            get { return minLength; }
            set { minLength = value; }
        }

        private int maxLength = int.MinValue;
        [XmlAttribute("MaxLength")]
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        [XmlAttribute("Expression")]
        public string Expression { get; set; }

        [XmlAttribute("ErrorMessage")]
        public string ErrorMessage { get; set; }
    }
}
