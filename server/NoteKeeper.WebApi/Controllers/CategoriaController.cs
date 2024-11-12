using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.WebApi.ViewModels;
using Serilog;

namespace NoteKeeper.WebApi.Controllers;

[Route("api/categorias")]
[ApiController]
public class CategoriaController(ServicoCategoria servicoCategoria, IMapper mapeador) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var resultado = await servicoCategoria.SelecionarTodosAsync();

		if (resultado.IsFailed)
		{
			return StatusCode(500);
		}

		var viewModel = mapeador.Map<ListarCategoriaViewModel[]>(resultado.Value);

		return Ok(viewModel);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var resultado = await servicoCategoria.SelecionarPorIdAsync(id);

		if (resultado.IsFailed)
		{
			return StatusCode(500);
		}
		else if (resultado.Value is null)
		{
			return NotFound(resultado.Errors);
		}

		var viewModel = mapeador.Map<VisualizarCategoriaViewModel>(resultado.Value);

		return Ok(viewModel);
	}

	[HttpPost]
	public async Task<IActionResult> Post(InserirCategoriaViewModel categoriaVm)
	{
		var categoria = mapeador.Map<Categoria>(categoriaVm);

		var resultado = await servicoCategoria.InserirAsync(categoria);

		if (resultado.IsFailed)
		{
			return BadRequest(resultado.Errors);
		}

		return Ok(categoriaVm);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(Guid id, EditarCategoriaViewModel categoriaVm)
	{
		var selecaoCategoriaOriginal = await servicoCategoria.SelecionarPorIdAsync(id);

		if (selecaoCategoriaOriginal.IsFailed)
		{
			return NotFound(selecaoCategoriaOriginal.Errors);
		}

		var categoriaEditada = mapeador.Map(categoriaVm, selecaoCategoriaOriginal.Value);

		var resultado = await servicoCategoria.EditarAsync(categoriaEditada);

		if (resultado.IsFailed)
		{
			return BadRequest(resultado.Errors);
		}

		return Ok(resultado.Value);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var resultado = await servicoCategoria.ExcluirAsync(id);

		if (resultado.IsFailed)
		{
			return NotFound(resultado.Errors);
		}

		return Ok();
	}
}