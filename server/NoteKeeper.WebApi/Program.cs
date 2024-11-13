using NoteKeeper.WebApi.Config;
using Serilog;

namespace NoteKeeper.WebApi;

public class Program
{
	public static void Main(string[] args)
	{
		const string politicaCors = "_minhaPoliticaCors";

		var builder = WebApplication.CreateBuilder(args);

		builder.Services.ConfigureDbContext(builder.Configuration);

		builder.Services.ConfigureCoreServices();

		builder.Services.ConfigureAutoMapper();

		builder.Services.ConfigureCors(politicaCors);

		builder.Services.ConfigureControllersWithFilters();

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddSwaggerGen();

		builder.Services.ConfigureSerilog(builder.Logging, builder.Configuration);

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