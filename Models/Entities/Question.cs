using System.Collections.Generic;
namespace Matematik_Marketi.Models.Entities
{
    public class Question
    {
        public int Id { get; set; }                  // Primary Key
        public string QuestionText { get; set; } = string.Empty; // Soru metni
        public string CorrectAnswer { get; set; } = string.Empty; // Doğru cevap

        // Navigation properties
        public ICollection<GameQuestion> GameQuestions { get; set; } = new List<GameQuestion>();
    }
}
