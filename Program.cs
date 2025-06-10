using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
var credentials = builder.Configuration.GetConnectionString("credentials");
builder.Services.AddDbContext<RestaurantDB>(opt => opt.UseNpgsql(credentials, o => o.MapEnum<enum_data>("enum_data")));
builder.Services.AddScoped<DishService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//dishes endpoints
app.MapGet("dish/get_all", async (DishService ds) =>
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

app.MapGet("dish/{id}", async (int id, DishService ds) =>
{
    var data = await ds.Get(id);
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok(data);
    }
});

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

app.MapPut("dish/update/{id}", async (int id, DishDTO dish, DishService ds) =>
{
    var data = await ds.Update(id, dish);
    if (data is string)
    {
        return Results.BadRequest(data);
    }
    else
    {
        return Results.Ok(data);
    }
});

app.MapDelete("dish/delete/{id}", async (int id, DishService ds) =>
{
    var data = await ds.Delete(id);
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
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
