using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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