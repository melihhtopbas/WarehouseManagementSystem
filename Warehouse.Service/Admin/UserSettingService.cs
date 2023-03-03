using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;

namespace Warehouse.Service.Admin
{
    public class UserSettingService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public UserSettingService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<UserListViewModel> _getUserListIQueryable(Expression<Func<Data.Users, bool>> expr)
        {
            
             

            return (from b in _context.Users.AsExpandable().Where(expr)
                    .Where(x=>x.Role != "admin")
                    select new UserListViewModel()
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Mail = b.Mail,
                        Password= b.Password,
                         Phone= b.Phone,
                         Surname= b.Surname,
                         City = b.Cities.Name,
                         Country = b.Countries.Name,
                         UserName= b.UserName,
                        

                    });
        }

        public IQueryable<UserListViewModel> GetUserListIQueryable(UserSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Users>(true);/*AND*/ 
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                predicate.And(a => a.UserName.Contains(model.Name));

            }
            return _getUserListIQueryable(predicate);
        }
    

        public async Task<UserListViewModel> GetUserListViewAsync(long userId)
        {

            var predicate = PredicateBuilder.New<Data.Users>(true);/*AND*/
            predicate.And(a => a.Id == userId);
            var user = await _getUserListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return user;
        }
    }
}
