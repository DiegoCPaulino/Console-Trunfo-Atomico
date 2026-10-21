# Quanto sobra? — calculadora de decaimento radioativo

Aplicação de console em C# (.NET 10) que calcula quanto de um isótopo radioativo ainda resta depois de certo tempo e guarda cada cálculo num histórico. É a calculadora "Quanto sobra?" do **Trunfo Atômico**, projeto do grupo para o Challenge FIAP em parceria com o CTMSP.

## O problema

A tecnologia nuclear ainda é pouco compreendida. Para a maior parte das pessoas, ela se resume a usinas, vazamentos de radiação e uso militar. Quase ninguém associa a palavra "nuclear" ao que ela faz no dia a dia: está na saúde, no diagnóstico e no tratamento de doenças; na indústria; na agricultura; nos laboratórios de pesquisa; e na própria geração de energia. A imagem que fica é a do perigo, e não a do uso.

Esse é o problema que o grupo escolheu enfrentar no Challenge, que tem como parceiro o CTMSP, o Centro Tecnológico da Marinha em São Paulo: aproximar as pessoas da tecnologia nuclear mostrando onde ela realmente aparece e como funciona.

## A solução do grupo

A resposta do grupo é o **Trunfo Atômico**, um app educativo de cartas. Cada carta é um isótopo radioativo com um uso real no dia a dia, e o atributo principal de cada uma é a **meia-vida**: o tempo que leva para metade do material decair. É a meia-vida que sustenta a ideia central do jogo: os isótopos que entram no corpo do paciente duram horas ou dias, enquanto os usados na geração de energia duram milhões de anos.

O app tem três funcionalidades. O **álbum** reúne as cartas, cada uma com o seu isótopo e o seu uso. O **duelo** coloca duas cartas frente a frente para comparar atributos. E a calculadora **"Quanto sobra?"** responde à pergunta que a meia-vida provoca: se eu tenho tantos gramas deste isótopo, quanto ainda resta depois de tanto tempo? Este console é essa terceira funcionalidade.

## Como o .NET se aplica à solução do grupo

Este console é o motor de cálculo de decaimento do Trunfo Atômico. A conta que ele faz é a mesma que o app em Flutter mostra na tela "Quanto sobra?", e a mesma que o painel da proposta de IoT do grupo usaria para saber quanto ainda resta de cada fonte radioativa guardada num hospital. Aqui, em .NET, essa conta existe como um programa que roda sozinho, recebe os dados pelo teclado e guarda o histórico de todos os cálculos feitos.

A aplicação é um console em C# sobre o .NET 10 e usa apenas o que a própria plataforma oferece. A simulação é uma classe com os dados protegidos, validados no construtor, e a fórmula do decaimento é um método da própria entidade. O histórico é gravado em JSON com o `System.Text.Json`, que já vem no .NET, sem banco de dados e sem nenhum pacote externo. Os erros de digitação são tratados com exceções, inclusive uma exceção própria para a busca sem resultado, e nenhum deles fecha o programa.

## Por que não é igual à entrega de Java

A API REST que o grupo entregou em Java é o **catálogo** do Trunfo Atômico: ela cadastra, lista e busca **isótopos**, com os dados de cada um, como a meia-vida. Este console não repete esse trabalho. O que ele cadastra, lista e busca são **simulações de decaimento**: cada registro é um cálculo feito por alguém, com o isótopo, a meia-vida usada, a quantidade inicial e o tempo decorrido. Esse registro não existe na API.

São entidades diferentes — lá, o isótopo; aqui, a simulação — e funcionalidades diferentes: na API se consulta o que é cada isótopo; no console se calcula quanto dele sobra e se guarda o histórico desses cálculos. As duas entregas se complementam, mas nenhuma faz o trabalho da outra.

## O que este console faz

```
===========================================
  TRUNFO ATOMICO - Quanto sobra?
  Calculadora de decaimento radioativo
===========================================
1 - Nova simulacao
2 - Historico de simulacoes
3 - Buscar por isotopo
0 - Sair
```

- **1 - Nova simulação:** pede o nome do isótopo, a meia-vida em dias, a quantidade inicial em gramas e o tempo decorrido em dias; mostra quanto resta, em gramas e em percentual, e grava no histórico.
- **2 - Histórico de simulações:** lista todas as simulações gravadas, com o resultado de cada uma.
- **3 - Buscar por isótopo:** mostra as simulações cujo nome contém o trecho digitado, sem diferenciar maiúsculas de minúsculas.
- **0 - Sair:** encerra o programa.

Cada simulação aparece assim:

```
[1] Iodo-131
    Quantidade inicial: 100,0000 g
    Tempo decorrido: 16,0498 dias
    Meia-vida: 8,0249 dias
    Restam: 25,0000 g (25,00% do inicial)
```

Dados inválidos não derrubam o programa: texto onde se espera número, meia-vida ou quantidade iguais a zero ou negativas, tempo negativo, nome vazio, busca sem resultado e opção de menu inexistente mostram uma mensagem e voltam ao menu.

## Arquitetura

```
TrunfoAtomicoConsole/
├── Program.cs
├── Models/
│   ├── Simulacao.cs
│   └── SimulacaoNaoEncontradaException.cs
└── Data/
    └── ArquivoSimulacoes.cs
```

- `Models/Simulacao.cs` — a entidade: os cinco dados de uma simulação, as validações no construtor e os dois métodos de cálculo.
- `Models/SimulacaoNaoEncontradaException.cs` — exceção própria, lançada quando a busca não encontra nenhuma simulação.
- `Data/ArquivoSimulacoes.cs` — lê e grava o histórico no arquivo `simulacoes.json` e cria as simulações de exemplo na primeira execução.
- `Program.cs` — o menu: lê o teclado, aciona a entidade e o arquivo e trata os erros.

A regra de negócio fica dentro da entidade: a quantidade restante é a quantidade inicial multiplicada por 0,5 elevado ao número de meias-vidas decorridas (tempo ÷ meia-vida). O `Program` só pede os dados e mostra o resultado.

A persistência é um arquivo JSON, sem banco de dados. O arquivo guarda apenas os dados digitados; o resultado é recalculado toda vez que aparece na tela, para nunca ficar diferente dos dados que o geraram.

## Como rodar

**Pré-requisitos:**

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
- Sistema com configuração regional em português do Brasil, o padrão do Windows em português, porque os números são lidos com vírgula decimal.

Não é preciso instalar banco de dados nem nenhum pacote: tudo o que o programa usa já vem no .NET.

Clone o repositório:

```bash
git clone https://github.com/DiegoCPaulino/Console-Trunfo-Atomico.git
```

**No Visual Studio:** abra o arquivo `TrunfoAtomicoConsole.slnx` e aperte **F5**.

**No terminal:** entre na pasta do projeto e rode:

```bash
cd Console-Trunfo-Atomico/TrunfoAtomicoConsole
dotnet run
```

## Dados de teste

Na primeira execução, o programa cria sozinho o arquivo `simulacoes.json` com três simulações de exemplo, para que a listagem e a busca funcionem sem precisar digitar nada:

| Id | Isótopo | Meia-vida (dias) | Quantidade inicial | Tempo decorrido (dias) | Resta |
| --- | --- | --- | --- | --- | --- |
| 1 | Iodo-131 | 8,0249 | 100 g | 16,0498 | 25,0000 g (25,00%) |
| 2 | Tecnécio-99m | 0,25028 | 50 g | 0,25028 | 25,0000 g (50,00%) |
| 3 | Cobalto-60 | 1925,38 | 200 g | 0 | 200,0000 g (100,00%) |

Escolha a opção **2** para vê-las e teste a busca, na opção **3**, com `iodo`.

O arquivo é criado na pasta de onde o programa é executado: com F5, em `bin\Debug\net10.0`; com `dotnet run`, na pasta do projeto. Ele não faz parte do repositório. Se for apagado, o programa recria os três exemplos na próxima execução.

**Sobre os números: o separador decimal é a vírgula.** Digite `8,0249`, e não `8.0249`. O ponto não é aceito, nem como separador de milhar: digite `1925,38`, e não `1.925,38`. O motivo é que, no Windows em português, o ponto é o separador de milhar, e aceitá-lo faria `8.0249` virar 80249 sem nenhum aviso. Por isso o programa recusa qualquer número com ponto e mostra a mensagem *"Valor invalido. Digite um numero, usando virgula como separador decimal."*

Dentro do `simulacoes.json` os números aparecem com ponto, que é o formato padrão do JSON. Isso não muda o que se digita no programa.

## Nota sobre os dados

As meias-vidas das simulações de exemplo foram conferidas contra a avaliação **NUBASE2020**, consultada pela base **NuDat 3.0** do NNDC (National Nuclear Data Center), e convertidas para dias — as que a base informa em anos, com o ano juliano de 365,25 dias. São os mesmos valores usados na API de Java do grupo.

## Integrantes

| Integrante | RM |
| --- | --- |
| Diego Paulino | 566841 |
| Guilherme Dabul | 559901 |
| Renan Lima | 568321 |
| Ryan Mazali | 567168 |
| Thiago Alexandre | 567486 |

Turma **2TDSPR** · Análise e Desenvolvimento de Sistemas · FIAP
