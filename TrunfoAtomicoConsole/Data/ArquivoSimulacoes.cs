using System.Text.Json;
using TrunfoAtomicoConsole.Models;

namespace TrunfoAtomicoConsole.Data
{
    // Sem este record o System.Text.Json não remonta a Simulacao, porque as propriedades dela têm private set.
    public record SimulacaoJson(
        int Id,
        string NomeIsotopo,
        double MeiaVidaDias,
        double QuantidadeInicialGramas,
        double TempoDias);

    public static class ArquivoSimulacoes
    {
        const string Arquivo = "simulacoes.json";

        public static List<Simulacao> Carregar()
        {
            if (!File.Exists(Arquivo))
            {
                List<Simulacao> exemplos = CriarExemplos();
                Salvar(exemplos);
                return exemplos;
            }

            try
            {
                string texto = File.ReadAllText(Arquivo);
                List<SimulacaoJson>? lidas = JsonSerializer.Deserialize<List<SimulacaoJson>>(texto);

                List<Simulacao> simulacoes = new List<Simulacao>();
                if (lidas != null)
                {
                    foreach (SimulacaoJson item in lidas)
                    {
                        simulacoes.Add(new Simulacao(item.Id, item.NomeIsotopo, item.MeiaVidaDias, item.QuantidadeInicialGramas, item.TempoDias));
                    }
                }
                return simulacoes;
            }
            catch (Exception)
            {
                // Se o arquivo estragar, o programa quebraria em toda abertura; melhor avisar e seguir com os exemplos.
                Console.WriteLine("O arquivo simulacoes.json está vazio ou corrompido. Usando as simulações de exemplo.");
                return CriarExemplos();
            }
        }

        public static void Salvar(List<Simulacao> simulacoes)
        {
            List<SimulacaoJson> paraGravar = new List<SimulacaoJson>();
            foreach (Simulacao simulacao in simulacoes)
            {
                paraGravar.Add(new SimulacaoJson(simulacao.Id, simulacao.NomeIsotopo, simulacao.MeiaVidaDias, simulacao.QuantidadeInicialGramas, simulacao.TempoDias));
            }

            JsonSerializerOptions opcoes = new JsonSerializerOptions { WriteIndented = true };
            string texto = JsonSerializer.Serialize(paraGravar, opcoes);
            File.WriteAllText(Arquivo, texto);
        }

        private static List<Simulacao> CriarExemplos()
        {
            List<Simulacao> exemplos = new List<Simulacao>();

            // Meias-vidas da NUBASE2020 via NuDat 3.0, convertidas para dias com o ano juliano de 365,25 dias.
            exemplos.Add(new Simulacao(1, "Iodo-131", 8.0249, 100, 16.0498));
            exemplos.Add(new Simulacao(2, "Tecnécio-99m", 0.25028, 50, 0.25028));
            exemplos.Add(new Simulacao(3, "Cobalto-60", 1925.38, 200, 0));

            return exemplos;
        }
    }
}
