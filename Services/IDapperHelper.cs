namespace SenexPontosAPI.Services
{
    public interface IDapperHelper
    {
        Task<IEnumerable<T>> QueryAsync<T>(string procedure, object parametros = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string procedure, object parametros = null);
        Task ExecuteAsync(string procedure, object parametros = null);
    }

}
