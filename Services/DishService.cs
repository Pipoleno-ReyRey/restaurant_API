
using Microsoft.EntityFrameworkCore;

public class DishService : DataInteface
{
    private readonly RestaurantDB db;
    public DishService(RestaurantDB db) {
        this.db = db;
    }

    public async Task<object> Get(object name_id)
    {
        if (name_id is string && string.IsNullOrEmpty(name_id.ToString()))
        {
            return "that dish doesnt exist";
        }
        else
        {
            try
            {
                string name = name_id.ToString()!.Replace("_", " ").ToLower();
                var dish = await db.dish.FirstOrDefaultAsync(dish => dish.name == name);
                if (dish is null)
                {
                    return "that dish doesnt exist";
                }
                else
                {
                    string searchDishName = dish.name!.Replace(" ", "_").ToLower();
                    return new DishDTO
                    {
                        name = dish.name,
                        description = dish.description,
                        ingredients = dish.ingredients,
                        type = dish.type.ToString()!.Replace("_", " "),
                        time = dish.time,
                        price = dish.price,
                        img = dish.img,
                        endpoints = [$"UPDATE: dish/update/{searchDishName}", $"DELETE: dish/delete/{searchDishName}"]
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
            Dish[] data = await db.dish.ToArrayAsync();
            if (data is null)
            {
                return "theres not dishes";
            }
            else
            {
                var dishes = new List<DishDTO>();
                foreach (var dish in data)
                {
                    string searchDishName = dish.name!.Replace(" ", "_").ToLower();
                    dishes.Add(new DishDTO
                    {
                        name = dish.name,
                        description = dish.description,
                        ingredients = dish.ingredients,
                        type = dish.type.ToString()!.Replace("_", " "),
                        time = dish.time,
                        price = dish.price,
                        img = dish.img,
                        endpoints = [$"UPDATE: dish/update/{searchDishName}", $"DELETE: dish/delete/{searchDishName}"]
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
            dto.name = dto.name!.ToLower();
            dto.type = dto.type!.Replace(" ", "_").ToLower();
            string[] types = ["entrada", "plato_fuerte", "bebida", "postre"];

            if (dto is DishDTO && types.Any(t => t.Equals(dto.type)) && dishes.Any(dish => dish.name == dto.name) == false)
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
                dto.endpoints = [$"GET: dish/{dto.name.Replace(" ", "_").ToLower()}", $"UPDATE: dish/update/{dto.name.Replace(" ", "_").ToLower()}", $"DELETE: /dish/delete/{dto.name.Replace(" ", "_").ToLower()}"];
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

    public async Task<object> Update(object name_id, object data)
    {
        try
        {
            if (name_id is string && string.IsNullOrEmpty(name_id.ToString()))
            {
                return "that name doesnt valid";
            }
            else if (await db.dish.AnyAsync(d => d.name == name_id.ToString()!.Replace("_", " ").ToLower()))
            {
                string name = name_id.ToString()!.Replace("_", " ").ToLower();
                var dish = await db.dish.FirstOrDefaultAsync(d => d.name == name);
                string[] types = ["entrada", "plato fuerte", "bebida", "postre"];
                if (dish is null)
                {
                    return "dish doesnt exist";
                }
                else if (data is DishDTO dto && types.Any(t => t == dto.type))
                {
                    dto.type = dto.type?.Replace(" ", "_");
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

                    dto.endpoints = [$"GET: dish/{dto.name?.Replace(" ", "_").ToLower()}", $"UPDATE: dish/update/{dto.name?.Replace(" ", "_").ToLower()}", $"DELETE: /dish/delete/{dto.name?.Replace(" ", "_").ToLower()}"];
                    return dto;
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

    public async Task<object> Delete(object name_id)
    {
        try
        {
            if (string.IsNullOrEmpty(name_id.ToString()))
            {
                return "the name cant be empty";
            }
            else if (db.dish.Any(d => d.name == name_id.ToString()!.Replace("_", " ").ToLower()))
            {
                string name = name_id.ToString()!.Replace("_", " ").ToLower();
                int? idDish = (await db.dish.FirstOrDefaultAsync(d => d.name == name))?.id;
                await db.ordersDishes.Where(od => od.dishId == idDish).ExecuteDeleteAsync();
                db.SaveChanges();
                await db.dish.Where(d => d.name == name).ExecuteDeleteAsync();
                db.SaveChanges();
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