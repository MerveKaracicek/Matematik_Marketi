using System;
using System.Collections.Generic;
namespace Matematik_Marketi.Models.Entities
{
    public class GameQuestion
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int ProductId { get; set; }
        public int QuestionId { get; set; }
        public bool IsAnswered { get; set; } = false; // Soruyu cevapladı mı
        public bool IsCorrect { get; set; } = false;  // Doğru cevapladı mı

        // Navigation properties
        public Game? Game { get; set; }
        public Product? Product { get; set; }
        public Question? Question { get; set; }
    }
}
