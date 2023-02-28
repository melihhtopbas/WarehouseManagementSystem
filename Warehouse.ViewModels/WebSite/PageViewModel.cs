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
    public class PageListViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Link { get; set; }
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Active")]
        public bool Active { get; set; }

    }
    public class PageDetailViewModel
    {

        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

    }
}
