﻿using Microsoft.EntityFrameworkCore;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.Infra.Orm.Compartilhado;
using NoteKeeper.Infra.Orm.ModuloCategoria;
using NoteKeeper.Infra.Orm.ModuloNota;
using NoteKeeper.WebApi.Config.Mapping;
using NoteKeeper.WebApi.Config.Mapping.Action;

namespace NoteKeeper.WebApi;

public class Program
{
	public static void Main(string[] args)
	{
		const string politicaCors = "_minhaPoliticaCors";

		//Configuração de Injeção
		var builder = WebApplication.CreateBuilder(args);

		var connectionString = builder.Configuration.GetConnectionString("SqlServer");

		builder.Services.AddDbContext<IContextoPersistencia, NoteKeeperDbContext>(optionsBuilder =>
			{
				optionsBuilder.UseSqlServer(connectionString, dbOptions =>
				{
					dbOptions.EnableRetryOnFailure();
				});

			});

		builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoriaOrm>();
		builder.Services.AddScoped<ServicoCategoria>();

		builder.Services.AddScoped<IRepositorioNota, RepositorioNotaOrm>();
		builder.Services.AddScoped<ServicoNota>();
		builder.Services.AddScoped<ConfigurarCategoriaMappingAction>();

		builder.Services.AddAutoMapper(config =>
		{
			config.AddProfile<CategoriaProfile>();
			config.AddProfile<NotaProfile>();
		});

		//Configuração Cors
		builder.Services.AddCors(options =>
		{
			options.AddPolicy(name: politicaCors, policy =>
			{
				policy
				.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod();
			});
		});

		builder.Services.AddControllers();

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddSwaggerGen();


		//Execução da API
		var app = builder.Build();

		app.UseSwagger();
		app.UseSwaggerUI();

		//Migrações de Banco de Dados
		{
			using var scope = app.Services.CreateScope();

			var dbcontext = scope.ServiceProvider.GetRequiredService<IContextoPersistencia>();

			if (dbcontext is NoteKeeperDbContext noteKeeperDbContext)
			{
				MigradorBancoDados.AtualizarBancoDados(noteKeeperDbContext);
			}

		}

		app.UseHttpsRedirection();

		app.UseCors(politicaCors);

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
