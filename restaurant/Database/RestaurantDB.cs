using Microsoft.EntityFrameworkCore;

public class RestaurantDB : DbContext
{
    public DbSet<Dish> dish { get; set; }
    public DbSet<Order> orders { get; set; }
    public DbSet<OrderDish> ordersDishes { get; set; }
    public RestaurantDB(DbContextOptions opt) : base(opt) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OrderDish>().HasKey(od => new { od.dishId, od.orderId });
    }
}