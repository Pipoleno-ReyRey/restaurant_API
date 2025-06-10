using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

public class Dish
{
    [Key]
    public int? id { get; set; }
    [Required]
    [StringLength(maximumLength: 250)]
    public string? name { get; set; }
    [Required]
    public string? description { get; set; }
    [Required]
    [StringLength(maximumLength: 100)]
    public enum_data? type { get; set; }
    [Required]
    [StringLength(maximumLength: 300)]
    public string? ingredients { get; set; }
    [Required]
    public TimeOnly? time { get; set; }
    [Required]
    public float? price { get; set; }
    [Required]
    public string? img { get; set; }
}