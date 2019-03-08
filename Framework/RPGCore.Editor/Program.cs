using Chromely.CefGlue.Winapi;
using Chromely.CefGlue.Winapi.ChromeHost;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Infrastructure;
using System;
using System.Reflection;
using WinApi.Windows;

namespace RPGCore.Editor
{
	class Program
	{
		static int Main (string[] args)
		{
			try
			{
				HostHelpers.SetupDefaultExceptionHandlers ();

				string startUrl = "local://app/main.html";

				ChromelyConfiguration config = ChromelyConfiguration
											  .Create ()
											  .WithAppArgs (args)
											  .WithHostSize (1200, 900)
											  .WithLogFile ("logs\\chromely.cef_new.log")
											  .UseDefaultLogger ("logs\\chromely_new.log")
											  .WithStartUrl (startUrl)
											  .WithLogSeverity (LogSeverity.Info)
											  .UseDefaultResourceSchemeHandler ("local", string.Empty)
											  .UseDefaultHttpSchemeHandler ("http", "chromely.com")
											  .WithDebuggingMode (true)

											  // The single process should only be used for debugging purpose.
											  // For production, this should not be needed when the app is published and an cefglue_winapi_netcoredemo.exe 
											  // is created.

											  // Alternate approach for multi-process, is to add a subprocess application
											  // .WithCustomSetting(CefSettingKeys.BrowserSubprocessPath, full_path_to_subprocess)
											  .WithCustomSetting (CefSettingKeys.SingleProcess, true);

				var factory = WinapiHostFactory.Init ("GraphIcon.ico");
				using (var window = factory.CreateWindow (
					() => new CefGlueBrowserHost (config),
					"RPGCore Editor",
					constructionParams: new FrameWindowConstructionParams ()))
				{
					// Register external url schems
					window.RegisterUrlScheme (new UrlScheme ("https://github.com/mattkol/Chromely", true));

					// Register current/local assembly:
					window.RegisterServiceAssembly (Assembly.GetExecutingAssembly ());

					// Register external assemblies directory:
					// string serviceAssembliesFolder = @"C:\ChromelyDlls";
					// window.RegisterServiceAssemblies (serviceAssembliesFolder);

					// Scan assemblies for Controller routes 
					window.ScanAssemblies ();

					window.SetSize (config.HostWidth, config.HostHeight);
					window.CenterToScreen ();
					window.Show ();
					return new EventLoop ().Run (window);
				}
			}
			catch (Exception exception)
			{
				Log.Error (exception);
			}

			return 0;
		}
	}
}
