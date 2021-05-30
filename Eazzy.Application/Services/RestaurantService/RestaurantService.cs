using Eazzy.Application.Models.Account;
using Eazzy.Application.Models.Restaurant;
using Eazzy.Application.Services.AccountService;
using Eazzy.Domain.Models.RestaurantManagement;
using Eazzy.Domain.Models.TenantManagement;
using Eazzy.Domain.Models.TenantManagement.Enums;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.RestaurantService
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<Table> _tableRepository;
        private readonly IAccountService _accountService;

        public RestaurantService(IRepository<Tenant> tenantRepository,
            IRepository<Table> tableRepository,
            IAccountService accountService)
        {
            _tenantRepository = tenantRepository;
            _tableRepository = tableRepository;
            _accountService = accountService;
        }

        public IPagedList<Tenant> GetAllRestaurant(GetRestaurantFilter filter)
        {
            var tenants = _tenantRepository.Table;

            if (!string.IsNullOrEmpty(filter.Name))
            {
                tenants = tenants.Where(x => x.Name.Contains(filter.Name));
            }

            return new PagedList<Tenant>(tenants, filter.PageIndex, filter.PageSize);
        }

        public void SetTableLocked(int id)
        {
            var table = _tableRepository.Find(id);

            table.IsFree = false;

            _tableRepository.Update(table);
        }

        public void SetTableFree(int id)
        {
            var table = _tableRepository.Find(id);

            table.IsFree = true;

            _tableRepository.Update(table);
        }

        public async Task CreateNewRestaurant(Tenant tenant, SignUpRequest signUpRequest)
        {
            if (tenant == null)
                throw new ArgumentNullException(nameof(tenant));

            _tenantRepository.Add(tenant);

            signUpRequest.TenantId = tenant.Id;
            await _accountService.Register(signUpRequest);
        }

        public IPagedList<Table> GetTables(int tenantId, bool? freeTable, int pageIndex = 1, int pageSize = 10)
        {
            var tables = _tableRepository.Table.Where(x => x.TenantId == tenantId);

            if (freeTable.HasValue)
            {
                tables = tables.Where(x => x.IsFree == freeTable.Value);
            }

            return new PagedList<Table>(tables, pageIndex, pageSize);
        }

        public void InsertTable(Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _tableRepository.Add(table);
        }

        public void InsertTable(IEnumerable<Table> tables)
        {
            if (tables == null)
                throw new ArgumentNullException(nameof(tables));

            _tableRepository.Add(tables);
        }

        public void UpdateTable(Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _tableRepository.Update(table);
        }

        public void DeleteTable(Table table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _tableRepository.Delete(table);
        }

        public Tenant FindById(int id)
        {
            var restaurant = _tenantRepository.Find(id);

            return restaurant;
        }

        public decimal GetRestaurantOrderTotalAndTax(int id, decimal total, out decimal tax)
        {
            var restaurant = _tenantRepository.Find(id);

            switch (restaurant.TaxType)
            {
                case TaxType.AMOUNT:
                    total += restaurant.TaxAmount.Value;
                    tax = restaurant.TaxAmount.Value;
                    break;
                case TaxType.PERCENTAGE:
                    var temp = total;
                    total += total * restaurant.TaxAmount.Value / 100;
                    tax = temp - total;
                    break;
                default:
                    tax = 0;
                    break;
            }

            return total;
        }
    }
}
