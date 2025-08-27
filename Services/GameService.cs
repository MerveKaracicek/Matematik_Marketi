using Matematik_Marketi.Models.Entities;
using Matematik_Marketi.Data;

namespace Matematik_Marketi.Services;

public class GameService
{
    private readonly AppDbContext _context;

    public GameService(AppDbContext context)
    {
        _context = context;
    }

    // Yeni oyun başlat
    public Game StartGame(int userId)
    {
        var game = new Game
        {
            Lives = 3,
            Status = GameStatus.InProgress,
            CreatedAt = DateTime.Now,
            UserId = userId
        };

        _context.Games.Add(game);
        _context.SaveChanges();

        return game;
    }

    // Oyun bul
    public Game? GetGameById(int gameId)
    {
        return _context.Games.Find(gameId);
    }

    // Daha sonra alışveriş listesi ve soru işlemleri de buraya eklenebilir
}
