# Implementando Domain Driven Design com CQRS e EventSourcing

Para este artigo, estarei usando exemplos em C# utilizando Dotnet Core 3.1

## Introdução

Atualmente, os sistemas devem estar sempre preparados para a grande quantidade de demandas e para isso é necessário ter conhecimento em frameworks, design patterns, padrões de projetos por fim ter conhecimento em arquitetura. 

Neste artigo, estarei apresentando um pouco sobre Domain Driven Design e CQRS. Com isto, sempre surgem algumas duvidas, como: quando devo utilizar? quais as vantagens e desvantagens? devo utilizar duas bases de dados? onde será esclarecido.

## Caso de Uso - Sistema Bancário 

O exemplo será aplicado em sima de um contexto de sistema bancário especificamente em um sistema de Saldo onde existem varias transações como:
- transferência
- saque
- deposito
- extrato
- pagamento 

onde em um modelo tradicional seria criado uma base de dados de Sistema bancario com uma tabela chamada Saldo como é ilustrado abaixo.

----------------------- imagem Monolito para DB ------------------------

Como  neste cenário será realizado varias transações no banco de dados 
Com a grande quantidade de demandas, a necessidade de escalar o back-end, vem de forma natural. Neste caso os recursos do banco de dados se tornariam muito concorrentes trazendo travamentos, lentidões e deadlocks.

----------------------imagem com problema aplicacao escalavel de travamento no banco de dados -------------------------------------

A primeira solução que iria vir, seria escalar verticalmente adicionando mais recursos no seu servidor, porém seria uma solução um pouco mais custosa.

------------ imagem escalar banco verticalmente custo ---------------

## CQRS - command query responsibility segregation

Então se pensarmos como arquitetos e tirar um pouco da responsabilidade da infra, podemos criar algo como um banco de dados master e vários slave

------------ Imagem Banco Master Slave ---------------

Com isto, chegamos a estrutura do **CQRS (command query responsibility segregation)** do português **(Separação entre comando e consulta)**. O CQRS é um padrão de arquitetura em que como o nome é autoexplicativo é separado em dois objetos de leitura/commands e escrita/query. Não é necessário o uso de dois bancos de dados porém para ter o máximo de desempenho é ideal ter pelo menos duas bases dependendo do caso, um para escrita e outro para leitura, porém existem varias formas de implementar o CQRS.

Para o modelo que estará sendo ilustrando, será utilizado dois bancos de dados. O banco de dados de escrita

#### Duvidas
Duvidas frequentes que muitos devs que estão iniciando com o CQRS tem, é:

**1. Quando devo utilizar CQRS?**

Sistemas onde a concorrência é alta contendo altas requisições de leitura e escrita em uma mesma base de dados.

**2. vantagens**

Ganho de performasse em aplicações complexas

**3. Desvantagens**

Aumento da complexidade na implementação, é muito importante em uma equipe o conhecimento de DDD (Domain Driven Design), logo não devo utilizar CQRS em todos os cenários 

**4. Como vou sincronizar a base de escrita com a base de leitura?**

Existem varias formas de sincronizar as duas bases, a forma que irei abordar neste artigo é utilizando consistência eventual.









