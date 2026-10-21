using TrunfoAtomicoConsole.Data;
using TrunfoAtomicoConsole.Models;

namespace TrunfoAtomicoConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Simulacao> simulacoes = ArquivoSimulacoes.Carregar();
            string opcao = "";

            while (opcao != "0")
            {
                Console.WriteLine();
                Console.WriteLine("===========================================");
                Console.WriteLine("  TRUNFO ATOMICO - Quanto sobra?");
                Console.WriteLine("  Calculadora de decaimento radioativo");
                Console.WriteLine("===========================================");
                Console.WriteLine("1 - Nova simulacao");
                Console.WriteLine("2 - Historico de simulacoes");
                Console.WriteLine("3 - Buscar por isotopo");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opcao: ");
                opcao = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    if (opcao == "1")
                    {
                        Console.Write("Nome do isotopo (ex: Iodo-131): ");
                        string nome = Console.ReadLine();
                        Console.Write("Meia-vida em dias (use virgula, ex: 8,0249): ");
                        double meiaVidaDias = double.Parse(Console.ReadLine());
                        Console.Write("Quantidade inicial em gramas (use virgula, ex: 100,5): ");
                        double quantidadeInicialGramas = double.Parse(Console.ReadLine());
                        Console.Write("Tempo decorrido em dias (use virgula, ex: 16,0498): ");
                        double tempoDias = double.Parse(Console.ReadLine());

                        // Sem banco não há Identity para numerar; o maior Id + 1 garante um número que ainda não existe.
                        int maiorId = 0;
                        foreach (Simulacao simulacao in simulacoes)
                        {
                            if (simulacao.Id > maiorId)
                                maiorId = simulacao.Id;
                        }

                        Simulacao nova = new Simulacao(maiorId + 1, nome, meiaVidaDias, quantidadeInicialGramas, tempoDias);

                        Console.WriteLine();
                        ExibirSimulacao(nova);

                        simulacoes.Add(nova);
                        ArquivoSimulacoes.Salvar(simulacoes);
                        Console.WriteLine();
                        Console.WriteLine("Simulacao salva no historico.");
                    }
                    else if (opcao == "2")
                    {
                        if (simulacoes.Count == 0)
                        {
                            Console.WriteLine("Nenhuma simulacao cadastrada.");
                        }
                        else
                        {
                            foreach (Simulacao simulacao in simulacoes)
                            {
                                ExibirSimulacao(simulacao);
                                Console.WriteLine();
                            }
                        }
                    }
                    else if (opcao == "3")
                    {
                        Console.Write("Nome ou parte do nome do isotopo: ");
                        string trecho = Console.ReadLine();

                        List<Simulacao> encontradas = new List<Simulacao>();
                        foreach (Simulacao simulacao in simulacoes)
                        {
                            if (simulacao.NomeIsotopo.ToLower().Contains(trecho.ToLower()))
                                encontradas.Add(simulacao);
                        }

                        if (encontradas.Count == 0)
                            throw new SimulacaoNaoEncontradaException("Nenhuma simulação encontrada para o isótopo informado.");

                        Console.WriteLine();
                        foreach (Simulacao simulacao in encontradas)
                        {
                            ExibirSimulacao(simulacao);
                            Console.WriteLine();
                        }
                    }
                    else if (opcao == "0")
                    {
                        Console.WriteLine("Encerrando o programa. Ate a proxima!");
                    }
                    else
                    {
                        Console.WriteLine("Opcao invalida.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Valor invalido. Digite um numero, usando virgula como separador decimal.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (SimulacaoNaoEncontradaException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocorreu um erro inesperado: " + ex.Message);
                }

                if (opcao != "0")
                {
                    Console.WriteLine();
                    Console.Write("Pressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        static void ExibirSimulacao(Simulacao simulacao)
        {
            Console.WriteLine("[" + simulacao.Id + "] " + simulacao.NomeIsotopo);
            Console.WriteLine("    Quantidade inicial: " + simulacao.QuantidadeInicialGramas.ToString("F4") + " g");
            Console.WriteLine("    Tempo decorrido: " + simulacao.TempoDias + " dias");
            Console.WriteLine("    Meia-vida: " + simulacao.MeiaVidaDias + " dias");
            // Recalculado a cada exibição, e não lido do arquivo, para o resultado nunca divergir dos dados.
            Console.WriteLine("    Restam: " + simulacao.CalcularQuantidadeRestanteGramas().ToString("F4") + " g (" + simulacao.CalcularPercentualRestante().ToString("F2") + "% do inicial)");
        }
    }
}
