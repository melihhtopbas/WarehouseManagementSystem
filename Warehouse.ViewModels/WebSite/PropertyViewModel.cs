using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.ViewModels.Common;

namespace Warehouse.ViewModels.WebSite
{
    public class PropertyListViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ImageListViewModel MainImage { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public class PropertyDetailViewModel
    {

        public string Name { get; set; }
        public ImageListViewModel MainImage { get; set; }
        public string Link { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }

    }
}
