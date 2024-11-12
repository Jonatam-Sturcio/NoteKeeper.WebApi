using Microsoft.EntityFrameworkCore;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.Infra.Orm.Compartilhado;
using NoteKeeper.Infra.Orm.ModuloCategoria;
using NoteKeeper.Infra.Orm.ModuloNota;
using NoteKeeper.WebApi.Config;
using NoteKeeper.WebApi.Config.Mapping;
using NoteKeeper.WebApi.Config.Mapping.Action;
using NoteKeeper.WebApi.Filters;
using Serilog;

namespace NoteKeeper.WebApi;

public class Program
{
	public static void Main(string[] args)
	{
		const string politicaCors = "_minhaPoliticaCors";

		//Configuração de Injeção
		var builder = WebApplication.CreateBuilder(args);

		var connectionString = builder.Configuration["SQL_SERVER_CONNECTION_STRING"];

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

		//Configuração politica Cors
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

		builder.Services.AddControllers(options =>
		{
			options.Filters.Add<ResponseWrapperFilter>();
		});

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddSwaggerGen();

		var key = builder.Configuration["SERILOG_LICENSE_KEY"];
		builder.Services.ConfigureSerilog(builder.Logging, key);

		//Execução da API
		var app = builder.Build();

		app.UseGlobalExceptionHandler();

		app.UseSwagger();
		app.UseSwaggerUI();

		var migracaoConcluida = app.AutoMigrateDatabase();

		if (migracaoConcluida) Log.Information("Migração do branco de dados conclída");
		else Log.Information("Nenhuma migração de bando de dados pendente");

		app.UseHttpsRedirection();

		app.UseCors(politicaCors);

		app.UseAuthorization();

		app.MapControllers();
		try
		{
			app.Run();
		}
		catch (Exception ex)
		{
			Log.Fatal("Ocorreu um erro que ocasionou no fechamento da aplicação", ex);
			return;
		}
	}
}