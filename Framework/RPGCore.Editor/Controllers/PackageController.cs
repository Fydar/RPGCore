using Chromely.Core.RestfulService;
using RPGCore.Packages;
using System;

namespace Chromely.CefSharp.Win.Controllers
{
	[ControllerProperty (Name = "PackageController", Route = "package")]
	public class PackageController : ChromelyController
	{
		PackageExplorer explorer;

		public PackageController ()
		{
			RegisterGetRequest ("/package/icon.png", GetPackageIcon);
		}
		
		private ChromelyResponse GetPackageIcon (ChromelyRequest request)
		{
			string assetId = (string)request.Parameters["Asset"];

			PackageResource iconResource = default(PackageResource);
			foreach(var asset in explorer.Folders)
			{
				if (asset.Root == assetId)
				{
					foreach (var resource in asset.Assets)
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
				Data = iconResource.LoadData ()
			};
			return response;
		}
	}
}
