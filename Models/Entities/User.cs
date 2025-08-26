using System.Collections.Generic;
namespace Matematik_Marketi.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty; //for prevent null error
        
        // Navigation properties
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}