using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
namespace Utilities.Extension.DataShaping
{
   public static class DataShaping
    {
        public static IEnumerable<ExpandoObject> Shaper<T>(this IEnumerable<T> source, string fieldName)
        {
            if(source == null)
            {
                throw new ArgumentNullException("source");
            }

            var expandObjectList = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fieldName))
            {
                var propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfo);

            }
            else
            {
                var filedAfterSplit = fieldName.Split(',');
                foreach(var field in filedAfterSplit)
                {
                   var propName = field.Trim();

                   var propInfo = typeof(T).GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                   
                    if(propInfo == null)
                    {
                        throw new MovieTicketBookingExceptions($"Property {propName} wasn't found in" +
                                                                $"{typeof(T)}");

                    }
                    propertyInfoList.Add(propInfo);

                }

            }

            foreach(var item in source)
            {
                var dataShapedObject = new ExpandoObject();
                 foreach(var propertyInfo in propertyInfoList)
                {
                   var propValue = propertyInfo.GetValue(item);
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propValue);

                }
                 expandObjectList.Add(dataShapedObject);
            }
            return expandObjectList;
        }
    }
}
