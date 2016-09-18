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
    }
}