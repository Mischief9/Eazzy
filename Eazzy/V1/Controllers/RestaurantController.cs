using Eazzy.Application.Models.Restaurant;
using Eazzy.Application.Services.CustomerService;
using Eazzy.Application.Services.ImageService;
using Eazzy.Application.Services.RestaurantService;
using Eazzy.Domain.Models.RestaurantManagement;
using Eazzy.Domain.Models.TenantManagement;
using Eazzy.Infrastructure;
using Eazzy.Models.Restaurant;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Eazzy.V1.Controllers
{
    [Route("v1/restaurant")]
    public class RestaurantController : WebApiController
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ICustomerService _customerService;
        private readonly IImageService _imageService;

        public RestaurantController(IRestaurantService restaurantService,
            ICustomerService customerService,
            IImageService imageService)
        {
            _restaurantService = restaurantService;
            _customerService = customerService;
            _imageService = imageService;
        }

        [HttpGet("All")]
        [ProducesResponseType(typeof(List<RestaurantResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetRestaurants([FromQuery] GetRestaurantFilter filter)
        {
            var restaurants = _restaurantService.GetAllRestaurant(filter);

            var model = restaurants.Select(x => new RestaurantResponse()
            {
                Id = x.Id,
                TenantStatus = x.TenantStatus,
                Address = x.Address,
                CreateDateOnUtc = x.CreateDateOnUtc,
                ImageUrl = _imageService.GetImageUrlByName(x.ImageFileName),
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                TaxAmount = x.TaxAmount,
                TaxPercentage = x.TaxPercentage,
                TaxType = x.TaxType,
                TimeZone = x.TimeZone,
                UpdatedDateTimeOnUtc = x.UpdatedDateTimeOnUtc,
                Description = x.Description
            });

            return Ok(new { data = model, totalCount = restaurants.TotalCount });
        }

        [HttpPost("register/newrestaurant")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        // [Authorize(Roles = "System Administrator")]
        public async Task<IActionResult> RegisterNewRestaurant([FromBody] AddNewRestaurant newRestaurant)
        {
            var newTenant = new Tenant()
            {
                Name = newRestaurant.Name,
                TenantStatus = newRestaurant.TenantStatus,
                Address = newRestaurant.Address,
                CreateDateOnUtc = DateTime.UtcNow,
                UpdatedDateTimeOnUtc = DateTime.UtcNow,
                PhoneNumber = newRestaurant.PhoneNumber,
                TaxType = newRestaurant.TaxType,
                TaxAmount = newRestaurant.TaxAmount,
                TaxPercentage = newRestaurant.TaxPercentage,
                Description = newRestaurant.Description
            };

            await _restaurantService.CreateNewRestaurant(newTenant, newRestaurant.SignUpRequest);

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(Tenant), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult ChangeRestaurantDetails()
        {
            var id = GetCurrentCustomer().User.TenantId;

            if (!id.HasValue)
            {
                return Fail(HttpStatusCode.NotFound, "Restaurant wasn't found.");
            }

            var restaurant = _restaurantService.FindById(id.Value);

            if (restaurant == null)
            {
                return Fail(HttpStatusCode.NotFound, "Restaurant wasn't found.");
            }

            return Ok(restaurant);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Tenant), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult RestaurantDetails([FromRoute] int id)
        {
            var restaurant = _restaurantService.FindById(id);

            if (restaurant == null)
            {
                return Fail(HttpStatusCode.NotFound, "Restaurant wasn't found.");
            }

            var model = new RestaurantResponse()
            {
                Id = restaurant.Id,
                TenantStatus = restaurant.TenantStatus,
                Address = restaurant.Address,
                CreateDateOnUtc = restaurant.CreateDateOnUtc,
                Description = restaurant.Description,
                ImageUrl = _imageService.GetImageUrlByName(restaurant.ImageFileName),
                Name = restaurant.Name,
                PhoneNumber = restaurant.PhoneNumber,
                TaxAmount = restaurant.TaxAmount,
                TaxPercentage = restaurant.TaxPercentage,
                TaxType = restaurant.TaxType,
                TimeZone = restaurant.TimeZone,
                UpdatedDateTimeOnUtc = restaurant.UpdatedDateTimeOnUtc
            };

            return Ok(model);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(Tenant), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult ChangeRestaurantDetails([FromForm] UpdateRestaurantModel model)
        {
            var customer = GetCurrentCustomer();
            var tenantId = customer.User.TenantId;

            if (!tenantId.HasValue)
            {
                return Fail(HttpStatusCode.NotFound, "Tenant wasn't found");
            }

            var restaurant = _restaurantService.FindById(tenantId.Value);

            if (restaurant == null)
            {
                return Fail(HttpStatusCode.NotFound, "Restaurant wasn't found.");
            }

            restaurant.Name = model.Name;
            restaurant.PhoneNumber = model.PhoneNumber;
            restaurant.TaxType = model.TaxType;
            restaurant.TaxAmount = model.TaxAmount;
            restaurant.TaxPercentage = model.TaxPercentage;
            restaurant.TimeZone = model.TimeZone;
            restaurant.UpdatedDateTimeOnUtc = DateTime.UtcNow;
            restaurant.Description = model.Description;

            if (model.Image != null)
            {
                var fileName = _imageService.Upload(model.Image);
                restaurant.ImageFileName = fileName;
            }

            _restaurantService.UpdateTenant(restaurant);

            return Ok(restaurant);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "System Administrator")]
        public IActionResult DeleteRestaurant([FromRoute] int id)
        {
            var restaurant = _restaurantService.FindById(id);

            if (restaurant == null)
            {
                return Fail(HttpStatusCode.NotFound, "Restaurant wasn't found.");
            }

            restaurant.IsDeleted = true;

            _restaurantService.UpdateTenant(restaurant);

            return NoContent();
        }

        [HttpGet("tables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetTables([FromQuery] bool? freeTable,
            [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var customer = GetCurrentCustomer();
            var tenantId = customer.User.TenantId;

            if (!tenantId.HasValue)
            {
                return Fail(HttpStatusCode.NotFound, "Tenant wasn't found");
            }

            var tables = _restaurantService.GetTables(tenantId.Value, freeTable, pageIndex, pageSize);

            return Ok(new { data = tables.Data, totalCount = tables.TotalCount });
        }


        [HttpPost("tables")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult AddTable([FromBody] AddTableModel model)
        {
            var customer = GetCurrentCustomer();
            var tenantId = customer.User.TenantId;

            if (!tenantId.HasValue)
            {
                return Fail(HttpStatusCode.NotFound, "Tenant wasn't found");
            }

            var newTable = new Table()
            {
                TableNumber = model.TableNumber,
                TenantId = tenantId.Value,
                IsFree = true
            };

            _restaurantService.InsertTable(newTable);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("tables/{tableId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult ChangeTable([FromRoute] int tableId)
        {
            var table = _restaurantService.FindTableById(tableId);

            if (table == null)
            {
                return Fail(HttpStatusCode.NotFound, "Table wasn't found");
            }

            return Ok(table);
        }


        [HttpPatch("tables/{tableId}")]
        [ProducesResponseType(typeof(Table), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult ChangeTable([FromRoute] int tableId, [FromBody] AddTableModel model)
        {
            var table = _restaurantService.FindTableById(tableId);

            if (table == null)
            {
                return Fail(HttpStatusCode.NotFound, "Table wasn't found");
            }

            table.TableNumber = model.TableNumber;

            _restaurantService.UpdateTable(table);

            return Ok(table);
        }

        [HttpDelete("tables/{tableId}")]
        [ProducesResponseType(typeof(Table), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult DeleteTable([FromRoute] int tableId)
        {
            var table = _restaurantService.FindTableById(tableId);

            if (table == null)
            {
                return Fail(HttpStatusCode.NotFound, "Table wasn't found");
            }

            _restaurantService.DeleteTable(table);

            return Ok(table);
        }

        [HttpPost("freetable")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult SetTableFree([FromQuery] int id)
        {
            _restaurantService.SetTableFree(id);

            return NoContent();
        }
    }
}
