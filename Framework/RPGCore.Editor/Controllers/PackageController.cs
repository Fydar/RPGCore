using Chromely.Core.RestfulService;
using RPGCore.Packages;
using System;
using System.Collections;

namespace Chromely.CefSharp.Win.Controllers
{
	[ControllerProperty (Name = "PackageController", Route = "package")]
	public class PackageController : ChromelyController
	{
		PackageExplorer explorer;

		public PackageController ()
		{
			RegisterGetRequest ("/package/icon", GetPackageIcon);

			Console.WriteLine ("Exported package...");
			explorer = PackageExplorer.Load ("Content/Core.bpkg");
			foreach (var folder in explorer.Folders)
			{
				Console.WriteLine (folder.Root);
				foreach (var asset in folder.Assets)
				{
					Console.WriteLine ("\t" + asset.ToString ());
				}
			}
		}
		
		private ChromelyResponse GetPackageIcon (ChromelyRequest request)
		{
			Console.WriteLine ("ad");
			try
			{
				Console.WriteLine ($"--------");
				foreach (var kvp in request.Parameters)
				{
					Console.WriteLine ($"{kvp.Key}: {kvp.Value}");
				}
				Console.WriteLine ($"--------");

				string assetId = (string)request.Parameters["Asset"];
				Console.WriteLine (assetId);
				Console.WriteLine ($"--------");

				PackageResource iconResource = default (PackageResource);
				foreach (var asset in explorer.Folders)
				{
					if (asset.Root == assetId)
					{
						foreach (var resource in asset.Assets)
						{
							if (resource.Name.EndsWith (".png", StringComparison.Ordinal))
							{
								iconResource = resource;
								Console.WriteLine (resource.Name);
								continue;
							}
						}
					}
				}

				ChromelyResponse response = new ChromelyResponse (request.Id)
				{
					Data = Convert.ToBase64String (iconResource.LoadData ())
				};
				return response;
			}
			catch(Exception e)
			{
				Console.WriteLine (e);
				return null;
			}
		}
	}
}
