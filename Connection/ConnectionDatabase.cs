namespace RestaurantDishesAPI.Connection
{
    public class ConnectionDatabase
    {
        public string? connection = string.Empty;

        public ConnectionDatabase()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            connection = builder.GetSection("ConnectionStrings:connectionDatabase").Value;
        }
    }
}
