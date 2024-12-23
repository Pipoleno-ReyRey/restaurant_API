using MySql.Data.MySqlClient;
using RestaurantDishesAPI.Connection;
using RestaurantDishesAPI.Models;

public class Orders
    {
        List<Order> ordersList = new List<Order>();

        public async Task<Order> CreateOrder(string nameCustomer, string dishes)
        {             
            float count = await new OrderReceipt().GetPriceOrder(dishes);
            ConnectionDatabase connection = new ConnectionDatabase();
            MySqlConnection mySqlConnection = new MySqlConnection(connection.connection);
            Order order = new Order();
            mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand($"insert into orderReceipt(nameCustomer, dateOrder, count) values ('{nameCustomer}', NOW(), {count});", mySqlConnection);
            command.ExecuteNonQuery();
            mySqlConnection.Close();

            return order;
        }

        public async Task<List<Order>> OrderGet()
        {
            Order order = new Order();
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
                order = new Order();
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