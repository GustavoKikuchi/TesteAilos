using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetBalanceResponseStore> GetBalance(GetAccountByIdRequestStore request)
        {
            var response = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<GetBalanceResponseStore>(@"
                        SELECT 
                            cc.numero,
                            cc.nome,
                            COALESCE(SUM(CASE WHEN m.tipomovimento = 'C' THEN m.valor ELSE 0 END), 0) -
                            COALESCE(SUM(CASE WHEN m.tipomovimento = 'D' THEN m.valor ELSE 0 END), 0) AS saldo
                        FROM 
                            contacorrente cc
                        LEFT JOIN 
                            movimento m ON m.idcontacorrente = cc.idcontacorrente
                        WHERE 
                            cc.idcontacorrente = @idContaCorrente 
                        GROUP BY 
                            cc.numero, 
                            cc.nome;

                ", new { idContaCorrente = request.Id });


            return response;
        }

        public async Task<GetAccountResponseStore> GetIsValidAccount(GetAccountByIdRequestStore request)
        {
            return await _unitOfWork.Connection.QuerySingleOrDefaultAsync<GetAccountResponseStore>(@"
                        SELECT ativo
                        FROM 
                            contacorrente 
                        WHERE 
                            idcontacorrente = @idcontacorrente
                ", new { idcontacorrente = request.Id });
        }
      
    }
}
