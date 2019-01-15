using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ContentCreator.Models;
using ElectronNET.API.Entities;
using ElectronNET.API;
using System.Linq;

namespace ContentCreator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
			var menu = new MenuItem[] {
			new MenuItem
			{
			  Label = "Create Contact",
			  Click = () => Electron .WindowManager.BrowserWindows
					.First()
					.LoadURL($"http://localhost:{BridgeSettings.WebPort}/Contacts/Create")
				},
				new MenuItem
				{
				  Label = "Remove",
				  Click = () => Electron.Tray.Destroy()
				}
			};
			Electron.Tray.Show("/Assets/GraphIcon Large.png", menu);
			return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
