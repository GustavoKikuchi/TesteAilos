namespace Questao5.Infrastructure.TableMap
{
    public class Idempotencia
    {
        public Guid ChaveIdempotencia { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }        
    }
}
