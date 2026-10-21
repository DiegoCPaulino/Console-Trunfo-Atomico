namespace TrunfoAtomicoConsole.Models
{
    public class Simulacao
    {
        public int Id { get; private set; }
        public string NomeIsotopo { get; private set; } = string.Empty;

        // double, e não decimal: as meias-vidas vão de 0,07 a 455 bilhões de dias, e o Math.Pow usa double.
        public double MeiaVidaDias { get; private set; }
        public double QuantidadeInicialGramas { get; private set; }
        public double TempoDias { get; private set; }

        // Sem EF Core nada chama este construtor; ele fica para manter o molde da entidade vista em aula.
        protected Simulacao() { }

        // Recebe o Id porque não há banco para gerá-lo; quem calcula o número é o Program.
        public Simulacao(int id, string nomeIsotopo, double meiaVidaDias, double quantidadeInicialGramas, double tempoDias)
        {
            if (string.IsNullOrWhiteSpace(nomeIsotopo))
                throw new ArgumentException("O nome do isótopo é obrigatório.");

            if (meiaVidaDias <= 0)
                throw new ArgumentException("A meia-vida deve ser maior que zero.");

            if (quantidadeInicialGramas <= 0)
                throw new ArgumentException("A quantidade inicial deve ser maior que zero.");

            // Zero é válido porque nada decaiu ainda; negativo daria mais material do que o inicial.
            if (tempoDias < 0)
                throw new ArgumentException("O tempo decorrido não pode ser negativo.");

            Id = id;
            NomeIsotopo = nomeIsotopo;
            MeiaVidaDias = meiaVidaDias;
            QuantidadeInicialGramas = quantidadeInicialGramas;
            TempoDias = tempoDias;
        }

        public double CalcularQuantidadeRestanteGramas()
        {
            // A cada meia-vida sobra metade, então a fração restante é 0,5 elevado ao número de meias-vidas.
            return QuantidadeInicialGramas * Math.Pow(0.5, TempoDias / MeiaVidaDias);
        }

        public double CalcularPercentualRestante()
        {
            return Math.Pow(0.5, TempoDias / MeiaVidaDias) * 100;
        }
    }
}
