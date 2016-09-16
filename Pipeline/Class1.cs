using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Xunit;

namespace Pipeline
{
    public interface IFunction<in TIn, out TOut>
    {
        TOut Process(TIn @in);
    }

    public class Nothing
    {

    }

    public class Data
    {
        public string Name { get; set; }
    }

    public class DataViewModel
    {
        public string Name { get; set; }
    }

    public class ReadData : IFunction<Nothing, Data>
    {
        public Data Process(Nothing @in)
        {
            return new Data { Name = "Test" };
        }
    }

    public class MapData : IFunction<Data, DataViewModel>
    {
        public DataViewModel Process(Data @in)
        {
            return new DataViewModel { Name = @in.Name };
        }
    }

    public class Deco<TIn, TOut> : IFunction<TIn, TOut>
    {
        private readonly IFunction<TIn, TOut> _inner;

        public Deco(IFunction<TIn, TOut> inner)
        {
            _inner = inner;
        }
        public TOut Process(TIn @in)
        {
            //
            var process = _inner.Process(@in);
            //
            return process;
        }
    }

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

        public void Add<TIn, TOut>(IFunction<TIn, TOut> function)
        {
            _list.Add(function);
        }
    }
    public class Class1
    {
        [Fact]
        public void Do()
        {
            var readData = new ReadData();
            var mapData = new MapData();

            var pipeLine = new PipeLine();
            pipeLine.Add(readData);
            pipeLine.Add(mapData);

            var run = pipeLine.Run(new Nothing());

            readData.Process(new Nothing());
        }
    }
}
