using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.WebApi.ViewModels;

namespace NoteKeeper.WebApi.Controllers;

[Route("api/notas")]
[ApiController]
public class NotaController(ServicoNota servicoNota, IMapper mapeador) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var resultado = await servicoNota.SelecionarTodosAsync();

		var viewModel = mapeador.Map<ListarNotaViewModel[]>(resultado.Value);

		return Ok(viewModel);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var resultado = await servicoNota.SelecionarPorIdAsync(id);

		if (resultado.Value is null)
		{
			return NotFound(resultado.Errors);
		}

		var viewModel = mapeador.Map<VisualizarNotaViewModel>(resultado.Value);

		return Ok(viewModel);
	}

	[HttpPost]
	public async Task<IActionResult> Post(InserirNotaViewModel notaVm)
	{
		var nota = mapeador.Map<Nota>(notaVm);

		var resultado = await servicoNota.InserirAsync(nota);

		if (resultado.IsFailed)
		{
			return BadRequest(resultado.Errors);
		}

		return Ok(notaVm);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(Guid id, EditarNotaViewModel notaVm)
	{
		var resultado = await servicoNota.SelecionarPorIdAsync(id);

		if (resultado.Value is null)
		{
			return NotFound(resultado.Errors);
		}

		var notaEditada = mapeador.Map(notaVm, resultado.Value);

		var edicaoResult = await servicoNota.EditarAsync(notaEditada);

		if (edicaoResult.IsFailed)
		{
			BadRequest(edicaoResult.Errors);
		}

		return Ok(notaVm);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var resultado = await servicoNota.ExcluirAsync(id);

		if (resultado.IsFailed)
		{
			return BadRequest(resultado.Errors);
		}

		return Ok();
	}
}