Para rodar: 
 - certifique que o Docker Desktop esteja aberto
 - No terminal, acesse a pasta src/Teste.Api
 - Execute o comando: docker compose up --build -d
 - Acesse o Swagger em: http://localhost:8080/swagger

Para rodar sem docker:
  - SDK do .NET 10 instalado
  - Navegue até a pasta do projeto (src/Teste.Api) e execute: dotnet ef database update
  - Ainda na pasta src/Teste.Api, execute o comando: dotnet run
  - Verifique a porta no terminal após o comando
  - Acesse o Swagger em: http://localhost:PORTA/swagger

Exemplos de requests: (Porta: 8080 com docker)

Adicionar Cliente (POST):
POST http://localhost:8080/clientes
Body: 
{
    "nome": "Guilherme",
    "email": "Guilherme@hotmail.com",
    "telefone": "21987179172",
    "cep": "26310240",
    "numero": "0",
    "complemento": "apartamento 1"
}

Listar Clientes com Filtro (GET)
GET http://localhost:8080/clientes?cidade=Queimados&uf=rj

Lista Clientes por Id (GET)
GET http://localhost:8080/clientes/(Id)

Atualizar Cliente (PUT)
PUT http://localhost:8080/clientes/(Id)
Body: 
{
    "nome": "Guilherme",
    "email": "guiferrao@hotmail.com",
    "telefone": "21987179170",
    "cep": "21310210",
    "numero": "0",
    "complemento": "apartamento 1"
}

Para testar a memoria cache basta criar um cliente com um cep e repetir esse mesmo cep dentro de 5 minutos, e verá uma diferença de 500ms para 50ms aproximadamente.
