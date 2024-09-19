using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253505_AZAROV.UI.Models;

namespace WEB_253505_AZAROV.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "Лабораторная работа №2";
        var viewModel = new HomeViewModel {};
        ViewBag.selectList = new SelectList(new List<ListDemo> {
            new ListDemo{ Id = 1, Name="Item 1"},
            new ListDemo{ Id = 2, Name="Item 2"},
            new ListDemo{ Id = 3, Name="Item 3"},
        }, "Id", "Name", viewModel);
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
