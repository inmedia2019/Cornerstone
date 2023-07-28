using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Cornerstone.Code;

namespace Cornerstone.Code
{
    public class FormUtil {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>System.String.</returns>
        public static List<string> SetValue(string content)
        {
            List<FormValue> list = JsonHelper.ToObject<List<FormValue>>(content);
            List<string> temp = new List<string>();
            SetFormValue(list, temp);
            return temp;
        }

        private static List<string> SetFormValue(List<FormValue> list, List<string> temp)
        {
            foreach (var item in list)
            {
                if (item.tag == "grid")
                {
                    foreach (var column in item.columns)
                    {
                        SetFormValue(column.list, temp);
                    }
                }
                else
                {
                    temp.Add(item.id);
                }
            }
            return temp;
        }

        public static List<string> SetValueByWeb(string webForm)
        {
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            var t = referencedAssemblies
                .SelectMany(a => a.GetTypes().Where(t => t.FullName.Contains("Cornerstone.Domain.") && t.FullName.Contains("."+webForm + "Entity"))).FirstOrDefault();
            List<string> temp = new List<string>();
            PropertyInfo[] pArray = t.GetProperties();
            Array.ForEach<PropertyInfo>(pArray, p =>
            {
                temp.Add(p.Name);
            });
            return temp;
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="afterChange">修改后对象</param>
        /// <param name="beforeChange">修改前对象</param>
        /// <returns></returns>
        public static string ForeachCompare<T>(T afterChange, T beforeChange)
        {
            var changeStr = "";
            Type afterChangedata = afterChange.GetType();
            PropertyInfo[] PropertyList = afterChangedata.GetProperties();
            foreach (PropertyInfo afterChangeitem in PropertyList)
            {
                Type beforeChangedata = afterChange.GetType();
                PropertyInfo[] PropertyListdb = beforeChangedata.GetProperties();
                foreach (PropertyInfo beforeChangeitem in PropertyListdb)
                {
                    if (afterChangeitem.Name == beforeChangeitem.Name)
                    {
                        var beforeChangevalue = beforeChangeitem.GetValue(beforeChange, null) == null ? "" : beforeChangeitem.GetValue(beforeChange, null).ToString();
                        var afterChangevalue = afterChangeitem.GetValue(afterChange, null) == null ? "" : afterChangeitem.GetValue(afterChange, null).ToString();
                        if (beforeChangevalue != afterChangevalue)
                        {
                            changeStr = changeStr + afterChangeitem.Name + "  (" + beforeChangevalue + ") - (" + afterChangevalue + ") | ";
                        }
                        break;
                    }
                }
            }
            return changeStr;
        }
    }
}
