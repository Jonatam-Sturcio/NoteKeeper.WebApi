using Microsoft.EntityFrameworkCore;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Infra.Orm.Compartilhado;
using NoteKeeper.Infra.Orm.ModuloCategoria;
using NoteKeeper.WebApi.Config.Mapping;

namespace NoteKeeper.WebApi;

public class Program
{
	public static void Main(string[] args)
	{
		//Configuração de Injeção
		var builder = WebApplication.CreateBuilder(args);

		var connectionString = builder.Configuration.GetConnectionString("SqlServer");

		builder.Services.AddDbContext<IContextoPersistencia, NoteKeeperDbContext>(optionsBuilder =>
			{
				optionsBuilder.UseSqlServer(connectionString);
			}
			);

		builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoriaOrm>();
		builder.Services.AddScoped<ServicoCategoria>();

		builder.Services.AddAutoMapper(config =>
		{
			config.AddProfile<CategoriaProfile>();
		});

		builder.Services.AddControllers();

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddSwaggerGen();


		//Execução da API
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}
