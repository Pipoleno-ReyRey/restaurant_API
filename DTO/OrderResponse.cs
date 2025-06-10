public class OrderResponse
{
    public int? id { get; set; }
    public string? name { get; set; }
    public List<DishDTO>? dishes { get; set; }
    public float? count { get; set; }
    public DateTime? date { get; set; }
}