using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Properties.Core.Entities;
using Properties.Core.Interfaces;

namespace Properties.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        // GET: api/properties
        [HttpGet]
            public IActionResult GetAllProperties([FromQuery] PropertyQueryParams queryParams)
            {
                var result = _propertyService.GetAllPropertiesAsync(queryParams);

                var response = new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                };

                return Ok(response);
            }

        // GET: api/properties/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return NotFound($"Property with ID {id} not found.");
            }
            return Ok(property);
        }

        // POST: api/properties
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProperty([FromBody] Property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _propertyService.AddPropertyAsync(property);
            return CreatedAtAction(nameof(GetPropertyById), new { id = property.Id }, property);
        }

        // PUT: api/properties/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProperty(int id, [FromBody] Property property)
        {
            if (id != property.Id)
            {
                return BadRequest("ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _propertyService.UpdatePropertyAsync(property);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            await _propertyService.DeletePropertyAsync(id);
            return NoContent();
        }
    }
}