namespace Pipeline
{
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
}