using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantDishesAPI.Models;

namespace RestaurantDishesAPI.FoodController
{ 
    [ApiController]
    [Route("api")]
    public class FoodController : ControllerBase
    {
        [HttpGet("getFood")]
        public async Task<ActionResult<List<Dish>>> GetDishes()
        {
            Dishes dishes = new Dishes();
            List<Dish> listDishes = await dishes.dishesGet();
            return listDishes;
        }

        [HttpGet("getDish/{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            Dishes dishes = new Dishes();
            Dish dish = await dishes.GetDish(id);
            return dish;
        }

        [HttpPost("postDish")]
        public async void PostDish([FromBody] Dish dish)
        {
            Dishes dishes = new Dishes();
            dishes.AddDish(dish);
        }

        [HttpPut("editDish/{id}")]
        public async Task<string> EditDish(int id, [FromBody] Dish dish){
            Dishes dishes = new Dishes();
            dishes.EditDish(id, dish);
            return "edited";
        }

        [HttpDelete("deleteDish/{id}")]
        public async Task<string> DeleteDish(int id){
            Dishes dishes = new Dishes();
            dishes.DeleteDish(id);
            return "deleted";
        }
    }
}
