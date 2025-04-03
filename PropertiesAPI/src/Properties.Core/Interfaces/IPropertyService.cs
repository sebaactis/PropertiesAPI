using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Properties.Core.Entities;

namespace Properties.Core.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetAllPropertiesAsync();
        Task<Property> GetPropertyByIdAsync(int id);
        Task AddPropertyAsync(Property property);
        Task UpdatePropertyAsync(Property property);
        Task DeletePropertyAsync(int id);
    }
}