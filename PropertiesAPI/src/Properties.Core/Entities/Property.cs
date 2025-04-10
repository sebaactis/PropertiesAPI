using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Properties.Core.Entities
{
    public class Property
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [MinLength(10, ErrorMessage = "The name must be at least 10 characters")]
        [MaxLength(100, ErrorMessage = "The name must be at most 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The description is required")]
        [MinLength(30, ErrorMessage = "The description must be at least 30 characters")]
        [MaxLength(250, ErrorMessage = "The description must be at most 250 characters")]
        public string Description { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
        public int Price { get; set; }
        public int Bedrooms { get; set; }
        public bool IsAvailable { get; set; }
    }
}