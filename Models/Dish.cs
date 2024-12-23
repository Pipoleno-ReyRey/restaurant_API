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
