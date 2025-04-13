using Properties.Core.Entities;
using Properties.Core.Models.DTO;

namespace Properties.Core.Interfaces
{
    public interface IPropertyService
    {
        PagedList<Property> GetAllPropertiesAsync(PropertyQueryParams queryParams);
        Task<Property?> GetPropertyByIdAsync(Guid id);
        Task<(bool Success, Property? Property, IEnumerable<string>? Errors)> AddPropertyAsync(CreatePropertyDTO property);
        Task<(bool Success, Property? Property, IEnumerable<string>? Errors)> UpdatePropertyAsync(Guid id, UpdatePropertyDTO property);
        Task<Property?> DeletePropertyAsync(Guid id);
    }
}