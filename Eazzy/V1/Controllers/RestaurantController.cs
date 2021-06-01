using Eazzy.Application.Models.Restaurant;
using Eazzy.Application.Services.CustomerService;
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

        public RestaurantController(IRestaurantService restaurantService,
            ICustomerService customerService)
        {
            _restaurantService = restaurantService;
            _customerService = customerService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<Tenant>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetRestaurants([FromQuery] GetRestaurantFilter filter)
        {
            var restaurants = _restaurantService.GetAllRestaurant(filter);

            return Ok(restaurants);
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
                TaxPercentage = newRestaurant.TaxPercentage
            };

            await _restaurantService.CreateNewRestaurant(newTenant, newRestaurant.SignUpRequest);

            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Tenant), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult ChangeRestaurantDetails([FromRoute]int id)
        {
            var restaurant = _restaurantService.FindById(id);

            if (restaurant == null)
            {
                return Fail(HttpStatusCode.NotFound, "Restaurant wasn't found.");
            }

            return Ok(restaurant);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(Tenant),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult ChangeRestaurantDetails([FromBody] UpdateRestaurantModel model)
        {
            var customer = GetCurrentCustomer();
            var tenantId = customer.User.TenantId;

            if (!tenantId.HasValue)
            {
                return Fail(HttpStatusCode.NotFound, "Tenant wasn't found");
            }

            var restaurant = _restaurantService.FindById(tenantId.Value);

            if(restaurant == null)
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

            return Ok(tables);
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
        [ProducesResponseType(typeof(Table),StatusCodes.Status200OK)]
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
