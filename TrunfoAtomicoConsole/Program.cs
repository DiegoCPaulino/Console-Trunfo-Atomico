using TrunfoAtomicoConsole.Models;

namespace TrunfoAtomicoConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Testar(1, 10, 80, 0);
            Testar(2, 10, 80, 10);
            Testar(3, 10, 80, 30);
            Testar(4, 8.0249, 100, 16.0498);
            Testar(5, 0, 80, 10);
            Testar(6, 10, -5, 10);
            Testar(7, 10, 80, -1);

            Console.WriteLine("Fim dos testes.");
        }

        static void Testar(int caso, double meiaVidaDias, double quantidadeGramas, double tempoDias)
        {
            Console.Write("Caso " + caso + " | meia-vida " + meiaVidaDias + " | " + quantidadeGramas + " g | " + tempoDias + " dias -> ");

            try
            {
                Simulacao simulacao = new Simulacao(caso, "Teste", meiaVidaDias, quantidadeGramas, tempoDias);
                Console.WriteLine(simulacao.CalcularQuantidadeRestanteGramas().ToString("F4") + " g - " + simulacao.CalcularPercentualRestante().ToString("F2") + "%");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("ArgumentException: " + ex.Message);
            }
        }
    }
}
