using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet_EntityFramework_CRUD.Entities
{
    public class Contato
    {
        public int Id { get; set; } 
        public string Nomo { get; set; }
        public string Telefone { get; set; }
        public bool Ativo { get; set; }
    }
}