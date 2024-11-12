using Serilog;
using System.Runtime.CompilerServices;

namespace NoteKeeper.WebApi.Config;

public static class SerilogConfigExtensions
{
	public static void ConfigureSerilog(this IServiceCollection services, ILoggingBuilder logging)
	{
		Log.Logger = new LoggerConfiguration()
			.WriteTo.Console()
			.CreateLogger();

		logging.ClearProviders();

		services.AddLogging(builder => builder.AddSerilog(dispose: true));
	}
}