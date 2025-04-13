using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Properties.Core.Entities;
using Properties.Core.Interfaces;
using Properties.Core.Models;
using Properties.Core.Models.DTO;
using System.Net;

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

        [HttpGet]
        public IActionResult GetAllProperties([FromQuery] PropertyQueryParams queryParams)
        {
            try
            {
                var result = _propertyService.GetAllPropertiesAsync(queryParams);

                var response = ApiResponse<PagedList<Property>>.Success(
                    method: "GET",
                    url: "/api/properties",
                    statusCode: (int)HttpStatusCode.OK,
                    message: "Data returned successfully",
                    data: result
                    );

                return Ok(response);

            }
            catch (Exception error)
            {
                var response = ApiResponse<PagedList<Property>>.Error(
                    method: "GET",
                    url: "/api/properties",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    message: $"Error to try to get the data: {error}"
                    );

                return BadRequest(response);
            }


        }

        // GET: api/properties/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPropertyById(Guid id)
        {
            var result = await _propertyService.GetPropertyByIdAsync(id);

            if (result == null)
            {
                var badResponse = ApiResponse<Property>.Error(
                    method: "GET",
                    url: "/api/properties",
                    statusCode: (int)HttpStatusCode.NotFound,
                    message: "Property not found"
                    );

                return NotFound(badResponse);
            }

            var response = ApiResponse<Property>.Success(
                method: "GET",
                url: "/api/properties",
                statusCode: (int)HttpStatusCode.OK,
                message: "Data returned successfully",
                data: result
                );

            return Ok(response);
        }

        // POST: api/properties
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProperty([FromBody] CreatePropertyDTO property)
        {
            if (!ModelState.IsValid)
            {
                var response = ApiResponse<Property>.Error(
                    method: "POST",
                    url: "/api/properties",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    message: ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault() ?? "Invalid data"
                );

                return BadRequest(response);
            }

            try
            {
                var result = await _propertyService.AddPropertyAsync(property);

                if (!result.Success)
                {
                    var errorResponse = ApiResponse<Property>.Error(
                        method: "POST",
                        url: "/api/properties",
                        statusCode: (int)HttpStatusCode.BadRequest,
                        message: "Error creating the property"
                    );

                    return BadRequest(errorResponse);
                }

                var successResponse = ApiResponse<Property>.Success(
                    method: "POST",
                    url: "/api/properties",
                    statusCode: (int)HttpStatusCode.Created,
                    message: "Property created successfully",
                    data: result.Property
                );

                return CreatedAtAction(nameof(GetPropertyById), new { id = result.Property.Id }, successResponse);
            }
            catch (Exception error)
            {
                var errorResponse = ApiResponse<Property>.Error(
                    method: "POST",
                    url: "/api/properties",
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: $"An unexpected error occurred: {error.Message}"
                );

                return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
            }


        }

        // PUT: api/properties/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] UpdatePropertyDTO propertyDto)
        {
            if (!ModelState.IsValid)
            {
                var response = ApiResponse<Property>.Error(
                    method: "PUT",
                    url: "/api/properties",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    message: ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault() ?? "Invalid data"
                );

                return BadRequest(response);
            }

            var result = await _propertyService.UpdatePropertyAsync(id, propertyDto);

            var successResponse = ApiResponse<Property>.Success(
                    method: "POST",
                    url: "/api/properties",
                    statusCode: (int)HttpStatusCode.Created,
                    message: "Property created successfully",
                    data: result.Property
                );

            return Ok(successResponse);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var deletedProperty = await _propertyService.DeletePropertyAsync(id);

            if (deletedProperty == null)
            {
                var errorResponse = ApiResponse<Property>.Error(
                    method: "DELETE",
                    url: $"/api/properties/{id}",
                    statusCode: (int)HttpStatusCode.NotFound,
                    message: "Property not found"
                );

                return NotFound(errorResponse);
            }

            var successResponse = ApiResponse<Property>.Success(
                data: deletedProperty,
                method: "DELETE",
                url: $"/api/properties/{id}",
                statusCode: (int)HttpStatusCode.OK
            );

            return Ok(successResponse);
        }
    }
}