using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace RPGCore.BehaviourEditor.Server
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices (IServiceCollection services)
		{
			// Adds the Server-Side Blazor services, and those registered by the app project's startup.
			services.AddServerSideBlazor<App.Startup> ();

			services.AddResponseCompression (options =>
			 {
				 options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat (new[]
				 {
					MediaTypeNames.Application.Octet,
					WasmMediaTypeNames.Application.Wasm,
				 });
			 });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure (IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseResponseCompression ();

			if (env.IsDevelopment ())
			{
				app.UseDeveloperExceptionPage ();
			}

			// Use component registrations and static files from the app project.
			// app.UseServerSideBlazor<App.Startup> ();
			app.UseServerSideBlazor<App.Startup> ();

			if (HybridSupport.IsElectronActive)
			{
				Task.Run (ElectronBootstrap);
			}
		}

		public async Task ElectronBootstrap ()
		{
			var browserWindow = await Electron.WindowManager.CreateWindowAsync (new BrowserWindowOptions
			{
				Title = "Content Creator",
				Width = 1152,
				Height = 864,
				Show = false,
				//Icon = "~/Assets/GraphIcon.ico",
				SkipTaskbar = false
				//TitleBarStyle = "hidden"
			});
			browserWindow.OnReadyToShow += () => browserWindow.Show ();
		}
	}
}
