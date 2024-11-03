using Dapper;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;

namespace Questao5.Infrastructure.Database.Repository
{
    public class MovementRepository : IMovementRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovementRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Insert(InsertMovementRequestStore request)
        {
            var sql = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento,tipomovimento,valor)
                        VALUES (@idMovimento , @idContaCorrente , @dataMovimento , @tipoMovimento , @valor);";

            var parameters = new
            {
                idMovimento = request.Id,
                idContaCorrente = request.IdContaCorrente,
                dataMovimento = request.DataMovimento,
                tipoMovimento = request.TipoMovimento,
                valor = request.Valor
            };

            var rowsAffected = await _unitOfWork.Connection.ExecuteAsync(sql, parameters);

            return rowsAffected;
        }
    }
}
