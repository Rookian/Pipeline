namespace Pipeline
{
    public interface IFunction<in TIn, out TOut>
    {
        TOut Process(TIn @in);
    }
}