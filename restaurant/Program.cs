using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
var credentials = builder.Configuration.GetConnectionString("credentials");
builder.Services.AddDbContext<RestaurantDB>(opt => opt.UseSqlServer(credentials));
builder.Services.AddScoped<DishService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//dishes endpoints

//get dishes
app.MapGet("dish/all", async (DishService ds) =>
{
    var data = await ds.GetAll();
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok(data);
    }
});

//get dish
app.MapGet("dish/{name}", async (string name, DishService ds) =>
{
    var data = await ds.Get(name);
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok(data);
    }
});

//post dish
app.MapPost("dish/post", async (DishDTO dish, DishService ds) =>
{
    var data = await ds.Post(dish);
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok(data);
    }
});

//update dish
app.MapPut("dish/update/{name}", async (string name, DishDTO dish, DishService ds) =>
{
    var data = await ds.Update(name, dish);
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok(data);
    }
});

//delete dish
app.MapDelete("dish/delete/{name}", async (string name, DishService ds) =>
{
    var data = await ds.Delete(name);
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok("dish delete");
    }
});


//orders endpoints
app.MapGet("order/{id}", async (int id, OrderService service) =>
{
    var data = await service.Get(id);
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok(data);
    }
});

//post order
app.MapPost("order", async (OrderDTO order, OrderService service) =>
{
    var data = await service.Post(order);
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok(data);
    }
});

//delete order
app.MapDelete("order/{id}", async (int id, OrderService service) =>
{
    var data = await service.Delete(id);
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok(data);
    }
});

app.Run();
