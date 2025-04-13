
using Properties.Application.Mapping;
using Properties.Core.Entities;
using Properties.Core.Interfaces;
using Properties.Core.Models.DTO;
using Properties.Infrastructure.Data;

namespace Properties.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly AppDbContext _context;
        private readonly IEntityValidator<Property> _validator;

        public PropertyService(AppDbContext context, IEntityValidator<Property> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<(bool Success, Property? Property, IEnumerable<string>? Errors)> AddPropertyAsync(CreatePropertyDTO propertyDTO)
        {

            Property property = PropertyMapper.MapToPropertyCreate(propertyDTO);

            IEnumerable<string> errors = await _validator.ValidatePropertyAsync(property);

            if (errors.Any())
            {
                return (false, null, errors);
            }

            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();

            return (true, property, null);

        }

        public async Task<Property?> DeletePropertyAsync(Guid id)
        {
            var property = await _context.Properties.FindAsync(id);

            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
                return property;
            }

            return null;
        }

        public PagedList<Property> GetAllPropertiesAsync(PropertyQueryParams queryParams)
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(queryParams.SearchTerm) ||
                                        p.Description.Contains(queryParams.SearchTerm));
            }

            // Ordenar resultados
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                switch (queryParams.SortBy.ToLower())
                {
                    case "price":
                        query = queryParams.OrderBy ? query.OrderBy(p => p.Price) :
                                                    query.OrderByDescending(p => p.Price);
                        break;
                    default:
                        query = queryParams.OrderBy ? query.OrderBy(p => p.Id) :
                                                    query.OrderByDescending(p => p.Id);
                        break;
                }
            }

            // Aplicar paginación
            return PagedList<Property>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);
        }

        public async Task<Property?> GetPropertyByIdAsync(Guid id)
        {
            return await _context.Properties.FindAsync(id);
        }

        public async Task<(bool Success, Property? Property, IEnumerable<string>? Errors)> UpdatePropertyAsync(Guid id, UpdatePropertyDTO updatePropertyDTO)
        {
            Property property = await GetPropertyByIdAsync(id);

            if(property == null)
            {
                throw new ArgumentNullException("The property doesnt exists");
            }

            Property propertyMap = PropertyMapper.MapToPropertyUpdate(property, updatePropertyDTO);

            IEnumerable<string> errors = await _validator.ValidatePropertyAsync(propertyMap);

            if (errors.Any())
            {
                return (false, null, errors);
            }

            _context.Properties.Update(propertyMap);
            await _context.SaveChangesAsync();

            return (true, property, null);
        }
    }
}