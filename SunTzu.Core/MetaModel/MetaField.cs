using System;
using System.Text;

namespace SunTzu.Core.MetaModel
{
    [Serializable]
    public class MetaField
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string AuthCode { get; set; }
        public bool IsEnable { get; set; }
        public int DisplayOrderInList { get; set; }
        public int DisplayOrderInEdit { get; set; }
        public string ValidateFormat { get; set; }
        public string DisplayLabel { get; set; }
        public bool IsShowInList { get; set; }
        public bool IsShowInEdit { get; set; }
        public bool IsShowInDetail { get; set; }
        public bool IsReadOnly { get; set; }
        public string DropDownEntityName { get; set; }
        public string DropDownValueField { get; set; }
        public string DropDownTextField { get; set; }
        public string ForeignKeyOfEntityName { get; set; }
        public string ForeignKeyFieldName { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                sb.AppendFormat("{0}:{1}", property.Name, property.GetValue(this, null)).AppendLine();
            }
            //Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

        public static readonly MetaField Id = new MetaField() { FieldName = "Id", FieldType = "int" };
            
    }
}
