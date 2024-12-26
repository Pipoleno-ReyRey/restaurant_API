using Microsoft.AspNetCore.Mvc;
using RestaurantDishesAPI.Models;

namespace RestaurantDishesAPI.OrderController{
    [ApiController]
    [Route("api")]
    public class OrderController: ControllerBase{
        
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

