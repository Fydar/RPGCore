// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Win
{
	using Chromely.CefGlue.Winapi;
	using Chromely.CefGlue.Winapi.ChromeHost;
	using Chromely.Core;
	using Chromely.Core.Helpers;
	using Chromely.Core.Infrastructure;
	using System;
	using System.Reflection;
	using WinApi.Windows;

	class Program
	{
		static int Main (string[] args)
		{
			try
			{
				HostHelpers.SetupDefaultExceptionHandlers ();

				/*
                * Start url (load html) options:
                */

				// Options 1 - real standard urls 
				// string startUrl = "https://google.com";

				// Options 2 - using local resource file handling with default/custom local scheme handler 
				// Requires - (sample) UseDefaultResourceSchemeHandler("local", string.Empty)
				//            or register new resource scheme handler - RegisterSchemeHandler("local", string.Empty,  new CustomResourceHandler())
				string startUrl = "local://app/chromely.html";

				// Options 3 - using file protocol - using default/custom scheme handler for Ajax/Http requests
				// Requires - (sample) UseDefaultResourceSchemeHandler("local", string.Empty)
				//            or register new resource handler - RegisterSchemeHandler("local", string.Empty,  new CustomResourceHandler())
				// Requires - (sample) UseDefaultHttpSchemeHandler("http", "chromely.com")
				//            or register new http scheme handler - RegisterSchemeHandler("http", "test.com",  new CustomHttpHandler())
				// string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
				// string startUrl = $"file:///{appDirectory}app/chromely.html";
				ChromelyConfiguration config = ChromelyConfiguration
											  .Create ()
											  .WithAppArgs (args)
											  .WithHostSize (1200, 900)
											  .WithLogFile ("logs\\chromely.cef_new.log")
											  .WithStartUrl (startUrl)
											  .WithLogSeverity (LogSeverity.Info)
											  .UseDefaultLogger ("logs\\chromely_new.log")
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
					string serviceAssembliesFolder = @"C:\ChromelyDlls";
					window.RegisterServiceAssemblies (serviceAssembliesFolder);

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
