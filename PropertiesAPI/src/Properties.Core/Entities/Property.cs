using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Properties.Core.Entities
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int Price { get; set; }
        public int Bedrooms { get; set; }
        public bool IsAvailable { get; set; }
    }
}