﻿using Microsoft.AspNetCore.Blazor.Hosting;

namespace BehaviourEditor.Client
{
	public class Program
	{
		public static void Main (string[] args)
		{
			CreateHostBuilder (args).Build ().Run ();
		}

		public static IWebAssemblyHostBuilder CreateHostBuilder (string[] args) =>
			BlazorWebAssemblyHost.CreateDefaultBuilder ()
				.UseBlazorStartup<Startup> ();
	}
}
