using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.Admin
{
    public class OrderPackageProductViewModel
    {
        
    }
    public class OrderPackageIntoAddProductViewModel
    {
        public OrderPackageIntoAddProductViewModel()
        {
            ProductGroupsEditViewModels = new List<ProductGroupsEditViewModel>();
        }
        public long Id { get; set; }

        public IEnumerable<ProductGroupsEditViewModel> ProductGroupsEditViewModels { get; set; }


        public long OrderId { get; set; }
        public long PackageId { get; set; }
    }
    public class OrderPackageIntoListProductViewModel
    {

    }
   
}
