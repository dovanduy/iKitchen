using System.Text;

namespace SunTzu.Core.Data
{
    public class BaseEntity
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                sb.AppendFormat("{0}:{1}", property.Name, property.GetValue(this, null)).AppendLine();
            }
            return sb.ToString();
        }
    }
}
