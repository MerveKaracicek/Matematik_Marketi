using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Matematik_Marketi.Models;

namespace Matematik_Marketi.Controllers;  //projenin hangi bölümünde olduğunu belirtir

public class HomeController : Controller  //HomeController, MVC Controller'dan türemiş
{
    private readonly ILogger<HomeController> _logger;  //ILogger, loglama işlemleri için kullanılır

    public HomeController(ILogger<HomeController> logger)  //Constructor, HomeController oluşturulduğunda ILogger'ı alır
    {
        _logger = logger;
    }

    public IActionResult AnaMenu()  /*/Home/AnaMenu URL’si açıldığında çalışır.return View(); → Views/Home/AnaMenu.cshtml dosyasını döndürür.*/
    {
        return View();
    }

    public IActionResult Oyun()
    {
        return View();
    }
    public IActionResult AlisverisListesi()
    {
        return View();
    }
    public IActionResult Soru()
    {
        return View();
    }
    public IActionResult Bitis()
    {
        return View();
    }

    // Bu sayfa cachelenmesin diye eklenmiş.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    //Hata sayfası için, genellikle uygulama çalışırken hata olursa otomatik açılır.
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
