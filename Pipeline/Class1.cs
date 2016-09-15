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

    public class Class1
    {
        public void Do()
        {
            var readData = new ReadData();
            var mapData = new MapData();

            readData.Process(new Nothing());
        }
    }
}
