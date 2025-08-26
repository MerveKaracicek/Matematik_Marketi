namespace Matematik_Marketi.Models.Entities
{
    public class ShoppingListItem
    {
        public int Id { get; set; }                
        public int ShoppingListId { get; set; }    
        public int ProductId { get; set; }         
        public bool IsCollected { get; set; } = false; // Ürün toplandı mı?

        // Navigation properties
        public ShoppingList? ShoppingList { get; set; } 
        public Product? Product { get; set; }           
    }
}
