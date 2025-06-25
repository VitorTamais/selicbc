
# SelicBC

Esse é o SelicBC, um projeto em C# (.NET 8) para consulta, exportação e análise da Taxa Selic, com persistência opcional em SQLite e interface de console amigável. Este repositório foi desenvolvido com foco em clareza, boas práticas e robustez, visando facilitar a manutenção e a evolução do sistema em ambiente corporativo.



# Índice

- [Visão Geral](#visão-geral)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Como Executar](#como-executar)
- [Exemplo de Uso](#exemplo-de-uso)
- [Técnicas e Boas Práticas Utilizadas](#técnicas-e-boas-práticas-utilizadas)
  
## Visão Geral

O **SelicBC** é uma aplicação de linha de comando que permite:

- Consultar a série histórica da Taxa Selic Meta anual diretamente da API oficial do Banco Central do Brasil.

- Exibir, exportar e analisar dados em diferentes formatos.

- Calcular juros compostos de forma precisa, convertendo a taxa anual para mensal de acordo com as melhores práticas financeiras.

- Registrar e visualizar logs detalhados de todas as operações realizadas pelo usuário, com backup opcional em banco de dados local SQLite.


## Funcionalidades

- **Coleta Automática de Dados:** Busca todos os registros da Selic desde 01/08/1986 até a data atual, sem necessidade de entrada manual de datas.

- **Exibição de Dados:** Permite consultar intervalos personalizados, mostrando data, taxa anual (%) e taxa mensal simples (%) em formato de tabela.

- **Exportação de Dados:** Exporta dados para CSV ou Excel (planilha formatada com título, cabeçalho destacado, colunas ajustadas e rodapé com data de geração).

- **Cálculo de Juros Compostos:** Calcula juros mensais e anuais de forma exata, usando a conversão correta da taxa anual para mensal composta, com arredondamento financeiro.

- **Backup e Logs:** Registra todas as ações do usuário (opção, entrada, retorno, data/hora) e permite backup incremental em SQLite, além de visualização detalhada dos logs.

- **Interface Amigável:** Menu organizado, espaçamento, separadores e feedbacks claros para facilitar o uso, mesmo para iniciantes.

## Tecnologias Utilizadas

- **.NET 8 (C#):** Plataforma principal para desenvolvimento da aplicação.

- **Entity Framework Core:** ORM para persistência em banco de dados SQLite, com migrações e mapeamento de entidades.

- **EPPlus:** Biblioteca para geração de planilhas Excel (.xlsx) com formatação avançada.

- **API Banco Central do Brasil:** Fonte oficial dos dados da Taxa Selic Meta anual.

- **HttpClient:** componente nativo para consumo de APIs REST, usado para obter dados da API do Banco Central.




## Estrutura do Projeto

```bash
SelicBC
├── Core/
│   └── Contratos.cs
│
├── Modelos/
│   ├── RegistroSelic.cs
│   └── LogAcao.cs
│
├── Dados/
│   └── ContextoAplicacao.cs
│
├── Servicos/
│   └── ServicoSelic.cs
│
├── Exportadores/
│   ├── ExportadorCsv.cs
│   └── ExportadorExcel.cs
│
├── Auxiliares/
│   ├───Conversores/
│   │   ├──ConversorData.cs
│   │   └──ConversorDecimal.cs
│   ├── ConversorTaxa.cs
│   ├── LeitorEntrada.cs
│   └── CalculadorJuros.cs
│
└── Program.cs
```

- **Core:** Interfaces para desacoplamento e testes.

- **Modelos:** Entidades de domínio e logs.

- **Dados:** Contexto EF Core e configuração do banco.

- **Serviços:** Consumo da API do Banco Central.

- **Exportadores:** Geração de relatórios CSV e Excel.

- **Auxiliares:** Utilitários para conversão, leitura e cálculo.

- **Program.cs:** Menu principal e orquestração das operações.


## Como Executar

#### Instalar o .NET SDK 8.0

#### Windows

Acesse [dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0) e clique em **Download .NET SDK 8.0**.
Execute o instalador e aceite os termos.

Abra o PowerShell e execute:

```bash
dotnet --version
```
Deve exibir algo como: `8.0.x`


#### Linux (Ubuntu/Debian)

No terminal, baixe o script de instalação:

```bash
wget https://aka.ms/install-dotnet.sh -O install-dotnet.sh
sudo bash install-dotnet.sh 8.0
```

Verifique a instalação:

```bash
dotnet --version
```
Deve exibir: `8.0.x`


### Configurar Variáveis de Ambiente (opcional)

#### Windows

Vá em **Variáveis de Ambiente → Path → Editar**.
Adicione o seguinte caminho:

```
%USERPROFILE%\.dotnet\tools
```

Abra um novo PowerShell e instale:

```bash
dotnet tool install --global dotnet-ef --version 8.0.8
```

---

#### Linux

Instale a ferramenta:

```bash
dotnet tool install --global dotnet-ef --version 8.0.8
```

Adicione ao `PATH` (no `~/.bashrc` ou `~/.profile`):

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
```

Recarregue:

```bash
source ~/.bashrc
```



### Clonar o Repositório

```bash
git clone https://github.com/SEU_USUARIO/SelicBC.git
cd SelicBC
```


### Instalar Pacotes NuGet

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.8
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.8
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8
dotnet tool install --global dotnet-ef --version 8.0.8
dotnet add package EPPlus --version 8.0.1
```

Esses pacotes permitem usar SQLite, gerar planilhas Excel e utilizar o EF Core pela CLI.



### Criar e Aplicar Migrações

```bash
dotnet ef migrations add Inicial
dotnet ef database update
```

Isso cria as tabelas `Registros` e `Logs` no banco `selicbc.db`.


### Compilar e Executar a Aplicação

Restaurar dependências:

```bash
dotnet restore
```

Compilar o projeto:

```bash
dotnet build
```

Executar:

```bash
dotnet run
```

O menu interativo aparecerá no console.

## Exemplo de Uso

```bash
===== SelicBC =====

1) Coletar Dados API
2) Exibir Dados (2.1 mais recente, 2.2 mais antiga)
3) Exportar Dados (1=CSV, 2=XLSX)
4) Calcular Juros Compostos
5) Fazer Backup
6) Visualizar Backups
9) Sair

Escolha uma opção: 4

Mês/Ano (MM/yyyy): 06/2025
Valor inicial (R$): 10000

Original     : R$ 10.000,00
Juros Mensal : R$ 526,00
Total Mensal : R$ 10.526,00
Juros Anual  : R$ 8.200,00
Total Anual  : R$ 18.200,00
```



## Técnicas e Boas Práticas Utilizadas

- **SOLID e Clean Code:** Separação de responsabilidades, uso de interfaces e modularização para facilitar manutenção e testes.

- **Validação de Entrada:** Todos os dados inseridos pelo usuário são validados e tratados para evitar exceções e garantir robustez.

- **Conversão Financeira Correta:** Conversão da taxa anual para mensal composta usando a fórmula:

```bash
taxaMensal = Math.Pow(1 + taxaAnual, 1.0 / 12) - 1
```
e arredondamento financeiro para garantir precisão nos cálculos.

- **Exportação Profissional:** Planilhas Excel com título, cabeçalho destacado, colunas ajustadas, bordas e rodapé com data/hora de geração.

- **Logs Detalhados:** Cada ação do usuário é registrada com data/hora, opção, entrada e retorno, permitindo auditoria e rastreabilidade.

- **Persistência Opcional:** O backup só é realizado quando o usuário desejar, evitando gravações desnecessárias e facilitando o controle de sessões.

- **Interface de Console Amigável:** Menus claros, espaçamento, separadores e feedbacks visuais para melhor experiência do usuário.

# Referências

- [API Banco Central do Brasil](https://api.bcb.gov.br/dados/serie/bcdata.sgs.4390/dados?formato=json)
- [Documentação EF Core – SQLite](https://learn.microsoft.com/pt-br/ef/core/providers/sqlite/?tabs=dotnet-core-cli)
- [EPPlus](https://github.com/EPPlusSoftware/EPPlus)
- [System.Text.Json](https://www.nuget.org/packages/System.Text.json)
