using Dapper;
using System.Data;

namespace ExaminationSystem.Domain.Implementations
{
    public class DapperRepository<T> : IDapperRepository<T> where T : BaseModel
    {
        private readonly IDbConnection _dbConnection;

        public DapperRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string query, object? param = null)
        {
            return await _dbConnection.QueryAsync<T>(query, param);
        }

        public async Task<T> GetByIdAsync(string query, object? param = null)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(query, param);
        }


    }
}
