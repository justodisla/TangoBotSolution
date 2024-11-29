namespace TangoBot.API.Streaming
{
    public interface IQuoteDataHistory
    {
        void AppendData(QuoteDataHistory.DataPoint dataPoint);
    }
}