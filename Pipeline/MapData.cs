namespace Pipeline
{
    public class MapData : IFunction<Data, DataViewModel>
    {
        public DataViewModel Process(Data @in)
        {
            return new DataViewModel { Name = @in.Name };
        }
    }
}