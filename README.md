# desafio-contas
Desafio controle de contas

## Solução
Um comerciante precisa controlar o seu fluxo de caixa diário com os lançamentos (débitos e créditos), também precisa de um relatório que disponibilize o saldo diário consolidado.

## Requisitos de negócio
- Serviço que faça o controle dos lançamentos
- Serviço do consolidado diário

## Requisitos técnicos
- Desenho da solução
- Pode ser feito na linguagem que você domina
- Boas práticas são bem vindas (Design Patterns, Padrões de arquitetura, SOLID, etc)
- Readme com instruções de como subir a aplicação local, container e utilização dos serviços
- Hospedar em repositório público (GitHub)

#### Modelo de negócio
A solução desenvolvida se resume a duas APIs REST para registro de conta e lançamentos.
Foi idealizado a necessidade da existência de uma conta na qual são registrados os lançamentos e todo o controle de saldo diário.

- A conta possui um nome e seu saldo
- Os lançamentos são registrados com uma descrição, valor, data e tipo (débito ou crédito)

#### Solução técnica
A tecnologia utilizada na solução foi AspNetCore WebApi (MVC pattern) em DotNet 6, em modelo de microserviço em container, com alguns objetos largamente utilizados no mercado a saber:
- Swagger
- Mediator
- FluentValidator
- ApiVersion
- AutoMapper

Como solução de persistência foi utilizado banco de dados MongoDB.

Foi adotado Clean Architecture como padrão arquitetural, nas camadas:
- **WebApi**: camada responsável por expor as APIs REST, utilizando minimal API 
- **Application**: camada de aplicação modelada utilizando-se do Pattern Mediator CQRS (Command Query Responsability Segregation), que por sua vez contribui fortemente na utilização dos padrões SOLID
- **Domain**: camada com as entidades básicas da solução
- **Infrastructure**: camada de abstração para serviços de infraestrutura
- **Infrastructure MongoDB**: camada de infraestrutura com os repositórios de acesso ao MongoDB

Foram utilizados alguns design patterns, a saber:
- Abstract Factory
- Dependency Injection
- Decorator
- Singleton
- Adapter
- Composite
- Mediator
- Command
- Async/Await

### Instruções para subir a aplicação local

#### Pré-requisitos

Para o funcionamento da aplicação é necessário a instalação dos seguintes pré-requisitos:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Dotnet 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

#### Baixando codigo fonte

1. Clone o repositório do "Desafio Lançamentos" executando a instrução:
	```
	git clone https://github.com/wilson-generoso/desafio-inmetrics.git
	```

2. Entre na pasta api/DesafioLancamentos (no contexto da pasta clonada):
	```
	cd api/DesafioLancementos
	```

#### Testando a aplicação + Cobertura de testes

3. Execute o teste da aplicação para verificar a cobertura de codigo:
	```
	dotnet test DesafioContas.sln /p:CollectCoverage=true
	```

#### Subindo o ambiente

4. Execute a seguinte instrução para inicializar a aplicação:
	```
	docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" --ansi never up --build --remove-orphans
	```

5. Abra a url da [documentação da API](https://localhost:8091/swagger)

6. Consuma a API para atender ao requisitos do desafio (você pode utilizar a tela de documentação se desejar):
***Os comandos abaixo são exemplos executados em console***

- Crie uma conta através do endpoint [POST]/v1/Account
	```
	curl --location --request POST 'https://localhost:8091/v1/Account' \
	--header 'Content-Type: application/json' \
	--data-raw '{
	  "name": "Teste",
	  "balance": 20.91
	}'
	```

- Copie o "id" da conta criada, retornado na resposta, e registre um lançamento de crédito
	```
	curl --location --request POST 'https://localhost:8091/v1/Entry' \
	--header 'Content-Type: application/json' \
	--data-raw '{
	  "accountId": "{substitua aqui com o 'id' da conta}",
	  "description": "Primeiro lançamento",
	  "date": "2022-10-10",
	  "value": 10,
	  "type": 2
	}'	
	```
	
- Consulte o saldo da conta
	```
	curl --location --request GET 'https://localhost:8091/v1/Account/{adicione aqui o 'id' da conta}'
	```

- Registre novo lançamento de débito
	```
	curl --location --request POST 'https://localhost:8091/v1/Entry' \
	--header 'Content-Type: application/json' \
	--data-raw '{
	  "accountId": "76702b95-7c07-470e-a1fb-33e711a557b6",
	  "description": "Registro debito",
	  "date": "2022-10-10",
	  "value": 15.37,
	  "type": 1
	}'
	```

- Consulte o extrato consolidado das operações da conta
	```
	curl --location --request GET 'https://localhost:8091/v1/Entry/{adicione aqui o 'id' da conta}'
	```