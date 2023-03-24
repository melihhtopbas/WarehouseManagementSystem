using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.ViewModels.Common
{
    public class CurrentUserViewModel
    {
        public long Id { get; set; }


        public string UserName { get; set; }

       


        public string Password { get; set; }
        public string Role { get; set; }



        public string Name { get; set; }

        public string Surname { get; set; }

        public string Mail { get; set; }

        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int MessageCount { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public bool IsMainUser { get; set; }
    }
    public class RoleViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<UserRolesViewModel> UserRolesViewModel { get; set; }

    }
    public class UserRolesViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
