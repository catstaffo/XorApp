using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using XorApp.Services;
using XorApp.Models;

namespace XorApp.Controllers;

public class XorController : Controller
{
    private readonly XorService _xorService;

    public XorController(XorService xorService)
    {
        _xorService = xorService;
    }

    public IActionResult Index(XorIndexPM pm)
    {
        if (HybridSupport.IsElectronActive)
        {
            Electron.IpcMain.On("open-file-manager", async (args) =>
            {
                string path = await Electron.App.GetPathAsync(PathName.Home);
                await Electron.Shell.ShowItemInFolderAsync(path);

            });
        }

        var vm = _xorService.BuildXorIndex(pm);
        return View(vm);
    }
    
    public IActionResult Submit(XorIndexPM pm)
    {
        var vm = _xorService.BuildXorIndex(pm);
        return View("Index", vm);
    }

    public JsonResult GenerateKey(int bitLength)
    {
        var test = _xorService.BuildKeyView(bitLength);
        return new JsonResult(_xorService.BuildKeyView(bitLength));
    }
}
