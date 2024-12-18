﻿using FluentResults;
using NoteKeeper.Dominio.ModuloNota;

namespace NoteKeeper.Aplicacao.ModuloNota;

public class ServicoNota
{
	private readonly IRepositorioNota _repositorioNota;

	public ServicoNota(IRepositorioNota repositorioNota)
	{
		_repositorioNota = repositorioNota;
	}

	public async Task<Result<Nota>> InserirAsync(Nota nota)
	{
		var validador = new ValidadorNota();

		var resultado = await validador.ValidateAsync(nota);

		if (!resultado.IsValid)
		{
			var erros = resultado.Errors.Select(err => err.ErrorMessage);

			return Result.Fail(erros);
		}

		await _repositorioNota.InserirAsync(nota);

		return Result.Ok(nota);
	}

	public async Task<Result<Nota>> EditarAsync(Nota nota)
	{
		var validador = new ValidadorNota();

		var resultado = await validador.ValidateAsync(nota);

		if (!resultado.IsValid)
		{
			var erros = resultado.Errors.Select(err => err.ErrorMessage);

			return Result.Fail(erros);
		}
		_repositorioNota.Editar(nota);

		return Result.Ok(nota);
	}

	public async Task<Result<Nota>> ExcluirAsync(Guid id)
	{
		var nota = await _repositorioNota.SelecionarPorIdAsync(id);

		_repositorioNota.Excluir(nota);

		return Result.Ok();
	}

	public async Task<Result<List<Nota>>> SelecionarTodosAsync()
	{
		var notas = await _repositorioNota.SelecionarTodosAsync();

		return Result.Ok(notas);
	}

	public async Task<Result<Nota>> SelecionarPorIdAsync(Guid id)
	{
		var nota = await _repositorioNota.SelecionarPorIdAsync(id);

		return Result.Ok(nota);
	}

	public async Task<Result<List<Nota>>> Filtrar(Func<Nota, bool> predicate)
	{
		var notas = await _repositorioNota.Filtrar(predicate);

		return Result.Ok(notas);
	}

	public Result<Nota> AlterarStatus(Nota nota)
	{
		nota.Arquivada = !nota.Arquivada;

		_repositorioNota.Editar(nota);

		return Result.Ok(nota);
	}
}