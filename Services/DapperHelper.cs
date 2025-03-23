using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace SenexPontosAPI.Services
{
    public class DapperHelper : IDapperHelper
    {
        private readonly string _connectionString;

        public DapperHelper(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<T>> QueryAsync<T>(string procedure, object parametros = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>(procedure, parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string procedure, object parametros = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(procedure, parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task ExecuteAsync(string procedure, object parametros = null)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(procedure, parametros, commandType: CommandType.StoredProcedure);
        }
    }

}
