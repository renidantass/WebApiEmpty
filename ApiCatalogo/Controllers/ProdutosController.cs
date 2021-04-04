using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPorPreco()
        {
            try
            {
                var produtos = await _uow.ProdutoRepository.GetProdutosPorPreco();
                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDTO;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar produtos no banco de dados");
            }
        }


        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] QueryStringParameters produtosParameters)
        {
            try
            {
                var prod = await _uow.ProdutoRepository.GetProdutos(produtosParameters);

                var metadata = new
                {
                    prod.TotalCount,
                    prod.PageSize,
                    prod.CurrentPage,
                    prod.TotalPages,
                    prod.HasNext,
                    prod.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return _mapper.Map<List<ProdutoDTO>>(prod);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível obter os produtos do banco de dados");
            }
        }

        [HttpGet("/primeiro")]
        [HttpGet("primeiro")]
        public async Task<ActionResult<ProdutoDTO>> GetFirst()
        {
            try
            {
                var prod = await _uow.ProdutoRepository.Get().FirstOrDefaultAsync();
                return _mapper.Map<ProdutoDTO>(prod);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro na consulta ao banco de dados");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id, string parm2)
        {
            try
            {
                var prod = await _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);
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
        public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                var prod = _mapper.Map<Produto>(produtoDto);
                _uow.ProdutoRepository.Add(prod);
                await _uow.Commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(prod);

                return new CreatedAtRouteResult("ObterProduto", new { id = produtoDTO.ProdutoId }, produtoDTO);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Não foi possível adicionar item ao banco de dados");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest("Produto não encontrado para alterar");
                }

                var prod = _mapper.Map<Produto>(produtoDto);

                _uow.ProdutoRepository.Update(prod);
                await _uow.Commit();
                return Ok("Item editado com sucesso");
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao editar item no banco de dados");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            try
            {
                var prod = await _uow.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (prod == null)
                {
                    return BadRequest();
                }

                _uow.ProdutoRepository.Delete(prod);
                await _uow.Commit();

                var produto = _mapper.Map<ProdutoDTO>(prod);

                return produto;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar item no banco de dados");
            }
        }
    }
}
