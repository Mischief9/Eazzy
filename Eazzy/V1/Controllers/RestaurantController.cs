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

        [HttpPost("freetable")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult SetTableFree([FromQuery] int id)
        {
            _restaurantService.SetTableFree(id);

            return NoContent();
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

        [HttpGet("tables")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetTables([FromQuery] bool? freeTable,
            [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);
            var tenantId = customer.User.TenantId;

            if (!tenantId.HasValue)
            {
                return Fail(System.Net.HttpStatusCode.NotFound, "Tenant wasn't found");
            }

            var tables = _restaurantService.GetTables(tenantId.Value, freeTable, pageIndex, pageSize);

            return Ok(tables);
        }

        [HttpPost("tables/add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddTable([FromBody]AddTableModel model)
        {
            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);
            var tenantId = customer.User.TenantId;

            if (!tenantId.HasValue)
            {
                return Fail(System.Net.HttpStatusCode.NotFound, "Tenant wasn't found");
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
    }
}
