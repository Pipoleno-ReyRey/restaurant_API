
using Humanizer;
using Microsoft.EntityFrameworkCore;

public class OrderService : DataInteface
{
    private readonly RestaurantDB db;
    public OrderService(RestaurantDB restaurantDB)
    {
        db = restaurantDB;
    }

    public async Task<object> Get(object name_id)
    {
        try
        {
            if (name_id is int && int.Parse(name_id.ToString()!) <= 0 || name_id is string)
            {
                return "id isnt valid";
            }
            else
            {
                int id = int.Parse(name_id.ToString()!);
                if (db.orders.Any(o => o.id == id) == false)
                {
                    return "order dont exist";
                }
                else
                {
                    OrderDish[] data = await db.ordersDishes.Where(od => od.orderId == id).ToArrayAsync();
                    List<Dish> dishes = new List<Dish>();
                    foreach (OrderDish id_ in data)
                    {
                        Dish? item = await db.dish.FirstOrDefaultAsync(o => o.id == id_.dishId);
                        dishes.Add(item!);
                    }
                    List<DishDTO>? dishesOrder = new List<DishDTO>();
                    foreach (OrderDish dish in data)
                    {
                        Dish? x = dishes.Where(d => d.id == dish.dishId).FirstOrDefault();
                        dishesOrder.Add(new DishDTO
                        {
                            name = x!.name,
                            description = x.description,
                            type = x.type.ToString()!.Replace("_", " "),
                            ingredients = x.ingredients,
                            time = x.time,
                            price = x.price,
                            img = Convert.ToBase64String(x.img!),
                            amount = dish.amount
                        });
                    }
                    var order = await db.orders.FirstOrDefaultAsync(o => o.id == data[0].orderId);
                    return new OrderResponse
                    {
                        id = order!.id,
                        name = order.name,
                        dishes = dishesOrder,
                        count = order.count,
                        date = order.date
                    };
                }
            }
        }
        catch (Exception error)
        {
            return error.Message;
        }
    }

    public async Task<object> GetAll()
    {
        try
        {
            return await db.orders.ToListAsync();
        }
        catch (Exception error)
        {
            return error.Message;
        }
    }

    public async Task<object> Post(object data)
    {
        try
        {
            if (data is OrderDTO dto && dto.dishes != null)
            {
                Order order = new Order();
                order.name = dto.name;
                order.date = DateTime.Now.ToUniversalTime();
                order.count = 0;
                foreach (DishDTO dish in dto.dishes)
                {
                    order.count += dish.amount * dish.price;
                }
                await db.orders.AddAsync(order);
                db.SaveChanges();

                List<OrderDish> orderDish = new List<OrderDish>();

                foreach (DishDTO dish in dto.dishes)
                {
                    int? id = (await db.dish.FirstOrDefaultAsync(d => d.name!.ToLower() == dish.name!.ToLower()))!.id;
                    orderDish.Add(new OrderDish
                    {
                        orderId = order.id,
                        amount = dish.amount,
                        dishId = id
                    });
                }
                await db.ordersDishes.AddRangeAsync(orderDish);
                db.SaveChanges();
                return order;
            }
            else
            {
                return "not order added";
            }
        }
        catch (Exception error)
        {
            return error.Message;
        }
    }

    public Task<object> Update(object name_id, object data)
    {
        throw new NotImplementedException();
    }

    public async Task<object> Delete(object name_id)
    {
        if (name_id is int && int.Parse(name_id.ToString()!) <= 0 || name_id is string)
        {
            return "id isnt valid";
        }
        else
        {
            try
            {
                int id = int.Parse(name_id.ToString()!);
                if (await db.ordersDishes.AnyAsync(d => d.orderId == id))
                {
                    var data = await db.orders.FirstOrDefaultAsync(od => od.id == id);
                    await db.ordersDishes.Where(od => od.orderId == id).ExecuteDeleteAsync();
                    await db.orders.Where(o => o.id == id).ExecuteDeleteAsync();
                    await db.SaveChangesAsync();
                    return "deleted";
                }
                else
                {
                    return "dish not deleted";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }
    }
}
