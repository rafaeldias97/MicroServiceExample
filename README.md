# Iniciando com CQRS e EventSourcing

Para este artigo, estará sendo apresentado exemplos em C# utilizando Dotnet Core 3.1

## 1. Introdução

Atualmente, os sistemas devem estar sempre preparados para a grande quantidade de demandas e para isso é necessário ter conhecimento em frameworks, design patterns, padrões de projetos e por fim, em arquitetura. Com isto, o **DDD** e **CQRS** recebem muito destaque por solucionar problemas de domínios complexos e desempenho.
Neste artigo será apresentado um pouco sobre como utilizar CQRS. Com isto, sempre surgem algumas duvidas, como: Quando devo utilizar? Quais as vantagens e desvantagens? Devo utilizar duas bases de dados? se acalmeee, tudo será esclarecido. 

## 2. Caso de Uso - Transferencia Bancaria

O exemplo será aplicado em cima de um contexto de sistema bancário especificamente em um sistema de Saldo onde existem varias transações como:
- transferência
- saque
- deposito
- extrato
- pagamento 

![](https://raw.githubusercontent.com/rafaeldias97/MicroServiceExample/master/files/dbtradicional.png)

Será implementadas apenas as transações de **transferência** e **extrato**.
Em um modelo tradicional seria criado uma base de dados com uma tabela chamada Saldo como é ilustrado acima.

![](https://raw.githubusercontent.com/rafaeldias97/MicroServiceExample/master/files/dbfailed.png)

Nota-se que neste cenario serão realizadas varias transações no banco de dados, devido a grande quantidade de demandas, naturalmente vem a necessidade de escalar o serviço de sistema de conta, porém, os recursos do banco de dados se tornariam concorrentes trazendo travamentos, lentidões e deadlocks.

![](https://raw.githubusercontent.com/rafaeldias97/MicroServiceExample/master/files/price.png)

A primeira solução que iria vir, seria escalar verticalmente, adicionando mais recursos no seu servidor, contudo seria uma solução mais custosa.

## 3. CQRS - command query responsibility segregation

![](https://raw.githubusercontent.com/rafaeldias97/MicroServiceExample/master/files/dbMasterSlave.png)

Então, se pensarmos como arquitetos e tirarmos um pouco da responsabilidade da infra, poderemos criar algo como um banco de dados master/slave.
Chegamos a estrutura do **CQRS (command query responsibility segregation)** do português **(Separação entre comando e consulta)**. O CQRS é um padrão de arquitetura em que como o nome é autoexplicativo onde consiste na separação de **leitura/commands** e **escrita/query**. Não é necessário o uso de dois bancos de dados porém, para ter o máximo de desempenho é ideal ter pelo menos duas bases, dependendo do caso, uma para leitura e outra para escrita, porém, existem varias formas de implementar o CQRS.

![](https://raw.githubusercontent.com/rafaeldias97/MicroServiceExample/master/files/modelproject.png)

Para o modelo que estará sendo ilustrando, será utilizado dois bancos de dados. O banco de dados de escrita e outro para leitura. Lembrando que o CQRS deve ser utilizado apenas em sistemas onde a **concorrência é alta** e contenha **altas requisições de leitura e escrita em uma mesma base de dados**, caso contrário o aumento da complexidade de codigo seria desnecessário visto que, não haverá ganhos significativos na performace.
Existem quatro maneiras de sincronizar o banco de dados Desnormalizado com o banco de dados normalizado, sendo elas: 
1. Atualização Automatica
2. Atualização Eventual
3. Atualização Controlada
4. Atualização por demanda

#### 3.1 Atualização Automatica
A atualização automática é disparada de forma sincrona, então no momento que é feita a requisição o evento de commit é realizado
#### 3.2 Atualização Eventual
Sendo a melhor maneira de se implementar CQRS a atualização eventual ocorre de forma assincrona, geralmente é utilizado servicos de fila como RabbitMQ e Apache Kafka proporcionando a **concistencia eventual** para a base desnormalizada.
#### 3.3 Atualização por Controlada
Uma espécia de JOB é executado, assim, é disparado um evento para a sincronização perioticamente.
#### 3.4 Atualização por Demanda
A cada consulta é realizada uma verificação da consistencia entre as bases, caso esteja desatualizada, é realizado a consistencia de atualização.

Como foi explanado acima, a Atualização Eventual é a melhor maneira de se utilizar CQRS, porém, como a concistencia é eventual, pode ser que os dados de consulta ainda estejam desatualizados, mas com algumas técnicas de front-end será possivel apresentar dados atualizados ao usuario, todavia esses dados não estarão atualizados na base de dados.

## 4. DDD - Domain Driven Design
Antes de iniciar a implementação, vamos entender um pouco sobre Domain Driven Design que significa **Projeto Orientado a Dominio** em que foi apresentado por Eric Evans. O Domain driven design tem conhecimentos de resolução de problemas a mais de 20 anos.
O dominio de negócio deve ser isolado, assim como outras partes do sistema, só pra deixar claro, o DDD não é uma arquitetura e sim um paradigma que permite com que voce implemente para determinada arquitetura.
Com o aumento da complexidade de código utilizando o CQRS, o DDD se torna muito util quando implementado em conjunto, é muito importante  que o time tenha conhecimento em DDD.
Com o DDD, voce torna o seu codigo legivel, escalável, testável e de facil manutenção, a seguir, será apresentado o modelo em cima do DDD que será utilizado para o projeto.

![](https://raw.githubusercontent.com/rafaeldias97/MicroServiceExample/master/files/DDDandCQRS.png)

Então como pode ser observado, separamos a arquitetura em formato de camadas. Para melhor visualização, cada camada será apresentada junto com a implementação no tópico a seguir.

## 5. Implementação
Os exemplos que serão utilizados para a implementação serão utilizados em cima da linguagem C# e DotNet Core 3.1. Serão apresentados apenas alguns exemplos, para melhor entendimento o repositorio do projeto está no [Github](https://github.com/rafaeldias97/MicroServiceExample).
#### 5.1 Camada de dominio
Vai ser apresentada apenas a camada de dominio, as outras camadas será abordada em outro artigo. A camada de Dominio é o centro da sua aplicação, todas as outras camadas irão conhecer o dominio, porém o dominio não conhecerá as outras camadas, é onde toda a estrutura da sua regra de negócio e algumas implementações do CQRS serão realizadas.

##### 5.1.1 Pastas de Dominio

