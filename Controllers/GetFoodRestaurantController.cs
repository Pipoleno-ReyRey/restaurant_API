using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantDishesAPI.Models;
namespace RestaurantDishesAPI.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class GetFoodRestaurantController : ControllerBase
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
            dishes.AddDish(dish.nameDish, dish.descriptionDish, dish.typeDish, dish.ingredients, dish.timeGetsReady, dish.price, dish.img);
        }

        [HttpPost("postOrder/{nameCustomer}+{dishes}")]
        public async void postOrder(string nameCustomer, string dishes)
        {
            OrderReceipt orderReceipt = new OrderReceipt();
            orderReceipt.CreateOrderUnionDish(nameCustomer, dishes);
        }

        [HttpGet("getOrders")]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            Orders orders = new Orders();
            List<Order> ordersList = await orders.OrderGet();
            return ordersList;
        }
    }
}
