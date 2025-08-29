using Matematik_Marketi.Models.Entities;
using Matematik_Marketi.Data;
using Microsoft.EntityFrameworkCore;
using System;
namespace Matematik_Marketi.Services;

public class GameService
{
    private readonly AppDbContext _context;
    private readonly Random _random = new Random();

    public GameService(AppDbContext context)
    {
        _context = context;
    }

    // Yeni oyun başlat --> Game+ShoppingList+ShoppingListItems
    public Game StartGame(int userId)
    {
        // Game nesnesi oluştur
        var game = new Game
        {
            Lives = 3,
            Status = GameStatus.InProgress,
            CreatedAt = DateTime.Now,
            UserId = userId
        };

        //ShoppingList oluştur
        var shoppingList = new ShoppingList();
        game.ShoppingList = shoppingList;



        _context.Games.Add(game);
        _context.SaveChanges();

        // Rastgele Product seçimi
        var allProductIds = _context.Products.Select(p => p.Id).ToList(); // Tüm ürün ID'lerini al

        if (allProductIds.Count < 3)  //3ten az ürün varsa hata fırlat
        {
            throw new InvalidOperationException("Yeterli ürün yok.Product tablosunda en az 3 ürün olmalı.");
        }

        int upperExclusive = Math.Min(6, allProductIds.Count) + 1;
        int count = _random.Next(3, upperExclusive);  // 3 ile 6 arasında rastgele bir sayı seç

        var rng = new Random();      // Listeyi karıştır
        for (int i = allProductIds.Count - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            int temp = allProductIds[i];
            allProductIds[i] = allProductIds[j];
            allProductIds[j] = temp;
        }

        var selectedIds = allProductIds.Take(count).ToList(); // İlk 'count' kadar ürünü seç

        var items = new List<ShoppingListItem>(); // ShoppingListItem nesnelerini oluştur
        foreach (var id in selectedIds)
        {
            items.Add(new ShoppingListItem
            {
                ShoppingListId = shoppingList.Id,
                ProductId = id,
                IsCollected = false
            });
        }

        _context.ShoppingListItems.AddRange(items);
        _context.SaveChanges();


        return game;
    }
    // Oyun için soruları ata
    public void AssignQuestionsToGame(int gameId)
    {
        // Oyun ve alışveriş listesini yükle
        var game = _context.Games
            .Include(g => g.ShoppingList)
                .ThenInclude(sl => sl.Items)
            .FirstOrDefault(g => g.Id == gameId);

        if (game == null || game.ShoppingList?.Items == null) return;

        // Tüm soruları al
        var allQuestions = _context.Questions.ToList();
        var random = new Random();
        var gameQuestions = new List<GameQuestion>();

        // Her ürün için rastgele bir soru ata, tekrarı önle
        foreach (var item in game.ShoppingList.Items)
        {
            if (allQuestions.Count == 0)
                break; // Soru kalmadıysa dur

            int index = random.Next(allQuestions.Count);
            var question = allQuestions[index];

            // Seçilen soruyu listeden çıkar, tekrarlanmasın
            allQuestions.RemoveAt(index);

            gameQuestions.Add(new GameQuestion
            {
                GameId = game.Id,
                ProductId = item.ProductId,
                QuestionId = question.Id,
                IsAnswered = false,
                IsCorrect = false
            });
        }

        _context.GameQuestions.AddRange(gameQuestions);
        _context.SaveChanges();
    }

    public bool CheckAnswer(int gameId, int productId, string userAnswer, out bool isCorrect)
    {
        var gameQuestion = _context.GameQuestions
            .Include(gq => gq.Question)
            .Include(gq => gq.Game)
                .ThenInclude(g => g.ShoppingList)
            .Include(gq => gq.Product)
            .FirstOrDefault(gq => gq.GameId == gameId && gq.ProductId == productId);



        if (gameQuestion == null)
        {
            isCorrect = false;
            return false; // soru bulunamadı
        }

        gameQuestion.IsAnswered = true;

        if (gameQuestion.Question.CorrectAnswer == userAnswer)
        {
            gameQuestion.IsCorrect = true;

            var shoppingItem = _context.ShoppingListItems
                .FirstOrDefault(sli => sli.ShoppingListId == gameQuestion.Game.ShoppingList.Id
                                       && sli.ProductId == productId);
            if (shoppingItem != null)
                shoppingItem.IsCollected = true;

            isCorrect = true;
        }
        else
        {
            gameQuestion.IsCorrect = false;
            gameQuestion.Game.Lives--;

            isCorrect = false;
        }

        // En sonda tek sefer çağır
        _context.SaveChanges();

        return true;
    }






    // Oyun bul
    public Game? GetGameById(int gameId)
    {
        return _context.Games.Find(gameId);
    }

    public Game? GetGameWithList(int gameId)  //Oyunla alaklı alışveriş listesini ve ürünleri getir
    {
        return _context.Games
            .Include(g => g.ShoppingList)
                .ThenInclude(sl => sl.Items)
                    .ThenInclude(i => i.Product)
            .Include(g => g.GameQuestions)
                .ThenInclude(gq => gq.Question)
            .FirstOrDefault(g => g.Id == gameId);
    }

    public GameQuestion? GetGameQuestion(int gameId, int productId)
    {
        return _context.GameQuestions
            .Include(gq => gq.Question)
            .FirstOrDefault(gq => gq.GameId == gameId && gq.ProductId == productId);
    }

    public GameQuestion? GetRandomQuestionForProduct(int gameId, int productId)
    {
        return _context.GameQuestions
                       .Include(gq => gq.Question)
                       .Where(gq => gq.GameId == gameId && gq.ProductId == productId && !gq.IsAnswered)
                       .OrderBy(q => Guid.NewGuid()) // rastgele
                       .FirstOrDefault();
    }
    // Oyun durumunu kontrol et
    public GameStatus CheckGameStatus(int gameId)
    {
        var game = _context.Games
            .Include(g => g.ShoppingList)
                .ThenInclude(sl => sl.Items)
            .FirstOrDefault(g => g.Id == gameId);

        if (game == null) return GameStatus.InProgress;

        bool allCollected = game.ShoppingList.Items.All(i => i.IsCollected);
        bool hasLives = game.Lives > 0;

        if (allCollected && hasLives)
        {
            game.Status = GameStatus.Won; // Enum kullanımı
            _context.SaveChanges();
            return GameStatus.Won;
        }
        else if (!allCollected && !hasLives)
        {
            game.Status = GameStatus.Lost; // Enum kullanımı
            _context.SaveChanges();
            return GameStatus.Lost;
        }

        return GameStatus.InProgress; // Oyun devam ediyor
    }
}


