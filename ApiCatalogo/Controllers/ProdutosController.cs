using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            try
            {
                return await _context.Produtos.AsNoTracking().ToListAsync();
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível obter os produtos do banco de dados");
            }
        }

        [HttpGet("/primeiro")]
        [HttpGet("primeiro")]
        public async Task<ActionResult<Produto>> GetFirstAsync()
        {
            try
            {
                return await _context.Produtos.AsNoTracking().FirstOrDefaultAsync();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro na consulta ao banco de dados");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetAsync(int id, string parm2)
        {
            try
            {
                var prod = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (prod == null)
                {
                    return NotFound($"Produto com id {id} não encontrado");
                }

                return prod;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Não foi possível o obter produto de id {id} do banco de dados");
            }
        }

        [HttpPost]
        public  ActionResult Post([FromBody] Produto produto)
        {
            try
            {
                _context.Produtos.Add(produto);
                _context.SaveChanges();
                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Não foi possível adicionar item ao banco de dados");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest("Produto não encontrado para alterar");
                }

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok("Item editado com sucesso");
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao editar item no banco de dados");
            }
        }

        [HttpDelete("/{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            try
            {
                var prod = _context.Produtos.FirstOrDefault(prod => prod.ProdutoId == id);
                //var prod = _context.Produtos.Find(id);

                if (prod == null)
                {
                    return BadRequest();
                }

                _context.Produtos.Remove(prod);
                _context.SaveChanges();
                return prod;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar item no banco de dados");
            }
        }
    }
}
