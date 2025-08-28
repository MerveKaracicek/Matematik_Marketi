using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Matematik_Marketi.Models;
using Microsoft.EntityFrameworkCore;
using Matematik_Marketi.Services;
using Matematik_Marketi.Models.Entities;


namespace Matematik_Marketi.Controllers;  //projenin hangi bölümünde olduğunu belirtir

public class HomeController : Controller  //HomeController, MVC Controller'dan türemiş
    {private readonly GameService _gameService;
    private readonly ILogger<HomeController> _logger;  //ILogger, loglama işlemleri için kullanılır

    public HomeController(ILogger<HomeController> logger, GameService gameService)  //Constructor, HomeController oluşturulduğunda ILogger'ı alır
    {
        _logger = logger;
        _gameService = gameService;
    }

    public IActionResult AnaMenu()  /*/Home/AnaMenu URL’si açıldığında çalışır.return View(); → Views/Home/AnaMenu.cshtml dosyasını döndürür.*/
    {
        return View();
    }

    public IActionResult Oyun(int gameId)
    {
        var game = _gameService.GetGameWithList(gameId);
        if (game == null)
            return Content("Oyun bulunamadı.");
        return View(game);
    }
   
    public IActionResult AlisverisListesi(int gameId)
    {
        var game = _gameService.GetGameWithList(gameId);
        if (game?.ShoppingList == null)
            return Content("Alışveriş listesi bulunamadı.");
        return View(game.ShoppingList);
    }
    public IActionResult Soru(int gameId)
    {
        var game = _gameService.GetGameWithList(gameId);
        if (game == null)
            return Content("Oyun bulunamadı.");
        return View(game);
    }
   
    public IActionResult Bitis(int gameId)
    {
        var game = _gameService.GetGameWithList(gameId);
        if (game == null)
            return Content("Oyun bulunamadı.");
        return View(game);
    }
   
    // Bu sayfa cachelenmesin diye eklenmiş.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    //Hata sayfası için, genellikle uygulama çalışırken hata olursa otomatik açılır.
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
