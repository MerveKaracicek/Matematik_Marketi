using Microsoft.EntityFrameworkCore;
using Matematik_Marketi.Models.Entities;
namespace Matematik_Marketi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
        public DbSet<GameQuestion> GameQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //1 Game - 1 ShoppingList
            modelBuilder.Entity<Game>()
                .HasOne(g => g.ShoppingList)
                .WithOne(sl => sl.Game)
                .HasForeignKey<ShoppingList>(sl => sl.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            //1 User - N Game
            modelBuilder.Entity<Game>()
                .HasOne(g => g.User)
                .WithMany(u => u.Games)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            //1 Game → N GameQuestion 
            modelBuilder.Entity<GameQuestion>()
                .HasOne(gq => gq.Game)
                .WithMany(g => g.GameQuestions)
                .HasForeignKey(gq => gq.GameId)
                .OnDelete(DeleteBehavior.Cascade); // Oyun silinirse sorular da silinsin


            // 1 Question → N GameQuestion
            modelBuilder.Entity<GameQuestion>()
                .HasOne(gq => gq.Question)
                .WithMany(q => q.GameQuestions)
                .HasForeignKey(gq => gq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Soru silinirse oyunlar silinmez

            //1 ShoppingList - N ShoppingListItem
            modelBuilder.Entity<ShoppingListItem>()
                .HasOne(sli => sli.ShoppingList)
                .WithMany(sl => sl.Items)
                .HasForeignKey(sli => sli.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade); 

            //1 Product - N ShoppingListItem
            modelBuilder.Entity<ShoppingListItem>()
                .HasOne(sli => sli.Product)
                .WithMany()  // Product tarafında collection yok, opsiyonel
                .HasForeignKey(sli => sli.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Product silinirse itemlar silinmez

            //1 Product - N GameQuestion
            modelBuilder.Entity<GameQuestion>()
                .HasOne(gq => gq.Product)
                .WithMany()
                .HasForeignKey(gq => gq.ProductId)
                .OnDelete(DeleteBehavior.Restrict); 
                
            // Unique constraint on Product Name
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();




            

        }
    }
}
