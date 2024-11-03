using Dapper;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database.Repository
{
    public class IdempotenceRepository : IIdempotenceRepository
    {
        private readonly IUnitOfWork _unitOfWork;        
        
        public IdempotenceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetIdempotenceResponseStore> Get(GetIdempotenceByIdRequestStore request)
        {            
            var response = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<GetIdempotenceResponseStore>(@"
                        SELECT 
                            chave_idempotencia,
                            requisicao,
                            resultado 
                        FROM 
                            idempotencia                         
                        WHERE 
                            chave_idempotencia = @chaveIdempotencia                         
                ", new { chaveIdempotencia = request.Id });


            return response;
        }

        public async Task<int> Insert(InsertIdempotenceRequestStore request)
        {            
            var sql = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                        VALUES (@chaveIdempotencia , @requisicao , @resultado);"
            ;
            var parameters = new
            {
                chaveIdempotencia = request.Id,
                requisicao = request.Requisicao,
                resultado = request.Resultado
            };

            var rowsAffected = await _unitOfWork.Connection.ExecuteAsync(sql, parameters);

            return rowsAffected;
        }
    }
}
