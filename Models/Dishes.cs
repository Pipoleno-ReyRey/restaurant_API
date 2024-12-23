using RestaurantDishesAPI.Connection;
using MySql.Data.MySqlClient;

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
