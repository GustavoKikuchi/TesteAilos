using System.Data;

namespace Questao5.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
