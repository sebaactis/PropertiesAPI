using Properties.Core.Entities;
using Properties.Core.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Properties.Application.Mapping
{
    public static class PropertyMapper
    {
        public static Property MapToPropertyCreate(CreatePropertyDTO createPropertyDTO)
        {
            if (createPropertyDTO == null)
            {
                throw new ArgumentNullException("The object to mapping cannot be null");
            }

            Property property = new Property
            {
                Name = createPropertyDTO.Name,
                Description = createPropertyDTO.Description,
                Address = createPropertyDTO.Address,
                Price = createPropertyDTO.Price,
                Bedrooms = createPropertyDTO.Bedrooms,
                IsAvailable = createPropertyDTO.IsAvailable
            };

            return property;
        }

        public static Property MapToPropertyUpdate(Property property, UpdatePropertyDTO updatePropertyDTO)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property), "The property to mapping cannot be null on this method");
            }

            if (updatePropertyDTO == null)
            {
                throw new ArgumentNullException(nameof(updatePropertyDTO), "The object to mapping cannot be null");
            }

            property.Name = !string.IsNullOrWhiteSpace(updatePropertyDTO.Name) ? updatePropertyDTO.Name : property.Name;
            property.Description = !string.IsNullOrWhiteSpace(updatePropertyDTO.Description) ? updatePropertyDTO.Description : property.Description;
            property.Address = !string.IsNullOrWhiteSpace(updatePropertyDTO.Address) ? updatePropertyDTO.Address : property.Address;
            property.Price = updatePropertyDTO.Price ?? property.Price;
            property.Bedrooms = updatePropertyDTO.Bedrooms ?? property.Bedrooms;
            property.IsAvailable = updatePropertyDTO.IsAvailable ?? property.IsAvailable;


            return property;
        }
    }
}
