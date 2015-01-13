using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Reflection
{
    public static class PropertyInfoExtensions
    {
        public static Action<object, object> GetSetterAction(this PropertyInfo prop)
        {
            ParameterExpression targetExp = Expression.Parameter(typeof(object), "target");
            ParameterExpression valueExp = Expression.Parameter(typeof(object), "value");

            UnaryExpression instanceCast = (prop.DeclaringType.IsValueType)
                ? Expression.Convert(targetExp, prop.DeclaringType)
                : Expression.TypeAs(targetExp, prop.DeclaringType);

            UnaryExpression valueCast = (prop.PropertyType.IsValueType)
                ? Expression.Convert(valueExp, prop.PropertyType)
                : Expression.TypeAs(valueExp, prop.PropertyType);

            MethodCallExpression setCall = Expression.Call(
                instanceCast,
                prop.GetSetMethod(),
                valueCast
                );

            var setter = Expression.Lambda<Action<object, object>>(setCall, new ParameterExpression[] { targetExp, valueExp })
                .Compile();

            return setter;
        }
    }
}
