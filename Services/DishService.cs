
using Microsoft.EntityFrameworkCore;

public class DishService : DataInteface
{
    private readonly RestaurantDB db;
    public DishService(RestaurantDB db) {
        this.db = db;
    }

    public async Task<object> Get(int id)
    {
        if (id <= 0)
        {
            return "that id doesnt exist";
        }
        else
        {
            try
            {
                var dish = await db.dish.FirstOrDefaultAsync(dish => dish.id == id);
                if (dish is null)
                {
                    return "that dish doesnt exist";
                }
                else
                {
                    return new DishDTO
                    {
                        name = dish.name,
                        description = dish.description,
                        ingredients = dish.ingredients,
                        type = dish.type.ToString(),
                        time = dish.time,
                        price = dish.price,
                        img = dish.img
                    };
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }
            
        }
    }

    public async Task<object> GetAll()
    {
        try
        {
            var data = await db.dish.ToListAsync();
            if (data is null)
            {
                return "theres not dishes";
            }
            else
            {
                var dishes = new List<DishDTO>();
                foreach (var dish in data)
                {
                    dishes.Add(new DishDTO
                    {
                        name = dish.name,
                        description = dish.description,
                        ingredients = dish.ingredients,
                        type = dish.type.ToString(),
                        time = dish.time,
                        price = dish.price,
                        img = dish.img
                    });
                }
                return dishes;
            }
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
            var dishes = await db.dish.ToListAsync();
            var dto = (DishDTO)data;
            dto.type = dto.type!.Replace(" ", "_").ToLower();
            string[] types = ["entrada", "plato_fuerte", "bebida", "postre"];
            if (dto is DishDTO && types.Any(t => t.Equals(dto.type)) && dishes.Any(dish => dish.name?.ToLower() == dto.name?.ToLower()) == false)
            {
                enum_data type;
                Enum.TryParse(dto.type, out type);
                var dish = new Dish
                {
                    name = dto.name!.ToLower(),
                    description = dto.description!.ToLower(),
                    type = type,
                    time = dto.time,
                    price = dto.price,
                    ingredients = dto.ingredients!.ToLower(),
                    img = dto.img
                };
                await db.dish.AddAsync(dish);
                await db.SaveChangesAsync();
                return dto;
            }
            else
            {
                return "dish not added";
            }
        }
        catch (Exception error)
        {
            return error.Message;
        }
    }

    public async Task<object> Update(int id, object data)
    {
        try
        {
            if (id <= 0)
            {
                return "that id doesnt valid";
            }
            else if (await db.dish.AnyAsync(d => d.id == id))
            {
                var dish = await db.dish.FirstOrDefaultAsync(d => d.id == id);
                string[] types = ["entrada", "plato fuerte", "bebida", "postre"];
                if (dish is null)
                {
                    return "id doesnt exist";
                }
                else if (data is DishDTO dto && types.Any(t => t == dto.type))
                {
                    dto.type?.Replace(" ", "_");
                    enum_data type;
                    Enum.TryParse(dto.type, out type);
                    dish.name = dto.name;
                    dish.description = dto.description;
                    dish.type = type;
                    dish.time = dto.time;
                    dish.price = dto.price;
                    dish.ingredients = dto.ingredients;
                    dish.img = dto.img;

                    await db.SaveChangesAsync();

                    return data;
                }
                else
                {
                    return "that not a dish";
                }
            }
            else
            {
                return "dish doesnt exist";
            }
        }
        catch (Exception error)
        {
            return error.Message;
        }
    }

    public async Task<object> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return "that id doesnt valid";
            }
            else if (db.dish.Any(d => d.id == id))
            {
                await db.dish.Where(d => d.id == id).ExecuteDeleteAsync();
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return "dish doesnt exist";
            }
        }
        catch (Exception error)
        {
            return error.Message;
        }
    }
}