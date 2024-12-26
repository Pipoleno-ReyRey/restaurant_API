using RestaurantDishesAPI.Connection;
using MySql.Data.MySqlClient;

namespace RestaurantDishesAPI.Models
{

    internal class OrderReceipt
    {
        public int idCustomer { get; set; }
        public int idDish { get; set; }

        private async Task<List<Dish>> GetOrderDishes()
        {
            Dishes dishes = new Dishes();
            List<Dish> dishesList = await dishes.dishesGet();
            return dishesList;
        }

        public async Task<float> GetPriceOrder(string dishes)
        {
            float count = 0;
            var dishes1 = await GetOrderDishes();
            foreach (var dish in dishes1)
            {
                if(dishes.ToLower().Contains(dish.nameDish.Substring(0, dish.nameDish.Length - 1).ToLower())){
                    count += float.Parse(dish.price.ToString());
                }
                
            }
            return count;
        }

        public async Task<Order> CreateOrderElement(string? nameCustomer, string dishes)
        {
            Orders order = new Orders();
            return await order.CreateOrder(nameCustomer, "await GetPriceOrder(dishes)");
        }

        public async void CreateOrderUnionDish(string nameCustomer, string dishes)
        {
            await new Orders().CreateOrder(nameCustomer, dishes);
            List<int?> idDishes = new List<int?>();
            var dishes1 = await GetOrderDishes();
            foreach (var dish in dishes1)
            {
                if(dishes.ToLower().Contains(dish.nameDish.Substring(0, dish.nameDish.Length - 1).ToLower())){
                    idDishes.Add(dish.Id);
                }   
            }

            int idOrder = 0;
            ConnectionDatabase connection = new ConnectionDatabase();
            MySqlConnection connector = new MySqlConnection(connection.connection);
            connector.Open();
            MySqlCommand command = new MySqlCommand("SELECT id FROM orderReceipt ORDER BY id DESC LIMIT 1;", connector);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                idOrder = Int32.Parse(reader["id"].ToString());
            }
            connector.Close();

            foreach(int idDish in idDishes){
                connector.Open();
                MySqlCommand command2 = new MySqlCommand($"insert into orderReceipt_dishes(idOrderReceipt, idDish) values({idOrder}, {idDish});", connector);
                command2.ExecuteNonQuery();
                connector.Close();
            }
        }
    }
}
