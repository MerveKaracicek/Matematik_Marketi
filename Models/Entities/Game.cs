using System;
using System.Collections.Generic;

namespace Matematik_Marketi.Models.Entities
{
    public enum GameStatus
    {
        InProgress = 0,
        Won = 1,
        Lost = 2
    }

    public class Game
    {
        public int Id { get; set; }                  // Primary Key
        public int UserId { get; set; }              // Foreign Key     
        public int Lives { get; set; } = 3;          
        public GameStatus Status { get; set; } = GameStatus.InProgress;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User? User { get; set; }               // Relation: 1 Game → 1 User
        public ShoppingList? ShoppingList { get; set; } // 1 Game → 1 ShoppingList
        public ICollection<GameQuestion> GameQuestions { get; set; } = new List<GameQuestion>(); // 1 Game → N GameQuestion
    }
}
