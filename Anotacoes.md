# Entity Framework
* É um framework ORm (Object-Relational Mapping) criado para facilitar a integração com banco de dados, mapeando tabelas e gerando comandos SQL de forma automática.
* abstrai a geração de queries, comunicação e manipulação do banco de dados
* mapeia o banco de dados
# Entendendo CRUD
* Creat, Read, Update e Delete.
* Manipulações mais básicas do banco dadados
# instalando pacotes

* digitar no terminal
* não precisa executar novamente, executa apenas uma vez
* global instalado globalmente na máquina,para facilitar a execução

```console
    dotnet tool install --global dotnet-ef
```
* para instalar o pacote  executar no terminal:
* esse pacote precisa instar em todo projeto
```console
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```
# Classe Contato
* Criar uma pasta chamada `Entities` e colocar a classe `Contato`  Dentro
```csharp
    public class Contato
    {
        public int Id { get; set; } 
        public string Nomo { get; set; }
        public string Telefone { get; set; }
        public bool Ativo { get; set; }
    }
```
# Criando context
* Criar uma pasta chamada `Context` 
* Criar uma classe dentro e herdar de DbContext
```csharp

        using Microsoft.EntityFrameworkCore ;
        using DotNet_EntityFramework_CRUD.Entities ; 

        namespace DotNet_EntityFramework_CRUD.Context
        {
            public class AgendaContext: DbContext
            {
                public AgendaContext(DbContextOptions<AgendaContext> options):base(options){

                }
                
                public DbSet<Contato> Contatos{get;set;}

            }
        }

        
```

# Configurando a 

* `appsettings.Development.json` usado para ambiente de desenvolvimento
* `appsettings.json` usado para ambiente de produção após publicar a api
* Devemos fazer a configuração no arquivo de desenvolvimento
* `Server=localhost\\sqlexpress` é o servidor instalado
* `Initial Catolog = Agenda` é a base de dados
* `Integrated Security = True`  será usada a autenticação do windowns

```json

    {
    "Logging": {
        "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
        }
    }, "ConnectionStrings": {
        "ConexaoPadrao":"Server=localhost\\sqlexpress; Initial Catolog = Agenda; Integrated Security = True"
    }
    }

```
* na classe program acrescentar

```csharp
    using DotNet_EntityFramework_CRUD.Context;
    using Microsoft.EntityFrameworkCore;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.services.AddDbContext<AgendaContext>(options=>
        options.UseSqlServer(builder.Configuration.GetconnectionString("ConexaoPadrao"))
    )
```
# Migrations
* Executar o banco de dados
* Criando uma migration
```console
    dotnet-ef migrations add CriacaoTabelaContato
```
* serão criadas tres classes dentro de uma pasta chamada Migrations, de forma automatica pelo entity framework
* Uma classe possui o nome no singular `Contato` porém no banco de dados por padrão é no plural `Contatos`
* para aplicar a migration
```console
     dotnet-ef database update
```
* Na base de dados a tabela `EFMIgrationsHistory` serve para armazenar as migrations já executadas, para não duplicar tabelas.
* Na base é possível ver que existe a tabela Contado dentro da base agenda

# Post
* criar o construtor passando o context
* metodo creat colocar o atributo `httpPost` e o parâmetro `contato`

```csharp 

    [ApiController]
    [Route("[Controller]")]
    public class ContatoController:ControllerBase
    {
        private readonly AgendaContext _context;
        public ContatoController(AgendaContext context){
            _context = context;
        }

        [HttpPost()]
        public IActionResult Create(Contato contato){
            _context.Add(contato);
            _context.SaveChanges();
            return Ok(contato);
        }
        
    }

```

# Obter pelo ID
```csharp
    [HttpGet("{id}")]
    public IActionResult ObterPeloID(int id){
        
        var contato = _context.Contatos.Find(id);
        if(contato==null)
            return NotFound();

        return Ok(contato);
    }
```

# Atualizando dado
```csharp
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Contato contato){
        
        var contatoBanco = _context.Contatos.Find(id);
        if(contatoBanco==null)
            return NotFound();

        contatoBanco.Nome=contato.Nome;
        contatoBanco.Telefone = contato.Telefone;
        contatoBanco.Ativo = contato.Ativo;
        _context.Contatos.Update(contatoBanco);
        _context.SaveChanges();

        return Ok(contatoBanco);

    }
```
# deletando dado
```csharp
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id){
            
            var contatoBanco = _context.Contatos.Find(id);
            if(contatoBanco==null)
                return NotFound();

            _context.Contatos.Remove(contatoBanco);
            _context.SaveChanges();
            
            return NoContent();

        }
```
# Obtendo pelo nome
```csharp
        [HttpGet("ObterPorNome/{nome}")]
        public IActionResult ObterPorNome(string nome){

            var contatos = _context.Contatos.Where(x=> x.Nome.Contains(nome));

            if(contatos==null)
                return NotFound();

            return Ok(contatos);
        }
```

# Verbos HTTP
* .[https://www.restapitutorial.com/lessons/httpmethods.html]

# Retornando o endpoint

```csharp
        [HttpPost()]
        public IActionResult Create(Contato contato){
            _context.Add(contato);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPeloID),new{id = contato.Id}, contato);
        }

```