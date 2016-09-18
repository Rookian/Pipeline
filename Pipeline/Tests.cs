using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Shouldly;
using Xunit;

namespace Pipeline
{
    public class Tests
    {
        [Fact]
        public void Integrate_functions_using_a_pipeline()
        {
            var pipeLine = new PipeLine();

            var result = pipeLine
                .Add(new ReadData())
                .Add(new MapData())
                .Run(new Nothing());

            var viewModel = result.ShouldBeOfType<DataViewModel>();
            viewModel.Name.ShouldBe("Test");
        }

        [Fact]
        public void Integrate_functions_using_simple_CSharp()
        {
            var viewModel = new MapData().Process(
                new ReadData().Process(new Nothing()));

            viewModel.Name.ShouldBe("Test");
        }

        [Fact]
        public void Integragte_functions_using_the_plus_operator()
        {
            CombinableFunction read = new Read();
            CombinableFunction map = new Map();
            
            var all = read += map;
            var result = all.Run();

            var viewModel = result.ShouldBeOfType<DataViewModel>();
            viewModel.Name.ShouldBe("Test");
        }
    }

    public class CombinableFunction
    {
        protected CombinableFunction Next { get; private set; }

        public object Run()
        {
            var functions = new List<object> { this, Next };

            var lastObj = (object)null;
            foreach (var function in functions)
            {
                var methodInfo = function.GetType().GetMethod(nameof(IFunction<string, object>.Process));
                var invoke = methodInfo.Invoke(function, BindingFlags.Default, null, new[] { lastObj }, CultureInfo.CurrentCulture);
                lastObj = invoke;
            }

            return lastObj;
        }

        public static CombinableFunction operator +(CombinableFunction left, CombinableFunction right)
        {
            left.Next = right;
            return left;
        }
    }

    public abstract class FunctionBae<TIn, TOut> : CombinableFunction
    {
        public abstract TOut Process(TIn @in);
    }

    public class Read : FunctionBae<Nothing, Data>
    {
        public override Data Process(Nothing @in)
        {
            return new Data { Name = "Test" };
        }
    }

    public class Map : FunctionBae<Data, DataViewModel>
    {
        public override DataViewModel Process(Data @in)
        {
            return new DataViewModel { Name = @in.Name };
        }
    }
}