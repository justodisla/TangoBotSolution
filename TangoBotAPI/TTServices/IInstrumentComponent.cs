namespace TangoBot.API.TTServices
{
    public interface IInstrumentComponent
    {
        Task<List<Instrument>> GetActiveInstrumentsAsync();
        Task<Instrument> GetInstrumentBySymbolAsync(string symbol);
    }
}