using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using XorApp.Models;
using ElectronNET.API;

namespace XorApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (!HybridSupport.IsElectronActive) return View();
        
        Electron.PowerMonitor.OnLockScreen += () =>
        {
            _logger.LogInformation("Screen locked");
        };

        Electron.PowerMonitor.OnUnLockScreen += () =>
        {
            _logger.LogInformation("Screen unlocked");
        };

        Electron.PowerMonitor.OnSuspend += () =>
        {
            _logger.LogInformation("System sleeping");
        };

        Electron.PowerMonitor.OnResume += () =>
        {
            _logger.LogInformation("System resuming");
        };

        Electron.PowerMonitor.OnAC += () =>
        {
            _logger.LogInformation("System now charging");
        };

        Electron.PowerMonitor.OnBattery += () =>
        {
            _logger.LogInformation("System now on battery");
        };
        return View();
    }

    public IActionResult License()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
