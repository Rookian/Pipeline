namespace Pipeline
{
    public class ReadData : IFunction<Nothing, Data>
    {
        public Data Process(Nothing @in)
        {
            return new Data { Name = "Test" };
        }
    }
}