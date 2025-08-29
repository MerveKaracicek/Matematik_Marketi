using Microsoft.AspNetCore.Mvc;
using Matematik_Marketi.Models.Entities;
using Matematik_Marketi.Services;
using Matematik_Marketi.Models.Dto;

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
        _gameService.AssignQuestionsToGame(game.Id);

        return Json(new { gameId = game.Id });
    }

    public IActionResult AlisverisListesi(int gameId)
    {
        var game = _gameService.GetGameWithList(gameId);
        if (game == null || game.ShoppingList == null)
            return NotFound(); // null gelirse sayfa bulunamadı

        return View("~/Views/Home/AlisverisListesi.cshtml", game.ShoppingList);
    }

    public IActionResult Soru(int gameId, int productId)
    {
        var gameQuestion = _gameService.GetGameQuestion(gameId, productId);
        if (gameQuestion == null)
            return Content("Bu ürün için soru bulunamadı.");

        if (gameQuestion == null)
            return RedirectToAction("Play", new { gameId = gameId });


        return View("~/Views/Home/Soru.cshtml", gameQuestion);
    }


    [HttpGet]
    public IActionResult GetQuestionForProduct(int gameId, int productId)
    {
        var gameQuestion = _gameService.GetRandomQuestionForProduct(gameId, productId);
        if (gameQuestion == null)
            return NotFound();

        return Json(new { questionId = gameQuestion.QuestionId });
    }

    

[HttpPost]
public IActionResult CevapKontrol([FromBody] AnswerDto dto)
{
    bool isCorrect;
    bool isAnswered = _gameService.CheckAnswer(dto.GameId, dto.ProductId, dto.UserAnswer, out isCorrect);

    if(isAnswered)
    {
        return Json(new { success = true, correct = isCorrect, productId = dto.ProductId });
    }
    else
    {
        return Json(new { success = false, message = "Soru işleme alınamadı." });
    }
}



}
