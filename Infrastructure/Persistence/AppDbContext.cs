using Microsoft.EntityFrameworkCore;
using webApi.Domain.Entity;

namespace webApi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<DetailOrder> DetailOrders { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<TypeBook> TypeBooks { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasOne(a => a.User)
                  .WithOne(u => u.Account)
                  .HasForeignKey<User>("AccountId");

            entity.HasOne(a => a.ShoppingCart)
                  .WithOne(s => s.Account)
                  .HasForeignKey<ShoppingCart>("AccountId");

            entity.HasMany(a => a.Roles)
                  .WithMany(r => r.Accounts)
                  .UsingEntity(j => j.ToTable("AccountRole"));

            entity.HasMany(a => a.Orders)
                  .WithOne(o => o.Account)
                  .HasForeignKey("AccountId");

            entity.HasMany(a => a.Reviews)
                  .WithOne(r => r.Account)
                  .HasForeignKey("AccountId");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasOne(b => b.TypeBook)
                  .WithMany(t => t.Books)
                  .HasForeignKey("type_book_Id");

            entity.HasMany(b => b.Images)
                  .WithOne(i => i.Book)
                  .HasForeignKey("book_Id");

            entity.HasMany(b => b.Reviews)
                  .WithOne(r => r.Book)
                  .HasForeignKey("book_Id");

            entity.HasMany(b => b.ShoppingCarts)
                  .WithMany(s => s.Books)
                  .UsingEntity(j => j.ToTable("BookShoppingCart"));
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(o => o.Discount)
                  .WithMany(d => d.Orders)
                  .HasForeignKey("discount_id");
        });

        modelBuilder.Entity<DetailOrder>(entity =>
        {
            entity.HasOne(do_ => do_.Order)
                  .WithMany(o => o.DetailOrder)
                  .HasForeignKey("order_id");

            entity.HasMany(do_ => do_.Books)
                  .WithMany()
                  .UsingEntity(j => j.ToTable("DetailOrderBook"));
        });
    }
}