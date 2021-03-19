using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    public class CategoriasRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriasRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
