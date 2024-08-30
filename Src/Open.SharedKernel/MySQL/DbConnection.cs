using System.Data;
using Dapper;
using MySqlConnector;
using Open.SharedKernel.Settings;

namespace Open.SharedKernel.MySQL;

public class DbConnection : IDbConnection
{
    #region Declare + Constructor

    private readonly MySqlConnection _connection;
    private readonly string _connectionString;
    private MySqlTransaction _transaction;
    private ConnectionState _currentState = ConnectionState.Connecting;
    private int _numberOfConnection = 0;

    public DbConnection(string dbConfig = "MasterDb")
    {
        _connectionString = CoreSettings.ConnectionStrings?[dbConfig] ??
                            throw new ArgumentNullException(_connectionString);
        _connection = new MySqlConnection(_connectionString);

        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
            _currentState = ConnectionState.Open;
            _numberOfConnection = 1;
        }
    }

    #endregion

    public MySqlConnection Connection => _connection;

    public MySqlTransaction CurrentTransaction
    {
        get
        {
            if (_transaction == null || _transaction.Connection == null)
            {
                _transaction = _connection.BeginTransaction();
            }

            return _transaction;
        }
    }
    
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql,
        object? param = null,
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text)
    {
        try
        {
            if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
            {
                using (var newConnection = new MySqlConnection(_connectionString))
                {
                    try
                    {
                        _numberOfConnection++;
                        return await newConnection.QueryAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                    }
                    finally
                    {
                        _numberOfConnection--;
                    }
                }
            }

            _currentState = ConnectionState.Fetching;
            return await _connection.QueryAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout,
                commandType: commandType);
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql,
        object? param = null, 
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text)
    {
        try
        {
            if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
            {
                using (var newConnection = new MySqlConnection(_connectionString))
                {
                    try
                    {
                        _numberOfConnection++;
                        return await newConnection.QueryFirstOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                    }
                    finally
                    {
                        _numberOfConnection--;
                    }
                }
            }

            _currentState = ConnectionState.Fetching;
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, 
        object? param = null,
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text)
    {
        try
        {
            if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
            {
                using (var newConnection = new MySqlConnection(_connectionString))
                {
                    try
                    {
                        _numberOfConnection++;
                        return await newConnection.QuerySingleOrDefaultAsync<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
                    }
                    finally
                    {
                        _numberOfConnection--;
                    }
                }
            }

            _currentState = ConnectionState.Fetching;
            return await _connection.QuerySingleOrDefaultAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql,
        object? param = null,
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text)
    {
        try
        {
            if (_currentState == ConnectionState.Fetching || _currentState == ConnectionState.Executing)
            {
                using (var newConnection = new MySqlConnection(_connectionString))
                {
                    try
                    {
                        _numberOfConnection++;
                        return await newConnection.QueryMultipleAsync(sql, param, CurrentTransaction,
                            commandTimeout: commandTimeout, commandType: commandType);
                    }
                    finally
                    {
                        _numberOfConnection--;
                    }
                }
            }

            _currentState = ConnectionState.Fetching;
            return await _connection.QueryMultipleAsync(sql, param, CurrentTransaction, commandTimeout: commandTimeout,
                commandType: commandType);
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<int> ExecuteAsync(string sql,
        object? param = null, 
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text,
        bool autoCommit = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _currentState = ConnectionState.Executing;

            var result = await _connection.ExecuteAsync(sql, param, CurrentTransaction, commandTimeout, commandType);
            if (autoCommit)
            {
                await CommitAsync(cancellationToken);
            }

            return result;
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<object?> ExecuteScalarAsync(string sql,
        object? param = null,
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text,
        bool autoCommit = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _currentState = ConnectionState.Executing;

            var result =
                await _connection.ExecuteScalarAsync(sql, param, CurrentTransaction, commandTimeout, commandType);
            if (autoCommit)
            {
                await CommitAsync(cancellationToken);
            }

            return result;
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async Task<IEnumerable<T>> ExecuteAndGetResultAsync<T>(string sql,
        object param,
        int? commandTimeout = null,
        CommandType? commandType = CommandType.Text,
        bool autoCommit = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _currentState = ConnectionState.Executing;

            return await _connection.QueryAsync<T>(sql, param, CurrentTransaction, commandTimeout: commandTimeout, commandType: commandType);
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    public async ValueTask<MySqlBulkCopyResult> WriteToServerAsync<T>(DataTable dataTable, 
        IList<T> entites,
        bool autoCommit = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _currentState = ConnectionState.Executing;

            // Create object of MySqlBulkCopy which help to insert  
            var bulk = new MySqlBulkCopy(_connection, CurrentTransaction);

            // Assign Destination table name  
            bulk.DestinationTableName = dataTable.TableName;

            // Mapping column
            int index = 0;
            foreach (DataColumn col in dataTable.Columns)
            {
                bulk.ColumnMappings.Add(new MySqlBulkCopyColumnMapping(index++, col.ColumnName));
            }

            await _connection.ExecuteAsync("SET GLOBAL local_infile=1", null, CurrentTransaction);

            // Insert bulk Records into DataBase.
            _connection.InfoMessage += (s, e) =>
            {
                // Log.Error(string.Join(" ----> ", e.Errors.Select(e => e.Message)));
            };

            var result = await bulk.WriteToServerAsync(dataTable, cancellationToken);
            if (autoCommit)
            {
                await CommitAsync(cancellationToken);
            }

            return result;
        }
        finally
        {
            if (_numberOfConnection <= 1)
            {
                _currentState = ConnectionState.Open;
            }
        }
    }

    #region UnitOfWork

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await CurrentTransaction.CommitAsync(cancellationToken);
    }
        
    public void BeginTransaction()
    {
        throw new NotImplementedException();
    }
    
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await CurrentTransaction.RollbackAsync(cancellationToken);
    }
    
    #endregion

    #region Dispose
    public void Dispose()
    {
        //GC.SuppressFinalize(this);
        if (_connection != null && _connection.State == ConnectionState.Open)
        {
            _connection.Close();
        }
    }
    #endregion
}
