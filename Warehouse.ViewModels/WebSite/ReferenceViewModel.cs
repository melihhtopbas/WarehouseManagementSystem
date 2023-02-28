using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.WebSite
{
    public class ReferenceListViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Active")]
        public bool Active { get; set; }

    }
}
