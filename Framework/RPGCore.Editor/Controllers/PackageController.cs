using Chromely.Core.RestfulService;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using LitJson;

namespace RPGCore.Editor.Controllers
{
	[ControllerProperty (Name = "PackageController", Route = "package")]
	public class PackageController : ChromelyController
	{
		readonly PackageExplorer explorer;

		public PackageController ()
		{
			RegisterGetRequest ("/package/icon", GetPackageIcon);
			RegisterGetRequest ("/package/list", ListAllAssets);

			Console.WriteLine ("Exported package...");
			explorer = PackageExplorer.Load ("Content/Core.bpkg");
			foreach (var asset in explorer.Assets)
			{
				Console.WriteLine (asset.Root);
				foreach (var resources in asset.Resources)
				{
					Console.WriteLine ("\t" + resources.ToString ());
				}
			}
		}

		class ListAllAssetsResponse
		{
			public List<string> Assets;

			public ListAllAssetsResponse (List<string> assets)
			{
				Assets = assets;
			}
		}

		private ChromelyResponse ListAllAssets (ChromelyRequest request)
		{
			try
			{
				var assets = new List<string> ();
				foreach (var asset in explorer.Assets)
				{
					assets.Add (asset.Root);
				}

				ChromelyResponse response = new ChromelyResponse (request.Id)
				{
					Data = new ListAllAssetsResponse (assets)
				};
				return response;
			}
			catch (Exception e)
			{
				Console.WriteLine (e);
				return null;
			}
		}

		private ChromelyResponse GetPackageIcon (ChromelyRequest request)
		{
			try
			{
				var parametersData = (Dictionary<string, JsonData>.KeyCollection)request.Parameters["Keys"];
				string key = null;
				foreach (var paramKey in parametersData)
				{
					key = paramKey;
				}

				PackageResource iconResource = default (PackageResource);
				foreach (var asset in explorer.Assets)
				{
					if (asset.Root == key)
					{
						foreach (var resource in asset.Resources)
						{
							if (resource.Name.EndsWith (".png", StringComparison.Ordinal))
							{
								iconResource = resource;
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
			catch (Exception e)
			{
				Console.WriteLine (e);
				return null;
			}
		}
	}
}
