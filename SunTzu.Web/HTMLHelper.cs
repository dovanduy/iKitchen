using System;
using System.Collections;
using System.Text;
using System.Web;
using SunTzu.Utility;

namespace SunTzu.Web
{
    /*
     * Title:HTML帮助类
     * Description:
     * Author:夏勇兴
     * Create Date:2009-05-08
     * */
    public class HTMLHelper
    {
        /// <summary>
        /// 绑定Request提交上来的数据到实体类字段 by 夏勇兴
        /// </summary>
        /// <param name="model"></param>
        public static void BindModel(object model)
        {
            var request = HttpContext.Current.Request;
            foreach (var pi in model.GetType().GetProperties())
            {
                var newValue = request[pi.Name];
                if (newValue != null)   // 有提交值，则绑定
                {
                    // 替换字符
                    //newValue = newValue.Replace('\\', '_');
                    //newValue = newValue.Replace('/', '_');
                    Type type= pi.PropertyType;

                    if(type.IsGenericType)   // 如果是Nullable泛型
                    {
                        type = Nullable.GetUnderlyingType(type);
                    }

                    try
                    {
                        if (type.Equals(typeof(Guid)))
                        {
                            if (string.Empty != (newValue))
                                pi.SetValue(model, newValue.ParseToGuid(), null);
                        }
                        else
                            pi.SetValue(model, Convert.ChangeType(newValue, type), null);
                    }
                    catch
                    {
                        if (pi.PropertyType.IsGenericType)
                        {
                            // 转换失败时，Nullable泛型设置为null
                            pi.SetValue(model, null, null);
                        }
                        // 无效转换
                        //throw;
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// 实体类转换为json格式数据 by 夏勇兴
        /// </summary>
        /// <param name="model"></param>
        public static string ToJson(IList model)
        {
            StringBuilder json = new StringBuilder();
            //json.Append("{Employees:[");
            
            //int i = 0;
            json.Append("[");
            foreach (var data in model)
            {
                json.Append(ToJson(data));
                json.Append(",");

            }
            json.Append("]");
            json.Replace(",]", "]");
            return json.ToString();
        }

        public static string ToJson(object data)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            foreach (var pi in data.GetType().GetProperties())
            {
                json.Append("\"");
                json.Append(pi.Name);
                json.Append("\":\"");
                if (pi.PropertyType.Equals(typeof(DateTime)))
                {
                    json.Append(((DateTime)pi.GetValue(data, null)).ToString());
                }
                else if (pi.PropertyType.Equals(typeof(DateTime?)))
                {
                    var val = (DateTime?)pi.GetValue(data, null);
                    if (val.HasValue)
                        json.Append(val.Value.ToString());
                }
                else if (pi.PropertyType.Equals(typeof(IList)))
                {  
                    json.Append(pi.GetValue(data, null));
                }
                else
                {
                    json.Append(pi.GetValue(data, null));
                }
                json.Append("\",");
            }
            json.Append("}");
            // 去除最后一个,号
            json.Replace(",}", "}");
            return json.ToString();
        }
    }
}
