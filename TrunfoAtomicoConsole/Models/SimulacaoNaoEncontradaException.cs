namespace TrunfoAtomicoConsole.Models
{
    public class SimulacaoNaoEncontradaException : Exception
    {
        public SimulacaoNaoEncontradaException(string mensagem) : base(mensagem) { }
    }
}
