using Microsoft.Data.Sqlite;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private readonly DatabaseConfig _databaseConfig;

        public UnitOfWork(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
            _connection = new SqliteConnection(_databaseConfig.Name);
            _connection.Open();
        }

        public IDbConnection Connection => _connection;

        public IDbTransaction Transaction => _transaction;


        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            _transaction.Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();            
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
