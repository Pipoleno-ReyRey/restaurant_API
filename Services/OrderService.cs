
using Microsoft.EntityFrameworkCore;


public class OrderService : DataInteface
{
    private readonly RestaurantDB db;
    public OrderService(RestaurantDB restaurantDB)
    {
        db = restaurantDB;
    }

    public async Task<object> Get(int id)
    {
        try
        {
            if (id <= 0)
            {
                return "id not valid";
            }
            else
            {
                if (db.orders.Any(o => o.id == id) == false)
                {
                    return "order dont exist";
                }
                else
                {
                    OrderDish[] data = await db.ordersDishes.Where(od => od.orderId == id).ToArrayAsync();
                    Dish[] dishes = await db.dish.ToArrayAsync();
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
                            img = x.img,
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
                await db.SaveChangesAsync();

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
                await db.SaveChangesAsync();
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

    public Task<object> Update(int id, object data)
    {
        throw new NotImplementedException();
    }

    public async Task<object> Delete(int id)
    {
        if (id <= 0)
        {
            return "id dont valid";
        }
        else
        {
            try
            {
                if (await db.ordersDishes.AnyAsync(d => d.orderId == id))
                {
                    var data = await db.orders.FirstOrDefaultAsync(od => od.id == id);
                    await db.ordersDishes.Where(od => od.orderId == id).ExecuteDeleteAsync();
                    await db.orders.Where(o => o.id == id).ExecuteDeleteAsync();
                    await db.SaveChangesAsync();
                    return data!;
                }
                else
                {
                    return "not deleted";
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
        }
    }
}
