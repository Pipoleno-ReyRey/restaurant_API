using RestaurantDishesAPI.Connection;
using MySql.Data;
using MySql.Data.MySqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;

namespace RestaurantDishesAPI.Models
{
    public class Dish
    {
        public int? Id { get; set; }
        public string? nameDish { get; set; }
        public string? descriptionDish { get; set; }
        public string? typeDish { get; set; }
        public string? ingredients { get; set; }
        public string? timeGetsReady { get; set; }
        public float? price { get; set; }
        public string? img { get; set; }

        public Dish(int? id, string? nameDish, string? descriptionDish, string? typeDish, string? ingredients, string? timeGetsReady, float? price, string? img)
        {
            Id = id;
            this.nameDish = nameDish;
            this.descriptionDish = descriptionDish;
            this.typeDish = typeDish;
            this.ingredients = ingredients;
            this.timeGetsReady = timeGetsReady;
            this.price = price;
            this.img = img;
        }
    }

    public class Dishes
    {
        public List<Dish> dishes = new List<Dish>();

        public async Task<List<Dish>> dishesGet()
        {
            ConnectionDatabase connection = new ConnectionDatabase();
            MySqlConnection connector = new MySqlConnection(connection.connection);
            await connector.OpenAsync();
            MySqlCommand command = new MySqlCommand("select * from Dish", connector);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = 0;
                Int32.TryParse(reader["id"].ToString(), out id);
                var nameDish = reader["nameDish"].ToString();
                var descriptionDish = reader["descriptionDish"].ToString();
                var typeDish = reader["typeDish"].ToString();
                var ingredients = reader["ingredients"].ToString();
                var timeGetsReady = reader["timeGetsReady"].ToString();
                float price = 0;
                float.TryParse(reader["price"].ToString(), out price);
                var img = reader["imgDish"].ToString();
                Dish dish = new Dish(id, nameDish, descriptionDish, typeDish, ingredients, timeGetsReady, price, img);
                dishes.Add(dish);
            }
            await connector.CloseAsync();

            return dishes;
        }

        public async Task<Dish> GetDish(int id)
        {
            Dish dish = new Dish(0, "", "", "", "", "", 0, "");
            ConnectionDatabase conn = new ConnectionDatabase();
            MySqlConnection connection = new MySqlConnection(conn.connection);
            await connection.OpenAsync();
            MySqlCommand command = new MySqlCommand($"select * from Dish where id = {id};", connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var ID = 0;
                Int32.TryParse(reader["id"].ToString(), out ID);
                var nameDish = reader["nameDish"].ToString();
                var descriptionDish = reader["descriptionDish"].ToString();
                var typeDish = reader["typeDish"].ToString();
                var ingredients = reader["ingredients"].ToString();
                var timeGetsReady = reader["timeGetsReady"].ToString();
                float price = 0;
                float.TryParse(reader["price"].ToString(), out price);
                var img = reader["imgDish"].ToString();
                dish = new Dish(ID, nameDish, descriptionDish, typeDish, ingredients, timeGetsReady, price, img);
            }
            return dish;
        }

        public async void AddDish(string? nameDish, string? descriptionDish, string? typeDish, string? ingredients, string? timeGetsReady, float? Price, string? img)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            MySqlConnection connection = new MySqlConnection(conn.connection);
            connection.Open();
            MySqlCommand command = new MySqlCommand($"insert into Dish(nameDish, descriptionDish, typeDish, ingredients, timeGetsReady, price, imgDish) values ('{nameDish + "."}', '{descriptionDish}', '{typeDish}', '{ingredients}', '{timeGetsReady}', '{Price}', '{img}')", connection);
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
    }

    public class Order
    {
        public int? id { get; set; }
        public string nameCustomer { get; set; }
        public float? count { get; set; }
        public DateTime date { get; set; }

        public Order(string nameCustomer) 
        { 
            this.nameCustomer = nameCustomer;
            this.count = 0.0f;
            this.date = DateTime.Today;
        }
    }

    public class Orders
    {
        List<Order> ordersList = new List<Order>();

        public async Task<Order> CreateOrder(string nameCustomer, float? count)
        {
            ConnectionDatabase connection = new ConnectionDatabase();
            DateTime date = DateTime.Today;
            MySqlConnection mySqlConnection = new MySqlConnection(connection.connection);
            Order order = new Order("");
            mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand($"insert into orderReceipt(idOrder, nameCustomer, dateOrder, count) values ({order.id}, '{order.nameCustomer}', NOW(), {order.count});", mySqlConnection);
            command.ExecuteNonQuery();
            mySqlConnection.Close();

            mySqlConnection.Open();
            MySqlCommand command2 = new MySqlCommand($"select * from orderReceipt where id = {order.id};", mySqlConnection);
            MySqlDataReader reader = command2.ExecuteReader();
            while (reader.Read())
            {
                order.id = Int32.Parse(reader["id"].ToString());
            }
            mySqlConnection.Close();

            return order;
        }

        public async Task<List<Order>> OrderGet()
        {
            Order order = new Order("");
            ConnectionDatabase connection = new ConnectionDatabase();
            MySqlConnection connector = new MySqlConnection(connection.connection);
            connector.Open();
            MySqlCommand command = new MySqlCommand("select * from orderReceipt;", connector);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = 0;
                Int32.TryParse(reader["id"].ToString(), out id);
                var nameCustomer = reader["nameCustomer"].ToString();
                DateTime date = DateTime.Parse(reader["dateOrder"].ToString());
                float? count = reader.IsDBNull(3) ? 0.0f: float.Parse(reader["count"].ToString());
                order = new Order(nameCustomer);
                order.id = id;
                order.nameCustomer = nameCustomer;
                order.date = date;
                order.count = count;
                ordersList.Add(order);

            }
            connector.Close();

            return ordersList;
        }
    }

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

        private async Task<float?> GetPriceOrder(string dishes)
        {
            float? count = 0;
            List<Dish> dishes1 = await GetOrderDishes();
            foreach (var dish in dishes1)
            {
                if (dishes.Contains(dish.nameDish.Substring(0, dish.nameDish.Length - 1).ToLower()))
                {
                    count += float.Parse(dish.price.ToString());
                }
            }
            return count;
        }

        private async Task<List<Dish>> GetDishesOrder(string dishes)
        {
            List<Dish> dishesOrder = new List<Dish>();
            List<Dish> dishes1 = await GetOrderDishes();
            foreach (var dish in dishes1)
            {
                if (dishes.Contains(dish.nameDish.Substring(0, dish.nameDish.Length - 1).ToLower()))
                {
                    dishesOrder.Add(dish);
                }
            }
            return dishesOrder;
        }
        public async Task<Order> GetOrderSpecific(int id, string dishes)
        {
            Order order = new Order("");
            ConnectionDatabase connection = new ConnectionDatabase();
            MySqlConnection mySqlConnection = new MySqlConnection(connection.connection);
            mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand($"select * from orderReceipt where id = {order.id};", mySqlConnection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var nameCustomer = reader["nameCustomer"].ToString();
                DateTime date = DateTime.Parse(reader["dateOrder"].ToString());
                float count = float.Parse(reader["count"].ToString());
                order = new Order(nameCustomer);
                order.id = id;
                order.count = await GetPriceOrder(dishes);
            }
            mySqlConnection.Close();

            mySqlConnection.Open();
            MySqlCommand mySqlCommand = new MySqlCommand($"update orderReceipt set count = {GetPriceOrder(dishes)} where id = {id};", mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return order;
        }

        public async Task<Order> CreateOrderElement(string? nameCustomer, string dishes)
        {
            Orders order = new Orders();
            return await order.CreateOrder(nameCustomer, await GetPriceOrder(dishes));
        }

        public async void CreateOrderUnionDish(string dishes, string nameCustomer)
        {
            int index = 0;
            Orders order = new Orders();
            Order orderElement = await order.CreateOrder(nameCustomer, await GetPriceOrder(dishes));
            OrderReceipt orderReceipt = new OrderReceipt();
            List<Dish> dishesList = await orderReceipt.GetDishesOrder(dishes);
            ConnectionDatabase connection = new ConnectionDatabase();
            MySqlConnection mySqlConnection = new MySqlConnection(connection.connection);           
            while (index < dishesList.Count)
            {
                mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand($"insert into orderReceipt_dishes(idOrderReceipt, idDish) values ({orderElement.id}, {dishesList[index].Id})", mySqlConnection);
                command.ExecuteNonQuery();
                mySqlConnection.Close();
                index++;
            }
        }
    }
}
