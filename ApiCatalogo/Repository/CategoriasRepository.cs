using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Repository
{
    public class CategoriasRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriasRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Categoria>> GetCategorias(QueryStringParameters categoriasParameters)
        {
            return await PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.Nome), categoriasParameters.PageNumber, categoriasParameters.PageSize);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return await Get().Include(x => x.Produtos).ToListAsync();
        }
    }
}
