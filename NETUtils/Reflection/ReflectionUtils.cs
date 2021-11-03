using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace De.JanRoslan.NETUtils.Reflection {


    /// <summary>
    /// Utility methods regarding Reflection
    /// </summary>
    public class ReflectionUtils {


        /// <summary>
        /// Sets a property of an object instance to a given value via reflection
        /// </summary>
        /// <param name="instance">The instance of the class that contains the property</param>
        /// <param name="propName">The name of the property that should be modified</param>
        /// <param name="value">The new value for the property</param>
        /// <param name="parent"></param>
        public static void SetInternalProperty(object instance, string propName, object value, string parent = null) {

            PropertyInfo propInfo;
            if(parent != null) {
                propInfo = instance.GetType().GetRuntimeProperty(parent);
                instance = propInfo.GetValue(instance);
                propInfo = propInfo.PropertyType.GetRuntimeProperty(propName);
            } else {
                propInfo = instance.GetType().GetRuntimeProperty(propName);
            }

            if(propInfo != null && propInfo.CanWrite) {
                propInfo.SetValue(instance, value);
            }
        }
    }
}
