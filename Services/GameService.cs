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
                .FirstOrDefault(g => g.Id == gameId);
        }
}
