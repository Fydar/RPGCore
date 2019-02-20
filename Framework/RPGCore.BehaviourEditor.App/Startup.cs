using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using RPGCore.BehaviourEditor.App.Services;

namespace RPGCore.BehaviourEditor.App
{
	public class Startup
	{
		public void ConfigureServices (IServiceCollection services)
		{
			// Since Blazor is running on the server, we can use an application service
			// to read the forecast data.
			services.AddSingleton<WeatherForecastService> ();
		}

		public void Configure (IBlazorApplicationBuilder app)
		{
			app.AddComponent<App> ("app");
		}
	}
}
