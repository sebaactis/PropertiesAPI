using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Properties.Core.Entities;
using Properties.Core.Interfaces;
using Properties.Infrastructure.Data;

namespace Properties.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly AppDbContext _context;

        public PropertyService(AppDbContext context)
        {
            _context = context;
        }

        public Task AddPropertyAsync(Property property)
        {
            _context.Properties.Add(property);
            return _context.SaveChangesAsync();
        }

        public Task DeletePropertyAsync(int id)
        {
            var property = _context.Properties.Find(id);

            if (property != null) {
                _context.Properties.Remove(property);
                return _context.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            return await _context.Properties.ToListAsync();
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