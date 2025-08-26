using System.Collections.Generic;

namespace Matematik_Marketi.Models.Entities
{
    public class ShoppingList
    {
        public int Id { get; set; }          // Primary Key
        public int GameId { get; set; }      // Foreign Key → 1 oyun = 1 alışveriş listesi

        // Navigation properties
        public Game? Game { get; set; }       // İlişki: 1 ShoppingList → 1 Game
        public ICollection<ShoppingListItem> Items { get; set; } = new List<ShoppingListItem>(); // Liste içindeki ürünler
    }
}
