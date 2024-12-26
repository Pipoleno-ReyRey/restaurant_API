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
                Dish dish = new Dish();
                var ID = 0;
                Int32.TryParse(reader["id"].ToString(), out ID);
                dish.Id = ID;
                dish.nameDish = reader["nameDish"].ToString();
                dish.descriptionDish = reader["descriptionDish"].ToString();
                dish.typeDish = reader["typeDish"].ToString();
                dish.ingredients = reader["ingredients"].ToString();
                dish.timeGetsReady = reader["timeGetsReady"].ToString();
                float price = 0;
                float.TryParse(reader["price"].ToString(), out price);
                dish.price = price;
                dish.img = reader["imgDish"].ToString(); 
                dishes.Add(dish);
            }
            await connector.CloseAsync();

            return dishes;
        }

        public async Task<Dish> GetDish(int id)
        {
            Dish dish = new Dish();
            ConnectionDatabase conn = new ConnectionDatabase();
            MySqlConnection connection = new MySqlConnection(conn.connection);
            await connection.OpenAsync();
            MySqlCommand command = new MySqlCommand($"select * from Dish where id = {id};", connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var ID = 0;
                Int32.TryParse(reader["id"].ToString(), out ID);
                dish.Id = ID;
                dish.nameDish = reader["nameDish"].ToString();
                dish.descriptionDish = reader["descriptionDish"].ToString();
                dish.typeDish = reader["typeDish"].ToString();
                dish.ingredients = reader["ingredients"].ToString();
                dish.timeGetsReady = reader["timeGetsReady"].ToString();
                float price = 0;
                float.TryParse(reader["price"].ToString(), out price);
                dish.price = price;
                dish.img = reader["imgDish"].ToString();                
            }
            return dish;
        }

        public async void AddDish(Dish dish)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            MySqlConnection connection = new MySqlConnection(conn.connection);
            connection.Open();
            MySqlCommand command = new MySqlCommand($"insert into Dish(nameDish, descriptionDish, typeDish, ingredients, timeGetsReady, price, imgDish) values ('{dish.nameDish + "."}', '{dish.descriptionDish}', '{dish.typeDish}', '{dish.ingredients}', '{dish.timeGetsReady}', '{dish.price}', '{dish.img}')", connection);
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async void DeleteDish(int id)
        {
            ConnectionDatabase conn = new ConnectionDatabase();
            MySqlConnection connection = new MySqlConnection(conn.connection);
            connection.Open();
            MySqlCommand command = new MySqlCommand($"delete from Dish where id = {id}", connection);
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async void EditDish(int id, Dish dish){

            ConnectionDatabase conn = new ConnectionDatabase();
            MySqlConnection connection = new MySqlConnection(conn.connection);
            connection.Open();
            MySqlCommand command = new MySqlCommand($"update Dish set nameDish='{dish.nameDish}', descriptionDish='{dish.descriptionDish}', typeDish='{dish.typeDish}', ingredients='{dish.ingredients}', timeGetsReady='{dish.timeGetsReady}', price={dish.price}, imgDish='{dish.img}' where id = {id}", connection);
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
    }
