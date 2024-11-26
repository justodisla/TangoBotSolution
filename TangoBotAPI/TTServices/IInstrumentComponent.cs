
namespace HttpClientLib.InstrumentApi
{
    public interface IInstrumentComponent
    {
        Task<List<Instrument>> GetActiveInstrumentsAsync();
        Task<Instrument> GetInstrumentBySymbolAsync(string symbol);
    }
}