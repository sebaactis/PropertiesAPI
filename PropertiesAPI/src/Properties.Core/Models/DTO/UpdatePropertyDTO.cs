using System.ComponentModel.DataAnnotations;

namespace Properties.Core.Models.DTO
{
    public class UpdatePropertyDTO
    {
        [MinLength(10, ErrorMessage = "The name must be at least 10 characters")]
        [MaxLength(100, ErrorMessage = "The name must be at most 100 characters")]
        public string? Name { get; set; }

        [MinLength(30, ErrorMessage = "The description must be at least 30 characters")]
        [MaxLength(250, ErrorMessage = "The description must be at most 250 characters")]
        public string? Description { get; set; }

        [MinLength(10, ErrorMessage = "The address must be at least 10 characters")]
        [MaxLength(40, ErrorMessage = "The address must be at most 40 characters")]
        public string? Address { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than 0")]
        public int? Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The number of bedrooms must be at least 1")]
        public int? Bedrooms { get; set; }

        public bool? IsAvailable { get; set; }
    }
}
