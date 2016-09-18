using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Pipeline
{
    public class PipeLine
    {
        readonly List<object> _list = new List<object>();

        public object Run(object obj)
        {
            var lastObj = obj;
            foreach (var function in _list)
            {
                var methodInfo = function.GetType().GetMethod(nameof(IFunction<string, object>.Process));
                var invoke = methodInfo.Invoke(function, BindingFlags.Default, null, new[] { lastObj }, CultureInfo.CurrentCulture);
                lastObj = invoke;
            }

            return lastObj;
        }

        public PipeLine Add<TIn, TOut>(IFunction<TIn, TOut> function)
        {
            _list.Add(function);
            return this;
        }
    }


}