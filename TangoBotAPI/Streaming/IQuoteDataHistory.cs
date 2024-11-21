namespace TangoBotAPI.Streaming
{
    public interface IQuoteDataHistory
    {
        void AppendData(QuoteDataHistory.DataPoint dataPoint);
    }
}