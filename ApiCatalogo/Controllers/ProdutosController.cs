using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public ProdutosController(IUnitOfWork uow, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorPreco()
        {
            try
            {
                var produtos = _uow.ProdutoRepository.GetProdutosPorPreco().ToList();
                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDTO;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar produtos no banco de dados");
            }
        }


        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            try
            {
                var prod = _uow.ProdutoRepository.Get().ToList();
                return _mapper.Map<List<ProdutoDTO>>(prod);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível obter os produtos do banco de dados");
            }
        }

        [HttpGet("/primeiro")]
        [HttpGet("primeiro")]
        public ActionResult<ProdutoDTO> GetFirst()
        {
            try
            {
                var prod = _uow.ProdutoRepository.Get().FirstOrDefault();
                return _mapper.Map<ProdutoDTO>(prod);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro na consulta ao banco de dados");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id, string parm2)
        {
            try
            {
                var prod = _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (prod == null)
                {
                    return NotFound($"Produto com id {id} não encontrado");
                }

                return _mapper.Map<ProdutoDTO>(prod);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Não foi possível o obter produto de id {id} do banco de dados");
            }
        }

        [HttpPost]
        public  ActionResult Post([FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                var prod = _mapper.Map<Produto>(produtoDto);
                _uow.ProdutoRepository.Add(prod);
                _uow.Commit();
                return new CreatedAtRouteResult("ObterProduto", new { id = produtoDto.ProdutoId }, produtoDto);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Não foi possível adicionar item ao banco de dados");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest("Produto não encontrado para alterar");
                }

                var prod = _mapper.Map<Produto>(produtoDto);

                _uow.ProdutoRepository.Update(prod);
                _uow.Commit();
                return Ok("Item editado com sucesso");
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao editar item no banco de dados");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
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

                var produto = _mapper.Map<ProdutoDTO>(prod);

                return produto;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar item no banco de dados");
            }
        }
    }
}
