using Microsoft.EntityFrameworkCore;
using Matematik_Marketi.Data;
using Matematik_Marketi.Models.Entities;
using Matematik_Marketi.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<GameService>();

var app = builder.Build();

// Seed Data - ürünleri otomatik ekle
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Products.Any())
    {
        var products = new List<Product>
        {
            new Product { Name = "Süt" },
            new Product { Name = "Peynir" },
            new Product { Name = "Yumurta" },
            new Product { Name = "Domates" },
            new Product { Name = "Havuç" },
            new Product { Name = "Salatalık" },
            new Product { Name = "Diş Macunu" },
            new Product { Name = "Sabun" },
            new Product { Name = "Deterjan" },
            new Product { Name = "Çilek" },
            new Product { Name = "Muz" },
            new Product { Name = "Armut" }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}

// Seed Questions
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    QuestionSeeder.SeedQuestions(dbContext);
}




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
