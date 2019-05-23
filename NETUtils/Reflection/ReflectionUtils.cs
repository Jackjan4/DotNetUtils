using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace De.JanRoslan.NETUtils.Reflection
{
    public class ReflectionUtils
    {

        
        public static void SetInternalProperty(object instance, string propName, object value, string parent = null) {

            PropertyInfo propInfo = null;
            if (parent != null) {
                propInfo = instance.GetType().GetRuntimeProperty(parent);
                instance = propInfo.GetValue(instance);
                propInfo = propInfo.PropertyType.GetRuntimeProperty(propName);
            } else {
                propInfo = instance.GetType().GetRuntimeProperty(propName);
            }

             if (propInfo != null && propInfo.CanWrite) {
                propInfo.SetValue(instance, value);
            }
        }
    }
}
