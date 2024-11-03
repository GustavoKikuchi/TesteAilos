using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Response;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;

namespace Questao5.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna o saldo da conta corrente de acordo com os parametros informados.
        /// </summary>
        /// <response code="200">Saldo da conta corrente</response>
        /// <response code="400"></response>        
        [HttpGet("balance")]
        [ProducesResponseType(typeof(BaseResponse<GetBalanceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GetBalanceResponse>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> GetBalance([FromQuery] Guid id)
        {
            var query = new GetBalanceQuery(id);

            return Ok(await _mediator.Send(query));
        }

        /// <summary>
        /// Realiza uma operação de crédito ou débito na conta corrente.
        /// </summary>
        /// <response code="200">Id da movimentação</response>
        /// <response code="400"></response>        
        [HttpPost("movement")]
        [ProducesResponseType(typeof(BaseResponse<PostMovementCommandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PostMovementCommandResponse>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> PostMovement([FromBody] PostMovementCommandRequest request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
