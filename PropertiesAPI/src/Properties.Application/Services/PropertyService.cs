
using System.ComponentModel.DataAnnotations;
using Properties.Core.Entities;
using Properties.Core.Interfaces;
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

        public async Task AddPropertyAsync(Property property)
        {
            IEnumerable<string> errors = await _validator.ValidatePropertyAsync(property);

            if (errors.Any())
            {
                throw new Exception("Error en la validacion");
            }

            // Lógica para agregar la propiedad
            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();
        }

        public Task DeletePropertyAsync(int id)
        {
            var property = _context.Properties.Find(id);

            if (property != null)
            {
                _context.Properties.Remove(property);
                return _context.SaveChangesAsync();
            }

            return Task.CompletedTask;
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

        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties.FindAsync(id);
        }

        public async Task UpdatePropertyAsync(Property property)
        {
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
        }
    }
}