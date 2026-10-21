using TrunfoAtomicoConsole.Data;
using TrunfoAtomicoConsole.Models;

namespace TrunfoAtomicoConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool arquivoExistia = File.Exists("simulacoes.json");
            Console.WriteLine("Arquivo existia antes de carregar: " + arquivoExistia);

            List<Simulacao> simulacoes = ArquivoSimulacoes.Carregar();
            Console.WriteLine("Carregar() devolveu " + simulacoes.Count + " simulacoes:");
            foreach (Simulacao simulacao in simulacoes)
            {
                Imprimir(simulacao);
            }

            if (!arquivoExistia)
            {
                simulacoes.Add(new Simulacao(4, "Carbono-14", 2081925.0, 10, 2081925.0));
                ArquivoSimulacoes.Salvar(simulacoes);

                Console.WriteLine();
                Console.WriteLine("Carbono-14 acrescentado e lista salva.");
                Console.WriteLine("Arquivo gravado em: " + Path.GetFullPath("simulacoes.json"));
                Console.WriteLine("Conteudo do arquivo:");
                Console.WriteLine(File.ReadAllText("simulacoes.json"));
            }
        }

        static void Imprimir(Simulacao simulacao)
        {
            Console.WriteLine("  [" + simulacao.Id + "] " + simulacao.NomeIsotopo
                + " | meia-vida " + simulacao.MeiaVidaDias
                + " | " + simulacao.QuantidadeInicialGramas + " g"
                + " | " + simulacao.TempoDias + " dias"
                + " -> " + simulacao.CalcularQuantidadeRestanteGramas().ToString("F4") + " g - "
                + simulacao.CalcularPercentualRestante().ToString("F2") + "%");
        }
    }
}
