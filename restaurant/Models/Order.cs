using System.ComponentModel.DataAnnotations;

public class Order
    {
        [Key]
        public int? id { get; set; }
        [Required]
        [StringLength(maximumLength: 250)]
        public string? name { get; set; }
        [Required]
        public float? count { get; set; }
        [Required]
        public DateTime? date { get; set; }
    }