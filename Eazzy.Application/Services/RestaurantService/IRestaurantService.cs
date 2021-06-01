using Eazzy.Application.Models.Account;
using Eazzy.Application.Models.Restaurant;
using Eazzy.Domain.Models.RestaurantManagement;
using Eazzy.Domain.Models.TenantManagement;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.RestaurantService
{
    public interface IRestaurantService
    {
        IPagedList<Tenant> GetAllRestaurant(GetRestaurantFilter filter);

        Tenant FindById(int id);

        void UpdateTenant(Tenant tenant);

        void SetTableFree(int id);

        Task CreateNewRestaurant(Tenant tenant, SignUpRequest signUpRequest);

        IPagedList<Table> GetTables(int tenantId, bool? freeTable, int pageIndex = 1, int pageSize = 10);

        Table FindTableById(int id);

        void InsertTable(Table table);

        void InsertTable(IEnumerable<Table> tables);

        void UpdateTable(Table table);

        void DeleteTable(Table table);

        void SetTableLocked(int id);

        decimal GetRestaurantOrderTotalAndTax(int id, decimal total, out decimal tax);
    }
}
