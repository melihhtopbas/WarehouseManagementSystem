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
    }
}
