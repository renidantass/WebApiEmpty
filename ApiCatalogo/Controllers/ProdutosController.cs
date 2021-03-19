using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;
        public ProdutosController(IUnitOfWork uow, ILogger<CategoriasController> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            try
            {
                return _uow.ProdutoRepository.GetProdutosPorPreco().ToList();
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar produtos no banco de dados");
            }
        }


        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                return _uow.ProdutoRepository.Get().ToList();
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível obter os produtos do banco de dados");
            }
        }

        [HttpGet("/primeiro")]
        [HttpGet("primeiro")]
        public ActionResult<Produto> GetFirst()
        {
            try
            {
                return _uow.ProdutoRepository.Get().FirstOrDefault();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro na consulta ao banco de dados");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id, string parm2)
        {
            try
            {
                var prod = _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);
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
                _uow.ProdutoRepository.Add(produto);
                _uow.Commit();
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

                _uow.ProdutoRepository.Update(produto);
                _uow.Commit();
                return Ok("Item editado com sucesso");
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao editar item no banco de dados");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            try
            {
                var prod = _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (prod == null)
                {
                    return BadRequest();
                }

                _uow.ProdutoRepository.Delete(prod);
                _uow.Commit();
                return prod;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar item no banco de dados");
            }
        }
    }
}
