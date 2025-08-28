using Microsoft.AspNetCore.Mvc;
using Matematik_Marketi.Models.Entities;
using Matematik_Marketi.Services;

namespace Matematik_Marketi.Controllers;


public class GameController : Controller
{
    private readonly GameService _gameService;

    public GameController(GameService gameService)
    {
        _gameService = gameService;
    }

    public IActionResult Start()
    {
        int userId = 1; // şimdilik sabit, daha sonra oturumdan alınacak
        var game = _gameService.StartGame(userId);

        return RedirectToAction("Play", new { gameId = game.Id });
    }

    public IActionResult Play(int gameId)
    {
        var game = _gameService.GetGameWithList(gameId);
        if (game == null) return NotFound();

        return View("~/Views/Home/Oyun.cshtml", game);
    }
    [HttpPost]
    public IActionResult StartGame()
    {
        int userId = 1;
        var game = _gameService.StartGame(userId);
        return Json(new { gameId = game.Id });
    }

public IActionResult AlisverisListesi(int gameId)
{
    var game = _gameService.GetGameWithList(gameId);
    if(game == null || game.ShoppingList == null)
        return NotFound(); // null gelirse sayfa bulunamadı

    return View("~/Views/Home/AlisverisListesi.cshtml", game.ShoppingList);
}





}
