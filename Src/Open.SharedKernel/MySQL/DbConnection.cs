using System.Data;
using Dapper;
using MySqlConnector;

namespace Open.SharedKernel.MySQL;

public class DbConnection : IDbConnection
{
    public void Dispose()
    {
        // TODO release managed resources here
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public void BeginTransaction()
    {
        throw new NotImplementedException();
    }

    public MySqlConnection Connection { get; set; }
    public MySqlTransaction CurrentTransaction { get; set; }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null,
        CommandType? commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null,
        CommandType? commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = null,
        CommandType? commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null,
        int? commandTimeout = null, CommandType? commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public async Task<int> ExecuteAsync(string sql, object param, int? commandTimeout = null,
        CommandType? commandType = CommandType.Text,
        bool autoCommit = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<object> ExecuteScalarAsync(string sql, object param, int? commandTimeout = null,
        CommandType? commandType = CommandType.Text,
        bool autoCommit = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> ExecuteAndGetResultAsync<T>(string sql, object param, int? commandTimeout = null,
        CommandType? commandType = CommandType.Text,
        bool autoCommit = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<MySqlBulkCopyResult> WriteToServerAsync<T>(DataTable dataTable, IList<T> entites,
        bool autoCommit = false,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
